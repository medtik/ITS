<template>
  <v-content>
    <v-layout column>
      <v-flex>
        <!--SEARCH-->
        <v-jumbotron gradient="-225deg, #FFFEFF 0%, #D7FFFE 100%" height="230px">
          <v-container fill-height>
            <v-layout>
              <v-flex text-xs-center>
                <v-text-field
                  label="Tìm kiếm"
                  v-model="searchInput"
                ></v-text-field>
                <v-layout row justify-center>
                  <v-flex xs10 sm6 md4>
                    <AreaInput
                      @input="onAreaSelect"
                      :value="areaIdInput"
                      :readonly="lockAreaIdInput"
                      alias="area"
                      itemsPath="areas"
                      loadingPath="areasLoading"
                      getItemPath="getAll"
                    ></AreaInput>
                  </v-flex>
                </v-layout>
                <v-btn color="primary"
                       :loading="searchLoading"
                       @click="onSearchClick">
                  <v-icon>
                    fas fa-search
                  </v-icon>
                  Tìm
                </v-btn>
              </v-flex>
            </v-layout>
          </v-container>
        </v-jumbotron>
      </v-flex>
      <!--RESULT-->
      <v-flex v-if="isShowResult" class="grey lighten-4">
        <!--Add to plans-->
        <v-layout justify-center>
          <v-flex shrink v-if="isShowPlanSection">
            <ChoosePlanDaySection
              :confirmable="locationsCheck.length > 0"
              :confirmLoading="loading.confirm"
              :selectedLocationCount="selectedLocationCount"
              @select="onSelect"
              @confirm="onConfirm"
              @addLocations="onConfirmAddLocations"
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
        <!--Locations-->
        <v-layout column>
          <v-flex py-4 class="display-2 font-weight-medium text-xs-center white">
            Kết quả
          </v-flex>
          <v-flex v-for="location in locations"
                  :key="location.id" mb-2 elevation-1 class="white">
            <LocationFullWidth v-bind="location"
                               :isCheckable="selectingMode"
                               @save="onSave"/>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
    <ChoosePlanDialog
      :dialog="dialog.choosePlan"
      @select="onPlanSelect"
      @close="dialog.choosePlan = false"/>
  </v-content>
</template>
<script>
  import {
    AreaInput,
    ChoosePlanDialog
  } from "../../common/input";
  import {
    LocationFullWidth,
  } from "../../common/block";
  import ChoosePlanDaySection from "./ChoosePlanDaySection";
  import {mapGetters} from "vuex";
  import _ from "lodash";

  export default {
    name: "SearchView",
    components: {
      AreaInput,
      ChoosePlanDialog,
      LocationFullWidth,
      ChoosePlanDaySection
    },
    data() {
      return {
        searchInput: '',
        areaIdInput: '',

        selectedLocation: '',
        selectedPlan: '',
        requestMessage: '',

        locationsCheck: [],
        locationsFullWidthSuffix:'',
        selectingMode: false,
        selectedPlanId: undefined,
        selectedDay: 0,
        lockAreaIdInput: false,
        choosePlanDayValue: {
          planId: undefined,
          planDay: undefined,
        },

        loading: {
          confirm: false
        },
        result: {
          show: true,
          loading: false,
        },
        dialog: {
          choosePlan: false,
        }
      }
    },
    computed: {
      ...mapGetters('search', {
        area: 'searchResultArea',
        locations: 'searchResultLocations',
        searchLoading: 'searchResultLoading'
      }),
      ...mapGetters('authenticate', {
        isShowPlanSection: 'isLoggedIn'
      }),
      ...mapGetters({
        context: 'searchContext'
      }),
      selectedLocationCount() {
        return _.filter(this.locationsCheck, locationCheck => {
          return locationCheck.isCheck;
        }).length;
      },
      isShowResult() {
        return this.locations && this.locations.length > 0;
      }
    },
    mounted(){
      if(this.context && this.context.areaId){
        this.areaIdInput = this.context.areaId;
        this.lockAreaIdInput = true;
      }
    },
    methods: {
      onSearchClick() {
        this.$store.commit('searchContext', {
          areaId: this.areaId
        });
        this.$store.dispatch('search/fetchSearchResult', {
          search: this.searchInput,
          areaId: this.areaId
        })
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
      onPlanSelect(plan) {
        this.dialog.choosePlan = false;

        this.$store.dispatch('plan/addLocationToPlan', {
          locationId: this.selectedLocation.id,
          planId: plan.id
        })
      },
      onAreaSelect(areaId){
        this.areaIdInput = areaId;
      },
      onSigninClick() {
        this.$store.commit('signinContext', {
          context: {
            returnRoute: {
              name: 'Search'
            }
          }
        });

        this.$router.push({
          name: 'Signin'
        })
      },
      onConfirm() {
        this.loading.confirm = true;
        this.addLocation()
          .then(() => {
            this.loading.confirm = false;
          })
      },
      onConfirmAddLocations() {
        this.loading.addLocationConfirm = true;
        this.addLocation(true)
          .then(() => {
            this.loading.addLocationConfirm = false;
          });
      },
      onSelect({planId, planDay}) {
        this.selectedPlanId = planId;
        this.selectedDay = planDay;
      },
      onSelectingMode() {
        this.selectingMode = true;
      },
      addLocation(noForward) {
        let addLocationToPlanRequests = _.map(this.locationsCheck, (location) => {
          return {
            locationId: location.id,
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
              if (!noForward) {
                this.$router.push({
                  name: 'PlanDetail',
                  query: {
                    id: this.selectedPlanId
                  }
                })
              }
              resolve();
            })
        });
      },
      resetLocationsFullWidth() {
        this.locationsFullWidthSuffix = _.uniqueId('lfw');
      }
    }

  }
</script>
