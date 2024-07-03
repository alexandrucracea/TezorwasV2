using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.Buttons;
using System.Runtime.InteropServices.ComTypes;
using TezorwasV2.Model;
using TezorwasV2.ViewModel;
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
            var checkBox = sender as SfCheckBox;
            var receiptItemToRecycle = checkBox.BindingContext as ReceiptItemModel;
            viewModel.RecycleReceiptItemCommand.Execute(receiptItemToRecycle);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var popup = new AddReceiptItemPopup();
        var productToAdd = await this.ShowPopupAsync(popup);
        if(productToAdd is not null)
        {
            await viewModel.AddProductToReceipt(productToAdd as AddProductToReceiptDto);

        }
    }

    private async void ChangingName_Completed(object sender, EventArgs e)
    {
        if(ReceiptNameEntry.Text.Length > 0)
        {
            ReceiptNameEntry.Unfocus();

            await viewModel.ChangeReceiptName(ReceiptNameEntry.Text);
        }
    }

    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
       var item = sender as SwipeItem;
        if(item is null)
        {
            return;
        }

        var productToDelete = item.BindingContext as ReceiptItemModel;
        await viewModel.DeleteProduct(productToDelete);
    }

    private async void SwipeItem_Invoked_1(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null)
        {
            return;
        }

        var productToUpdate = item.BindingContext as ReceiptItemModel;

        var itemToTransfer = new ItemDataDto
        {
            ProductName = productToUpdate.Name,
            WhatToRecycle = "",
        };

        var viewModelPopup = new AddReceiptsItemPopupViewModel();
        viewModelPopup.SetData(itemToTransfer);

        var popup = new AddReceiptItemPopup(viewModelPopup);

        var productUpdated = await this.ShowPopupAsync(popup);

        if (productUpdated is not null)
        {
            await viewModel.DeleteProduct(productToUpdate);
            await viewModel.AddProductToReceipt(productUpdated as AddProductToReceiptDto);
        }
    }

    public class ItemDataDto
    {
        public string ProductName { get; set; }
        public string WhatToRecycle { get; set; }
    }
}