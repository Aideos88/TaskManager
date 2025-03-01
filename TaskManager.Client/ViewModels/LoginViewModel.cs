﻿using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Client.Views;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        UsersRequestService _usersRequestService;


        #region COMMAND
        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        public DelegateCommand<object> LoginFromCacheCommand { get; private set; }
        #endregion

        public LoginViewModel()
        {
            _usersRequestService = new UsersRequestService();
            CurrentUserCache = GetUserCache();

            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
            LoginFromCacheCommand = new DelegateCommand<object>(LoginFromCache);
        }

        #region PROPERTIES
        private string _cachePath = Path.GetTempPath() + "usertaskmanager.txt";

        private Window _currentWnd;

        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }

        private UserCache _currentUserCache;
        public UserCache CurrentUserCache
        {
            get => _currentUserCache;
            set
            {
                _currentUserCache = value;
                RaisePropertyChanged(nameof(CurrentUserCache));
            }
        }


        private UserModel _currentUser;
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }
        #endregion

        #region METHODS
        private void GetUserFromDB(object parameter)
        {
            var passBox = parameter as PasswordBox;
            bool isNewUser = false;

            _currentWnd = Window.GetWindow(passBox);

            if (UserLogin != CurrentUserCache?.Login ||
                UserPassword != CurrentUserCache?.Password)
            {
                isNewUser = true;
            }

            UserPassword = passBox.Password;

            AuthToken = _usersRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken == null)
                return;

            CurrentUser = _usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                if (isNewUser)
                {
                    var saveUserCacheMessage = MessageBox.Show("Хотите сохранить логин и пароль?", "Сохранение данных", MessageBoxButton.YesNo);
                    if (saveUserCacheMessage == MessageBoxResult.Yes)
                    {
                        UserCache newUserCache = new UserCache
                        {
                            Login = UserLogin,
                            Password = UserPassword,
                        };
                        CreateUserCache(newUserCache);
                    }
                }
                OpenMainWindow();
            }
        }

        private void CreateUserCache(UserCache userCache)
        {
            string jsonUserCache = JsonConvert.SerializeObject(userCache);

            using (StreamWriter sw = new StreamWriter(_cachePath, false, Encoding.Default))
            {
                sw.Write(jsonUserCache);
                MessageBox.Show("Успех!");
            }
        }

        private UserCache GetUserCache()
        {
            bool isCacheExist = File.Exists(_cachePath);

            if (isCacheExist && File.ReadAllText(_cachePath)?.Length > 0)
                return isCacheExist ? JsonConvert.DeserializeObject<UserCache>(File.ReadAllText(_cachePath)) : null;
            return null;
        }

        private void LoginFromCache(object wnd)
        {
            _currentWnd = wnd as Window;
            UserLogin = CurrentUserCache.Login;
            UserPassword = CurrentUserCache.Password;
            AuthToken = _usersRequestService.GetToken(UserLogin, UserPassword);

            CurrentUser = _usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                OpenMainWindow();
            }
        }

        private void OpenMainWindow()
        {
            int workTime = _usersRequestService.GetWorkTimeMinutes(AuthToken);
            if (workTime > 0)
            {
                MainWindow window = new MainWindow();
                window.DataContext = new MainWindowViewModel(AuthToken, CurrentUser, window, workTime);
                window.Show();
                _currentWnd.Close();
            }
        }

        #endregion

    }
}
