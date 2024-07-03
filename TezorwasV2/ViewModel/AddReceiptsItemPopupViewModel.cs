using CommunityToolkit.Mvvm.ComponentModel;
using TezorwasV2.View.AppPages;

namespace TezorwasV2.ViewModel
{
    public partial class AddReceiptsItemPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private ReceiptItemView.ItemDataDto dataObject;

        [ObservableProperty]public string productName;
        [ObservableProperty]public string whatToRecycle;

        public void SetData(ReceiptItemView.ItemDataDto data)
        {
            DataObject = data;
            ProductName = data.ProductName;
            WhatToRecycle = data.WhatToRecycle;
        }
    }
}
