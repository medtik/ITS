<template>
  <v-content>
    <v-layout column>
      <v-toolbar color="light-blue" flat dark dense>
        <v-toolbar-title class="headline font-weight-black">
          Kết quả tìm kiếm
        </v-toolbar-title>
      </v-toolbar>
      <v-layout column class="grey lighten-4" py-3>
        <v-flex py-3 class="title text-xs-center white">
          <v-icon>place</v-icon>
          Địa điểm
        </v-flex>
        <v-flex v-for="location in locations"
                :key="location.id"
                elevation-2
                mb-2
                class="white">
          <LocationFullWidth v-bind="location"/>
        </v-flex>
        <v-flex>
          <v-btn block flat color="secondary" >
            Xem thêm
            &nbsp;
            <v-icon small>fas fa-angle-down</v-icon>
          </v-btn>
        </v-flex>
      </v-layout>
      <v-divider></v-divider>
      <v-layout column class="grey lighten-4" py-3>
        <v-flex py-3 class="title text-xs-center white">
          <v-icon>fas fa-suitcase</v-icon>
          &nbsp Chuyến đi
        </v-flex>
        <v-flex v-for="n in 2"
                :key="n+10"
                elevation-2
                mb-2
                class="white">
          <PlanFullWidth :voteCount="n+10"/>
        </v-flex>
        <v-flex>
          <v-btn block flat color="secondary">
            Xem thêm
            &nbsp;
            <v-icon small>fas fa-angle-down</v-icon>
          </v-btn>
        </v-flex>
      </v-layout>
      <v-flex style="height: 10vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import LocationFullWidth from "../../common/block/LocationFullWidth";
  import PlanFullWidth from "../../common/block/PlanFullWidth";
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
