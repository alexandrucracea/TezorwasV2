
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class TasksViewModel : ObservableObject
    {
        public ObservableCollection<TaskModel> AvailableTasks { get;} = new ObservableCollection<TaskModel>();
        public ObservableCollection<TaskModel> CompletedTasks { get;} = new ObservableCollection<TaskModel>();

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
            foreach(var task in AvailableTasks)
            {
                if (completedTasksCounter == AvailableTasksToday)
                {
                    AllTasksIncompleted = false;
                }
                if (!task.IsCompleted)
                {
                    task.IsCompleted = true;
                    completedTasksCounter++;
                    TasksAreCompleted = true;

                    CompletedTasks.Add(task);
                    AvailableTasks.Remove(task);

                    ProfileDto profileToUpdate = await _profileService.GetProfileInfo(_globalContext.ProfileId,_globalContext.UserToken);
                    profileToUpdate.Tasks = AvailableTasks.Concat(CompletedTasks).ToList();

                    await _profileService.UpdateAProfile(profileToUpdate,_globalContext.UserToken);
                    return;
                }
            }
            
            return;
            
        }
        private async void PopulateTasks()
        {
            var allProfiles = await _profileService.GetAllProfiles(_globalContext.UserToken);
            var currentUserProfile = allProfiles.FirstOrDefault(profile => profile.PersonId.Equals(_globalContext.PersonId));
            if(currentUserProfile != null)
            {
                _globalContext.ProfileId = currentUserProfile.Id;
                foreach(TaskModel task in currentUserProfile.Tasks)
                {
                    if (!task.IsCompleted)
                    {
                        AvailableTasks.Add(task);
                    }
                }
            }
            AvailableTasksToday = AvailableTasks.Count;
        }

        //TODO to add the xp as you complete a task
        //TOOD fix the message (why is not showing)
    }
}
