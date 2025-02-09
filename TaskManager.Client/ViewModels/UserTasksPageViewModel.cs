﻿using Prism.Mvvm;
using TaskManager.Client.Models;
using TaskManager.Client.Services;

namespace TaskManager.Client.ViewModels
{
    public class UserTasksPageViewModel : BindableBase
    {
        private AuthToken _token;
        private TasksRequestService _tasksRequestService;
        private UsersRequestService _usersRequestService;

        public UserTasksPageViewModel(AuthToken token)
        {
            _token = token;
            _tasksRequestService = new TasksRequestService();
            _usersRequestService = new UsersRequestService();
        }

        public List<TaskClient> AllTasks
        {
            get => _tasksRequestService.GetAllTasks(_token).Select(
                task =>
                {
                    var taskClient = new TaskClient(task);

                    if (task.CreatorId != null)
                    {
                        int creatorId = (int)task.CreatorId;
                        taskClient.Creator = _usersRequestService.GetUserById(_token, creatorId);
                    }
                    if (task.ExecutorId != null)
                    {
                        int executorId = (int)task.ExecutorId;
                        taskClient.Executor = _usersRequestService.GetUserById(_token, executorId);
                    }
                    return taskClient;
                }).ToList();
        }

    }
}
