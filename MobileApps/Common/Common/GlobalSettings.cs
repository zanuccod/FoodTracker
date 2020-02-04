using System;

namespace Common
{
    public class GlobalSettings
    {
        private const string DefaultEndpoint = "http://localhost:5000"; // i.e.: "http://YOUR_IP" or "http://YOUR_DNS_NAME"
        
        private string _baseIdentityEndpoint;

        #region Constructors

        private GlobalSettings()
        {
            BaseIdentityEndpoint = DefaultEndpoint;
        }
        
        public static GlobalSettings Instance { get; } = new GlobalSettings();
        
        #endregion

        #region Public Properties

        public string BaseIdentityEndpoint
        {
            get => _baseIdentityEndpoint;
            set
            {
                _baseIdentityEndpoint = value;
                UpdateEndpoint(_baseIdentityEndpoint);
            }
        }
        public string ClientId => "xamarin";

        public string ClientSecret => "xamarin_secret";

        public string TokenEndpoint { get; private set; }
        public string RegisterNewUsersEndpoint { get; private set; }
        #endregion

        #region Private Methods

        private void UpdateEndpoint(string endpoint)
        {
            var connectBaseEndpoint = $"{endpoint}/connect";
            TokenEndpoint = $"{connectBaseEndpoint}/token";

            RegisterNewUsersEndpoint = $"{endpoint}/api/account/register-user";

            var baseUri = ExtractBaseUri(endpoint);
        }
        private string ExtractBaseUri(string endpoint)
        {
            var uri = new Uri(endpoint);
            var baseUri = uri.GetLeftPart(System.UriPartial.Authority);

            return baseUri;
        }
        
        #endregion
    }
}