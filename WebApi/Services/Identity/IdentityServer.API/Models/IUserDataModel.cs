using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Models
{
    public interface IUserDataModel
    {
        Task<List<User>> GetUsersListAsync();
        Task<User> GetUserAsync(string username);
        Task InsertUserAsync(User item);
        Task UpdateUserAsync(User item);
        Task DeleteUserAsync(string username);
    }
}
