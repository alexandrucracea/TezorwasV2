
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
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
	protected override void  OnAppearing()
	{
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("037171"),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
    }

    private void SfCheckBox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
		if (_taskViewModel.CompleteTaskCommand.CanExecute(default))
		{
			_taskViewModel.CompleteTaskCommand.Execute(default);
		}
    }
}