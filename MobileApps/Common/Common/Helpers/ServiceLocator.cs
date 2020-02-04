using System;
using Common.Helpers;

namespace Common.Helper
{
    public static class ServiceLocator
    {
        private static readonly Lazy<ServicesContainer> instance = new Lazy<ServicesContainer>(() => new ServicesContainer());
        public static IServiceProvider Provider => instance.Value.Provider;
    }
}