using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Meals.API.Domains;
using Newtonsoft.Json;
using Xunit;

namespace Meals.API.IntegrationTests.Controller
{
    public class MealsControllerIntegrationTest : IntegrationTestBase
    {
        [Fact]
        public async Task AddMeal_NullItem_ShouldReturnBadRequest()
        {
            // Arrange
            var content = new StringContent(JsonConvert.SerializeObject(null));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await Client.PostAsync(string.Join("/", ServerBaseAddress, "api/meals/add-meal"), content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddMeal_ValidItem_ShouldAddItem()
        {
            // Arrange
            var meal = new Meal
            {
                Type = MealType.Breakfast,
                DateTime = DateTime.Now
            };
            meal.Foods.Add(new MealFood { });

            var content = new StringContent(JsonConvert.SerializeObject(meal));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await Client.PostAsync(string.Join("/", ServerBaseAddress, "api/meals/add-meal"), content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
