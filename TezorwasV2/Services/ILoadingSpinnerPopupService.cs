

using TezorwasV2.View;

namespace TezorwasV2.Services
{
    public interface ILoadingSpinnerPopupService
    {
        void ShowPopup(LoadingSpinnerPopup popup);
        void ClosePopup(LoadingSpinnerPopup popup);
    }
}
