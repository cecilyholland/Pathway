using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Pathway.Models;
using Pathway.Services.Data;

namespace Pathway.Data
{
    /// <summary>
    /// Initializes the database and generates sample data for development
    /// </summary>
    public class DatabaseInitializer
    {
        private readonly ILocalDatabase _database;
        private readonly IPreferences _preferences;

        public DatabaseInitializer(ILocalDatabase database, IPreferences preferences)
        {
            _database = database;
            _preferences = preferences;
        }

        /// <summary>
        /// Initializes the database and generates sample data if needed
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            try
            {
                // Check if first run flag is set
                bool isFirstRun = _preferences.Get("is_first_run", true);

                if (isFirstRun)
                {
                    // This is the first run, initialize with sample data
                    await GenerateSampleDataAsync();

                    // Set first run flag to false
                    _preferences.Set("is_first_run", false);
                }
                else
                {
                    // Not first run, but check if we need to add sample data
                    await GenerateSampleDataIfNeededAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                // In a real app, you might want to log this or show an error message
            }
        }

        /// <summary>
        /// Checks if the database is empty and generates sample data if needed
        /// </summary>
        private async Task GenerateSampleDataIfNeededAsync()
        {
            // Check if we've already generated sample data
            bool hasGeneratedSampleData = _preferences.Get("has_generated_sample_data", false);

            if (hasGeneratedSampleData)
            {
                return;
            }

            // Check if database is empty
            var plants = await _database.GetAllPlantsAsync();
            var areas = await _database.GetAllAreasAsync();

            if (plants.Count > 0 || areas.Count > 0)
            {
                // Data already exists, mark as generated
                _preferences.Set("has_generated_sample_data", true);
                return;
            }

            // Database is empty, generate sample data
            await GenerateSampleDataAsync();

            // Mark as generated
            _preferences.Set("has_generated_sample_data", true);
        }

        /// <summary>
        /// Generates sample data for the application
        /// </summary>
        private async Task GenerateSampleDataAsync()
        {
            // 1. Create sample areas
            var areaIds = await CreateSampleAreasAsync();

            // 2. Create sample species
            var speciesIds = await CreateSampleSpeciesAsync();

            // 3. Create sample plants
            var plantIds = await CreateSamplePlantsAsync(areaIds, speciesIds);

            // 4. Create sample tasks
            await CreateSampleTasksAsync(areaIds, plantIds);
        }

        private async Task<List<string>> CreateSampleAreasAsync()
        {
            var areaIds = new List<string>();

            // Create sample areas
            var areas = new List<Area>
            {
                new Area
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Main Campus Entrance",
                    Description = "The main entrance to the campus with decorative plantings",
                    CenterLatitude = 35.046619,
                    CenterLongitude = -85.295921,
                    Type = AreaType.Garden,
                    Status = AreaStatus.Active,
                    HasIrrigation = true,
                    IrrigationNotes = "Automatic sprinkler system",
                    LastMaintenanceDate = DateTime.Now.AddDays(-7),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new Area
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Central Quad",
                    Description = "Main open space in the center of campus",
                    CenterLatitude = 35.045922,
                    CenterLongitude = -85.295444,
                    Type = AreaType.Lawn,
                    Status = AreaStatus.Active,
                    HasIrrigation = true,
                    IrrigationNotes = "Zoned sprinkler system",
                    LastMaintenanceDate = DateTime.Now.AddDays(-3),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new Area
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Science Building Garden",
                    Description = "Ornamental garden with native plants outside the Science Building",
                    CenterLatitude = 35.047325,
                    CenterLongitude = -85.297081,
                    Type = AreaType.Garden,
                    Status = AreaStatus.Active,
                    HasIrrigation = false,
                    IrrigationNotes = "Hand watering only",
                    LastMaintenanceDate = DateTime.Now.AddDays(-5),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                }
            };

            // Save areas to database
            foreach (var area in areas)
            {
                await _database.AddAreaAsync(area);
                areaIds.Add(area.Id);
            }

            return areaIds;
        }

