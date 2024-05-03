using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using TezorwasV2.Helpers;
using TezorwasV2.View.AppPages;
using TezorwasV2.ViewModel;

namespace TezorwasV2.View.Authentication;

public partial class LoginView : ContentPage
{

    private readonly LoginViewModel _loginViewModel;
    public LoginView(LoginViewModel loginViewModel)
    {
 
        InitializeComponent();

        _loginViewModel = loginViewModel;
        BindingContext = _loginViewModel;
        var emailValidationBehaviour = SetEmailValidationBehaviour();
        EmailEntry.Behaviors.Add(emailValidationBehaviour);

    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {

        string emailToAuthenticate = EmailEntry.Text;
        string passwordToAuthenticate = PasswordEntry.Text;

        _loginViewModel.Username = emailToAuthenticate;
        _loginViewModel.Password = passwordToAuthenticate;

        HttpCallResponseData callResponse = await _loginViewModel.AuthenticateUser();
        if (callResponse.StatusCode == (int)Enums.StatusCodes.Success)
        {
            await Shell.Current.GoToAsync($"//{nameof(TasksView)}", true);
        }
        else
        {
            await DisplayAlert("CallResponse", callResponse.Response + " " + callResponse.StatusCode.ToString(), "OK");

        }
    }
    private EmailValidationBehavior SetEmailValidationBehaviour()
    {
        var invalidStyle = new Style(typeof(Microsoft.Maui.Controls.Entry));
        invalidStyle.Setters.Add(new Setter
        {
            Property = Microsoft.Maui.Controls.Entry.PlaceholderColorProperty,
            Value = Colors.Red
        });

        return new EmailValidationBehavior
        {
            InvalidStyle = invalidStyle,
            Flags = ValidationFlags.ValidateOnValueChanged,
            MinimumLength = 1
        };
    }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {

        await Shell.Current.GoToAsync("..", true);
    }

    private void EmailEntry_Completed(object sender, EventArgs e)
    {
        PasswordEntry.Focus();
    }
}