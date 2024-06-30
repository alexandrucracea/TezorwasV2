using TezorwasV2.View;

namespace TezorwasV2.Services
{
    public interface IAddHabbitPopupService
    {
        void ClosePopup(AddHabbitPopup popup);
        void ShowPopup(AddHabbitPopup popup);
    }
}