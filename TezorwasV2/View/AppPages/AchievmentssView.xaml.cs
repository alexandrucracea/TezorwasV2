using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace TezorwasV2.View.AppPages;

public partial class AchievmentssView : ContentPage
{
    public string link { get; set; }

    public AchievmentssView()
	{
		InitializeComponent();

        BindingContext = this;

        var imageUrl = "https://firebasestorage.googleapis.com/v0/b/tezorwas.appspot.com/o/welcome.png?alt=media&token=778f0b52-6ba4-4af3-b02a-73926680e8ae";
        var firstTaskUrl = "https://firebasestorage.googleapis.com/v0/b/tezorwas.appspot.com/o/1task.png?alt=media&token=b8d51e36-5446-4a3e-8e1f-75622f218460";
        var firstMonthUrl = "https://firebasestorage.googleapis.com/v0/b/tezorwas.appspot.com/o/firstMonth.png?alt=media&token=3c256765-9ae0-4d23-86c5-3d8f7316322b";
        var tentasksUrl = "https://firebasestorage.googleapis.com/v0/b/tezorwas.appspot.com/o/tentasks.png?alt=media&token=bae4c501-cf35-483b-8ac8-08b48ea9b27b";

        var uriImageSource = new UriImageSource
        {
            Uri = new Uri(imageUrl),
            CachingEnabled = false, // Setează la true dacă dorești caching
            CacheValidity = TimeSpan.FromDays(1) // Valabilitate cache, dacă este activat
        };

        imgTest.Source = uriImageSource;
        welcomeAchv.Source = uriImageSource;

        uriImageSource = new UriImageSource
        {
            Uri = new Uri(firstTaskUrl),
            CachingEnabled = false, // Setează la true dacă dorești caching
            CacheValidity = TimeSpan.FromDays(1) // Valabilitate cache, dacă este activat
        };

        img1stTask.Source = uriImageSource;
        firstTaks2.Source = uriImageSource;

        uriImageSource = new UriImageSource
        {
            Uri = new Uri(firstMonthUrl),
            CachingEnabled = false, // Setează la true dacă dorești caching
            CacheValidity = TimeSpan.FromDays(1) // Valabilitate cache, dacă este activat
        };

        firstMonth.Source = uriImageSource;
        firstMonth2.Source = uriImageSource;


        uriImageSource = new UriImageSource
        {
            Uri = new Uri(tentasksUrl),
            CachingEnabled = false, // Setează la true dacă dorești caching
            CacheValidity = TimeSpan.FromDays(1) // Valabilitate cache, dacă este activat
        };
        tenTasks1.Source = uriImageSource;
        tenTasks2.Source = uriImageSource;

    }
    protected override void OnAppearing()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("#037171"),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416 // Validate platform compatibility

        base.OnAppearing();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }
}