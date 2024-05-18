using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.ApplicationModel.Communication;
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


        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);

        HttpCallResponseData callResponse = await _loginViewModel.AuthenticateUser();
        if (callResponse.StatusCode == (int)Enums.StatusCodes.Success)
        {

            await Shell.Current.GoToAsync($"//{nameof(TasksView)}", true);
            popup.Close();
        }
        else
        {
            popup.Close();
            
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromRgb(244, 214, 210),
                TextColor = Color.FromRgb(150, 25, 17),
                CornerRadius = new CornerRadius(40),
                Font =Microsoft.Maui.Font.SystemFontOfSize(14)
                
            };

            var content = new Label
            {
                Text = "Incorrect email or password",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var snackbar = Snackbar.Make(callResponse.Response, null, actionButtonText: "", duration: new TimeSpan(0, 0, 3), snackbarOptions);
            
            await snackbar.Show(default);
            //await DisplayAlert("CallResponse", callResponse.Response + " " + callResponse.StatusCode.ToString(),"");

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
        EmailEntry.Text = "";
        PasswordEntry.Text="";
        await Shell.Current.GoToAsync("..", true);
    }
    private void EmailEntry_Completed(object sender, EventArgs e)
    {
        PasswordEntry.Focus();
    }

    private void PasswordEntry_Completed(object sender, EventArgs e)
    {
        LoginButton.Focus();
    }
}