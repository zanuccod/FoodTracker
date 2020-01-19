using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Models
{
    public class EfUserDataModel : EfDataContext, IUserDataModel
    {
        private readonly DbContextOptions options;

        #region Constructors

        public EfUserDataModel()
        {
            options = new DbContextOptionsBuilder().Options;
        }

        public EfUserDataModel(DbContextOptions options)
            :base(options)
        {
            this.options = options;
        }

        #endregion

        #region Public Users Methods

        public async Task<List<User>> GetUsersListAsync()
        {
            using var db = new EfDataContext(options);
            return await db.Users.ToListAsync().ConfigureAwait(true);
        }

        public async Task<User> GetUserAsync(string username)
        {
            using var db = new EfDataContext(options);
            return await db.Users.FindAsync(username).ConfigureAwait(true);
        }

        public async Task InsertUserAsync(User item)
        {
            using var db = new EfDataContext(options);
            await db.Users.AddAsync(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task UpdateUserAsync(User item)
        {
            using var db = new EfDataContext(options);
            db.Users.Update(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task DeleteUserAsync(string username)
        {
            using var db = new EfDataContext(options);
            var item = await db.Users.FindAsync(username);
            db.Users.Remove(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        #endregion
    }
}
