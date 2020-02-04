using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Common.IViews;
using Common.Presenters;

namespace FoodTracker.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class RegisterUserActivity : Activity, IRegisterUserView
    {
        private RegisterUserPresenter _presenter;
        private EditText _textUserName, _textPassword;
        private Button _btnRegister;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register_user);

            _textUserName = FindViewById<EditText>(Resource.Id.activity_register_user_text_username);
            _textPassword = FindViewById<EditText>(Resource.Id.activity_register_user_text_password);
            _btnRegister = FindViewById<Button>(Resource.Id.activity_register_user_btn_register);

            _presenter = new RegisterUserPresenter(this);

            _btnRegister.Click += _btnRegister_Click;
        }

        private void _btnRegister_Click(object sender, EventArgs e)
        {
            _presenter.Username = _textUserName.Text;
            _presenter.Password = _textPassword.Text;

            _presenter.RegisterCommand.Execute(null);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _btnRegister.Click -= _btnRegister_Click;
            _presenter.Dispose();
        }

        public void GoToLoginPage()
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        public void ShowPopupMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }
    }
}
