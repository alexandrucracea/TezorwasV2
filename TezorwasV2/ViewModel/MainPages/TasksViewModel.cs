using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class TasksViewModel : ObservableObject
    {
        public ObservableCollection<TaskModel> AvailableTasks { get; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<TaskModel> CompletedTasks { get; } = new ObservableCollection<TaskModel>();

        [ObservableProperty] public bool tasksAreCompleted = false;
        [ObservableProperty] public bool allTasksIncompleted = true;
        [ObservableProperty] public string noAvailableTasksMessage;
        [ObservableProperty] public string noCompletedTasksMessage;
        [ObservableProperty] public int availableTasksToday;
        [ObservableProperty] public int completedTasksToday;
        [ObservableProperty] public int availableXpToday;
        [ObservableProperty] public int actualXpGotToday;

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;
        private readonly IGptService _gptService;

        public TasksViewModel(IGlobalContext globalContext, IProfileService profileService, IGptService gptService)
        {
            _globalContext = globalContext;
            _profileService = profileService;
            _gptService = gptService;

            Task.Run(OnAppearing);

            NoCompletedTasksMessage = "You didn't complete any tasks today";

        }
        public async Task OnAppearing()
        {
            await PopulateTasks();
            if (CompletedTasks.Count == 0)
            {
                NoAvailableTasksMessage = "You don't have any available tasks";

            }
            else
            {
                NoAvailableTasksMessage = "You completed all the available tasks for today.";

            }

        }
        [RelayCommand]
        public async Task CompleteTask()
        {
            int completedTasksCounter = 0;
            foreach (var task in AvailableTasks)
            {

                if (!task.IsCompleted)
                {
                    task.IsCompleted = true;
                    completedTasksCounter++;
                    TasksAreCompleted = true;

                    if (completedTasksCounter == AvailableTasksToday)
                    {
                        AllTasksIncompleted = false;
                    }

                    CompletedTasks.Add(task);
                    AvailableTasks.Remove(task);

                    await UpdateProfilesCompletedTasks(task);
                    return;
                }
            }

            return;

        }
        private async Task PopulateTasks()
        {
            var allProfiles = await _profileService.GetAllProfiles(_globalContext.UserToken);
            var currentUserProfile = allProfiles.FirstOrDefault(profile => profile.PersonId.Equals(_globalContext.PersonId));
            if (currentUserProfile != null)
            {
                _globalContext.ProfileId = currentUserProfile.Id;
                foreach (TaskModel task in currentUserProfile.Tasks)
                {
                    if (!task.IsCompleted)
                    {
                        AvailableTasks.Add(task);
                        AvailableXpToday += task.XpEarned;
                    }
                    else
                    {
                        if (task.CompletionDate.Date == DateTime.Now.Date)
                        {
                            CompletedTasks.Add(task);
                            ActualXpGotToday += task.XpEarned;
                        }
                    }
                }
                double levelOfWaste = currentUserProfile.Habbits.FirstOrDefault().LevelOfWaste;

                //checking the there are any available tasks generated today
                DateTime todaysDate = DateTime.Now.Date;

                int tasksAvailableToday = AvailableTasks.Where(task => task.CreationDate.Date == todaysDate).Count();

                if (AvailableTasks.Count < 2 && tasksAvailableToday < 2)
                {
                    var tasksGenerated = await GenerateTasksWithGpt(levelOfWaste);

                    List<TaskModel> gptGeneratedTasks = new List<TaskModel>();
                    foreach (var task in tasksGenerated)
                    {
                        AvailableTasks.Add(new TaskModel
                        {
                            Name = "tasks_" + DateTime.Now.ToString(),
                            CreationDate = DateTime.Now,
                            Description = task.TaskDescription,
                            IsCompleted = false,
                            XpEarned = task.XpEarned,
                            CompletionDate = DateTime.Now //trebuie suprascris cand se completeaza taskul
                        });

                        gptGeneratedTasks.Add(new TaskModel
                        {
                            Name = "tasks_" + DateTime.Now.ToString(),
                            CreationDate = DateTime.Now,
                            Description = task.TaskDescription,
                            IsCompleted = false,
                            XpEarned = task.XpEarned,
                            CompletionDate = DateTime.Now //trebuie suprascris cand se completeaza taskul
                        });

                    }
                    await UpdateProfilesAvailableTasks(gptGeneratedTasks);
                }
            }
            AvailableTasksToday = AvailableTasks.Count + CompletedTasks.Count;
            CompletedTasksToday = CompletedTasks.Count;
        }
        private async Task UpdateProfilesAvailableTasks(dynamic generatedTasksByGpt)
        {
            ProfileDto profileToUpdate = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            foreach (var task in generatedTasksByGpt)
            {
                profileToUpdate.Tasks.Add(task);
            }
            await _profileService.UpdateAProfile(profileToUpdate, _globalContext.UserToken);
        }
        private async Task UpdateProfilesCompletedTasks(TaskModel task)
        {
            task.CompletionDate = DateTime.Now;

            ProfileDto profileToUpdate = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            profileToUpdate.Tasks = AvailableTasks.Concat(CompletedTasks).ToList();
            profileToUpdate.Xp += task.XpEarned;
            profileToUpdate.Level = UpdateLevelIfNecessary(profileToUpdate);


            await _profileService.UpdateAProfile(profileToUpdate, _globalContext.UserToken);

            CompletedTasksToday = CompletedTasks.Count;
            AvailableTasksToday += CompletedTasksToday;

            ActualXpGotToday += task.XpEarned;

        }
        static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
        static int UpdateLevelIfNecessary(ProfileDto profileToUpdate)
        {
            //var levelXp = (int)levelsXp.GetValue(0);
            //var levelDescription = GetEnumDescription((Enums.Level)levelsXp.GetValue(0));
            Array levelsXp = Enum.GetValues(typeof(Enums.Level));
            for (int i = 0; i < levelsXp.Length - 1; i++)
            {
                var level = levelsXp.GetValue(i);
                if (profileToUpdate.Xp > (int)level)
                {
                    profileToUpdate.Level = profileToUpdate.Level + 1;
                    return profileToUpdate.Level;
                }
            }

            return profileToUpdate.Level;
        }
        private async Task<dynamic> GenerateTasksWithGpt(double levelOfWaste)
        {
            var tasksGeneratedString = await _gptService.GenerateTasks(levelOfWaste, _globalContext.UserToken);
            var tasksGenerated = GptObjectParser.ParseGptCompletionsData(tasksGeneratedString);

            return tasksGenerated;
        }
    }
}
