<template>
    <div id="map"></div>
</template>

<script>

    export default {

        mounted: function() {

        },
        methods: {

            initMap: function(center, zoom, allevamenti) {

                var options = {
                    center: center,
                    zoom: zoom,
                    mapId: "AIzaSyCSxyGo60OyYxJYi18QSBcOFpMOeErtc44"
                };
                
                var map = new google.maps.Map(document.getElementById("map"), options);

                const infoWindow = new google.maps.InfoWindow();

                for(var i = 0; i < allevamenti.length; i++) {
                    var allevamento = allevamenti[i];

                    // https://developers.google.com/maps/documentation/javascript/advanced-markers/basic-customization
                    let pin = new google.maps.marker.PinElement({
                                background: allevamento.Color,
                                // borderColor: allevamento.Color,
                                // glyphColor: allevamento.Color
                            });

                    var marker = new google.maps.marker.AdvancedMarkerElement({
                        position: { lat: allevamento.Lat, lng: allevamento.Lng },
                        map,
                        title: allevamento.Nome,
                        content: pin.element,
                        id: allevamento.Allevamento_Id,
                        gmpClickable: true
                    });                    

                    google.maps.event.addListener(marker, 'click', function (e) {

                        var allevamento = null;    
                        for(var i = 0; i < allevamenti.length; i++) {
                            if(allevamenti[i].Lat == e.latLng.lat() && allevamenti[i].Lng == e.latLng.lng()) {
                                allevamento = allevamenti[i];
                                break;
                            }
                        }

                        var infoWindow = new google.maps.InfoWindow({ 
                            content:    `<div class="p-2">
                                            <div class="row"><label class="col-3"><b>Allevamento:</b></label><div class="col-9"><b>${allevamento.Allevamento}</b></div></div>
                                            <div class="row"><label class="col-3">Acquirente:</label><div class="col-9">${allevamento.Acquirente}</div></div>
                                            <div class="row"><label class="col-3">Tipo latte:</label><div class="col-9">${allevamento.TipoLatte}</div></div>
                                            <div class="row"><label class="col-3">Gir:</label><div class="col-9">${allevamento.Giro}</div></div>
                                         </div>` 
                        });                        
                        infoWindow.open(map, this);
                    });
                }
            }

        }

    }

</script>

<style>

    #map {
        width: 100%;
        height: 100%;
    }

</style>