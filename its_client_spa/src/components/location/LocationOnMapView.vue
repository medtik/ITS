<template>
  <GmapMap
    :center="{lat:10.8290990, lng:106.6720520}"
    :zoom="18"
    map-type-id="terrain"
    style="width: 100%; height: 95vh"
  >
    <GmapMarker
      v-for="(marker, index) in markers" :key="index"
      :position="google && new google.maps.LatLng(marker.lat, marker.lng)"
      :icon="{size:google && new google.maps.Size(50, 50),url:'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGAAAABgCAQAAABIkb+zAAABjUlEQVR4Ae3WNVIFQRSF4cY9x9197i12QMqL0AXgEctAE9x1H/gGIMEiXBPch/Di2tWPrjr/ZKP9jSuEEEJIkth9Oele/nUZEY6HmmiCt2R7awCcx4N8KVtbBXCyaUa2swyQGsTtfM+upYDCIt6QLawDUI2cewsB1CjrWgjgMnq0GMDxfMWuxQCaY9diANWwazPAl3asBlAJu3YDpiwH8KrtgFvbAe7/AwAAAAAAAAAAAADQGbv6Jjo3D1jTCeAN84AZnQBaNA/o0AoYNg5winUCHI9xQGoQnWsDXOYEGgNI1K0LQN1KMgfgKLrRMvwbjjIKkKhL//k3CuBQXvozYDk2xDhA4ng++dPwT3LjlGQOIHEeHfz65jksyFGSWYBUmEjrvwJsOAlKMg+QMiKohe9+NPhbas0PU8rrAKkwjUa+94dKZzRSmKYkbwOkxGCninto9oMH+4hmudepSgxWkj6A3nQdAgAAADC6dwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIYQQegIwyvChyzBOyAAAAABJRU5ErkJggg=='}"
    />
  </GmapMap>
</template>

<script>
  import {gmapApi} from 'vue2-google-maps'

  export default {
    name: "LocationOnMapView",
    data() {
      return {
        markers: [
          {lat: 10.8290990, lng: 106.6720530},
          {lat: 10.8290360, lng: 106.6737590},
          {lat: 10.8290990, lng: 106.6737590},
          {lat: 10.8290360, lng: 106.6720530},

        ]
      }
    },
    computed: {
      google: gmapApi
    },
  }
</script>

<style scoped>

</style>
