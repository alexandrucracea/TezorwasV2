using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class HabbitsView : ContentPage
{
    HabbitViewModel viewModel;
    public HabbitsView(HabbitViewModel habbitViewModel)
    {
        InitializeComponent();
        viewModel = habbitViewModel;
        BindingContext = viewModel;
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

        await viewModel.GetAllProfileHabbits();
        itemsCollectionView.ItemsSource = viewModel.ProfileHabbits;

        popup.Close();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }
}