
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;
using TezorwasV2.View.AppPages;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class TasksViewModel : ObservableObject
    {
        public ObservableCollection<TasksView.Ceva> Mere { get; } = new ObservableCollection<TasksView.Ceva>();
        public ObservableCollection<TasksView.Ceva> Pere { get; } = new ObservableCollection<TasksView.Ceva>();

        public ObservableCollection<TaskModel> AvailableTasks { get;} = new ObservableCollection<TaskModel>();
        public ObservableCollection<TaskModel> CompletedTasks { get;} = new ObservableCollection<TaskModel>();

        [ObservableProperty] public bool tasksAreCompleted = false;
        [ObservableProperty] public bool allTasksIncompleted = true;
        [ObservableProperty] public string noAvailableTasksMessage;
        [ObservableProperty] public  string noCompletedTasksMessage;
        //TODO de facut usecaseul nostru

        private readonly IProfileService _profileService;
        private readonly IGlobalContext _globalContext;

        //public List<TaskModel> Tasks { get; set; }
        public TasksViewModel(IGlobalContext globalContext, IProfileService profileService)
        {
            _globalContext = globalContext;
            _profileService = profileService;

            Mere.Add(new TasksView.Ceva { Name = "Nume1", IsDone = false });
            Mere.Add(new TasksView.Ceva { Name = "Nume2", IsDone = false });
            Mere.Add(new TasksView.Ceva { Name = "Nume3", IsDone = false });

            NoAvailableTasksMessage = "You completed all the available tasks for today.";
            NoCompletedTasksMessage = "You didn't complete any tasks today";
            

        }
        [RelayCommand]
        public void CompleteTask()
        {
            int completedTasksCounter = 0;
            foreach(var task in Mere)
            {
                if(task.IsDone)
                {
                    
                    completedTasksCounter++;
                    TasksAreCompleted = true;
                    Pere.Add(task);
                    Mere.Remove(task);
                    return;
                }
            }
            if(completedTasksCounter == Mere.Count)
            {
                AllTasksIncompleted = false;
            }
            
        }
    }
}
