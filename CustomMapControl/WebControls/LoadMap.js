var map;
var zoomChangedTimeout;

function UpdateMap(latitude, longitude, zoomLevel, mapTypeId) {
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: zoomLevel,
        mapTypeId: mapTypeId,
    });
    Microsoft.Maps.Events.addHandler(map, 'viewchange', updateZoomLevel);
}

function updateZoomLevel() {
    var zoomLevel = map.getZoom();
    var zoomDiv = document.getElementById('zoomLevel');
    zoomDiv.innerHTML = 'Zoom level: ' + zoomLevel;
    sendZoomLevel();
}

function sendZoomLevel() {
    var val = map.getZoom();
    // Debounce the zoom event
    if (val % 0.5 == 0) {
        window.chrome.webview.postMessage({ "type": "zoomChanged", "data": val });
    }
}