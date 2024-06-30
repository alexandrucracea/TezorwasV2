using CommunityToolkit.Maui.Views;
using TezorwasV2.Model;

namespace TezorwasV2.View;

public partial class AddHabbitPopup : Popup
{
	public AddHabbitPopup()
	{
		InitializeComponent();
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