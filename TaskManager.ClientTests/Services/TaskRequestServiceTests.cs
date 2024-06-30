using System.Net;
using TaskManager.Client.Models;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class TasksRequestServiceTests
    {
        private AuthToken _token;
        private TasksRequestService _service;

        public TasksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new TasksRequestService();
        }

        [TestMethod()]
        public void GetAllTasksTest()
        {
            var tasks = _service.GetAllTasks(_token);
            Console.WriteLine(tasks.Count);
            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod()]
        public void GetTaskByIdTest()
        {
            var task = _service.GetTaskById(_token, 7);
            Assert.AreNotEqual(null, task);
        }

        [TestMethod()]
        public void GetTasksByDeskTest()
        {
            var task = _service.GetTasksByDesk(_token, 1);
            Assert.AreNotEqual(0, task.Count);
        }

        [TestMethod()]
        public void CreateTaskTest()
        {
            var task = new TaskModel("Задача для теста", "задача для тестирования сервиса", DateTime.Now, DateTime.Now, "тестирование");
            task.DeskId = 4;
            task.ExecutorId = 1;
            var result = _service.CreateTask(_token, task);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            var task = new TaskModel("Задача для теста UPD", "задача для тестирования сервиса UPD", DateTime.Now, DateTime.Now, "тестирование upd");
            task.Id = 10;
            task.ExecutorId = 8;
            var result = _service.UpdateTask(_token, task);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteTaskByIdTest()
        {
            var result = _service.DeleteTask(_token, 9);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}