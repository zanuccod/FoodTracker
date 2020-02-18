using System.Collections.Generic;
using System.Threading.Tasks;
using Meals.API.Domains;

namespace Meals.API.Services
{
    public interface IMealService
    {
        Task<ICollection<Meal>> FindAllAsync();
        Task<Meal> FindAsync(uint id);
        Task InsertAsync(Meal item);
        Task UpdateAsync(Meal item);
        Task DeleteAsync(uint id);
    }
}
