using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Common.Models;
using System.Net;
using TaskManager.Client.Models;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        private UsersRequestService _service;
        private AuthToken _token;

        public UsersRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new UsersRequestService();
        }

        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("admin", "qwerty123");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token);
            //Assert.IsNotNull(token.access_token);
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest = new UserModel("TestName", "TestSecondName",
                "Test@mail.ru", "12345678", UserStatus.User, "79504567812");

            var result = service.CreateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetAllUsersTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty123");
            var result = service.GetAllUsers(token);

            Console.WriteLine(result.Count);
            Assert.AreNotEqual(Array.Empty<UserModel>(), result.ToArray());
            //Assert.AreEqual(HttpStatusCode.OK, (List<UserModel>)result);
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty123");
            var result = service.DeleteUser(token, 5);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void CreateMultipleUsersTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest1 = new UserModel("Юля", "Филонова",
                "Юля@mail.ru", "12345678", UserStatus.User, "554466");
            UserModel userTest2 = new UserModel("Александра", "Викторовна",
                "Сандра@mail.ru", "12345678", UserStatus.User, "12345611");
            UserModel userTest3 = new UserModel("Валентина", "Александровна",
                "Валюша@mail.ru", "12345678", UserStatus.Editor, "777995515");

            List<UserModel> users = new List<UserModel>() { userTest1, userTest2, userTest3 };

            var result = service.CreateMultipleUsers(token, users);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateUsersTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest = new UserModel("Больше не тест", "и тут тоже",
                "ИдажеТУТ@mail.ru", "12345678", UserStatus.User, "79504567812");
            userTest.Id = 6;

            var result = service.UpdateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetProjectUserAdminTest()
        {
            var id = _service.GetProjectUserAdmin(_token, 4);
            Assert.AreEqual(null, id);
        }
    }
}