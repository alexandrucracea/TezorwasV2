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
        await Shell.Current.GoToAsync("..", true);
    }

}