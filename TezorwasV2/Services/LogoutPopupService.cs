using CommunityToolkit.Maui.Views;
using TezorwasV2.View;

namespace TezorwasV2.Services
{
    public class LogoutPopupService : ILogoutPopupService
    {
        public void ClosePopup(LogoutPopup popup)
        {
            popup.Close();

        }

        public void ShowPopup(LogoutPopup popup)
        {
            Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
            page.ShowPopup(popup);
        }
    }
}
