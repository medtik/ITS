<template>
  <GmapMap
    :center="{lat:10.8290990, lng:106.6720520}"
    :zoom="18"
    map-type-id="terrain"
    style="width: 500px; height: 500px"
  >
    <GmapMarker
      :key="index"
      v-for="(m, index) in markers"
      :position="m.position"
      :clickable="true"
      :draggable="true"
      @click="center=m.position"
    />
  </GmapMap>
</template>

<script>
  export default {
    name: "LocationOnMapView",
    data() {
      return {
        markers: []
      }
    }
  }
</script>

<style scoped>

</style>
