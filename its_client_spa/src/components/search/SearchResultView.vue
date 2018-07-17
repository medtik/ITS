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
        <v-spacer></v-spacer>
        <v-toolbar-items slot="extension">
            <v-btn flat>
              <v-icon>star</v-icon>
            </v-btn>
            <v-btn flat>
              <v-icon>place</v-icon>
            </v-btn>
            <v-btn flat>
              <v-icon>fas fa-suitcase</v-icon>
            </v-btn>
        </v-toolbar-items>
      </v-toolbar>
      <v-layout column>
        <v-flex pt-3 pb-2 class="title light-blue accent-3 white--text">
          <v-icon color="white">place</v-icon>
          Địa điểm
        </v-flex>
        <v-flex v-for="location in locations"
                :key="location.id"
                mb-1>
          <v-divider/>
          <LocationFullWidth v-bind="location"/>
          <v-divider/>
        </v-flex>
        <v-flex>
          <v-btn block flat color="secondary">
            Xem thêm
            &nbsp;
            <v-icon small>fas fa-angle-down</v-icon>
          </v-btn>
        </v-flex>
      </v-layout>

      <v-flex pt-4 pb-2 class="title light-blue white--text">
        <v-icon color="white">fas fa-suitcase</v-icon>
        &nbsp Chuyến đi
      </v-flex>
      <v-flex v-for="n in 4"
              :key="n+10"
              mb-1>
        <v-divider/>
        <PlanFullWidth :voteCount="n+10"/>
        <v-divider/>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import LocationFullWidth from "../shared/LocationFullWidth";
  import PlanFullWidth from "../shared/PlanFullWidth";

  export default {
    name: "SmartSearchResultView",
    components: {
      LocationFullWidth,
      PlanFullWidth
    },
    data() {
      return {
        locations: [
          {
            id: 1,
            text: 'abvc',
            primaryPhoto: 'https://picsum.photos/600/600',
            rating: 4.4,
            ratingCount: 12,
            address: '263/11 gì đó gì đó',
            reason: 'Vì bạn chọn "Đi phượt"'
          },
          {
            id: 2,
            text: 'abdsavc',
            primaryPhoto: 'https://picsum.photos/600/600',
            rating: 3.5,
            ratingCount: 6,
            address: '263/11 gì đó gì đó',
            reason: 'Vì bạn chọn "Đi phượt"'
          },
          {
            id: 3,
            text: 'abzvc',
            primaryPhoto: 'https://picsum.photos/600/600',
            rating: 4.0,
            ratingCount: 7,
            address: '263/11 gì đó gì đó',
            reason: 'Vì bạn chọn "Đi phượt"'
          },
        ]
      }
    }
  }
</script>

<style scoped>

</style>
