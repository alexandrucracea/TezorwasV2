using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using System.Reflection;
using TesseractOcrMaui;
using TezorwasV2.Helpers;
using TezorwasV2.Services;
using TezorwasV2.View.AppPages;
using TezorwasV2.View.Authentication;
using TezorwasV2.ViewModel;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; }
        public static MauiApp CreateMauiApp()
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("TezorwasV2.appsettings.json");

            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();


            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                   .UseMicrocharts()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Inter-VariableFont.ttf", "Inter-VariableFont");
                })
            .UseMauiCommunityToolkit()
            .ConfigureMauiHandlers(handlers =>

            {
#if ANDROID
                handlers.AddHandler(typeof(Shell), typeof(TezorwasV2.Platforms.Android.CustomShellHandler));
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Configuration.AddConfiguration(config);

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<IRegisterService, RegisterService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddTransient<IProfileService, ProfileService>();
            builder.Services.AddTransient<IArticleService, ArticleService>();
            builder.Services.AddTransient<IGptService, GptService>();
            builder.Services.AddTransient<IForgotPasswordService, ForgotPasswordService>();
            builder.Services.AddTransient<ILoadingSpinnerPopupService, LoadingSpinnerPopupService>();
            builder.Services.AddTransient<ILogoutPopupService, LogoutPopupService>();

            builder.Services.AddSingleton<IGlobalContext, GlobalContext>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<RegisterView>();
            builder.Services.AddSingleton<QuestionsViewModel>();
            builder.Services.AddSingleton<QuestionsView>();
            builder.Services.AddSingleton<TasksViewModel>();
            builder.Services.AddSingleton<TasksView>();
            builder.Services.AddSingleton<ScanReceiptView>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<ProfileView>();
            builder.Services.AddSingleton<ArticleViewModel>();
            builder.Services.AddSingleton<ArticlesView>();
            builder.Services.AddSingleton<ArticlePageViewModel>();
            builder.Services.AddSingleton<ArticlePage>();
            builder.Services.AddSingleton<HabbitViewModel>();
            builder.Services.AddSingleton<HabbitsView>();
            builder.Services.AddSingleton<ReceiptsViewModel>();
            builder.Services.AddSingleton<ReceiptsView>();
            builder.Services.AddSingleton<ScanReceiptViewModel>();
            builder.Services.AddSingleton<ScanReceiptView>();
            builder.Services.AddSingleton<ReceiptItemViewModel>();
            builder.Services.AddSingleton<ReceiptItemView>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsMenu>();
            builder.Services.AddSingleton<EvolutionChartViewModel>();
            builder.Services.AddSingleton<EvolutionChartView>();
            builder.Services.AddSingleton<RecentActivityViewModel>();
            builder.Services.AddSingleton<RecentActivityView>();
            builder.Services.AddSingleton<ForgotPasswordViewModel>();
            builder.Services.AddSingleton<ForgotPasswordView>();


            builder.Services.AddTesseractOcr(
                files =>
                {
                    // must have matching files in Resources/Raw folder
                    files.AddFile("ron.traineddata");
                    files.AddFile("eng.traineddata");
                });

            var app = builder.Build();
            Services = app.Services;
            return app;
        }
    }
}
