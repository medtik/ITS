<template>
  <v-content>
    <v-layout column>
      <v-toolbar color="light-blue" flat dark dense>
        <v-toolbar-title class="headline font-weight-black">
          Kết quả tìm kiếm
        </v-toolbar-title>
      </v-toolbar>
      <v-layout column class="grey lighten-4" py-3>
        <!--<v-flex py-3 class="title text-xs-center white">-->
        <!--<v-icon>place</v-icon>-->
        <!--Địa điểm-->
        <!--</v-flex>-->
        <v-flex v-for="location in locations"
                :key="location.id"
                elevation-2
                mb-2
                py-2
                class="white">
          <LocationFullWidth v-bind="location"
                             :isSearchResult="true"
                             @save="onSave"/>
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
    <ChoosePlanDialog
      :dialog="dialog.choosePlan"
      @select="onPlanSelect"
      @close="dialog.choosePlan = false"/>
    <SuccessDialog
      v-bind="success"
      @close="success.dialog = false"
    ></SuccessDialog>
  </v-content>
</template>

<script>
  import {
    LocationFullWidth,
    PlanFullWidth,
    SuccessDialog
  } from "../../common/block";

  import {ChoosePlanDialog} from "../../common/input";

  import {mapGetters} from "vuex";

  export default {
    name: "SmartSearchResultView",
    components: {
      LocationFullWidth,
      PlanFullWidth,
      ChoosePlanDialog,
      SuccessDialog
    },
    data() {
      return {

        selectedLocation: '',
        selectedPlan: '',
        requestMessage: '',

        dialog: {
          choosePlan: false,
        },
        success: {
          dialog: false,
          message: true,
        }
      }
    },
    computed: {
      ...mapGetters('smartSearch', {
        locations: 'searchResult'
      })
    },
    methods: {
      onSave(locationId) {
        this.selectedLocation = _.find(this.locations, (location) => {
          return location.id == locationId;
        });
        this.dialog.choosePlan = true
      },
      onPlanSelect(plan) {
        this.dialog.choosePlan = false;

        this.$store.dispatch('plan/addLocationToPlan', {
          locationId: this.selectedLocation.id,
          planId: plan.id
        })
          .then(value => {
            this.success = {
              dialog: true,
              message: 'Thêm vào chuyến đi thành công'
            }
          })
      }
    }
  }
</script>

<style scoped>

</style>
