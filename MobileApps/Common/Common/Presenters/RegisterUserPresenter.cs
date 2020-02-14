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
    public class RegisterUserPresenter : BaseViewPresenter, IDisposable
    {
        private readonly IRegisterUserView _view;
        private readonly IIdentityService _identityService;

        public string Username { get; set; }
        public string Password { get; set; }
        public Command RegisterCommand { get; }

        public RegisterUserPresenter(IRegisterUserView view, IIdentityService identityService = null)
        {
            _view = view;

            _identityService = identityService ?? ServiceLocator.Provider.GetService<IIdentityService>();

            RegisterCommand = new Command(async () => await RegisterNewUser());
        }

        private async Task RegisterNewUser()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                if (string.IsNullOrEmpty(Username))
                {
                    _view.ShowPopupMessage("Insert valid username");
                    return;
                }

                if (string.IsNullOrEmpty(Password))
                {
                    _view.ShowPopupMessage("Insert valid password");
                    return;
                }

                await _identityService.RegisterNewUser(Username, Password).ConfigureAwait(true);
                _view.GoToLoginPage();
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

        public void Dispose()
        {

        }
    }
}
