using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Meals.API.IntegrationTests.Controller
{
    public class IntegrationTestBase : IDisposable
    {
        private readonly TestServer server;
        protected readonly HttpClient Client;

        protected const string ServerBaseAddress = "https://localhost:5001";

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
    }
}
