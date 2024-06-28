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
        public ObservableCollection<ChartEntry> lineChartTasksEntries = new ObservableCollection<ChartEntry>();
        public ObservableCollection<ChartEntry> lineChartReceiptsEntries = new ObservableCollection<ChartEntry>();
        [ObservableProperty] private int numberOfTasks;
        [ObservableProperty] private int numberOfReceipts;

        private List<TaskModel> _tasks = new List<TaskModel>();
        private List<ReceiptModel> _receipts = new List<ReceiptModel>();

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;

        private Dictionary<string, int> _completedTasksPerMonth = new Dictionary<string, int>();
        private Dictionary<string, int> _receiptsPerMonth = new Dictionary<string, int>();



        public EvolutionChartViewModel(IProfileService profileService, IGlobalContext globalContext)
        {
            _profileService = profileService;
            _globalContext = globalContext;
        }
        public void GetAllProfileReceipts(ProfileDto profileToRead)
        {
            _receipts = new List<ReceiptModel>();
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
            _tasks = new List<TaskModel>();
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

        public void GetCompletedTasksPerMonth()
        {
            // Inițializează dicționarul cu toate lunile setate la 0
            _completedTasksPerMonth = new Dictionary<string, int>
                                        {
                                            { "January", 0 },
                                            { "February", 0 },
                                            { "March", 0 },
                                            { "April", 0 },
                                            { "May", 0 },
                                            { "June", 0 },
                                            { "July", 0 },
                                            { "August", 0 },
                                            { "September", 0 },
                                            { "October", 0 },
                                            { "November", 0 },
                                            { "December", 0 }
                                        };

            foreach (var task in _tasks)
            {
                if (task.IsCompleted) // Verifică dacă task-ul este completat
                {
                    var monthName = task.CompletionDate.ToString("MMMM"); // Obține denumirea lunii

                    if (_completedTasksPerMonth.ContainsKey(monthName))
                    {
                        _completedTasksPerMonth[monthName]++;
                    }
                }
            }
        }
        public void GetRecycledReceiptsPerMonth()
        {
            _receiptsPerMonth = new Dictionary<string, int>
                                {
                                    { "January", 0 },
                                    { "February", 0 },
                                    { "March", 0 },
                                    { "April", 0 },
                                    { "May", 0 },
                                    { "June", 0 },
                                    { "July", 0 },
                                    { "August", 0 },
                                    { "September", 0 },
                                    { "October", 0 },
                                    { "November", 0 },
                                    { "December", 0 }
                                };

            foreach (var receipt in _receipts)
            {
                if (receipt.ReceiptItems.All(item => item.IsRecycled)) // Verifică dacă toate produsele au fost reciclate
                {
                    var monthName = receipt.CreationDate.ToString("MMMM"); // Obține denumirea lunii

                    if (_receiptsPerMonth.ContainsKey(monthName))
                    {
                        _receiptsPerMonth[monthName]++;
                    }
                }
            }
        }



        public async Task GenerateChartsEntries()
        {
            ProfileDto profileToRead = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);

            GetAllProfileReceipts(profileToRead);
            GetAllProfileTasks(profileToRead);


            NumberOfTasks = _tasks.Count;
            NumberOfReceipts = _receipts.Count;

            #region BarChart
            barChartComparisonEntries.Clear();
            barChartComparisonEntries.Add(new ChartEntry(NumberOfReceipts)
            {
                Label = "Receipts",
                ValueLabel = NumberOfReceipts.ToString(),
                Color = SKColor.Parse("#036666")
            });
            barChartComparisonEntries.Add(new ChartEntry(NumberOfTasks)
            {
                Label = "Tasks",
                ValueLabel = NumberOfTasks.ToString(),
                Color = SKColor.Parse("#358d8d")
            });
            #endregion

            #region LineCharts
            SKColor startColor = SKColor.Parse("#b3d4d4");
            SKColor endColor = SKColor.Parse("#1c4643");

            GetCompletedTasksPerMonth();
            lineChartTasksEntries = new ObservableCollection<ChartEntry>();

            foreach (var kvp in _completedTasksPerMonth)
            {
                string month = kvp.Key;
                int completedTasks = kvp.Value;
                var colorToAdd = GenerateRandomColor(startColor, endColor);
                lineChartTasksEntries.Add(new ChartEntry(completedTasks)
                {
                    Label = month,
                    ValueLabel = completedTasks.ToString(),
                    Color = colorToAdd,
                    OtherColor = colorToAdd.WithAlpha(100)
                });
            }



            GetRecycledReceiptsPerMonth();
            lineChartReceiptsEntries  = new ObservableCollection<ChartEntry>();
            foreach (var kvp in _receiptsPerMonth)
            {
                string month = kvp.Key;
                int completedTasks = kvp.Value;
                var colorToAdd = GenerateRandomColor(startColor, endColor);
                lineChartReceiptsEntries.Add(new ChartEntry(completedTasks)
                {
                    Label = month,
                    ValueLabel = completedTasks.ToString(),
                    Color = colorToAdd,
                    OtherColor = colorToAdd.WithAlpha(100)
                });
            }
            #endregion
        }


        public static SKColor GenerateRandomColor(SKColor startColor, SKColor endColor)
        {
            Random random = new Random();
            float t = (float)random.NextDouble(); // Generare aleatoare a factorului de interpolare între 0 și 1

            byte r = (byte)(startColor.Red + t * (endColor.Red - startColor.Red));
            byte g = (byte)(startColor.Green + t * (endColor.Green - startColor.Green));
            byte b = (byte)(startColor.Blue + t * (endColor.Blue - startColor.Blue));

            return new SKColor(r, g, b);
        }
    }
}

