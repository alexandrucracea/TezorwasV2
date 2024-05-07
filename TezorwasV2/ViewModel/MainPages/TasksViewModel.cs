using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
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

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;

        public TasksViewModel(IGlobalContext globalContext, IProfileService profileService)
        {
            _globalContext = globalContext;
            _profileService = profileService;

            PopulateTasks();

            NoAvailableTasksMessage = "You completed all the available tasks for today.";
            NoCompletedTasksMessage = "You didn't complete any tasks today";

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

                    await UpdateProfile(task);
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
                    }
                }
            }
            AvailableTasksToday = AvailableTasks.Count;
        }
        private async Task UpdateProfile(TaskModel task)
        {

            ProfileDto profileToUpdate = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            profileToUpdate.Tasks = AvailableTasks.Concat(CompletedTasks).ToList();
            profileToUpdate.Xp += task.XpEarned;
            profileToUpdate.Level = UpdateLevelIfNecessary(profileToUpdate);

            
            await _profileService.UpdateAProfile(profileToUpdate, _globalContext.UserToken);
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
            for (int i = 0; i < levelsXp.Length-1; i++)
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
    }
}
