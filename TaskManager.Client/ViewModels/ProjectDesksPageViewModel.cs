using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Client.Views;
using TaskManager.Client.Views.AddWindows;
using TaskManager.Client.Views.Pages;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class ProjectDesksPageViewModel : BindableBase
    {
        private CommonViewService _viewService;
        private DesksRequestService _desksRequestService;
        private UsersRequestService _usersRequestService;
        private DeskViewService _deskViewService;
        private MainWindowViewModel _mainWindowVM;

        #region COMMANDS
        public DelegateCommand OpenNewDeskCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateDeskCommand { get; private set; }
        public DelegateCommand CreateOrUpdateDeskCommand { get; private set; }
        public DelegateCommand DeleteDeskCommand { get; private set; }
        public DelegateCommand SelectPhotoForDeskCommand { get; private set; }
        public DelegateCommand AddNewColumnItemCommand { get; private set; }
        public DelegateCommand<object> RemoveColumnItemCommand { get; private set; }
        public DelegateCommand<object> OpenDeskTasksPageCommand { get; private set; }


        #endregion

        public ProjectDesksPageViewModel(AuthToken token, ProjectModel project, MainWindowViewModel mainWindowVM)
        {
            _project = project;
            _token = token;
            _mainWindowVM = mainWindowVM;

            _viewService = new CommonViewService();
            _desksRequestService = new DesksRequestService();
            _usersRequestService = new UsersRequestService();
            _deskViewService = new DeskViewService(_token, _desksRequestService);

            UpdatePage();

            OpenNewDeskCommand = new DelegateCommand(OpenNewDesk);
            OpenUpdateDeskCommand = new DelegateCommand<object>(OpenUpdateDesk);
            CreateOrUpdateDeskCommand = new DelegateCommand(CreateOrUpdateDesk);
            DeleteDeskCommand = new DelegateCommand(DeleteDesk);
            SelectPhotoForDeskCommand = new DelegateCommand(SelectPhotoForDesk);
            AddNewColumnItemCommand = new DelegateCommand(AddNewColumnItem);
            RemoveColumnItemCommand = new DelegateCommand<object>(RemoveColumnItem);
            OpenDeskTasksPageCommand = new DelegateCommand<object>(OpenDeskTasksPage);
        }

        #region PROPERTIES

        private AuthToken _token;
        private ProjectModel _project;

        private List<ModelClient<DeskModel>> _projectDesks = new List<ModelClient<DeskModel>>();
        public List<ModelClient<DeskModel>> ProjectDesks
        {
            get => _projectDesks;
            set
            {
                _projectDesks = value;
                RaisePropertyChanged(nameof(ProjectDesks));
            }
        }

        private ModelClient<DeskModel> _selectedDesk;
        public ModelClient<DeskModel> SelectedDesk
        {
            get => _selectedDesk;
            set
            {
                _selectedDesk = value;
                RaisePropertyChanged(nameof(SelectedDesk));
            }
        }

        private ClientAction _typeActionWithDesk;
        public ClientAction TypeActionWithDesk
        {
            get => _typeActionWithDesk;
            set
            {
                _typeActionWithDesk = value;
                RaisePropertyChanged(nameof(TypeActionWithDesk));
            }
        }

        private ObservableCollection<ColumnBindingHelp> _columnsForNewDesk = new ObservableCollection<ColumnBindingHelp>()
        {
            new ColumnBindingHelp("Новая"),
            new ColumnBindingHelp("В процессе"),
            new ColumnBindingHelp("На рассмотрении"),
            new ColumnBindingHelp("Выполнено")
        };
        public ObservableCollection<ColumnBindingHelp> ColumnsForNewDesk
        {
            get => _columnsForNewDesk;
            set
            {
                _columnsForNewDesk = value;
                RaisePropertyChanged(nameof(ColumnsForNewDesk));
            }
        }

        public UserModel CurrentUser
        {
            get => _usersRequestService.GetCurrentUser(_token);
        }

        #endregion

        #region METHODS

        private void OpenNewDesk()
        {
            SelectedDesk = new ModelClient<DeskModel>(new DeskModel());
            TypeActionWithDesk = ClientAction.Create;
            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private void OpenUpdateDesk(object deskId)
        {
            SelectedDesk = _deskViewService.GetDeskClientById(deskId);

            if (CurrentUser.Id != SelectedDesk.Model.AdminId)
            {
                _viewService.ShowMessage("Вы не являетесь администратором!");
                return;
            }

            TypeActionWithDesk = ClientAction.Update;
            ColumnsForNewDesk = new ObservableCollection<ColumnBindingHelp>(SelectedDesk.Model.Columns.Select(c => new ColumnBindingHelp(c)));
            _deskViewService.OpenViewDeskInfo(deskId, this);
        }

        private void CreateOrUpdateDesk()
        {
            if (TypeActionWithDesk == ClientAction.Create)
                CreateDesk();

            if (TypeActionWithDesk == ClientAction.Update)
                UpdateDesk();

            UpdatePage();
        }

        private void CreateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            SelectedDesk.Model.ProjectId = _project.Id;

            var result = _desksRequestService.CreateDesk(_token, SelectedDesk.Model);
            _viewService.ShowActionResult(result, "Доска создана");
        }

        private void UpdateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            _deskViewService.UpdateDesk(SelectedDesk.Model);
        }

        private void DeleteDesk()
        {
            _deskViewService.DeleteDesk(SelectedDesk.Model.Id);
            UpdatePage();
        }
        private void UpdatePage()
        {
            SelectedDesk = null;
            ProjectDesks = _deskViewService.GetDesks(_project.Id);
            _viewService.CurrentOpenedWindow?.Close();
        }

        private void RemoveColumnItem(object item)
        {
            var itemToRemove = item as ColumnBindingHelp;
            ColumnsForNewDesk.Remove(itemToRemove);
        }

        private void AddNewColumnItem()
        {
            ColumnsForNewDesk.Add(new ColumnBindingHelp("Колонка"));
        }

        private void SelectPhotoForDesk()
        {
            SelectedDesk = _deskViewService.SelectPhotoForDesk(SelectedDesk);
        }

        private void OpenDeskTasksPage(object deskId)
        {
            SelectedDesk = _deskViewService.GetDeskClientById(deskId);
            var page = new DeskTasksPage();
            var context = new DeskTasksPageViewModel(_token, SelectedDesk.Model, page);
            _mainWindowVM.OpenPage(page, $"Задача {SelectedDesk.Model.Name}", context);
        }

        #endregion
    }
}
