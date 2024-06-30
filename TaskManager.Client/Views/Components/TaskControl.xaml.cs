using System.Windows.Controls;
using TaskManager.Client.Models;

namespace TaskManager.Client.Views.Components
{
    /// <summary>
    /// Логика взаимодействия для TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        public TaskControl(TaskClient task)
        {
            InitializeComponent();
            DataContext = task;
        }
    }
}
