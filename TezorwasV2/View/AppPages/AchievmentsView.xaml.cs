using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace TezorwasV2.View.AppPages;

public partial class AchievmentsView : ContentPage
{
	public AchievmentsView()
	{
		InitializeComponent();
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

}