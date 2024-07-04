using CommunityToolkit.Maui.Views;
using TezorwasV2.ViewModel;

namespace TezorwasV2.View;

public partial class AddReceiptItemPopup : Popup
{
    public readonly AddReceiptsItemPopupViewModel _viewModel;
    public AddReceiptItemPopup()
    {
        InitializeComponent();
    }
    public AddReceiptItemPopup(AddReceiptsItemPopupViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void SaveProduct(object sender, EventArgs e)
    {
        await CloseAsync(new AddProductToReceiptDto
        {
            ProductName = ProductNameEntry.Text,
            WhatToRecycle = WhatToRecycleEntry.Text
        });
    }
}

public class AddProductToReceiptDto
{
    public string ProductName { get;set;}
    public string WhatToRecycle { get;set;}
}