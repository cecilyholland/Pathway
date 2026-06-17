using System;
using System.Collections.Generic;
using Pathway1.Models;
using SQLite;

namespace Pathway.Models
{
    /// <summary>
    /// Represents a physical plant on campus
    /// </summary>
    [Table("Plants")]
    public class Plant : BaseModel
    {
        // Reference to plant species data from Trefle API
        public string SpeciesId { get; set; }

        // Location data
        public string AreaId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Plant metadata
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PlantedDate { get; set; }

        // Status and health tracking
        public PlantStatus Status { get; set; }
        public PlantHealthRating HealthRating { get; set; }
        public string HealthNotes { get; set; }
        public DateTime LastInspectionDate { get; set; }

        // Maintenance tracking
        public DateTime LastWateredDate { get; set; }
        public DateTime LastFertilizedDate { get; set; }
        public DateTime LastPrunedDate { get; set; }

        // Images of this specific plant
        public string PrimaryImagePath { get; set; }

        // Additional properties
        [Ignore]
        public Dictionary<string, string> AdditionalProperties { get; set; }

        public string AdditionalPropertiesJson { get; set; }

        // Update from another plant instance
        public void Update(Plant other)
        {
            if (other == null) return;

            SpeciesId = other.SpeciesId;
            AreaId = other.AreaId;
            Latitude = other.Latitude;
            Longitude = other.Longitude;
            Name = other.Name;
            Description = other.Description;
            PlantedDate = other.PlantedDate;
            Status = other.Status;
            HealthRating = other.HealthRating;
            HealthNotes = other.HealthNotes;
            LastInspectionDate = other.LastInspectionDate;
            LastWateredDate = other.LastWateredDate;
            LastFertilizedDate = other.LastFertilizedDate;
            LastPrunedDate = other.LastPrunedDate;
            PrimaryImagePath = other.PrimaryImagePath;
            AdditionalPropertiesJson = other.AdditionalPropertiesJson;
            LastModifiedAt = other.LastModifiedAt;
        }
    }

    /// <summary>
    /// Enumeration of plant status values
    /// </summary>
    public enum PlantStatus
    {
        Healthy,
        Stressed,
        Diseased,
        Damaged,
        Dormant,
        Dead,
        Removed
    }

    /// <summary>
    /// Enumeration of plant health rating values
    /// </summary>
    public enum PlantHealthRating
    {
        Excellent,
        Good,
        Fair,
        Poor,
        Critical
    }
}