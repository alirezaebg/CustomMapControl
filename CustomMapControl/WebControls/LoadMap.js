var map;

function updateMap(latitude, longitude, zoomLevel, mapTypeId) {
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: zoomLevel,
        mapTypeId: mapTypeId,
        navigationBarMode: Microsoft.Maps.NavigationBarMode.minified,
    });

    // add a pushpin
    addPushpin(map.getCenter());

    Microsoft.Maps.Events.addHandler(map, 'viewchangeend', updateMapZoom);
    Microsoft.Maps.Events.addHandler(map, 'viewchangeend', sendMapUpdate);
}

function updateMapZoom() {
    var zoomLevel = map.getZoom();
    var zoomDiv = document.getElementById('zoomLevel');
    zoomDiv.innerHTML = 'Zoom level: ' + zoomLevel;
    sendMapUpdate()
}

function sendMapUpdate() {
    var zoomLevel = map.getZoom();
    var center = map.getCenter();

    const centerLocation = {
        "latitude": center.latitude,
        "longitude": center.longitude,
    }

    const data = {
        "mapZoomLevel": zoomLevel,
        "mapCenter": centerLocation,
    }
    const message = {
        "type": "mapStateChanged",
        "data": data,
    };

    window.chrome.webview.postMessage(message);
}

function addPushpin(center) {
    var pushpin = new Microsoft.Maps.Pushpin(center, null);
    pushpin.setOptions({ icon: '../Assets/mappin.png', anchor: new Microsoft.Maps.Point(12, 39) })
    map.entities.push(pushpin);
}

function addPushpinsFromLayers(layers) {
    for (var i = 0; i < layers.length; i++) {
        // new layer
        var layer = new Microsoft.Maps.Layer();
        var mapElements = layers[i].MapElements;
        // iterate through map elements
        for (k = 0; k < mapElements.length; k++) {
            var geopoint = new Microsoft.Maps.Location(mapElements[k].Coordinate.latitude, mapElements[k].Coordinate.longitude);
            var pin = new Microsoft.Maps.Pushpin(geopoint);
            pin.setOptions({ icon: '../Assets/mappin.png', anchor: new Microsoft.Maps.Point(12, 39) })
            layer.add(pin);
        }
        map.layers.insert(layer);
    }
}