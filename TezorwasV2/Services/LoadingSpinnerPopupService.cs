using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezorwasV2.View;

namespace TezorwasV2.Services
{
    public class LoadingSpinnerPopupService : ILoadingSpinnerPopupService
    {
        public void ClosePopup(LoadingSpinnerPopup popup)
        {
           popup.Close();

        }

        public void ShowPopup(LoadingSpinnerPopup popup)
        {
            Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
            page.ShowPopup(popup);
        }
    }
}
