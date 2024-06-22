using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using TezorwasV2.Model;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ReceiptsView : ContentPage
{
    private ReceiptsViewModel viewModel;

    public ReceiptsView(ReceiptsViewModel receiptsViewModel)
    {
        InitializeComponent();
        viewModel = receiptsViewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("#eff1f3"),
            StatusBarStyle = StatusBarStyle.DarkContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
        await viewModel.GetAllProfileReceipts();
        viewModel.FilterDataCommand.Execute(DateTime.Now.Date);
        ApplyGradientBackground(viewModel.Receipts);
    }

    #region Events
    private void ReceiptsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentReceipt = e.CurrentSelection.FirstOrDefault() as ReceiptModel;
        if (currentReceipt is not null)
        {
            viewModel.GoToReceiptCommand.Execute(currentReceipt);
        }
    }
    private void CalendarReceipts_Tapped(object sender, Syncfusion.Maui.Calendar.CalendarTappedEventArgs e)
    {
        var selectedDate = e.Date;
        viewModel.FilterDataCommand.Execute(selectedDate);
    }
    #endregion

    #region Styling
    private void ApplyGradientBackground(dynamic receipts)
    {
        var gradientColors = GenerateGradientColors(receipts.Count, Color.FromArgb("#037171"), Color.FromArgb("#03312e"));

        for (int i = 0; i < receipts.Count; i++)
        {
            receipts[i].BackgroundColor = gradientColors[i];
        }
    }
    private List<Color> GenerateGradientColors(int steps, Color startColor, Color endColor)
    {
        var colors = new List<Color>();

        for (int i = 0; i < steps; i++)
        {
            var r = startColor.Red + (endColor.Red - startColor.Red) * i / (steps - 1);
            var g = startColor.Green + (endColor.Green - startColor.Green) * i / (steps - 1);
            var b = startColor.Blue + (endColor.Blue - startColor.Blue) * i / (steps - 1);

            colors.Add(new Color(r, g, b));
        }

        return colors;
    }
    #endregion
}
