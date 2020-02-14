using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Domains;
using Common.Models;
using Common.Services.RequestProvider;
using Newtonsoft.Json;

namespace Common.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IRequestProvider _requestProvider;

        public IdentityService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        
        public async Task<UserToken> GetTokenAsync(string username, string password)
        {
            var data = $"grant_type=password&username={username}&password={password}";
            var token = await _requestProvider.PostAsync<UserToken>(GlobalSettings.Instance.TokenEndpoint, 
                data, 
                GlobalSettings.Instance.ClientId, 
                GlobalSettings.Instance.ClientSecret);
            return token;
        }

        public async Task<User> RegisterNewUser(string username, string password)
        {
            var user = new User { Username = username, Password = password };
            return await _requestProvider.PostAsync(GlobalSettings.Instance.RegisterNewUsersEndpoint, user).ConfigureAwait(true);
        }
    }
}