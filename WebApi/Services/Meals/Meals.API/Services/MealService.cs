using System.Collections.Generic;
using System.Threading.Tasks;
using Meals.API.Domains;
using Meals.API.Models;
using Microsoft.Extensions.Logging;

namespace Meals.API.Services
{
    public class MealService : IMealService
    {
        private readonly IMealDataModel _mealDataModel;
        private readonly ILogger<MealService> _logger;

        public MealService(IMealDataModel mealDataModel, ILogger<MealService> logger)
        {
            _mealDataModel = mealDataModel;
            _logger = logger;
        }

        public async Task DeleteAsync(uint id)
        {
            await _mealDataModel.DeleteAsync(id).ConfigureAwait(false);
            _logger.LogDebug($"Delete Meal with id <{id}>");
        }

        public async Task<ICollection<Meal>> FindAllAsync()
        {
            return await _mealDataModel.FindAllAsync().ConfigureAwait(false);
        }

        public async Task<Meal> FindAsync(uint id)
        {
            return await _mealDataModel.FindAsync(id).ConfigureAwait(false);
        }

        public async Task InsertAsync(Meal item)
        {
            await _mealDataModel.InsertAsync(item).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Meal item)
        {
            await _mealDataModel.UpdateAsync(item).ConfigureAwait(false);
        }
    }
}
