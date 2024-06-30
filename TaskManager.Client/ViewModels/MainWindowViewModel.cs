using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Client.Views;
using TaskManager.Client.Views.AddWindows;
using TaskManager.Client.Views.Pages;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private CommonViewService _viewService;
        private readonly int _workTimeMinutes;

        #region COMMAND

        public DelegateCommand OpenMyInfoPageCommand { get; private set; }
        public DelegateCommand OpenMyDesksPageCommand { get; private set; }
        public DelegateCommand OpenMyTasksPageCommand { get; private set; }
        public DelegateCommand OpenMyProjectsPageCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        public DelegateCommand OpenUsersManagmentCommand;

        #endregion

        public MainWindowViewModel(AuthToken token, UserModel currentUser, Window currentWindow, int workTimeMinutes)
        {
            _viewService = new CommonViewService();

            _workTimeMinutes = workTimeMinutes;

            Token = token;
            CurrentUser = currentUser;
            _currentWindow = currentWindow;

            OpenMyInfoPageCommand = new DelegateCommand(OpenMyInfoPage);
            NavigationButtons.Add(_userInfoBtnName, OpenMyInfoPageCommand);

            OpenMyDesksPageCommand = new DelegateCommand(OpenDesksPage);
            NavigationButtons.Add(_userDesksBtnName, OpenMyDesksPageCommand);

            OpenMyTasksPageCommand = new DelegateCommand(OpenTasksPage);
            NavigationButtons.Add(_userTasksBtnName, OpenMyTasksPageCommand);

            OpenMyProjectsPageCommand = new DelegateCommand(OpenProjectsPage);
            NavigationButtons.Add(_userProjectsBtnName, OpenMyProjectsPageCommand);

            if (CurrentUser.Status == UserStatus.Admin)
            {
                OpenUsersManagmentCommand = new DelegateCommand(OpenUsersManagment);
                NavigationButtons.Add(_manageUsersBtnName, OpenUsersManagmentCommand);
            }

            LogoutCommand = new DelegateCommand(Logout);
            NavigationButtons.Add(_logoutBtnName, LogoutCommand);

            StartWork(_workTimeMinutes);

            OpenMyInfoPage();
        }


        #region PROPERTIS
        private readonly string _userProjectsBtnName = "Мои проекты";
        private readonly string _userDesksBtnName = "Мои доски";
        private readonly string _userTasksBtnName = "Мои задачи";
        private readonly string _userInfoBtnName = "О себе";
        private readonly string _logoutBtnName = "Выход";

        private Window _currentWindow;

        private readonly string _manageUsersBtnName = "Пользователи";

        private Dictionary<string, DelegateCommand> _navigationButtons = new Dictionary<string, DelegateCommand>();

        public Dictionary<string, DelegateCommand> NavigationButtons
        {
            get => _navigationButtons;
            set
            {
                _navigationButtons = value;
                RaisePropertyChanged(nameof(NavigationButtons));
            }
        }

        private AuthToken _authToken;
        public AuthToken Token
        {
            get => _authToken;
            private set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(Token));
            }
        }

        private UserModel _currentUser;
        public UserModel CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private string _selectedPageName;
        public string SelectedPageName
        {
            get => _selectedPageName;
            set
            {
                _selectedPageName = value;
                RaisePropertyChanged(nameof(SelectedPageName));
            }
        }

        private Page _selectedPage;
        public Page SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;

                RaisePropertyChanged(nameof(SelectedPage));
            }
        }



        #endregion

        #region METHODS
        private void OpenMyInfoPage()
        {
            var page = new UserInfoPage();
            OpenPage(page, _userInfoBtnName, this);
        }

        private void OpenDesksPage()
        {
            var page = new UserDesksPage();
            OpenPage(page, _userDesksBtnName, new UserDesksPageViewModel(Token));
        }

        private void OpenTasksPage()
        {
            var page = new UserTasksPage();
            OpenPage(page, _userTasksBtnName, new UserTasksPageViewModel(Token));
        }

        private void OpenProjectsPage()
        {
            var page = new ProjectsPage();
            OpenPage(page, _userProjectsBtnName, new ProjectsPageViewModel(Token, this));
        }

        private void Logout()
        {
            var question = MessageBox.Show("Are you sure?", "Logout", MessageBoxButton.YesNo);
            if (question == MessageBoxResult.Yes && _currentWindow != null)
            {
                Login login = new Login();
                login.Show();
                _currentWindow.Close();
            }
        }

        private void OpenUsersManagment()
        {
            SelectedPageName = _manageUsersBtnName;
            var page = new UsersPage();
            OpenPage(page, _manageUsersBtnName, new UsersPageViewModel(Token));

        }

        private void StopWork()
        {
            Login login = new Login();
            login.Show();
            _currentWindow.Close();
        }

        #endregion

        public void OpenPage(Page page, string pageName, BindableBase viewModel)
        {
            SelectedPageName = pageName;
            SelectedPage = page;
            SelectedPage.DataContext = viewModel;
        }

        private async void StartWork(int minutes)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(minutes * 60000);
            });
            StopWork();
        }

    }
}
