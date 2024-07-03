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
    //public AddReceiptItemPopup(string productName, string whatToRecycle)
    //{
    //    InitializeComponent();

    //    //viewModel.ProductName = productName;
    //    //viewModel.WhatToRecycle = whatToRecycle;
    //    BindingContext = this;
    //    var x = 3;

    //}
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