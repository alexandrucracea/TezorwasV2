using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microcharts;
using SkiaSharp;
using TezorwasV2.ViewModel.MainPagest;

namespace TezorwasV2.View.AppPages;

public partial class EvolutionChartView : ContentPage
{
    EvolutionChartViewModel viewModel;
    //ChartEntry[] entries = new[]
    //    {
    //        new ChartEntry(212)
    //        {
    //            Label = "Windows",
    //            ValueLabel = "112",
    //            Color = SKColor.Parse("#2c3e50")
    //        },
    //        new ChartEntry(248)
    //        {
    //            Label = "Android",
    //            ValueLabel = "648",
    //            Color = SKColor.Parse("#77d065")
    //        },
    //        new ChartEntry(128)
    //        {
    //            Label = "iOS",
    //            ValueLabel = "428",
    //            Color = SKColor.Parse("#b455b6")
    //        },
    //        new ChartEntry(514)
    //        {
    //            Label = ".NET MAUI",
    //            ValueLabel = "214",
    //            Color = SKColor.Parse("#3498db")
    //        }
    //    };
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

        await viewModel.GenerateBarChartComparisonEntries();
        chartView.Chart = new BarChart
        {
            Entries = viewModel.barChartComparisonEntries
            
        };
        //base.OnAppearing();
    }
}