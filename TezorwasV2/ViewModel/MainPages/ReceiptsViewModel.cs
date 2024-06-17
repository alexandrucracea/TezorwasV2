using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.Model;
using TezorwasV2.Helpers;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ReceiptsViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        private readonly IProfileService _profileService;

        [ObservableProperty] private string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
        [ObservableProperty] private bool isCalendarShown;
        [ObservableProperty] private string showCalendarBtnText;

        public ObservableCollection<ReceiptModel> Receipts { get; set; } = new ObservableCollection<ReceiptModel>();

        public ReceiptsViewModel(IGlobalContext globalContext, IProfileService profileService)
        {
            IsCalendarShown = false;
            ShowCalendarBtnText = "Calendar";
            _globalContext = globalContext;
            _profileService = profileService;
        }

        [RelayCommand]
        public void ChangeCalendarVisibility()
        {
            IsCalendarShown = !IsCalendarShown;
            //ShowCalendarBtnText = IsCalendarShown ? "Hide" : "Show";
        }

        public async Task GetAllProfileReceipts()
        {
            var profile = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);

            foreach (var receipt in profile.Receipts)
            {
                Receipts.Add(new ReceiptModel
                {
                    CompletionDate = receipt.CompletionDate,
                    CreationDate = receipt.CreationDate,
                    Id = receipt.Id,
                    ReceiptItems = receipt.ReceiptItems,
                });
            }
        }
    }


}
