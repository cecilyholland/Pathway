using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using Pathway.Data;

namespace Pathway
{
    public partial class App : Application
    {
        // API Keys - replace these with your actual API keys
        public static string TrefleApiKey = "YOUR_TREFLE_API_KEY";
        public static string WeatherApiKey = "YOUR_WEATHER_API_KEY";
        public static string GoogleMapsApiKey = "YOUR_GOOGLE_MAPS_API_KEY";

        // App-wide settings
        public static bool IsUserLoggedIn { get; set; }
        public static string? CurrentUserId { get; set; }    

        public App()
        {
            // Initialize the application
            Application.Current.UserAppTheme = AppTheme.Light; // Optional: set default theme

            // Load application resources from App.xaml if needed
            InitializeResources();

            // Initialize database with sample data (for development)
            InitializeDatabase();
        }


        // Set the main page
        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);
            window.Page = new AppShell();
            return window;
        }

        private void InitializeResources()
        {
            // This replaces InitializeComponent() if it's not auto-generated
            if (Resources == null)
            {
                Resources = new ResourceDictionary();
            }

            // Add any global resources here if needed
        }

        private void InitializeDatabase()
        {
            try
            {
                // Get the database initializer from DI
                var databaseInitializer = Application.Current.Handler.MauiContext?.Services
                    .GetService<DatabaseInitializer>();

                if (databaseInitializer != null)
                {
                    // Run database initialization in background
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        await databaseInitializer.InitializeDatabaseAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}