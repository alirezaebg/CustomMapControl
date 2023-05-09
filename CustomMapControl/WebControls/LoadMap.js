function GetMap(latitude, longitude) {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: 12,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
    });
}

Microsoft.Maps.Events.addHandler(map, 'viewchangeend', function () {
    // Get the final zoom level of the map after the view change
    var zoomLevel = map.getZoom();

    // Send the zoom level to C# using a hidden field
    document.getElementById('zoomLevel').value = zoomLevel;

    // Call a C# function to handle the zoom level change event
    window.external.ZoomLevelChanged(zoomLevel);
})