        private async Task<List<string>> CreateSampleSpeciesAsync()
        {
            var speciesIds = new List<string>();

            // Create sample species
            var species = new List<PlantSpecies>
            {
                new PlantSpecies
                {
                    Id = Guid.NewGuid().ToString(),
                    TrefleId = 1234,
                    ScientificName = "Quercus alba",
                    CommonName = "White Oak",
                    Family = "Fagaceae",
                    Genus = "Quercus",
                    ImageUrl = "https://example.com/white_oak.jpg",
                    MinimumTemperature = -30,
                    MaximumTemperature = 40,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new PlantSpecies
                {
                    Id = Guid.NewGuid().ToString(),
                    TrefleId = 5678,
                    ScientificName = "Echinacea purpurea",
                    CommonName = "Purple Coneflower",
                    Family = "Asteraceae",
                    Genus = "Echinacea",
                    ImageUrl = "https://example.com/coneflower.jpg",
                    MinimumTemperature = -20,
                    MaximumTemperature = 40,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new PlantSpecies
                {
                    Id = Guid.NewGuid().ToString(),
                    TrefleId = 9012,
                    ScientificName = "Phlox paniculata",
                    CommonName = "Garden Phlox",
                    Family = "Polemoniaceae",
                    Genus = "Phlox",
                    ImageUrl = "https://example.com/phlox.jpg",
                    MinimumTemperature = -25,
                    MaximumTemperature = 35,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                }
            };

            // Save species to database
            foreach (var s in species)
            {
                await _database.AddPlantSpeciesAsync(s);
                speciesIds.Add(s.Id);
            }

            return speciesIds;
        }

        private async Task<List<string>> CreateSamplePlantsAsync(List<string> areaIds, List<string> speciesIds)
        {
            var plantIds = new List<string>();

            // Create some sample plants
            var plants = new List<Plant>
            {
                new Plant
                {
                    Id = Guid.NewGuid().ToString(),
                    SpeciesId = speciesIds[0], // Oak
                    AreaId = areaIds[0], // Main Entrance
                    Latitude = 35.046619,
                    Longitude = -85.295921,
                    Name = "Main Entrance Oak",
                    Description = "Large oak tree at the main entrance",
                    PlantedDate = DateTime.Now.AddYears(-10),
                    Status = PlantStatus.Healthy,
                    HealthRating = PlantHealthRating.Excellent,
                    HealthNotes = "Thriving specimen",
                    LastInspectionDate = DateTime.Now.AddDays(-30),
                    LastWateredDate = DateTime.Now.AddDays(-7),
                    LastFertilizedDate = DateTime.Now.AddMonths(-3),
                    LastPrunedDate = DateTime.Now.AddYears(-1),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new Plant
                {
                    Id = Guid.NewGuid().ToString(),
                    SpeciesId = speciesIds[1], // Coneflower
                    AreaId = areaIds[2], // Science Building
                    Latitude = 35.047325,
                    Longitude = -85.297081,
                    Name = "Science Garden Coneflowers",
                    Description = "Purple coneflower cluster in the science garden",
                    PlantedDate = DateTime.Now.AddYears(-2),
                    Status = PlantStatus.Healthy,
                    HealthRating = PlantHealthRating.Good,
                    HealthNotes = "Some pest damage on leaves",
                    LastInspectionDate = DateTime.Now.AddDays(-14),
                    LastWateredDate = DateTime.Now.AddDays(-3),
                    LastFertilizedDate = DateTime.Now.AddMonths(-1),
                    LastPrunedDate = DateTime.Now.AddMonths(-6),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new Plant
                {
                    Id = Guid.NewGuid().ToString(),
                    SpeciesId = speciesIds[2], // Phlox
                    AreaId = areaIds[0], // Main Entrance
                    Latitude = 35.046630,
                    Longitude = -85.295940,
                    Name = "Entrance Garden Phlox",
                    Description = "Garden phlox in entrance planting beds",
                    PlantedDate = DateTime.Now.AddYears(-1),
                    Status = PlantStatus.Stressed,
                    HealthRating = PlantHealthRating.Fair,
                    HealthNotes = "Signs of drought stress, increase watering",
                    LastInspectionDate = DateTime.Now.AddDays(-7),
                    LastWateredDate = DateTime.Now.AddDays(-5),
                    LastFertilizedDate = DateTime.Now.AddMonths(-2),
                    LastPrunedDate = DateTime.Now.AddMonths(-3),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                }
            };

            // Save plants to database
            foreach (var plant in plants)
            {
                await _database.AddPlantAsync(plant);
                plantIds.Add(plant.Id);
            }

            return plantIds;
        }

        private async Task CreateSampleTasksAsync(List<string> areaIds, List<string> plantIds)
        {
            // Create some sample tasks
            var tasks = new List<MaintenanceTask>
            {
                new MaintenanceTask
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Water stressed phlox",
                    Description = "Increase watering for the entrance phlox showing drought stress",
                    DueDate = DateTime.Now.AddDays(1),
                    Priority = TaskPriority.High,
                    Status = Models.TaskStatus.NotStarted,
                    AreaId = areaIds[0],
                    PlantId = plantIds[2],
                    Type = TaskType.Watering,
                    WeatherReason = WeatherReason.Drought,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new MaintenanceTask
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Mow central quad",
                    Description = "Regular mowing of the central quad grass area",
                    DueDate = DateTime.Now.AddDays(3),
                    Priority = TaskPriority.Medium,
                    Status = Models.TaskStatus.NotStarted,
                    AreaId = areaIds[1],
                    Type = TaskType.Other,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                },
                new MaintenanceTask
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Inspect coneflowers for pests",
                    Description = "Check science garden coneflowers for further pest damage",
                    DueDate = DateTime.Now.AddDays(7),
                    Priority = TaskPriority.Medium,
                    Status = Models.TaskStatus.NotStarted,
                    AreaId = areaIds[2],
                    PlantId = plantIds[1],
                    Type = TaskType.Inspection,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsSynced = true
                }
            };

            // Save tasks to database
            foreach (var task in tasks)
            {
                await _database.AddTaskAsync(task);
            }
        }
    }
}
