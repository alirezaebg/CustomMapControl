var map;

function UpdateMap(latitude, longitude, zoomLevel, mapTypeId) {
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: zoomLevel,
        mapTypeId: mapTypeId,
        navigationBarMode: Microsoft.Maps.NavigationBarMode.minified,
    });

    // add a pushpin
    addPushpin(map.getCenter());

    Microsoft.Maps.Events.addHandler(map, 'viewchangeend', updateMapZoom);
    Microsoft.Maps.Events.addHandler(map, 'viewchangeend', updateMapCenter);
}

function updateMapZoom() {
    var zoomLevel = map.getZoom();
    var zoomDiv = document.getElementById('zoomLevel');
    zoomDiv.innerHTML = 'Zoom level: ' + zoomLevel;
    sendMapUpdate()
}

function updateMapCenter() {
    var mapCenter = map.getCenter();
    var centerDiv = document.getElementById('center');
    centerDiv.innerHTML = 'center: ' + mapCenter;
    sendMapUpdate()
}


function sendMapUpdate() {
    var zoomLevel = map.getZoom();
    var center = map.getCenter();

    const centerLocation = {
        "lattitude": center.latitude,
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