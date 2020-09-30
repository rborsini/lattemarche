<template>
    <div id="map"></div>
</template>

<script>

    export default {

        mounted: function() {

        },
        methods: {

            initMap: function(center, zoom, markers) {
                var options = {
                    center: center,
                    zoom: zoom
                };
                
                var map = new google.maps.Map(document.getElementById("map"), options);

                for(var i = 0; i < markers.length; i++) {
                    var m = markers[i];

                    const marker = new google.maps.Marker({
                        position: { lat: m.lat, lng: m.lng },
                        map,
                        label: 'P',
                        title: m.title
                    });

                    marker.addListener('click', () => {
                        var infoWindow = new google.maps.InfoWindow({ content: marker.title });
                        infoWindow.open(map, marker);
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