using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class SettingsMenu : ContentPage
{
    public SettingsViewModel viewModel;
    public SettingsMenu(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        viewModel = settingsViewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("#eff1f3"),
            StatusBarStyle = StatusBarStyle.DarkContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
        base.OnAppearing();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var popup = new LogoutPopup();
        bool continueLogOut = (bool)await this.ShowPopupAsync(popup);
        if (continueLogOut)
        {
            await viewModel.LogOut(continueLogOut);
        }
    }
}