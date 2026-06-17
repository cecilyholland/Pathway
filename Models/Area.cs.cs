using System;
using Pathway1.Models;
using SQLite;

namespace Pathway.Models
{
    /// <summary>
    /// Represents a geographic area on campus
    /// </summary>
    [Table("Areas")]
    public class Area : BaseModel
    {
        // Basic area information
        public string Name { get; set; }
        public string Description { get; set; }

        // Geographic data
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }

        // Area type and characteristics
        public AreaType Type { get; set; }
        public AreaStatus Status { get; set; }

        // Irrigation and maintenance details
        public bool HasIrrigation { get; set; }
        public string IrrigationNotes { get; set; }
        public DateTime LastMaintenanceDate { get; set; }

        // Special considerations
        public string SpecialNotes { get; set; }

        // Image for this area
        public string PrimaryImagePath { get; set; }

        // Update from another area instance
        public void Update(Area other)
        {
            if (other == null) return;

            Name = other.Name;
            Description = other.Description;
            CenterLatitude = other.CenterLatitude;
            CenterLongitude = other.CenterLongitude;
            Type = other.Type;
            Status = other.Status;
            HasIrrigation = other.HasIrrigation;
            IrrigationNotes = other.IrrigationNotes;
            LastMaintenanceDate = other.LastMaintenanceDate;
            SpecialNotes = other.SpecialNotes;
            PrimaryImagePath = other.PrimaryImagePath;
            LastModifiedAt = other.LastModifiedAt;
        }
    }

    /// <summary>
    /// Enumeration of area types
    /// </summary>
    public enum AreaType
    {
        Lawn,
        Garden,
        FlowerBed,
        ShrubBed,
        Woodland,
        Plaza,
        Path,
        Parking,
        Playground,
        SportField,
        Other
    }

    /// <summary>
    /// Enumeration of area status values
    /// </summary>
    public enum AreaStatus
    {
        Active,
        Dormant,
        UnderConstruction,
        PlannedWork,
        Damaged,
        Abandoned
    }
}