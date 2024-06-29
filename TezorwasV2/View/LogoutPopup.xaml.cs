using CommunityToolkit.Maui.Views;

namespace TezorwasV2.View;

public partial class LogoutPopup : Popup
{
	public LogoutPopup()
	{
		InitializeComponent();
	}

    private async void ContinueLogOutProcess(object sender, EventArgs e)
    {
        await CloseAsync(true);
    }
    private async void CancelLogOutProcess(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}