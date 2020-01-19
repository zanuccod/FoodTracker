using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace IdentityServer.API.IntegrationTest.Controller
{
    public class IntegrationTestBase : IDisposable
    {
        private readonly TestServer server;
        protected readonly HttpClient Client;

        protected const string ServerBaseAddress = "https://localhost:5001";
        private readonly string requestTokenAddress = string.Join("/", ServerBaseAddress, "connect/token");

        protected IntegrationTestBase()
        {
            server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup>());
            Client = server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            server.Dispose();
        }

        protected async Task<TokenResponse> RequestToken(string username, string password, string scope)
        {
            return await Client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = requestTokenAddress,
                GrantType = GrantTypes.ResourceOwnerPassword.First(),

                ClientId = "test_ClientId",
                ClientSecret = "test_client_key",
                Scope = scope,

                UserName = username,
                Password = password.ToSha256()
            });
        }
    }
}
