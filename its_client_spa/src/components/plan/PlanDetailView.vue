<template>
  <v-content>
    <v-toolbar flat color="light-blue" dark>
      <v-layout justify-center>
        <v-toolbar-title>
          {{plan.title}}
        </v-toolbar-title>
      </v-layout>
    </v-toolbar>
    <v-layout column>
      <v-flex v-for="location in plan.locations" :key="location.id">
        <LocationFullWidth v-bind="location"/>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import _locations from "../location/Locations";
  import LocationFullWidth from "../shared/LocationFullWidth";

  export default {
    name: "PlanDetailView",
    components: {
      LocationFullWidth
    },
    data() {
      return {
        plan: {
          title: 'Plan ABC',
          locations: _locations,
        }
      }
    }
  }
</script>

<style scoped>

</style>
