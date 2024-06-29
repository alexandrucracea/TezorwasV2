using CommunityToolkit.Maui.Views;

namespace TezorwasV2.View;

public partial class AddReceiptItemPopup : Popup
{
    public readonly string ProductName;
    public readonly string WhatToRecycle;
    public AddReceiptItemPopup()
    {
        InitializeComponent();
    }
    public AddReceiptItemPopup(string productName, string whatToRecycle)
    {
        InitializeComponent();
        ProductName = productName;
        WhatToRecycle = whatToRecycle;
        BindingContext = this;

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