using System;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Domains;
using Common.Models;

namespace Common.Services.Identity
{
    public interface IIdentityService
    {
        Task<UserToken> GetTokenAsync(string username, string password);
        Task<User> RegisterNewUser(string username, string password);
    }
}