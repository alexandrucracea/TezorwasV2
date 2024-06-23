using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class RecentActivityViewModel : ObservableObject
    {
        public ObservableCollection<ChartEntry> pieChartTasksEntries = new ObservableCollection<ChartEntry>();
        public ObservableCollection<ChartEntry> pieChartReceiptsEntries = new ObservableCollection<ChartEntry>();
        [ObservableProperty] private int numberOfCompletedTasks;
        [ObservableProperty] private int numberOfCompletedReceipts;

        private List<TaskModel> _tasks = new List<TaskModel>();
        private List<ReceiptModel> _receipts = new List<ReceiptModel>();

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;

        public RecentActivityViewModel(IProfileService profileService, IGlobalContext globalContext)
        {
            _profileService = profileService;
            _globalContext = globalContext;

            NumberOfCompletedTasks = 0;
            NumberOfCompletedReceipts = 0;
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


        public void GetCompletedTasksThisMonth()
        {
            foreach (var task in _tasks)
            {
                if (task.IsCompleted && (task.CompletionDate.Month == DateTime.Now.Month && task.CompletionDate.Year == DateTime.Now.Year))
                {
                    NumberOfCompletedTasks++;
                }
            }
        }

        public void GetCompletedReceiptsThisMonth()
        {

            foreach (var receipt in _receipts)
            {
                if (receipt.ReceiptItems.All(item => item.IsRecycled)) // Verifică dacă toate produsele au fost reciclate
                {
                    if ((receipt.CompletionDate.Month == DateTime.Now.Month && receipt.CompletionDate.Year == DateTime.Now.Year))
                    {
                        NumberOfCompletedReceipts++;
                    }
                }
            }
        }
        public async Task GenerateChartsEntries()
        {
            ProfileDto profileToRead = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);

            GetAllProfileReceipts(profileToRead);
            GetAllProfileTasks(profileToRead);

            GetCompletedTasksThisMonth();
            GetCompletedReceiptsThisMonth();

            #region PieCharts
            //Tasks
            pieChartTasksEntries.Add(new ChartEntry(_tasks.Count)
            {
                Label = "All",
                ValueLabel = _tasks.Count.ToString(),
                Color = SKColor.Parse("#036666")
            });
            pieChartTasksEntries.Add(new ChartEntry(NumberOfCompletedTasks)
            {
                Label = "Completed",
                ValueLabel = NumberOfCompletedTasks.ToString(),
                Color = SKColor.Parse("#358d8d")
            });


            //Receipts
            pieChartReceiptsEntries.Add(new ChartEntry(_receipts.Count)
            {
                Label = "All",
                ValueLabel = _receipts.Count.ToString(),
                Color = SKColor.Parse("#036666")
            });
            pieChartReceiptsEntries.Add(new ChartEntry(NumberOfCompletedReceipts)
            {
                Label = "Completed",
                ValueLabel = NumberOfCompletedReceipts.ToString(),
                Color = SKColor.Parse("#358d8d")
            });
            #endregion

        }
    }
}
