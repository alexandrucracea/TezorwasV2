using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPagest
{
    public partial class EvolutionChartViewModel : ObservableObject
    {
        public ObservableCollection<ChartEntry> chartEntriesReceipts = new ObservableCollection<ChartEntry>();
        public ObservableCollection<ChartEntry> chartEntriesTasks = new ObservableCollection<ChartEntry>();
        public ObservableCollection<ChartEntry> barChartComparisonEntries = new ObservableCollection<ChartEntry>();

        private List<TaskModel> _tasks = new List<TaskModel>();
        private List<ReceiptModel> _receipts = new List<ReceiptModel>();

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;

        public EvolutionChartViewModel(IProfileService profileService, IGlobalContext globalContext)
        {
            _profileService = profileService;
            _globalContext = globalContext;
        }
        public void GetAllProfileReceipts(ProfileDto profileToRead)
        {
            foreach (var receipt in profileToRead.Receipts)
            {
                _receipts.Add(new ReceiptModel
                {
                    CompletionDate = receipt.CompletionDate,
                    CreationDate = receipt.CreationDate,
                    Id = receipt.Id,
                    ReceiptItems = receipt.ReceiptItems,
                });
            }
        }
        public void GetAllProfileTasks(ProfileDto profileToRead)
        { 
            foreach (var task in profileToRead.Tasks)
            {
                _tasks.Add(new TaskModel
                {
                    Name = task.Name,
                    CompletionDate = task.CompletionDate,
                    CreationDate = task.CreationDate,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    XpEarned = task.XpEarned
                });
            }
        }

     
        public async Task GenerateBarChartComparisonEntries()
        {
            ProfileDto profileToRead = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            GetAllProfileReceipts(profileToRead);
            GetAllProfileTasks(profileToRead);
            barChartComparisonEntries.Add(new ChartEntry(_receipts.Count)
            {
                Label = "Receipts",
                ValueLabel = _receipts.Count.ToString(),
                Color = SKColor.Parse("#2c3e50")
            });
            barChartComparisonEntries.Add(new ChartEntry(_tasks.Count)
            {
                Label = "Tasks",
                ValueLabel = _tasks.Count.ToString(),
                Color = SKColor.Parse("#2c3e50")
            });
        }
    }
}

