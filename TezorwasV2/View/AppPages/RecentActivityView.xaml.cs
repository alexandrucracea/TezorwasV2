using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microcharts;
using SkiaSharp;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class RecentActivityView : ContentPage
{
    private RecentActivityViewModel viewModel;
    public RecentActivityView(RecentActivityViewModel recentActivityViewModel)
    {
        InitializeComponent();

        viewModel = recentActivityViewModel;
        BindingContext = viewModel;
    }
    protected async override void OnAppearing()
    {
        #region StatusBar
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("#eff1f3"),
            StatusBarStyle = StatusBarStyle.DarkContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
        #endregion

        await viewModel.GenerateChartsEntries();
        chartView1.Chart = new PieChart
        {
            Entries = viewModel.pieChartTasksEntries,
            BackgroundColor = new SKColor(240, 240, 241),
            LabelTextSize = 25,
            //ValueLabelTextSize = 40,
            //LabelOrientation = Orientation.Horizontal,
            //ValueLabelOrientation = Orientation.Horizontal,
            

        };
        chartView2.Chart = new PieChart
        {
            Entries = viewModel.pieChartReceiptsEntries,
            BackgroundColor = new SKColor(240, 240, 241),
            LabelTextSize = 25,
            //ValueLabelTextSize = 40,
            //LabelOrientation = Orientation.Horizontal,
            //ValueLabelOrientation = Orientation.Horizontal,
        };

        base.OnAppearing();
    }
}