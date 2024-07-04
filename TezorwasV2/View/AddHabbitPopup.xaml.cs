using CommunityToolkit.Maui.Views;
using TezorwasV2.Model;
using TezorwasV2.ViewModel;

namespace TezorwasV2.View;

public partial class AddHabbitPopup : Popup
{
    public readonly AddHabbitPopupViewModel _viewModel;

    public AddHabbitPopup()
    {
        InitializeComponent();
    }
    public AddHabbitPopup(AddHabbitPopupViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
	}

    private async void SaveHabbit(object sender, EventArgs e)
    {
        await CloseAsync(new HabbitModel
        {
            InputDate = DateTime.Now,
            Description = EdtHabbitDescription.Text,
            LevelOfWaste = wasteRating.Value
        });
    }
}

public class AddHabbitToProfileDto
{
    public string Description { get; set; }
    public double LevelOfWaste { get; set; }
}