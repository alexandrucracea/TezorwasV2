using Microsoft.Extensions.Configuration;
using TezorwasV2.View.Authentication;
using TezorwasV2.Helpers;

namespace TezorwasV2
{
    public partial class MainPage : ContentPage
    {
        IConfiguration configuration;
        public double ScreenHeight { get; }
        public double ScreenWidth { get; }
        public MainPage()
        {
            InitializeComponent();
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            ScreenHeight = DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = DeviceDisplay.MainDisplayInfo.Width;
            BindingContext = this;


        }
        private async void OnClickGoToPage(object sender, EventArgs e)
        {

            if (sender is Button button)
            {
                switch (button.StyleId)
                {
                    case Constants.LOGIN_BUTTON:
                        {
                            //var settings = configuration.GetRequiredSection("Settings").Get<Settings>();
                            await Shell.Current.GoToAsync(nameof(LoginView), true);
                            break;
                        }
                    case Constants.REGISTER_BUTTON:
                        {
                            await Shell.Current.GoToAsync(nameof(RegisterView), true);
                            break;
                        }
                }
            }
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    if (this.AnimationIsRunning("TransitionAnimation"))
        //        return;

        //    var parentAnimation = new Animation();


        //    //Intro Box Animation
        //    parentAnimation.Add(0.2, 0.7, new Animation(v => ButtonsContainer.Opacity = v, 0, 1, Easing.CubicIn));

        //    //Commit the animation
        //    parentAnimation.Commit(this, "TransitionAnimation", 16, 3000, null, null);
        //}

    }

}
