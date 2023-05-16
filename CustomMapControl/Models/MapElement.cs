using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace CustomMapControl.Models
{
    public class MapElement
    {
        public MapElement() { }

        public MapElement(Coordinate coordinate) 
        {
            Coordinate = coordinate;
        }

        public MapElement(string title, Coordinate coordinate, string iconImageUrl, Point anchorPoint)
        {
            Title = title;
            Coordinate = coordinate;
            IconImageURL = iconImageUrl;
            AnchorPoint = anchorPoint;
        }

        public string Title { get; set; } = string.Empty;

        public Coordinate Coordinate { get; set; }

        public string IconImageURL { get; set; } = string.Empty;

        public Point AnchorPoint { get; set; } = new Point(0, 0);
    }
}