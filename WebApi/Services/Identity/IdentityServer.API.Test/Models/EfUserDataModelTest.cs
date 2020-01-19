using System;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using IdentityServer.API.Models;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Test.Models
{
    [TestFixture]
    public class EfUserDataModelTest
    {
        private EfUserDataModel dataModel;

        [SetUp]
        public void BeforeEachTest()
        {
            var options = new DbContextOptionsBuilder()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
            dataModel = new EfUserDataModel(options);
        }

        [Test]
        public async Task InsertUser()
        {
            // Arrange
            var item = new User { Username = "username_test", Password = "password_test" };

            // Act
            await dataModel.InsertUserAsync(item).ConfigureAwait(true);

            // Assert
            Assert.AreEqual(1, dataModel.GetUsersListAsync().Result.Count);
        }

        [Test]
        public async Task GetAllUsers()
        {
            // Arrange
            var itemCount = 10;
            for (var i = 0; i < itemCount; i++)
            {
                await dataModel.InsertUserAsync(new User { Username = "username_test_" + i, Password = "password_test" });
            }

            // Act
            var items = await dataModel.GetUsersListAsync().ConfigureAwait(true);

            // Assert
            Assert.AreEqual(itemCount, items.Count);
        }

        [Test]
        public async Task GetUser()
        {
            // Arrange
            var item = new User { Username = "username_test", Password = "password_test" };
            await dataModel.InsertUserAsync(item);

            // Act
            var item2 = await dataModel.GetUserAsync(item.Username).ConfigureAwait(true);

            // Assert
            Assert.True(item.Equals(item2));
        }

        [Test]
        public async Task UpdateUser()
        {
            // Arrange
            const string newPassword = "password_test_1";

            var item = new User { Username = "username_test", Password = "password_test" };
            await dataModel.InsertUserAsync(item);

            // Act
            item.Password = newPassword;
            await dataModel.UpdateUserAsync(item);

            var item2 = await dataModel.GetUserAsync(item.Username).ConfigureAwait(true);

            // Assert
            Assert.AreEqual(newPassword, item2.Password);
        }

        [Test]
        public async Task DeleteUser()
        {
            // Arrange
            var item = new User { Username = "username_test", Password = "password_test" };
            await dataModel.InsertUserAsync(item);

            // Act
            await dataModel.DeleteUserAsync(item.Username).ConfigureAwait(true);

            // Assert
            Assert.AreEqual(0, dataModel.GetUsersListAsync().Result.Count);
        }
    }
}
