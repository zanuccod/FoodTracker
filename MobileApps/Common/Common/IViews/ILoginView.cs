namespace Common.IViews
{
    public interface ILoginView
    {
        void ShowPopupMessage(string message);
        void GoToMainPage();
        void GoToRegisterNewUserPage();
    }
}