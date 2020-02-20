using System.Collections.Generic;
using System.Threading.Tasks;
using Meals.API.Controllers;
using Meals.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace Meals.API.Tests.Controllers
{
    [TestFixture]
    public class MealsControllerTest
    {
        private readonly Mock<IMealDataModel> mockMealDataModel;
        private readonly MealsController controller;

        public MealsControllerTest()
        {
            mockMealDataModel = new Mock<IMealDataModel>();
            var mealDataModel = mockMealDataModel.Object;

            controller = new MealsController(mealDataModel, new NullLogger<MealsController>());
        }

        [Test]
        public async Task GetMeal_ZeroAsId_ShouldReturnBadRequest()
        {
            // Act
            var result = await controller.GetMeal(0);

            // Assert
            Assert.AreEqual(typeof(BadRequestObjectResult), result.Result.GetType());
        }

        [Test]
        public async Task AddMeal_NullInput_ShouldReturnBadRequest()
        {
            // Act
            var result = await controller.AddMeal(null).ConfigureAwait(true);

            // Assert
            Assert.AreEqual(typeof(BadRequestObjectResult), result.Result.GetType());
        }

    }
}
