using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.Model;
using System;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ReceiptsViewModel : ObservableObject
    {
        [ObservableProperty] private string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
        [ObservableProperty] private bool isCalendarShown;
        [ObservableProperty] private string showCalendarBtnText = "Show Calendar";

        public ObservableCollection<ReceiptModel> Receipts { get; set; } = new ObservableCollection<ReceiptModel>();

        public ReceiptsViewModel()
        {
            IsCalendarShown = false;
            ShowCalendarBtnText = "Show Calendar";
        }

        [RelayCommand]
        public void ChangeCalendarVisibility()
        {
            IsCalendarShown = !IsCalendarShown;
            ShowCalendarBtnText = IsCalendarShown ? "Hide Calendar" : "Show Calendar";
        }
    }
}
