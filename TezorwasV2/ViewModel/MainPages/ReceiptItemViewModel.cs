using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;
using TezorwasV2.View;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ReceiptItemViewModel : ObservableObject
    {
        public ReceiptModel ReceiptToShow { get; set; }
        [ObservableProperty] public string receiptTitle;
        [ObservableProperty] public DateTime receiptInputDate;
        [ObservableProperty] public DateTime receiptCompletionDate;
        [ObservableProperty] public bool isCompleted;
        public ObservableCollection<ReceiptItemModel> ReceiptItemsUnrecycled { get; } = new ObservableCollection<ReceiptItemModel>();
        public ObservableCollection<ReceiptItemModel> ReceiptItemsRecycled { get; } = new ObservableCollection<ReceiptItemModel>();
        [ObservableProperty] public bool allReceiptItemsUnRecycled = true;
        [ObservableProperty] public bool itemsAreRecycled = false;
        [ObservableProperty] public int availableReceiptItems;
        [ObservableProperty] public int recycledReceiptItems;
        [ObservableProperty] public int availableXpReceipt;
        [ObservableProperty] public int actualXpGotFromReceipt;



        private bool pageIsInitializing = true;


        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;

        public ReceiptItemViewModel(IProfileService profileService, IGlobalContext globalContext)
        {
            _profileService = profileService;
            _globalContext = globalContext;
        }

        public void PopulateReceiptTaskList()
        {
            ReceiptItemsUnrecycled.Clear();
            ReceiptItemsRecycled.Clear();
            if (ReceiptToShow is not null)
            {
                ReceiptTitle = "Receipt_" + ReceiptToShow.CreationDate.ToString("dd/MM/yyyy");
                ReceiptInputDate = ReceiptToShow.CreationDate.Date;
                ReceiptCompletionDate = ReceiptToShow.CompletionDate.Date;
                foreach (var receiptItem in ReceiptToShow.ReceiptItems)
                {
                    if (!receiptItem.IsRecycled)
                    {
                        ReceiptItemsUnrecycled.Add(receiptItem);
                        AvailableXpReceipt += receiptItem.XpEarned;
                    }
                    else
                    {
                        ReceiptItemsRecycled.Add(receiptItem);
                        ActualXpGotFromReceipt += receiptItem.XpEarned;
                    }
                }

                AvailableReceiptItems = ReceiptItemsUnrecycled.Count + ReceiptItemsRecycled.Count;
                RecycledReceiptItems = ReceiptItemsRecycled.Count;
                AvailableXpReceipt = AvailableXpReceipt + ActualXpGotFromReceipt;
            }
            pageIsInitializing = false;
        }

        [RelayCommand]
        public async Task RecycleReceiptItem()
        {
            int recycledItemsCounter = 0;
            if (!pageIsInitializing)
            {
                foreach (var receiptItem in ReceiptItemsUnrecycled)
                {
                    if (!receiptItem.IsRecycled)
                    {
                        receiptItem.IsRecycled = true;
                        recycledItemsCounter++;
                        ItemsAreRecycled = true;

                        if (recycledItemsCounter == AvailableReceiptItems)
                        {
                            AllReceiptItemsUnRecycled = false;
                        }

                        ReceiptItemsRecycled.Add(receiptItem);
                        ReceiptItemsUnrecycled.Remove(receiptItem);

                        await UpdateReceiptUnrecycledItems();
                        return;
                    }
                }



            }
            return;
        }

        private async Task UpdateReceiptUnrecycledItems()
        {
            ProfileDto profileToUpdate = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            if (profileToUpdate is not null)
            {
                foreach (var receipt in profileToUpdate.Receipts)
                {
                    if (receipt.Id.Equals(ReceiptToShow.Id))
                    {
                        receipt.ReceiptItems.Clear();
                        foreach (var receiptItem in ReceiptItemsUnrecycled)
                        {
                            receipt.ReceiptItems.Add(receiptItem);
                        }
                        foreach (var receiptItem in ReceiptItemsRecycled)
                        {
                            receipt.ReceiptItems.Add(receiptItem);
                        }
                    }
                }
            }

            await _profileService.UpdateAProfile(profileToUpdate, _globalContext.UserToken);
        }
        public async Task AddProductToReceipt(AddProductToReceiptDto productToAdd)
        {
            Random random = new Random();
            int xpForProduct = random.Next(1,11);
            ReceiptItemsUnrecycled.Add(new ReceiptItemModel
            {
                Name = productToAdd.ProductName.ToUpper() + ": recycle: " + productToAdd.WhatToRecycle,
                CreationDate = DateTime.Now,
                CompletionDate = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                IsRecycled = false,
                XpEarned = xpForProduct
            });

            await UpdateReceiptUnrecycledItems();
        }
    }
}
