using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace TezorwasV2.View.AppPages;

public partial class ProfileView : ContentPage
{
	public ProfileView()
	{
		InitializeComponent();
	}

    private async void AchievmentsBtn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AchievmentsView), true);
    }

    protected override void OnAppearing()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("037171"),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
    }

    private async void HabbitsBtn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HabbitsView), true);
    }
}