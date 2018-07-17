<template>
  <v-content>
    <v-layout column>
      <!--<v-flex class="headline font-weight-black light-blue white&#45;&#45;text text-xs-center" pa-2>-->
        <!--Kết quả tìm kiếm-->
      <!--</v-flex>-->
      <v-toolbar color="light-blue" flat dark dense>
        <v-toolbar-title class="headline font-weight-black">
          Kết quả tìm kiếm
        </v-toolbar-title>
      </v-toolbar>
      <v-layout column>
        <v-flex pt-3 pb-2 class="title text-xs-center">
          <v-icon>place</v-icon>
          Địa điểm
        </v-flex>
        <v-flex v-for="location in locations"
                :key="location.id"
                elevation-1
                mb-1>
          <LocationFullWidth v-bind="location"/>
        </v-flex>
        <v-flex>
          <v-btn block flat color="secondary">
            Xem thêm
            &nbsp;
            <v-icon small>fas fa-angle-down</v-icon>
          </v-btn>
        </v-flex>
      </v-layout>

      <v-flex pt-5 pb-2 class="title text-xs-center">
        <v-icon>fas fa-suitcase</v-icon>
        &nbsp Chuyến đi
      </v-flex>
      <v-flex v-for="n in 2"
              :key="n+10"
              elevation-1
              my-2>
        <PlanFullWidth :voteCount="n+10"/>
      </v-flex>
      <v-flex>
        <v-btn block flat color="secondary">
          Xem thêm
          &nbsp;
          <v-icon small>fas fa-angle-down</v-icon>
        </v-btn>
      </v-flex>
      <v-flex style="height: 10vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import LocationFullWidth from "../shared/LocationFullWidth";
  import PlanFullWidth from "../shared/PlanFullWidth";
  import _locations from "../location/Locations";

  export default {
    name: "SmartSearchResultView",
    components: {
      LocationFullWidth,
      PlanFullWidth
    },
    data() {
      return {
        locations: _locations
      }
    }
  }
</script>

<style scoped>

</style>
