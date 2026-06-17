using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using System.IO;
using Pathway.Services.Data;
using Pathway.ViewModels;
using Pathway.Views;
using Pathway.Data;

namespace Pathway
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register system services
            builder.Services.AddSingleton<IConnectivity>(Microsoft.Maui.Networking.Connectivity.Current);
            builder.Services.AddSingleton<IPreferences>(Microsoft.Maui.Storage.Preferences.Default);

            // Register database
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "pathway.db");
            builder.Services.AddSingleton<ILocalDatabase>(s => new LocalDatabase(dbPath));

            // Register database initializer
            builder.Services.AddSingleton<DatabaseInitializer>();

            // Register ViewModels
            builder.Services.AddTransient<BaseViewModel>();
            builder.Services.AddTransient<MapViewModel>();
            builder.Services.AddTransient<PlantDetailViewModel>();

            // Register Views
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<PlantDetailPage>();

            // Register ViewModels
            builder.Services.AddTransient<PlantDetailViewModel>();

            // Register Views
            builder.Services.AddTransient<PlantDetailPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}