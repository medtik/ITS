<template>
  <v-content>
    <v-layout column>
      <v-toolbar color="light-blue darken-2" flat dark dense>
        <v-toolbar-title class="headline font-weight-black">
          Kết quả tìm kiếm
        </v-toolbar-title>
      </v-toolbar>
      <v-layout column class="grey lighten-4" py-3>
        <!--CONTEXT-->
        <v-layout justify-center>
          <v-flex shrink>
            <ChoosePlanDaySection
              v-if="isShowPlanSection"
            ></ChoosePlanDaySection>
          </v-flex>
        </v-layout>


        <!--RESULT-->
        <v-flex v-for="location in locations"
                :key="location.id"
                elevation-2
                mb-2
                py-2
                class="white">
          <LocationFullWidth v-bind="location"
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
    SuccessDialog,
  } from "../../common/block";
  import ChoosePlanDaySection from "./ChoosePlanDaySection"
  import {mapGetters} from "vuex";

  export default {
    name: "SmartSearchResultView",
    components: {
      LocationFullWidth,
      PlanFullWidth,
      SuccessDialog,
      ChoosePlanDaySection
    },
    data() {
      return {
        selectedLocation: '',
        selectedPlan: '',
        requestMessage: '',
        success: {
          dialog: false,
          message: true,
        }
      }
    },
    computed: {
      ...mapGetters('smartSearch', {
        locations: 'searchResult'
      }),
      ...mapGetters('authenticate', {
        isShowPlanSection: 'isLoggedIn'
      })
    },
    methods: {
      onSave(locationId) {
        this.selectedLocation = _.find(this.locations, (location) => {
          return location.id == locationId;
        });
        this.dialog.choosePlan = true
      },
    }
  }
</script>

<style scoped>

</style>
