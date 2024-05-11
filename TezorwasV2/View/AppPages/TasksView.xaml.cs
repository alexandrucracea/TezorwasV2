
using CommunityToolkit.Mvvm.ComponentModel;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class TasksView : ContentPage
{

	private readonly TasksViewModel _taskViewModel;
	public partial class Ceva : ObservableObject
	{
		 [ObservableProperty] public string name;
		 [ObservableProperty] public bool isDone;
	}
	public TasksView(TasksViewModel tasksViewModel)
	{
		InitializeComponent();

		_taskViewModel = tasksViewModel;
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