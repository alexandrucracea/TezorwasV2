namespace TezorwasV2.View.AppPages;

public partial class FAQView : ContentPage
{
	public FAQView()
	{
		InitializeComponent();
	}

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }
}