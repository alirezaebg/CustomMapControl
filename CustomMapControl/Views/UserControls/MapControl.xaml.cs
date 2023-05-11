﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
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

        public MapControl()
        {
            InitializeComponent();
            InitializeAsync();
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

        private async void InitializeAsync()
        {
            await WebView2.EnsureCoreWebView2Async();
            StorageFile htmlFile = await LoadStringFromPackageFileAsync("LoadMap.html");
            WebView2.CoreWebView2.Navigate(htmlFile.Path);
        }

        private static async Task<StorageFile> LoadStringFromPackageFileAsync(string name)
        {
            // Using the storage classes to read the content from a file as a string.
            StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///WebControls/{name}"));
            return f;
        }

        private void WebView2_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            UpdateMap();
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

        private void UpdateMap()
        {
            RunJavaScriptFunction("UpdateMap", Center.Position.Latitude, Center.Position.Longitude, ZoomLevel, $"Microsoft.Maps.MapTypeId.{MapTypeId}");
        }

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
            mapControl.UpdateMap();
        }

        private static void OnMapTypeIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = (MapControl)d;
            mapControl.MapTypeId = (string)e.NewValue;
            mapControl.UpdateMap();
        }
    }
}