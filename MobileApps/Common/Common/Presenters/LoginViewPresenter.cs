using System;
using System.Threading.Tasks;
using Common.Helper;
using Common.Helpers;
using Common.IViews;
using Common.Services.Identity;
using Common.Services.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Presenters
{
    public class LoginViewPresenter : BaseViewPresenter, IDisposable
    {
        private readonly ILoginView _view;
        private readonly IIdentityService _identityService;
        private readonly ISettingsService _settingsService;

        public string Username { get; set; }
        public string Password { get; set; }
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewPresenter(ILoginView view, IIdentityService identityService = null, ISettingsService settingsService = null)
        {
            _view = view;

            _identityService = identityService ?? ServiceLocator.Provider.GetService<IIdentityService>();
            _settingsService = settingsService ?? ServiceLocator.Provider.GetService<ISettingsService>();

            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(async () => await Register());
        }

        public void Dispose()
        {
            Dispose();
        }

        private async Task Login()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                if (string.IsNullOrEmpty(Username))
                {
                    _view.ShowPopupMessage("Insert valid username");
                }

                if (string.IsNullOrEmpty(Password))
                {
                    _view.ShowPopupMessage("Insert valid password");
                }

                var userToken = await _identityService.GetTokenAsync(Username, Password).ConfigureAwait(true);
                _settingsService.AuthAccessToken = userToken.AccessToken;
                _view.ShowPopupMessage($"Token request correctly.");

                _view.GoToMainPage();
            }
            catch (Exception ex)
            {
                _view.ShowPopupMessage($"Failed to proceed with the login operation: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Register()
        {
            try
            {
                await Task.Run(() => _view.GoToRegisterNewUserPage()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _view.ShowPopupMessage($"Failed to proceed with the login operation: {ex.Message}");
            }
        }
    }
}