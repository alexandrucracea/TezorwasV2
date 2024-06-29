using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.Model;
using TezorwasV2.Helpers;
using TezorwasV2.Services;
using TezorwasV2.View.AppPages;
using TezorwasV2.View;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ReceiptsViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        private readonly IProfileService _profileService;
        private readonly ILoadingSpinnerPopupService _loadingSpinnerPopupService;
        private LoadingSpinnerPopup _loadingPopup = new LoadingSpinnerPopup();

        [ObservableProperty] private string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
        [ObservableProperty] private bool isCalendarShown;
        [ObservableProperty] private string showCalendarBtnText;
        [ObservableProperty] private bool receiptsCompleted;
        [ObservableProperty] private bool receiptsAvailable;

        public ObservableCollection<ReceiptModel> Receipts { get; set; } = new ObservableCollection<ReceiptModel>();

        public ReceiptsViewModel(IGlobalContext globalContext, IProfileService profileService, ILoadingSpinnerPopupService loadingSpinnerPopup)
        {
            IsCalendarShown = false;
            ShowCalendarBtnText = "Calendar";
            _globalContext = globalContext;
            _profileService = profileService;
            ReceiptsAvailable = true;
            ReceiptsCompleted = false;
            _loadingSpinnerPopupService = loadingSpinnerPopup;
        }

        [RelayCommand]
        public void ChangeCalendarVisibility()
        {
            IsCalendarShown = !IsCalendarShown;
        }

        public async Task GetAllProfileReceipts()
        {
            Receipts.Clear();
            _loadingPopup = new LoadingSpinnerPopup();
            _loadingSpinnerPopupService.ShowPopup(_loadingPopup);
            var profile = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);


            foreach (var receipt in profile.Receipts)
            {
                Receipts.Add(new ReceiptModel
                {
                    CompletionDate = receipt.CompletionDate,
                    CreationDate = receipt.CreationDate,
                    Name = receipt.Name,
                    Id = receipt.Id,
                    ReceiptItems = receipt.ReceiptItems,
                    BackgroundColor = receipt.BackgroundColor,
                });
            }
            _loadingSpinnerPopupService.ClosePopup(_loadingPopup);
        }
        [RelayCommand]
        public async Task GoToReceipt(dynamic receiptToTransfer)
        {
            var navigationParameters = new Dictionary<string, dynamic>
            {
                { "ReceiptToShow",receiptToTransfer},
            };
            await Shell.Current.GoToAsync(nameof(ReceiptItemView), true, navigationParameters);
        }

        [RelayCommand]
        public void FilterData(dynamic selectedDate)
        {
            List<ReceiptModel> filteredReceipts = new List<ReceiptModel>();
            if (Receipts is not null)
            {
                //bool recycleValueToCheck = ReceiptsAvailable ? ReceiptsAvailable : ReceiptsCompleted;
                foreach (var receipt in Receipts)
                {
                    if (ReceiptsCompleted)
                    {
                        if (receipt.CreationDate.Date == selectedDate && CheckIfReceiptIsCompleted(receipt) == true)
                        {
                            filteredReceipts.Add(receipt);
                        }
                    }
                    if (!ReceiptsCompleted)
                    {
                        if (receipt.CreationDate.Date == selectedDate && CheckIfReceiptIsCompleted(receipt) == false)
                        {
                            filteredReceipts.Add(receipt);
                        }
                    }

                }
                Receipts.Clear();
                foreach (var receipt in filteredReceipts)
                {
                    Receipts.Add(receipt);
                }
            }
        }
        [RelayCommand]
        public void UpdateReceiptsStatus()
        {
            if (ReceiptsAvailable)
            {
                ReceiptsCompleted = false;
            }
            else
            {
                ReceiptsCompleted = true;
            }
        }
        private bool CheckIfReceiptIsCompleted(ReceiptModel receipt)
        {
            bool receiptIsRecycled = true;
            foreach (var item in receipt.ReceiptItems)
            {
                if (!item.IsRecycled)
                {
                    receiptIsRecycled = false;
                    break;
                }
            }
            return receiptIsRecycled;
        }
    }


}
