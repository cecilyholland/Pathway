using System;
using System.Collections.Generic;
using Pathway1.Models;
using SQLite;

namespace Pathway.Models
{
    /// <summary>
    /// Represents a maintenance task for plants or areas
    /// </summary>
    [Table("MaintenanceTasks")]
    public class MaintenanceTask : BaseModel
    {
        // Basic task information
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }

        // Related entities
        public string AreaId { get; set; }
        public string PlantId { get; set; }
        public string AssigneeId { get; set; }

        // Task categorization
        public TaskType Type { get; set; }
        public WeatherReason? WeatherReason { get; set; }

        // Completion tracking
        public DateTime? CompletedDate { get; set; }
        public string CompletionNotes { get; set; }

        // SQLite doesn't support Lists directly, store comma-separated string
        // for multiple plants
        public string PlantIdsString { get; set; }

        [Ignore]
        public List<string> PlantIds
        {
            get => string.IsNullOrEmpty(PlantIdsString)
                ? new List<string>()
                : new List<string>(PlantIdsString.Split(','));
            set => PlantIdsString = value != null ? string.Join(",", value) : null;
        }

        // Update from another task instance
        public void Update(MaintenanceTask other)
        {
            if (other == null) return;

            Title = other.Title;
            Description = other.Description;
            DueDate = other.DueDate;
            Priority = other.Priority;
            Status = other.Status;
            AreaId = other.AreaId;
            PlantId = other.PlantId;
            AssigneeId = other.AssigneeId;
            Type = other.Type;
            WeatherReason = other.WeatherReason;
            CompletedDate = other.CompletedDate;
            CompletionNotes = other.CompletionNotes;
            PlantIdsString = other.PlantIdsString;
            LastModifiedAt = other.LastModifiedAt;
        }
    }

    /// <summary>
    /// Enumerates different types of tasks
    /// </summary>
    public enum TaskType
    {
        Watering,
        Pruning,
        Fertilizing,
        Mulching,
        PlantingNew,
        Transplanting,
        PestControl,
        DiseaseControl,
        RemovingDead,
        Inspection,
        Other
    }

    /// <summary>
    /// Enumerates task priorities
    /// </summary>
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    /// <summary>
    /// Enumeration of task status values
    /// </summary>
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Cancelled,
        Deferred
    }

    /// <summary>
    /// Enumerates different types of weather-related reasons for tasks
    /// </summary>
    public enum WeatherReason
    {
        Heat,
        Cold,
        Drought,
        HeavyRain,
        Storm,
        Snow,
        SeasonalChange,
        Preventative,
        Regular
    }
}