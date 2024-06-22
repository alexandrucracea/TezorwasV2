using CommunityToolkit.Maui.Views;

namespace TezorwasV2.View;

public partial class AddReceiptItemPopup : Popup
{
    public AddReceiptItemPopup()
    {
        InitializeComponent();
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