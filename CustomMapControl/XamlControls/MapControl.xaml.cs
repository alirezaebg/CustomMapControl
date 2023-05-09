using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomMapControl.XamlControls
{
    public sealed partial class MapControl : UserControl
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Items",
            typeof(Geopoint),
            typeof(MapControl),
            new PropertyMetadata(null, null));

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
    }
}