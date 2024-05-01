using Microsoft.Extensions.Configuration;

namespace TezorwasV2
{
    public partial class App : Application
    {
        IConfiguration configuration;
        public App()
        {

            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(settings.UILicense);

            }

            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
