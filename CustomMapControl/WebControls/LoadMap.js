var map;

function UpdateMap(latitude, longitude, zoomLevel, mapTypeId) {
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: zoomLevel,
        mapTypeId: mapTypeId,
    });
    Microsoft.Maps.Events.addHandler(map, 'viewchange', updateZoomLevel);
}

function updateZoomLevel() {
    sendZoomLevel();
    var zoomLevel = map.getZoom();
    var zoomDiv = document.getElementById('zoomLevel');
    zoomDiv.innerHTML = 'Zoom level: ' + zoomLevel;
    // to test the event handler change its color whenever the zoom level changes
    var letters = "0123456789ABCDEF";
    var color = "#";
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    zoomDiv.style.backgroundColor = color
}

function sendZoomLevel() {
    var val = map.getZoom();
    window.chrome.webview.postMessage(val);
}