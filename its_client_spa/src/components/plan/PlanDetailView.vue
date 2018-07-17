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
      <v-flex class="title" my-2>
        Chưa lên lịch
      </v-flex>
      <v-flex elevation-1 my-1>
        <v-layout column>
          <v-flex class="subheading font-weight-medium">
            SADASD
          </v-flex>
          <v-divider></v-divider>
          <v-flex class="body-1" px-2>
            SADASD
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex v-for="location in plan.locations"
              :key="location.id"
              elevation-1 my-1>
        <LocationFullWidth v-bind="location"/>
      </v-flex>
      <v-flex class="title" my-2>
        Ngày 1
      </v-flex>
      <v-flex v-for="location in plan.locations"
              :key="location.id"
              elevation-1 my-1>
        <LocationFullWidth v-bind="location"/>
      </v-flex>
      <v-flex style="height: 15vh">
        <!--Holder-->
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
