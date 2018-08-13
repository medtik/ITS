<template>
  <v-content>
    <v-container v-if="pageLoading">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
    <v-container fluid pa-0 v-else>
      <v-layout column>
        <v-flex v-for="location in nearbyLocations" :key="location.id">
          <LocationFullWidth v-bind="location">

          </LocationFullWidth>
        </v-flex>
      </v-layout>
    </v-container>
  </v-content>
</template>

<script>
  import {LocationFullWidth} from "../../common/block";

  export default {
    name: "NearbyLocation",
    components: {
      LocationFullWidth
    },
    computed: {
      pageLoading() {
        return true;
      },
      nearbyLocations() {
        return [];
      }
    },
    mounted(){
      this.$store.dispatch('fetchNearbyLocations');
    },
    onAddLocation(){

    }
  }
</script>

<style scoped>

</style>
