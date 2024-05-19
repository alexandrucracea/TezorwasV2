using CommunityToolkit.Maui.Views;
using TezorwasV2.ViewModel;

namespace TezorwasV2.View.Authentication;

public partial class RegisterView : ContentPage
{

    private readonly RegisterViewModel _registerViewModel;
    public readonly LoadingSpinnerPopup _popup;

    public RegisterView(RegisterViewModel registerViewModel)
    {
        InitializeComponent();
        _registerViewModel = registerViewModel;
        BindingContext = _registerViewModel;
        _popup = new LoadingSpinnerPopup();



    }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
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

    private void RegisterButton_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(_popup);

    }
    protected override void OnDisappearing()
    {
        _popup.Close();
    }
}