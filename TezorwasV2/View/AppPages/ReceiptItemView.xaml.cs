using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TezorwasV2.Model;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ReceiptItemView : ContentPage, IQueryAttributable
{
    ReceiptItemViewModel viewModel;
    public ReceiptItemView(ReceiptItemViewModel receiptItemViewModel)
    {
        InitializeComponent();
        viewModel = receiptItemViewModel;
        BindingContext = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ReceiptToShow"))
        {
            var receipt = query["ReceiptToShow"] as ReceiptModel;
            viewModel.ReceiptToShow = receipt;
            //viewModel.PopulateReceiptTaskList();
        }
    }
    protected override void OnAppearing()
    {
        viewModel.PopulateReceiptTaskList();

        itemsCollectionView.ItemsSource = viewModel.ReceiptItemsUnrecycled;
        RecycledItems.ItemsSource = viewModel.ReceiptItemsRecycled;

        #region StatusBar
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("037171"),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
        #endregion

    }

    private void SfCheckBox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        if (viewModel.RecycleReceiptItemCommand.CanExecute(default))
        {
            viewModel.RecycleReceiptItemCommand.Execute(default);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var popup = new AddReceiptItemPopup();
        var productToAdd = await this.ShowPopupAsync(popup);

        await viewModel.AddProductToReceipt(productToAdd as AddProductToReceiptDto);
    }
}