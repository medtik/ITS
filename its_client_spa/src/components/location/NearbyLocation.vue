<template>
  <v-content>
    <v-toolbar dark flat color="light-blue darken-2" dense>

    </v-toolbar>
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
<!--TODO test this-->
<script>
  import {LocationFullWidth} from "../../common/block";
  import {mapState} from "vuex";


  export default {
    name: "NearbyLocation",
    components: {
      LocationFullWidth
    },
    data() {
      return {
        long,
        lat,
        title,
      }
    },
    computed: {
      ...mapState('location', {
        pageLoading: state => state.loading.nearbyLocations,
        nearbyLocations: state => state.nearbyLocations
      })
    },
    created() {
      const {
        long,
        lat,
        title
      } = this.$route.query;

      this.long = long;
      this.lat = lat;
    },
    mounted() {
      this.$store.dispatch('location/fetchNearbyLocations', {
        long: this.long,
        lat: this.lat
      });
    },
  }
</script>

<style scoped>

</style>
