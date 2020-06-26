using System;
using Meals.API.Controllers;
using Meals.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meals.API.IntegrationTests.Controller
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureDatabaseServices(IServiceCollection services)
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            services.AddSingleton<IFoodDataModel>(new EfFoodDataModel(options));
            services.AddSingleton<IMealDataModel>(new EfMealsDataModel(options));

            // add this to prevent "404-NotFound" error when controller are on separate assembly
            // add an entry for every controller
            services.AddControllers().AddApplicationPart(typeof(MealsController).Assembly);
        }
    }
}
