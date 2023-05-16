using System.Collections.Generic;

namespace CustomMapControl.Models
{
    public class MapLayer
    {
        public MapLayer() { }

        public MapLayer(string layerName, IList<MapElement> mapElements)
        {
            LayerName = layerName;
            MapElements = mapElements;
        }

        public string LayerName { get; set; }

        public IList<MapElement> MapElements { get; set; }
    }
}