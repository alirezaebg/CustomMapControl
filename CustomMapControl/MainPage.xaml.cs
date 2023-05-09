using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CustomMapControl
{
    public enum MapType
    {
        aerial,
        birdseye,
        ordnanceSurvey,
        road,
        streetside,
        canvasDark,
        canvasLight,
        grayscale,
        mercator,
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

        private void InitializeMap()
        {
            MapCenter = new Geopoint(new BasicGeoposition() { Latitude = 47.6, Longitude = -122.349 });
            ZoomLevel = 10;
            MapTypeId = MapType.road.ToString();
        }
    }
}