

using CommunityToolkit.Mvvm.Input;
using TezorwasV2.Helpers;
using TezorwasV2.Services;
using TezorwasV2.View;
using TezorwasV2.View.AppPages;
using TezorwasV2.View.Authentication;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class SettingsViewModel
    {
        private readonly IGlobalContext _globalContext;
        private readonly ILogoutPopupService _logoutPopupService;
        private LogoutPopup logoutPopup = new LogoutPopup();
        public SettingsViewModel(IGlobalContext globalContext, ILogoutPopupService logoutPopupService)
        {
            _globalContext = globalContext;
            _logoutPopupService = logoutPopupService;
        }

        [RelayCommand]
        public async Task GoToEvolutionChartView()
        {
            await Shell.Current.GoToAsync(nameof(EvolutionChartView), true);
        }
        [RelayCommand]
        public async Task GoToRecentActivityChartView()
        {
            await Shell.Current.GoToAsync(nameof(RecentActivityView), true);
        }
        [RelayCommand]
        public async Task GoToTermsAndConditions()
        {
            await Shell.Current.GoToAsync(nameof(TermsAndConditionsView), true);
        }
        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        public async Task LogOut(dynamic continueLogOut)
        {
            if (continueLogOut)
            {
                _globalContext.ClearUserData();
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
            }    
        }
    }
}
