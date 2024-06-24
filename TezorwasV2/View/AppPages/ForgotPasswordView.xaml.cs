using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ForgotPasswordView : ContentPage
{
	private ForgotPasswordViewModel viewModel;
	public ForgotPasswordView(ForgotPasswordViewModel forgotPasswordViewModel)
	{
		InitializeComponent();
		viewModel = forgotPasswordViewModel;
		BindingContext = viewModel;
	}
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        EmailEntry.Text = "";
        await Shell.Current.GoToAsync("..", true);
    }
}