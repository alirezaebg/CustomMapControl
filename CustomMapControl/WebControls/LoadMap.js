function GetMap(latitude, longitude) {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: 12,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
    });
}