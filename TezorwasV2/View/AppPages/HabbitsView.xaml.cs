using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TezorwasV2.Model;
using TezorwasV2.ViewModel;
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

    private async void AddHabbit(object sender, EventArgs e)
    {
        var popup = new AddHabbitPopup();
        var habbitToAdd = await this.ShowPopupAsync(popup) as HabbitModel;

        if (habbitToAdd is not null)
        {
            await viewModel.AddHabbitToProfile(habbitToAdd);
        }
    }

    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null)
        {
            return;
        }

        var habbitToDelete = item.BindingContext as HabbitModel;
        await viewModel.DeleteHabbit(habbitToDelete);
    }

    private async void SwipeItem_Invoked_1(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null)
        {
            return;
        }

        var habbitToUpdate = item.BindingContext as HabbitModel;

        var itemToTransfer = new ReceiptDataDto
        {
            HabbitDescription = habbitToUpdate.Description,
            HabbitLevelOfWaste = habbitToUpdate.LevelOfWaste
        };

        var viewModelPopup = new AddHabbitPopupViewModel();
        viewModelPopup.SetData(itemToTransfer);

        var popup = new AddHabbitPopup(viewModelPopup);

        var habbitUpdated = await this.ShowPopupAsync(popup);

        if (habbitUpdated is not null)
        {
            await viewModel.DeleteHabbit(habbitToUpdate);
            await viewModel.AddHabbitToProfile(habbitUpdated as HabbitModel);
        }
    }

    public class ReceiptDataDto
    {
        public string HabbitDescription { get; set; }
        public double HabbitLevelOfWaste { get; set; }
    }
}