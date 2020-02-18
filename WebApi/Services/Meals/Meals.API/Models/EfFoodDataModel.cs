using System.Collections.Generic;
using System.Threading.Tasks;
using Meals.API.Domains;
using Microsoft.EntityFrameworkCore;

namespace Meals.API.Models
{
    public class EfFoodDataModel : EfDataContext, IFoodDataModel
    {
        private readonly DbContextOptions _options;

        #region Constructors

        public EfFoodDataModel()
        {
            _options = new DbContextOptionsBuilder().Options;
        }

        public EfFoodDataModel(DbContextOptions options)
            : base(options)
        {
            this._options = options;
        }

        #endregion

        public async Task DeleteAsync(uint id)
        {
            using var db = new EfFoodDataModel(_options);
            var item = await db.Foods.FindAsync(id);
            db.Foods.Remove(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<ICollection<Food>> FindAllAsync()
        {
            using var db = new EfFoodDataModel(_options);
            return await db.Foods.ToListAsync().ConfigureAwait(true);
        }

        public async Task<Food> FindAsync(uint id)
        {
            using var db = new EfFoodDataModel(_options);
            return await db.Foods.FindAsync(id).ConfigureAwait(true);
        }

        public async Task<Food> InsertAsync(Food item)
        {
            using var db = new EfFoodDataModel(_options);
            var entryEntity = await db.Foods.AddAsync(item);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return entryEntity.Entity;
        }

        public async Task<Food> UpdateAsync(Food item)
        {
            using var db = new EfFoodDataModel(_options);
            var entryEntity = db.Foods.Update(item);
            await db.SaveChangesAsync().ConfigureAwait(true);

            return entryEntity.Entity;
        }
    }
}
