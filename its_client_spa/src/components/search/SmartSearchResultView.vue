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
          <v-flex shrink v-if="isShowPlanSection">
            <ChoosePlanDaySection
              :confirmable="locationsCheck.length > 0"
              :confirmLoading="loading.confirm"
              @select="onSelect"
              @confirm="onConfirm"
              @selectingMode="onSelectingMode"
            ></ChoosePlanDaySection>
          </v-flex>
          <v-flex v-if="!isShowPlanSection"
                  shrink my-3
                  class="text-xs-center title">
            Bạn cần đăng nhập để thêm các địa điểm bên dưới vào chuyến đi
            <v-btn color="success" :to="{name:'Signin'}">
              Đăng nhập
            </v-btn>
          </v-flex>
        </v-layout>
        <!--RESULT-->
        <v-flex v-for="location in locations"
                :key="location.id"
                elevation-2 mb-2 py-2
                class="white">
          <LocationFullWidth v-bind="location"
                             :isCheckable="selectingMode"
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
  import ChoosePlanDaySection from "./ChoosePlanDaySection";
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
        selectedPlanId: undefined,
        selectedDay: 0,
        requestMessage: '',
        selectingMode: false,
        locationsCheck: [],
        choosePlanDayValue:{
          planId: undefined,
          planDay: undefined,
        },
        loading: {
          confirm: false,
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
      }),
      ...mapGetters('authenticate', {
        isShowPlanSection: 'isLoggedIn'
      }),
      ...mapGetters({
        context: 'searchContext'
      }),
    },
    methods: {
      onSelectingMode() {
        this.selectingMode = true;
      },
      onSelect({planId, planDay}) {
        this.selectedPlanId = planId;
        this.selectedDay = planDay;
      },
      onSave({id, check}) {
        let found = false;
        let locations = _.map(this.locationsCheck, (location) => {
          if (location.id == id) {
            location.check = check;
            found = true;
          }
          return location;

        });
        if (!found) {
          locations.push({
            id,
            check
          })
        }
        this.locationsCheck = locations;
      },
      onConfirm() {
        this.loading.confirm = true;
        let addLocationToPlanRequests = _.map(this.locationsCheck, (location) => {
          return {
            locationId: location.id,
            planId: this.selectedPlanId,
            planDay: this.selectedDay
          }
        });

        let responses = [];
        for (let req of addLocationToPlanRequests) {
          let res = this.$store.dispatch('plan/addLocationToPlan', req);
          responses.push(res);
        }
        Promise.all(responses)
          .then(value => {
            this.loading.confirm = false;
            this.$router.push({
              name: 'PlanDetail',
              query: {
                id: this.selectedPlanId
              }
            })
          })

      }
    }
  }
</script>

<style scoped>

</style>
