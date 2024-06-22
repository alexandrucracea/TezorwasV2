

using CommunityToolkit.Mvvm.Input;
using TezorwasV2.View.AppPages;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class SettingsViewModel
    {
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
    }
}
