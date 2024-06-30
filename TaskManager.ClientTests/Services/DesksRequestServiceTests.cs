using System.Net;
using TaskManager.Client.Models;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class DesksRequestServiceTests
    {
        private AuthToken _token;
        private DesksRequestService _service;

        public DesksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new DesksRequestService();
        }

        [TestMethod()]
        public void GetAllDesksTest()
        {
            var desks = _service.GetAllDesks(_token);
            Console.WriteLine(desks.Count);
            Assert.AreNotEqual(Array.Empty<DeskModel>(), desks.ToArray());
        }

        [TestMethod()]
        public void GetDeskByIdTest()
        {
            var desk = _service.GetDeskById(_token, 1);
            Assert.AreNotEqual(null, desk);
        }

        [TestMethod()]
        public void GetDeskByProjectTest()
        {
            var desk = _service.GetDesksByProject(_token, 1);
            Assert.AreEqual(2, desk.Count);
        }

        [TestMethod()]
        public void CreateDeskTest()
        {
            var desk = new DeskModel("Доска для теста", "доска для тестирования сервиса", true, new string[] {"Новая", "доска", "готова"});
            desk.ProjectId = 2;
            desk.AdminId = 1;
            var result = _service.CreateDesk(_token, desk);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateDeskTest()
        {
            var desk = new DeskModel("Доска для теста обновленная", "доска для тестирования сервиса после обновления", false, new string[] { "Новая", "доска", "обновленная" });
            desk.ProjectId = 2;
            desk.AdminId = 1;
            desk.Id = 5;
            var result = _service.UpdateDesk(_token, desk);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteDeskByIdTest()
        {
            var result = _service.DeleteDesk(_token, 5);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}