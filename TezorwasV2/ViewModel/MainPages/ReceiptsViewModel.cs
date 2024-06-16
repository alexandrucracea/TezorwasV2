using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TezorwasV2.Model;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ReceiptsViewModel : ObservableObject
    {
        public ObservableCollection<ReceiptModel> Receipts { get; set; } = new ObservableCollection<ReceiptModel>();
    }
}
