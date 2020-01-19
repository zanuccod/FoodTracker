using IdentityModel;
using IdentityServer.API.Domains;

namespace IdentityServer.API.IntegrationTest
{
    public static class PredefinedData
    {
        private static readonly string password = "demo";

        public static readonly User[] Profiles = {
            new User { Username = "demo", Password = password.ToSha256() },
            new User { Username = "tester@test.com", Password = password },
            new User { Username = "author@test.com", Password = password }
        };
    }
}
