using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CustomMapControl.XamlControls
{
    public sealed partial class MapControl : UserControl
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Center",
            typeof(Geopoint),
            typeof(MapControl),
            new PropertyMetadata(null, OnCenterPropertyChanged));

        public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register(
            "ZoomLevel",
            typeof(double),
            typeof(MapControl),
            new PropertyMetadata(null, OnZoomLevelPropertyChanged));

        public static readonly DependencyProperty MapTypeIdProperty = DependencyProperty.Register(
            "MapTypeId",
            typeof(string),
            typeof(MapControl),
            new PropertyMetadata("road", OnMapTypeIdPropertyChanged));

        public MapControl()
        {
            InitializeComponent();
            webView.Source = new Uri("ms-appx-web:///WebControls/LoadMap.html");
        }

        public Geopoint Center
        {
            get => (Geopoint)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public double ZoomLevel
        {
            get => (double)GetValue(ZoomLevelProperty);
            set => SetValue(ZoomLevelProperty, value);
        }

        public string MapTypeId
        {
            get => (string)GetValue(MapTypeIdProperty);
            set => SetValue(MapTypeIdProperty, value);
        }

        public event TypedEventHandler<MapControl, object> ZoomLevelChanged;

        private static void OnCenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.Center = (Geopoint)e.NewValue;
            mapControl.UpdateMap();
        }

        private static void OnZoomLevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.ZoomLevel = (double)e.NewValue;
            mapControl.OnZoomLevelChanged();
            mapControl.UpdateMap();
        }

        private static void OnMapTypeIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.MapTypeId = (string)e.NewValue;
            mapControl.UpdateMap();
        }

        private void OnZoomLevelChanged()
        {
            ZoomLevelChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateMap()
        {
            if (Center != null)
            {
                webView.NavigateToString("<html>" +
                    "<head>" +
                    "<title>loadmapwithoptionsHTML</title>" +
                    "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<style type='text/css'>body{margin:0;padding:0;overflow:hidden;font-family:'Segoe UI',Helvetica,Arial,Sans-Serif}</style>" +
                    "</head>" +
                    "<body>" +
                    "<div id='printoutPanel'></div><div id='myMap' style='width: 100vw; height: 100vh;'></div><script type='text/javascript'>" +
                    "function loadMapScenario() " +
                    "{var map = new Microsoft.Maps.Map(document.getElementById('myMap'), " +
                    "{/* No need to set credentials if already passed in URL */" +
                    "center: new Microsoft.Maps.Location(" + Center.Position.Latitude + "," + Center.Position.Longitude + ")," +
                    "mapTypeId: Microsoft.Maps.MapTypeId." + MapTypeId + "," +
                    "zoom:" + ZoomLevel + "});" +
                    "}</script>" +
                    "<script type='text/javascript' src='https://www.bing.com/api/maps/mapcontrol?key=wZ2tlfyoMyWywCI9lYEt~bU78_qOMH1YwIYINYUcj-Q~AgZM34duJQYFJ_BsGglzNOjx44yNHz0-oR6xi95bFeLwpnkV2PWRpxsTD6CSvFyK&callback=loadMapScenario' async defer></script>" +
                    "</body>" +
                    "</html>");
            }
        }
    }
}