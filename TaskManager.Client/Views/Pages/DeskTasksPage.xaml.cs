using System.Windows.Controls;

namespace TaskManager.Client.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DeskTasksPage.xaml
    /// </summary>
    public partial class DeskTasksPage : Page
    {
        public Grid TasksGrid { get; set; }

        public DeskTasksPage()
        {
            InitializeComponent();
            TasksGrid = tasksGrid;
        }
    }
}
