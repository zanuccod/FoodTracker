using Android.App;
using Android.OS;
using Android.Widget;
using Common.IViews;
using Common.Presenters;
using FoodTracker.Droid.Activities;

namespace FoodTracker.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : Activity, ILoginView
    {
        private LoginViewPresenter _presenter;

        private EditText _textUsername, _textPassword;
        private TextView _btnRegisterNewUser;
        private Button _btnLogin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            _presenter = new LoginViewPresenter(this);

            _textUsername = FindViewById<EditText>(Resource.Id.activity_login_text_username);
            _textPassword = FindViewById<EditText>(Resource.Id.activity_login_text_password);

            _btnRegisterNewUser = FindViewById<TextView>(Resource.Id.activity_login_btn_register_new_user);

            _btnLogin = FindViewById<Button>(Resource.Id.activity_login_btn_login);

            _btnLogin.Click += BtnLogin_Click;
            _btnRegisterNewUser.Click += _btnRegisterNewUser_Click;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _btnLogin.Click -= BtnLogin_Click;
            _btnRegisterNewUser.Click -= _btnRegisterNewUser_Click;

            _presenter.Dispose();
        }

        private void BtnLogin_Click(object sender, System.EventArgs e)
        {
            _presenter.Username = _textUsername.Text;
            _presenter.Password = _textPassword.Text;

            _presenter.LoginCommand.Execute(null);
        }

        private void _btnRegisterNewUser_Click(object sender, System.EventArgs e)
        {
            _presenter.RegisterCommand.Execute(null);
        }

        public void GoToMainPage()
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }

        public void ShowPopupMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        public void GoToRegisterNewUserPage()
        {
            StartActivity(typeof(RegisterUserActivity));
            Finish();
        }
    }
}