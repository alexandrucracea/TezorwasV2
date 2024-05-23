using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace TezorwasV2.View.AppPages;

public partial class ArticlesView : ContentPage
{
	public ArticlesView()
	{
		InitializeComponent();
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
}