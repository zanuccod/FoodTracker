using System.Collections.Generic;
using System.Threading.Tasks;
using Meals.API.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Meals.API.Models
{
    public class EfMealsDataModel : EfDataContext, IMealDataModel
    {
        private readonly DbContextOptions _options;

        #region Constructors

        public EfMealsDataModel()
        {
            _options = new DbContextOptionsBuilder().Options;
        }

        public EfMealsDataModel(DbContextOptions options)
            : base(options)
        {
            _options = options;
        }

        #endregion

        public async Task DeleteAsync(uint id)
        {
            using var db = new EfMealsDataModel(_options);
            var item = await db.Meals.FindAsync(id);
            db.Meals.Remove(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<ICollection<Meal>> FindAllAsync()
        {
            using var db = new EfMealsDataModel(_options);
            return await db.Meals.ToListAsync().ConfigureAwait(true);
        }

        public async Task<Meal> FindAsync(uint id)
        {
            using var db = new EfMealsDataModel(_options);
            return await db.Meals.FindAsync(id).ConfigureAwait(true);
        }

        public async Task<Meal> InsertAsync(Meal item)
        {
            using var db = new EfMealsDataModel(_options);
            var entityEntry = await db.Meals.AddAsync(item);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return entityEntry.Entity;
        }

        public async Task<Meal> UpdateAsync(Meal item)
        {
            using var db = new EfMealsDataModel(_options);
            var entityEntry = db.Meals.Update(item);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return entityEntry.Entity;
        }
    }
}
