using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Pathway.Models;
using Pathway.Services.Data;

namespace Pathway.ViewModels
{
    public class PlantDetailViewModel : BaseViewModel
    {
        private readonly ILocalDatabase _localDatabase;

        private Plant _plant;
        private PlantSpecies _plantSpecies;
        private string _areaName;
        private ObservableCollection<MaintenanceTask> _tasks;

        private int _daysSinceWatered;
        private int _daysSinceFertilized;
        private int _daysSincePruned;

        private string _careRecommendations;

        // Properties
        public Plant Plant
        {
            get => _plant;
            set => SetProperty(ref _plant, value);
        }

        public PlantSpecies PlantSpecies
        {
            get => _plantSpecies;
            set => SetProperty(ref _plantSpecies, value);
        }

        public string AreaName
        {
            get => _areaName;
            set => SetProperty(ref _areaName, value);
        }

        public ObservableCollection<MaintenanceTask> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }

        public int DaysSinceWatered
        {
            get => _daysSinceWatered;
            set => SetProperty(ref _daysSinceWatered, value);
        }

        public int DaysSinceFertilized
        {
            get => _daysSinceFertilized;
            set => SetProperty(ref _daysSinceFertilized, value);
        }

        public int DaysSincePruned
        {
            get => _daysSincePruned;
            set => SetProperty(ref _daysSincePruned, value);
        }

        public string CareRecommendations
        {
            get => _careRecommendations;
            set => SetProperty(ref _careRecommendations, value);
        }

        public bool HasCareRecommendations => !string.IsNullOrEmpty(CareRecommendations);

        public Color StatusBackgroundColor
        {
            get
            {
                if (Plant == null) return Colors.White;

                return Plant.Status switch
                {
                    PlantStatus.Healthy => Color.FromArgb("#E8F5E9"), // Light green
                    PlantStatus.Stressed => Color.FromArgb("#FFF8E1"), // Light yellow
                    PlantStatus.Diseased => Color.FromArgb("#FFEBEE"), // Light red
                    PlantStatus.Damaged => Color.FromArgb("#FFEBEE"), // Light red
                    PlantStatus.Dormant => Color.FromArgb("#E0F7FA"), // Light blue
                    _ => Colors.White
                };
            }
        }

        // Commands
        public ICommand LogWateringCommand { get; }
        public ICommand LogFertilizingCommand { get; }
        public ICommand LogPruningCommand { get; }
        public ICommand UpdateHealthCommand { get; }
        public ICommand CreateTaskCommand { get; }

        // Constructor for design-time preview
        public PlantDetailViewModel()
        {
            Title = "Plant Details";
            Tasks = new ObservableCollection<MaintenanceTask>();

            LogWateringCommand = new Command(ExecuteLogWatering);
            LogFertilizingCommand = new Command(ExecuteLogFertilizing);
            LogPruningCommand = new Command(ExecuteLogPruning);
            UpdateHealthCommand = new Command(ExecuteUpdateHealth);
            CreateTaskCommand = new Command(ExecuteCreateTask);
        }

        // Constructor with dependency injection
        public PlantDetailViewModel(ILocalDatabase localDatabase) : this()
        {
            _localDatabase = localDatabase;
        }

