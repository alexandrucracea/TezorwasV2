using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microcharts;
using SkiaSharp;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class EvolutionChartView : ContentPage
{
    EvolutionChartViewModel viewModel;
    public EvolutionChartView(EvolutionChartViewModel evolutionChartViewModel)
    {
        InitializeComponent();
        viewModel = evolutionChartViewModel;
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
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
        chartView.Chart = new BarChart
        {
            Entries = viewModel.barChartComparisonEntries,
            BackgroundColor = new SKColor(240,240,241),
            LabelTextSize = 40,
            ValueLabelTextSize = 40,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            CornerRadius = 20,

        };
        chartView2.Chart = new LineChart
        {
            Entries = viewModel.lineChartTasksEntries,
            BackgroundColor = new SKColor(240, 240, 241),
            EnableYFadeOutGradient = true,
            LabelTextSize = 40,
            ValueLabelTextSize = 40,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
        };
        chartView3.Chart = new LineChart
        {
            Entries = viewModel.lineChartReceiptsEntries,
            BackgroundColor = new SKColor(240, 240, 241),
            LabelTextSize = 40,
            ValueLabelTextSize = 40,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
        };
        //base.OnAppearing();
    }
}