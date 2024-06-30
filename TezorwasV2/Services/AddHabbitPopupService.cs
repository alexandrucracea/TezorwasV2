using CommunityToolkit.Maui.Views;
using TezorwasV2.View;

namespace TezorwasV2.Services
{
    public class AddHabbitPopupService : IAddHabbitPopupService
    {
        public void ClosePopup(AddHabbitPopup popup)
        {
            popup.Close();

        }

        public void ShowPopup(AddHabbitPopup popup)
        {
            Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
            page.ShowPopup(popup);
        }
    }
}