        // Initialize with plant ID
        public async void InitializeAsync(string plantId)
        {
            if (IsBusy || _localDatabase == null) return;

            try
            {
                IsBusy = true;

                // Load plant data
                Plant = await _localDatabase.GetPlantByIdAsync(plantId);

                if (Plant == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Plant not found", "OK");
                    return;
                }

                // Load plant species data
                if (!string.IsNullOrEmpty(Plant.SpeciesId))
                {
                    PlantSpecies = await _localDatabase.GetPlantSpeciesByIdAsync(Plant.SpeciesId);
                }

                // Calculate days since maintenance
                DaysSinceWatered = (int)(DateTime.Now - Plant.LastWateredDate).TotalDays;
                DaysSinceFertilized = (int)(DateTime.Now - Plant.LastFertilizedDate).TotalDays;
                DaysSincePruned = (int)(DateTime.Now - Plant.LastPrunedDate).TotalDays;

                // Load area name
                if (!string.IsNullOrEmpty(Plant.AreaId))
                {
                    var area = await _localDatabase.GetAreaByIdAsync(Plant.AreaId);
                    AreaName = area?.Name ?? "Unknown Area";
                }

                // Load associated tasks
                var tasks = await _localDatabase.GetTasksByPlantIdAsync(plantId);
                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }

                // Generate care recommendations
                GenerateBasicRecommendations();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load plant details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void GenerateBasicRecommendations()
        {
            // Simple recommendations based on days since maintenance
            var recommendations = new List<string>();

            if (DaysSinceWatered > 7)
            {
                recommendations.Add($"This plant hasn't been watered in {DaysSinceWatered} days. Consider watering soon.");
            }

            if (DaysSinceFertilized > 30)
            {
                recommendations.Add($"This plant hasn't been fertilized in {DaysSinceFertilized} days. Consider fertilizing if needed.");
            }

            if (DaysSincePruned > 90)
            {
                recommendations.Add($"This plant hasn't been pruned in {DaysSincePruned} days. Consider checking if pruning is needed.");
            }

            if (Plant.Status == PlantStatus.Stressed)
            {
                recommendations.Add("This plant is showing signs of stress. Check soil moisture and environmental conditions.");
            }
            else if (Plant.Status == PlantStatus.Diseased)
            {
                recommendations.Add("This plant shows signs of disease. Consider treatment options appropriate for the symptoms.");
            }

            CareRecommendations = string.Join(" ", recommendations);
        }

        private void ExecuteLogWatering()
        {
            var lastWateredDate = _plant.LastWateredDate; // Copy property value to a local variable
            LogMaintenance("Watering", lastWateredDate); // Pass the local variable by reference
            _plant.LastWateredDate = lastWateredDate; // Update the property with the modified value
            DaysSinceWatered = 0;
        }

        private void ExecuteLogFertilizing()
        {
            var lastFertilizedDate = _plant.LastFertilizedDate; // Copy property value to a local variable
            LogMaintenance("Fertilizing", lastFertilizedDate); // Pass the local variable by reference
            _plant.LastFertilizedDate = lastFertilizedDate; // Update the property with the modified value
            DaysSinceFertilized = 0;
        }

        private void ExecuteLogPruning()
        {
            var lastPrunedDate = _plant.LastPrunedDate; // Copy property value to a local variable
            LogMaintenance("Pruning", lastPrunedDate); // Pass the local variable by reference
            _plant.LastPrunedDate = lastPrunedDate; // Update the property with the modified value
            DaysSincePruned = 0;
        }

        private async void LogMaintenance(string activityType, DateTime dateProperty)
        {
            if (Plant == null || _localDatabase == null) return;

            try
            {
                // Update plant data
                dateProperty = DateTime.Now;
                Plant.LastModifiedAt = DateTime.Now;
                Plant.IsSynced = false;

                // Update in database
                await _localDatabase.UpdatePlantAsync(Plant);

                // Create work log entry
                var workLog = new WorkLog
                {
                    Id = Guid.NewGuid().ToString(),
                    PlantId = Plant.Id,
                    AreaId = Plant.AreaId,
                    ActivityType = activityType,
                    Notes = $"Plant {activityType.ToLower()} recorded",
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    IsSynced = false
                };

                await _localDatabase.AddWorkLogAsync(workLog);

                await Application.Current.MainPage.DisplayAlert("Success", $"{activityType} logged successfully", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to log {activityType.ToLower()}: {ex.Message}", "OK");
            }
        }

        private async void ExecuteUpdateHealth()
        {
            if (Plant == null || _localDatabase == null) return;

            try
            {
                // Show a dialog to update health status
                string action = await Application.Current.MainPage.DisplayActionSheet(
                    "Update Plant Health",
                    "Cancel",
                    null,
                    "Healthy",
                    "Stressed",
                    "Diseased",
                    "Damaged",
                    "Dormant");

                if (action == "Cancel" || string.IsNullOrEmpty(action))
                    return;

                // Parse the selected status
                if (Enum.TryParse<PlantStatus>(action, out var newStatus))
                {
                    // Update plant data
                    Plant.Status = newStatus;
                    Plant.LastInspectionDate = DateTime.Now;
                    Plant.LastModifiedAt = DateTime.Now;
                    Plant.IsSynced = false;

                    // Prompt for health notes
                    string result = await Application.Current.MainPage.DisplayPromptAsync(
                        "Health Notes",
                        "Enter any notes about the plant's health:",
                        initialValue: Plant.HealthNotes);

                    if (result != null) // Not cancelled
                    {
                        Plant.HealthNotes = result;
                    }

                    // Update in database
                    await _localDatabase.UpdatePlantAsync(Plant);

                    // Create work log entry
                    var workLog = new WorkLog
                    {
                        Id = Guid.NewGuid().ToString(),
                        PlantId = Plant.Id,
                        AreaId = Plant.AreaId,
                        ActivityType = "Health Inspection",
                        Notes = $"Status updated to {newStatus}. Notes: {Plant.HealthNotes}",
                        CreatedAt = DateTime.Now,
                        LastModifiedAt = DateTime.Now,
                        IsSynced = false
                    };

                    await _localDatabase.AddWorkLogAsync(workLog);

                    // Notify property changes for UI update
                    OnPropertyChanged(nameof(Plant));
                    OnPropertyChanged(nameof(StatusBackgroundColor));

                    // Regenerate recommendations
                    GenerateBasicRecommendations();

                    await Application.Current.MainPage.DisplayAlert("Success", "Health status updated successfully", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update health status: {ex.Message}", "OK");
            }
        }

        private void ExecuteCreateTask()
        {
            // Placeholder for task creation
            Application.Current.MainPage.DisplayAlert("Create Task",
                "This would navigate to the task creation screen with this plant pre-selected.", "OK");
        }
    }
}