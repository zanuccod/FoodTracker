using System;
using Microsoft.Extensions.DependencyInjection;
using Common.Services.RequestProvider;
using Common.Services.Settings;
using Common.Services.Identity;

namespace Common.Helpers
{
    public class ServicesContainer
    {
        private readonly IServiceCollection _instance;
        public readonly IServiceProvider Provider;

        public ServicesContainer()
        {
            _instance = new ServiceCollection();

            AddServicesCollection();

            Provider = _instance.BuildServiceProvider();
        }

        private void AddServicesCollection()
        {
            _instance.AddSingleton<IRequestProvider, RequestProvider>();
            _instance.AddSingleton<ISettingsService, SettingsService>();
            _instance.AddSingleton<IIdentityService, IdentityService>();
        }
    }
}
