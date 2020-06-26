using System;
using System.Threading.Tasks;
using Meals.API.Domains;
using Meals.API.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Meals.API.Tests.Models
{
    [TestFixture]
    public class EfMealsDataModelTest
    {
        private EfMealsDataModel dataModel;

        [SetUp]
        public void BeforeEachTest()
        {
            var options = new DbContextOptionsBuilder()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
            dataModel = new EfMealsDataModel(options);
        }

        [Test]
        public async Task InsertAsync()
        {
            // Arrange
            var item = CreateMockMeal();
            item.Foods.Add(CreateMockFood());

            // Act
            var entity = await dataModel.InsertAsync(item).ConfigureAwait(true);

            // because id was defined on insert operation 
            item.Id = entity.Id;

            // Assert
            Assert.AreEqual(1, dataModel.FindAllAsync().Result.Count);
            Assert.AreEqual(item, entity);
        }

        [Test]
        public async Task FindAllAsync()
        {
            // Arrange
            var itemCount = 10;
            Meal item;
            for (uint i = 1; i <= itemCount; i++)
            {
                item = CreateMockMeal(i);
                item.Foods.Add(CreateMockFood(i));
                await dataModel.InsertAsync(item).ConfigureAwait(true);
            }

            // Act
            var items = await dataModel.FindAllAsync().ConfigureAwait(true);

            // Assert
            Assert.AreEqual(itemCount, items.Count);
        }

        [Test]
        public async Task FindAsync()
        {
            // Arrange
            var item = CreateMockMeal();
            await dataModel.InsertAsync(item).ConfigureAwait(true);

            // Act
            var item2 = await dataModel.FindAsync(item.Id).ConfigureAwait(true);

            // Assert
            Assert.True(item.Equals(item2));
        }

        [Test]
        public async Task UpdateAsync()
        {
            // Arrange
            DateTime newTime = DateTime.Now;
            var item = CreateMockMeal();
            await dataModel.InsertAsync(item);

            // Act
            item.DateTime = newTime;
            var entity = await dataModel.UpdateAsync(item);

            var item2 = await dataModel.FindAsync(item.Id).ConfigureAwait(true);

            // Assert
            Assert.AreEqual(newTime, item2.DateTime);
            Assert.AreEqual(item, entity);
        }

        [Test]
        public async Task DeleteAsync()
        {
            // Arrange
            var item = CreateMockMeal();
            await dataModel.InsertAsync(item);

            // Act
            await dataModel.DeleteAsync(item.Id).ConfigureAwait(true);

            // Assert
            Assert.AreEqual(0, dataModel.FindAllAsync().Result.Count);
        }

        private Meal CreateMockMeal(uint id = 1)
        {
            return new Meal
            {
                Id = id,
                DateTime = DateTime.Now,
                Type = MealType.Breakfast
            };
        }

        private MealFood CreateMockFood(uint id = 1)
        {
            return new MealFood
            {
                Id = id,
                Quantity = 200,
                Unit = MeasureUnit.gram,
            };
        }
    }
}
