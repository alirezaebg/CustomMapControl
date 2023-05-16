using CustomMapControl.Models;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using MapElement = CustomMapControl.Models.MapElement;
using MapLayer = CustomMapControl.Models.MapLayer;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CustomMapControl
{
    public enum MapType
    {
        aerial,
        birdseye,
        road,
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitializeMap();
        }

        public Geopoint MapCenter { get; set; }

        public double ZoomLevel { get; set; }

        public string MapTypeId { get; set; }

        public IList<MapLayer> MapLayers { get; set;}

        private void InitializeMap()
        {
            MapCenter = new Geopoint(new BasicGeoposition() { Latitude = 47.6, Longitude = -122.349 });
            ZoomLevel = 10;
            MapTypeId = MapType.road.ToString();
            MapLayers = GenerateMapLayers();
        }

        private IList<MapLayer> GenerateMapLayers()
        {
            IList<MapLayer> layers = new List<MapLayer>();

            MapLayer pushpinLayer = new MapLayer("Pushpins", new List<MapElement>());

            MapElement pushpin1 = new MapElement(new Coordinate(47.4, -122.449));
            MapElement pushpin2 = new MapElement(new Coordinate(47.8, -122.249));

            pushpinLayer.MapElements.Add(pushpin1);
            pushpinLayer.MapElements.Add(pushpin2);

            layers.Add(pushpinLayer);

            return layers;
        }

        private void MapControl_ZoomLevelChanged(Views.UserControls.MapControl sender, object args)
        {
        }
    }
}