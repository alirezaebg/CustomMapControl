function GetMap() {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        center: new Microsoft.Maps.Location(51.50632, -0.12714),
        zoom: 12,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
    });
}