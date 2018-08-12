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
              :confirmLoading="loading.confirm"
              :addLocationConfirmLoading="loading.addLocationConfirm"
              :selectedLocationCount="selectedLocationCount"
              @select="onSelect"
              @addLocations="onConfirmAddLocations"
              @confirm="onConfirm"
              @create="onCreatePlan"
              @sendRequest="messageDialog.dialog = true"
              @selectingMode="onSelectingMode"
            ></ChoosePlanDaySection>
          </v-flex>
          <v-flex v-if="!isShowPlanSection"
                  shrink my-3
                  class="text-xs-center title">
            Bạn cần đăng nhập để thêm các địa điểm bên dưới vào chuyến đi
            <v-btn color="success" @click="onSigninClick">
                   Đăng nhập
            </v-btn>
          </v-flex>
        </v-layout>
        <!--RESULT-->
        <v-flex v-for="location in locations"
                :key="location.id +'_'+ locationsFullWidthSuffix"
                elevation-2 mb-2 py-2
                class="white">
          <LocationFullWidth v-bind="location"
                             :isCheckable="selectingMode"
                             @save="onSave">
            <template slot="action">
              <v-checkbox v-model="locationsCheck" :value="location.id">

              </v-checkbox>
            </template>
          </LocationFullWidth>
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
    <MessageInputDialog
      v-bind="messageDialog"
      @close="messageDialog.dialog = false"
      @confirm="onSendRequestConfirm"
    ></MessageInputDialog>
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
  import {
    MessageInputDialog
  } from "../../common/input";
  import ChoosePlanDaySection from "./ChoosePlanDaySection"
  import {mapGetters} from "vuex";

  export default {
    name: "SmartSearchResultView",
    components: {
      LocationFullWidth,
      PlanFullWidth,
      SuccessDialog,
      ChoosePlanDaySection,
      MessageInputDialog
    },
    data() {
      return {
        selectedLocation: '',
        selectedPlanId: undefined,
        selectedDay: 0,
        requestMessage: '',
        selectingMode: false,
        locationsCheck: [],
        locationsFullWidthSuffix: '',
        choosePlanDayValue: {
          planId: undefined,
          planDay: undefined,
        },
        messageDialog: {
          dialog: false,
          messageInput: '',
          title:'Ghi chú',
          message: "Ghi chú cho yêu cầu thêm địa điểm"
        },
        loading: {
          confirm: false,
          addLocationConfirm: false,
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
      selectedLocationCount() {
        return this.locationsCheck.length;
      },
    },
    methods: {
      onConfirmAddLocations() {
        this.loading.addLocationConfirm = true;
        this.addLocation(true)
          .then(() => {
            this.loading.addLocationConfirm = false;
          });
      },
      onSelectingMode() {
        this.selectingMode = true;
      },
      onSigninClick() {
        this.$store.commit('signinContext', {
          context: {
            returnRoute: {
              name: 'SmartSearchResult'
            }
          }
        });
        this.$router.push({
          name: 'Signin'
        })
      },
      onSelect({planId, planDay}) {
        this.selectedPlanId = planId;
        this.selectedDay = planDay;
      },
      onSave({id, check}) {
        let found = false;
        let locations = _.map(this.locationsCheck, (location) => {
          if (location.id == id) {
            location.isCheck = check;
            found = true;
          }
          return location;
        });
        if (!found) {
          locations.push({
            id,
            isCheck: check
          })
        }
        this.locationsCheck = locations;
      },
      onConfirm() {
        this.loading.confirm = true;
        this.$router.push({
          name: 'PlanDetail',
          query: {
            id: this.selectedPlanId
          }
        })
      },
      onSendRequestConfirm(){
        let addLocationToPlanRequests = _.map(this.locationsCheck, (locationId) => {
          return {
            locationId: locationId,
            planId: this.selectedPlanId
          }
        });


      },
      addLocation() {
        let addLocationToPlanRequests = _.map(this.locationsCheck, (locationId) => {
          return {
            locationId: locationId,
            planId: this.selectedPlanId,
            planDay: this.selectedDay
          }
        });
        this.locationsCheck = [];
        this.resetLocationsFullWidth();
        let responses = [];
        for (let req of addLocationToPlanRequests) {
          let res = this.$store.dispatch('plan/addLocationToPlan', req);
          responses.push(res);
        }


        return new Promise((resolve) => {
          Promise.all(responses)
            .then(value => {
              resolve();
            })
        });
      },
      resetLocationsFullWidth() {
        this.locationsFullWidthSuffix = _.uniqueId('lfw');
      },
      onCreatePlan(){
        this.$store.commit('createPlanContext', {
          context: {
            returnRoute: {
              name: 'SmartSearchResult'
            }
          }
        });
        this.$router.push({
          name: "PlanCreate"
        })
      }
    }
  }
</script>

<style scoped>

</style>
