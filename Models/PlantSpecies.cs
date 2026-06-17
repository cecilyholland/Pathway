using System;
using System.Collections.Generic;
using Pathway1.Models;
using SQLite;

namespace Pathway.Models
{
    /// <summary>
    /// Represents a plant species with data from the Trefle API
    /// </summary>
    [Table("PlantSpecies")]
    public class PlantSpecies : BaseModel
    {
        // Trefle API reference
        public int TrefleId { get; set; }

        // Basic plant information
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }

        // Images
        public string ImageUrl { get; set; }

        // Edibility information
        public bool Edible { get; set; }
        public string VegetableType { get; set; }
        public string EdiblePart { get; set; }

        // Physical characteristics
        public string FlowerColor { get; set; }
        public string FoliageColor { get; set; }

        // Growth seasons - stored as JSON strings in DB
        public string GrowthMonthsString { get; set; }
        public string BloomMonthsString { get; set; }
        public string FruitMonthsString { get; set; }

        // Growing requirements
        public int? MinimumPrecipitation { get; set; }
        public int? MaximumPrecipitation { get; set; }
        public int? MinimumTemperature { get; set; }
        public int? MaximumTemperature { get; set; }
        public string SoilTexture { get; set; }
        public string SoilHumidity { get; set; }
        public string LightRangeString { get; set; }

        // Additional data from Trefle as JSON
        public string AdditionalDataJson { get; set; }

        // Properties to handle conversion between JSON strings and object collections
        [Ignore]
        public List<string> GrowthMonths
        {
            get => string.IsNullOrEmpty(GrowthMonthsString)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(GrowthMonthsString);
            set => GrowthMonthsString = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [Ignore]
        public List<string> BloomMonths
        {
            get => string.IsNullOrEmpty(BloomMonthsString)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(BloomMonthsString);
            set => BloomMonthsString = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [Ignore]
        public List<string> FruitMonths
        {
            get => string.IsNullOrEmpty(FruitMonthsString)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(FruitMonthsString);
            set => FruitMonthsString = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [Ignore]
        public List<int> LightRange
        {
            get => string.IsNullOrEmpty(LightRangeString)
                ? new List<int>()
                : System.Text.Json.JsonSerializer.Deserialize<List<int>>(LightRangeString);
            set => LightRangeString = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}