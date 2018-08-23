<template>
  <v-content>
    <v-container v-if="!pageLoading" pa-0 fluid>
      <v-layout>

      </v-layout>
      <v-layout column mx-2>
        <v-flex my-4>
          <div class="title">
            Địa điểm nổi bật
          </div>
          <v-layout style="overflow-y: auto;">
            <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
              <LocationCard/>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex my-4>
          <div class="title">
            Chuyến đi nổi bật
          </div>
          <v-layout style="overflow-y: auto;">
            <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
              <PlanCard/>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex style="height: 15vh">
          <!--Holder-->
        </v-flex>
      </v-layout>
    </v-container>
    <v-container class="text-xs-center" v-if="pageLoading">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
  </v-content>
</template>

<script>
  import ParallaxHeader from "../../common/layout/ParallaxHeader";
  import PlanCard from "../../common/card/PlanCard";
  import LocationCard from "../../common/card/LocationCard";

  export default {
    name: "AreaDetail",
    components: {
      ParallaxHeader,
      LocationCard,
      PlanCard
    },
    data() {
      return {
        area: {
          name: 'Thành phố Hồ Chí Minh',
          photo: ''
        }
      }
    },

  }
</script>

<style scoped>
  .grid-layout {
    display: grid;
    grid-row-gap: 0.5rem;
  }
  .grid-layout.desktop {
    grid-template-columns: 600px auto;
    grid-template-rows: auto auto auto auto;
    grid-template-areas:
     "photo info-restaurant"
     "photo info-hotel"
     "photo info-activity"
     "photo info-service";
  }

  .grid-layout.mobile {
    grid-template-columns: 100%;
    grid-template-rows: auto auto auto auto auto;
    grid-template-areas:
      "photo"
      "info-restaurant"
      "info-hotel"
      "info-activity"
      "info-service"
      "info-service";
  }
</style>
