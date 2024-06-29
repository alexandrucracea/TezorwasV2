using TezorwasV2.View;

namespace TezorwasV2.Services
{
    public interface ILogoutPopupService
    {
        void ClosePopup(LogoutPopup popup);
        void ShowPopup(LogoutPopup popup);
    }
}