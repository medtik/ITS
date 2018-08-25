<template>
  <v-content>
    <v-container v-if="!pageLoading" pa-0 fluid>
      <div class="grid-layout desktop">
        <div style="grid-area: photo">
          <img :src="area.coverPhoto" style="width: 100%; height: 100%"/>
        </div>
        <v-layout style="grid-area: info-restaurant" class="light-blue white--text headline" align-center justify-start pl-4>
          <v-icon class="white--text">fas fa-utensils</v-icon>
          &nbsp; {{restaurantCount}} nhà hàng
        </v-layout>
        <v-layout style="grid-area: info-hotel" class="light-blue white--text headline" align-center justify-start pl-4>
          <v-icon class="white--text">fas fa-hotel</v-icon>
          &nbsp; {{hotelCount}} nơi ở
        </v-layout>
        <v-layout style="grid-area: info-activity" class="light-blue white--text headline" align-center justify-start pl-4>
          <v-icon class="white--text">fas fa-university</v-icon>
          &nbsp; {{activityCount}} dịch vụ giải trí
        </v-layout>
        <v-layout style="grid-area: info-service" class="light-blue white--text headline" align-center justify-start pl-4>
          <v-icon class="white--text">fas fa-gas-pump</v-icon>
          &nbsp; {{serviceCount}} dịch vụ công cộng
        </v-layout>
      </div>
      <v-layout column mx-2>
        <v-flex my-4>
          <div class="title">
            Địa điểm nổi bật
          </div>
          <v-layout style="overflow-y: auto;">
            <v-flex v-for="location in area.featuredLocation" :key="location.id" mx-2 mt-2>
              <LocationCard v-bind="location"/>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex justify-start mt-5 v-if="area.featuredPlan && area.featuredPlan.length > 0">
          <div class="title">Các chuyến đi nổi bật</div>
          <div style="overflow-x: auto;">
            <v-layout row my-1>
              <v-flex v-for="plan in area.featuredPlan"
                      :key="plan.id"
                      shrink>
                <PlanCard v-bind="plan"/>
              </v-flex>
            </v-layout>
          </div>
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
  import {mapState} from "vuex";

  export default {
    name: "AreaDetail",
    components: {
      ParallaxHeader,
      LocationCard,
      PlanCard
    },
    data() {
      return {
        areaId
      }
    },
    computed: {
      ...mapState('area', {
        pageLoading: state => state.loading.detailedArea,
        area: state => state.detailedArea
      }),
      restaurantCount() {
        if (this.area) {
          for (let category of this.area.locations) {
            if (category.name == "Ăn uống") {
              return category.counter;
            }
          }
        }
      },
      hotelCount() {
        if (this.area) {
          for (let category of this.area.locations) {
            if (category.name == "Nơi ở") {
              return category.counter;
            }
          }
        }

      },
      activityCount() {
        if (this.area) {
          let count = 0;
          for (let category of this.area.locations) {
            if (category.name == "Mua sắm" ||
              category.name == "Địa điểm thăm quan" ||
              category.name == "Giải trí") {
              count += category.counter;
            }
          }
          return count;
        }

      },
      serviceCount() {
        if (this.area) {
          let count = 0;
          for (let category of this.area.locations) {
            if (category.name == "Tiền tệ" ||
              category.name == "Trạm xăng" ||
              category.name == "Trụ sở ban ngành" ||
              category.name == "Other") {
              count += category.counter;
            }
          }
          return count;
        }

      }
    },
    created() {
      const {
        id
      } = this.$route.query;

      this.areaId = id;
    },
    mounted() {
      this.$store.dispatch('area/fetchDetailedArea', {id: this.areaId});
    }
  }
</script>

<style scoped>
  .grid-layout{
    display: grid;
  }

  .grid-layout.desktop {
    grid-template-columns: 50% auto;
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
    grid-template-areas: "photo" "info-restaurant" "info-hotel" "info-activity" "info-service" "info-service";
  }
</style>
