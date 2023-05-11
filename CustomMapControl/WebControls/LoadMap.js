function UpdateMap(latitude, longitude, zoomLevel, mapTypeId) {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: zoomLevel,
        mapTypeId: mapTypeId,
    });
}