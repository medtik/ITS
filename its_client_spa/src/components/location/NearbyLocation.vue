<template>
  <v-content>
    <v-toolbar dark flat color="light-blue darken-2" dense>
      <v-toolbar-title>
        {{title}}
      </v-toolbar-title>
    </v-toolbar>
    <v-container v-if="pageLoading" class="text-xs-center">
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
  import {mapState} from "vuex";
  import _ from "lodash";


  export default {
    name: "NearbyLocation",
    components: {
      LocationFullWidth
    },
    data() {
      return {
        long: undefined,
        lat: undefined,
        title: undefined,
      }
    },
    computed: {
      ...mapState('location', {
        pageLoading: state => state.loading.nearbyLocations,
        nearbyLocations: state => _.filter(state.nearbyLocations, (location) => {return location.range > 0})
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
      this.title = title;
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
