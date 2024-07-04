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

        [ObservableProperty] private bool tasksAreCompleted;
        [ObservableProperty] private bool allTasksIncompleted = true;
        [ObservableProperty] private string noAvailableTasksMessage;
        [ObservableProperty] private string noCompletedTasksMessage = "You didn't complete any tasks today";
        [ObservableProperty] private int availableTasksToday;
        [ObservableProperty] private int completedTasksToday;
        [ObservableProperty] private int availableXpToday;
        [ObservableProperty] private int actualXpGotToday;

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;
        private readonly IGptService _gptService;

        private bool generatedTasksToday = false;

        public TasksViewModel(IGlobalContext globalContext, IProfileService profileService, IGptService gptService)
        {
            _globalContext = globalContext;
            _profileService = profileService;
            _gptService = gptService;
        }

        [RelayCommand]
        public async Task CompleteTask(TaskModel task)
        {
            if (task != null)
            {
                task.IsCompleted = true;
                task.CompletionDate = DateTime.Now;

                AvailableTasks.Remove(task);
                CompletedTasks.Add(task);

                await UpdateProfilesCompletedTasks(task);



            }
        }

        public async Task PopulateTasks()
        {

            AvailableTasks.Clear();
            CompletedTasks.Clear();

            AvailableXpToday = 0;
            ActualXpGotToday = 0;
            AvailableTasksToday = 0;
            CompletedTasksToday = 0;

            var allProfiles = await _profileService.GetAllProfiles(_globalContext.UserToken);
            var currentUserProfile = allProfiles.FirstOrDefault(profile => profile.PersonId.Equals(_globalContext.PersonId));
            if (currentUserProfile != null)
            {
                _globalContext.ProfileId = currentUserProfile.Id;

                foreach (var task in currentUserProfile.Tasks)
                {
                    if (task.CreationDate.Date == DateTime.Now.Date && !task.IsCompleted)
                    {
                        AvailableTasks.Add(task);
                        AvailableXpToday += task.XpEarned;

                    }
                    else if (task.CompletionDate.Date == DateTime.Now.Date && task.IsCompleted)
                    {
                        CompletedTasks.Add(task);
                        ActualXpGotToday += task.XpEarned;
                    }
                }


                double levelOfWaste = currentUserProfile.Habbits.Average(habbit => habbit.LevelOfWaste);
                DateTime todaysDate = DateTime.Now.Date;

                int tasksCompletedToday = CompletedTasks.Count(task => task.CompletionDate.Date == todaysDate && task.IsCompleted);
                int taskCreatedToday = AvailableTasks.Count(task => task.CreationDate.Date == todaysDate && !task.IsCompleted);

                if (taskCreatedToday > 0)
                {
                    generatedTasksToday = true;
                }
                if (AvailableTasks.Count < 2 && tasksCompletedToday < 3 && !generatedTasksToday)
                {
                    var tasksGenerated = await GenerateTasksWithGpt(levelOfWaste);

                    List<TaskModel> gptGeneratedTasks = new List<TaskModel>();
                    foreach (var task in tasksGenerated)
                    {
                        var newTask = new TaskModel
                        {
                            Description = task.TaskDescription,
                            CreationDate = DateTime.Now,
                            IsCompleted = false,
                            XpEarned = task.XpEarned,
                            CompletionDate = DateTime.Now 
                        };

                        AvailableTasks.Add(newTask);
                        AvailableXpToday+= task.XpEarned;
                        gptGeneratedTasks.Add(newTask);
                    }
                    await UpdateProfilesAvailableTasks(gptGeneratedTasks);
                }
            }

            AvailableTasksToday = AvailableTasks.Count + CompletedTasks.Count;
            CompletedTasksToday = CompletedTasks.Count;
            AvailableXpToday += ActualXpGotToday;

        }

        private async Task UpdateProfilesAvailableTasks(List<TaskModel> generatedTasksByGpt)
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
            profileToUpdate.Xp += task.XpEarned;
            profileToUpdate.Level = UpdateLevelIfNecessary(profileToUpdate);
            foreach(TaskModel taskToCheck in profileToUpdate.Tasks)
            {
                if (taskToCheck.Description.Equals(task.Description))
                {
                    taskToCheck.IsCompleted = true;
                    break;
                }
            }
            await _profileService.UpdateAProfile(profileToUpdate, _globalContext.UserToken);


            CompletedTasksToday = CompletedTasks.Count;
            //AvailableTasksToday += CompletedTasksToday;
            ActualXpGotToday += task.XpEarned;
        }

        private int UpdateLevelIfNecessary(ProfileDto profileToUpdate)
        {
            Array levelsXp = Enum.GetValues(typeof(Enums.Level));
            for (int i = 0; i < levelsXp.Length - 1; i++)
            {
                var level = levelsXp.GetValue(i);
                if (profileToUpdate.Xp <= (int)level)
                {
                    var lvlDescription = level.GetType()
                                   .GetField(level.ToString())
                                   .GetCustomAttributes(typeof(DescriptionAttribute), false)
                               .Cast<DescriptionAttribute>()
                                   .FirstOrDefault()?.Description ?? level.ToString();

                    if (profileToUpdate.Level < int.Parse(lvlDescription))
                    {
                        profileToUpdate.Level++;
                    }

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
