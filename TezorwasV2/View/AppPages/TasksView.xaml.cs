
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Buttons;
using TezorwasV2.Model;
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

    [Obsolete]
    protected override async void  OnAppearing()
	{
        #region StatusBar
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("037171"),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
        #endregion

        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);

        await _taskViewModel.PopulateTasks();
        
        popup.Close();

    }

    private void SfCheckBox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
		if (_taskViewModel.CompleteTaskCommand.CanExecute(default))
		{
            var checkBox = sender as SfCheckBox;
            var taskToComplete = checkBox.BindingContext as TaskModel;
			_taskViewModel.CompleteTaskCommand.Execute(taskToComplete);
		}
    }
}