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
                      v-model="areaIdInput"
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
        <v-layout column>
          <v-flex py-4 class="display-2 font-weight-medium text-xs-center white">
            Kết quả
          </v-flex>
          <v-flex v-for="location in locations"
                  :key="location.id" mb-2 elevation-1 class="white">
            <LocationFullWidth v-bind="location"
                               :isSearchResult="true"
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
  import {mapGetters} from "vuex";
  import _ from "lodash";

  export default {
    name: "SearchView",
    components: {
      AreaInput,
      ChoosePlanDialog,
      LocationFullWidth
    },
    data() {
      return {
        searchInput: '',
        areaIdInput: '',

        selectedLocation: '',
        selectedPlan: '',
        requestMessage: '',

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
      isShowResult() {
        return this.locations && this.locations.length > 0;
      }
    },
    methods: {
      onSearchClick() {
        this.$store.dispatch('search/fetchSearchResult', {
          search: this.searchInput,
          areaId: this.areaId
        })
      },
      onSave(locationId) {
        this.selectedLocation = _.find(this.locations, (location) => {
          return location.id == locationId;
        });
        this.dialog.choosePlan = true
      },
      onPlanSelect(plan) {
        console.debug('SearchView-planSelected', plan);
      }
    }

  }
</script>

<style scoped>
</style>
