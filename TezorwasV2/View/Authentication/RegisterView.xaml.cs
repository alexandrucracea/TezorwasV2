using CommunityToolkit.Maui.Views;
using TezorwasV2.ViewModel;

namespace TezorwasV2.View.Authentication;

public partial class RegisterView : ContentPage
{

    private readonly RegisterViewModel _registerViewModel;

    public RegisterView(RegisterViewModel registerViewModel)
    {
        InitializeComponent();

        _registerViewModel = registerViewModel;
        BindingContext = _registerViewModel;


    }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);
        await Shell.Current.GoToAsync("..", true);
        popup.Close();
    }

    private void FirstNameEntry_Completed(object sender, EventArgs e)
    {
        LastNameEntry.Focus();
    }

    private void LastNameEntry_Completed(object sender, EventArgs e)
    {
        EmailEntry.Focus();
    }

    private void EmailEntry_Completed(object sender, EventArgs e)
    {
        PasswordEntry.Focus();
    }

    private void PasswordEntry_Completed(object sender, EventArgs e)
    {
        ConfirmPasswordEntry.Focus();
    }

    private void ConfirmPasswordEntry_Completed(object sender, EventArgs e)
    {
        RegisterButton.Focus();
    }
}