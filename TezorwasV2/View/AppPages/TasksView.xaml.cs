
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class TasksView : ContentPage
{
	//public ObservableCollection<Ceva> Mere { get;} = new ObservableCollection<Ceva>();
	private readonly TasksViewModel _taskViewModel;
	public partial class Ceva : ObservableObject
	{
		 [ObservableProperty] public string name;
		 [ObservableProperty] public bool isDone;
		 [ObservableProperty] public int xp = 10;
	}
	public TasksView(TasksViewModel tasksViewModel)
	{
		InitializeComponent();

		_taskViewModel = tasksViewModel;

        //Mere = new ObservableCollection<Ceva>
        //{
        //    new Ceva { Name = "Nume1", IsDone = true },
        //    new Ceva { Name = "Nume2", IsDone = false }
        //};
        BindingContext = _taskViewModel;
    }

    private void SfCheckBox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
		if (_taskViewModel.CompleteTaskCommand.CanExecute(default))
		{
			_taskViewModel.CompleteTaskCommand.Execute(default);
		}
    }
}