<template>
    <div id="map"></div>
</template>

<script>

    export default {

        mounted: function() {

        },
        methods: {

            initMap: function(center, zoom, prelievi) {
                var options = {
                    center: center,
                    zoom: zoom
                };
                
                var map = new google.maps.Map(document.getElementById("map"), options);

                for(var i = 0; i < prelievi.length; i++) {
                    var p = prelievi[i];

                    new google.maps.Marker({
                        position: { lat: p.Lat, lng: p.Lng },
                        map,
                        label: 'P'
                    });

                    new google.maps.Marker({
                        position: { lat: p.Allevamento_Lat, lng: p.Allevamento_Lng },
                        map,
                        label: 'A'
                    });

                    if(p.Trasbordo) {
                        new google.maps.Marker({
                            position: { lat: p.Trasbordo.Lat, lng: p.Trasbordo.Lng },
                            map,
                            label: 'T'
                        }); 
                    }

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