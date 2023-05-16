using CustomMapControl.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomMapControl.Views.UserControls
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

        public static readonly DependencyProperty LayersProperty = DependencyProperty.Register(
            "Layers",
            typeof(IList<MapLayer>),
            typeof(MapControl),
            new PropertyMetadata(null, OnMapLayersPropertyChanged));

        public MapControl()
        {
            InitializeComponent();
            InitializeAsync();
        }

        public event TypedEventHandler<MapControl, object> ZoomLevelChanged;

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

        public IList<MapLayer> Layers
        {
            get => (IList<MapLayer>)GetValue(LayersProperty);
            set => SetValue(LayersProperty, value);
        }

        private async void InitializeAsync()
        {
            await WebView2.EnsureCoreWebView2Async();
            ListenToMapViewChangeEvent();
            StorageFile htmlFile = await LoadStringFromPackageFileAsync("LoadMap.html");
            WebView2.CoreWebView2.Navigate(htmlFile.Path);
            WebView2.CoreWebView2.OpenDevToolsWindow();
        }

        private static async Task<StorageFile> LoadStringFromPackageFileAsync(string name)
        {
            // Using the storage classes to read the content from a file as a string.
            StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///WebControls/{name}"));
            return f;
        }

        private async void WebView2_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            // might not be necessary to ensure the following line?
            await WebView2.EnsureCoreWebView2Async();
            UpdateMap();
        }

        private void ListenToMapViewChangeEvent()
        {
            try
            {
                WebView2.WebMessageReceived += WebView2_WebMessageReceived;
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        private void WebView2_WebMessageReceived(WebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            // Listen for the message that the zoom level has changed
            if (args.WebMessageAsJson != null)
            {
                var message = JsonSerializer.Deserialize<MapStateModel>(args.WebMessageAsJson);
                if (message.Type == "mapStateChanged")
                {
                    ZoomLevel = message.Data.ZoomLevel;
                    Coordinate mapCenterCoordinate = message.Data.MapCenter;
                    Center = new Geopoint(new BasicGeoposition() { Latitude = mapCenterCoordinate.Latitude, Longitude = mapCenterCoordinate.Longitude });
                    Debug.WriteLine($"Zoom level: {ZoomLevel}");
                }
            }
        }

        private void OnZoomLevelChanged()
        {
            ZoomLevelChanged?.Invoke(this, null);
        }

        private void UpdateMap()
        {
            RunJavaScriptFunction("updateMap", Center.Position.Latitude, Center.Position.Longitude, ZoomLevel, $"Microsoft.Maps.MapTypeId.{MapTypeId}");

            string json = JsonSerializer.Serialize(Layers);
            RunJavaScriptFunction("addPushpinsFromLayers", json);
        }

        private async void RunJavaScriptFunction(string functionName, params object[] args)
        {
            try
            {
                string script = $"{functionName}({string.Join(",", args)})";
                string result = await WebView2.ExecuteScriptAsync(script);
                // process the result if needed
            }
            catch (Exception ex)
            {
                // handle exceptions
            }
        }

        private static void OnCenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.Center = (Geopoint)e.NewValue;
        }

        private static void OnZoomLevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.ZoomLevel = (double)e.NewValue;
            mapControl.OnZoomLevelChanged();
        }

        private static void OnMapTypeIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.MapTypeId = (string)e.NewValue;
        }

        private static void OnMapLayersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.Layers = (IList<MapLayer>)e.NewValue;
        }
    }
}