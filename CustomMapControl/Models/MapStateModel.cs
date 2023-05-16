using System.Text.Json.Serialization;

namespace CustomMapControl.Models
{
    public class MapStateModel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public MapStateData Data { get; set; }
    }

    public class MapStateData
    {
        [JsonPropertyName("mapZoomLevel")]
        public double ZoomLevel { get; set; }

        [JsonPropertyName("mapCenter")]
        public Coordinate MapCenter { get; set; }
    }
    
    public class Coordinate
    {
        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}