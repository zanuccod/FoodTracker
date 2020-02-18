using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Meals.API.Models
{
    public interface IDataModelBase<T>
    {
        Task<ICollection<T>> FindAllAsync();
        Task<T> FindAsync(uint id);
        Task<T> InsertAsync(T item);
        Task<T> UpdateAsync(T item);
        Task DeleteAsync(uint id);
    }
}
