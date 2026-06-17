using System;
using Pathway1.Models;
using SQLite;

namespace Pathway.Models
{
    /// <summary>
    /// Represents a record of work performed on plants or areas
    /// </summary>
    [Table("WorkLogs")]
    public class WorkLog : BaseModel
    {
        // Related entities
        public string TaskId { get; set; }
        public string PlantId { get; set; }
        public string AreaId { get; set; }
        public string UserId { get; set; }

        // Work details
        public string ActivityType { get; set; }
        public string Notes { get; set; }
        public TimeSpan? Duration { get; set; }

        // Weather conditions during work
        public double? Temperature { get; set; }
        public string WeatherCondition { get; set; }

        // Materials used
        public string MaterialsUsed { get; set; }
        public string EquipmentUsed { get; set; }

        // Media
        public string PhotoPath { get; set; }
    }
}