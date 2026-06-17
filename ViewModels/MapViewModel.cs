using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using Pathway.Models;
using Pathway.Services.Data;

namespace Pathway.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly IConnectivity _connectivity;

        // Map data
        private ObservableCollection<Area> _areas;
        private ObservableCollection<Plant> _plants;

        // Selected item details
        private string _selectedItemName;
        private string _selectedItemDescription;
        private string _selectedItemStatus;
        private bool _isDetailsPanelVisible;

        // Status flags
        private bool _isOffline;
        private bool _isSyncing;
        private string _searchQuery;

        // Commands
        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand SyncCommand { get; }
        public ICommand CloseDetailsPanelCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ViewTasksCommand { get; }

        // Properties
        public ObservableCollection<Area> Areas
        {
            get => _areas;
            set => SetProperty(ref _areas, value);
        }

        public ObservableCollection<Plant> Plants
        {
            get => _plants;
            set => SetProperty(ref _plants, value);
        }

        public string SelectedItemName
        {
            get => _selectedItemName;
            set => SetProperty(ref _selectedItemName, value);
        }

        public string SelectedItemDescription
        {
            get => _selectedItemDescription;
            set => SetProperty(ref _selectedItemDescription, value);
        }

        public string SelectedItemStatus
        {
            get => _selectedItemStatus;
            set => SetProperty(ref _selectedItemStatus, value);
        }

        public bool IsDetailsPanelVisible
        {
            get => _isDetailsPanelVisible;
            set => SetProperty(ref _isDetailsPanelVisible, value);
        }

        public bool IsOffline
        {
            get => _isOffline;
            set => SetProperty(ref _isOffline, value);
        }

        public bool IsSyncing
        {
            get => _isSyncing;
            set => SetProperty(ref _isSyncing, value);
        }

        public bool IsConnected => !IsOffline;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        // Constructor - simple version for demonstration
        public MapViewModel()
        {
            Title = "Campus Map";

            // Initialize collections
            Areas = new ObservableCollection<Area>();
            Plants = new ObservableCollection<Plant>();

            // Initialize commands
            SearchCommand = new Command(ExecuteSearch);
            FilterCommand = new Command(ExecuteFilter);
            SyncCommand = new Command(ExecuteSync);
            CloseDetailsPanelCommand = new Command(ExecuteCloseDetailsPanel);
            ViewDetailsCommand = new Command(ExecuteViewDetails);
            ViewTasksCommand = new Command(ExecuteViewTasks);

            // Set default connectivity status
            IsOffline = false;
        }

        // Constructor with dependencies injected
        public MapViewModel(ILocalDatabase localDatabase, IConnectivity connectivity)
            : this()  // Call the simple constructor first
        {
            _localDatabase = localDatabase;
            _connectivity = connectivity;

            // Subscribe to connectivity changes
            _connectivity.ConnectivityChanged += OnConnectivityChanged;

            // Initial setup
            IsOffline = _connectivity.NetworkAccess != NetworkAccess.Internet;

            // Load data
            LoadAreasAndPlantsAsync();
        }

        private async void LoadAreasAndPlantsAsync()
        {
            try
            {
                IsBusy = true;

                if (_localDatabase != null)
                {
                    // Load areas from local database
                    var areas = await _localDatabase.GetAllAreasAsync();
                    Areas.Clear();
                    foreach (var area in areas)
                    {
                        Areas.Add(area);
                    }

                    // Load plants from local database
                    var plants = await _localDatabase.GetAllPlantsAsync();
                    Plants.Clear();
                    foreach (var plant in plants)
                    {
                        Plants.Add(plant);
                    }
                }
                else
                {
                    // Add some dummy data if database is not available
                    Areas.Add(new Area
                    {
                        Id = "1",
                        Name = "Sample Area",
                        Description = "This is a sample area",
                        Status = AreaStatus.Active
                    });

                    Plants.Add(new Plant
                    {
                        Id = "1",
                        Name = "Sample Plant",
                        Description = "This is a sample plant",
                        Status = PlantStatus.Healthy
                    });
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ExecuteSearch()
        {
            // Simple search implementation
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // Reset to show all areas and plants
                LoadAreasAndPlantsAsync();
                return;
            }

            // Filter in-memory (would be better at database level in real app)
            var filteredAreas = new ObservableCollection<Area>(
                Areas.Where(a =>
                    a.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    a.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));

            var filteredPlants = new ObservableCollection<Plant>(
                Plants.Where(p =>
                    p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));

            // Update collections
            Areas = filteredAreas;
            Plants = filteredPlants;
        }

        private void ExecuteFilter()
        {
            // Placeholder for filter implementation
            Application.Current.MainPage.DisplayAlert("Filter", "Filter functionality would be implemented here", "OK");
        }

        private void ExecuteSync()
        {
            // Placeholder for sync implementation
            IsSyncing = true;

            // Simulate sync
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                IsSyncing = false;
                LoadAreasAndPlantsAsync();
                return false;
            });
        }

        private void ExecuteCloseDetailsPanel()
        {
            IsDetailsPanelVisible = false;
        }

        private void ExecuteViewDetails()
        {
            // Placeholder for navigation to details page
            Application.Current.MainPage.DisplayAlert("View Details", "Would navigate to details page", "OK");
        }

        private void ExecuteViewTasks()
        {
            // Placeholder for navigation to tasks page
            Application.Current.MainPage.DisplayAlert("View Tasks", "Would navigate to tasks page", "OK");
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsOffline = e.NetworkAccess != NetworkAccess.Internet;

            // If we just came back online, try to sync
            if (!IsOffline && !IsSyncing)
            {
                ExecuteSync();
            }
        }

        // Method to handle map pin clicks to show details
        public void ShowDetailsForArea(Area area)
        {
            SelectedItemName = area.Name;
            SelectedItemDescription = area.Description;
            SelectedItemStatus = area.Status.ToString();
            IsDetailsPanelVisible = true;
        }

        public void ShowDetailsForPlant(Plant plant)
        {
            SelectedItemName = plant.Name;
            SelectedItemDescription = plant.Description;
            SelectedItemStatus = plant.Status.ToString();
            IsDetailsPanelVisible = true;
        }



    //    private async void ExecuteViewDetails()
    //    {
    //        // Navigate to details page with a parameter
    //        var parameters = new Dictionary<string, object>
    //{
    //    { "PlantId", selectedPlantId }
    //};
    //        await Shell.Current.GoToAsync("plantdetail", parameters);
    //    }

    }
}