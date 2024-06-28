using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.Core.Carousel;
using TezorwasV2.ViewModel;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ProfileView : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;

    public ProfileView(ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        _profileViewModel = profileViewModel;
        BindingContext = _profileViewModel;
    }

    private async void AchievmentsBtn_Clicked(object sender, EventArgs e)
    {
        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);

        await Shell.Current.GoToAsync(nameof(AchievmentssView), true);

        popup.Close();

    }

    protected override async void OnAppearing()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("037171"),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416 // Validate platform compatibility


        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);

        await _profileViewModel.InitializeProfile();

        popup.Close();
    }

    private async void HabbitsBtn_Clicked(object sender, EventArgs e)
    {
        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);

        await Shell.Current.GoToAsync(nameof(HabbitsView), true);

        popup.Close();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
    }
}