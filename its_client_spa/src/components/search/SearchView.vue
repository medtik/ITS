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
          <v-flex shrink v-if="isLoggedIn">
            <ChoosePlanDaySection
              :confirmable="locationsCheckboxValues.length > 0"
              :selectedLocationCount="selectedLocationCount"
              @select="onSelect"
              @confirm="onCompleteClick"
              @create="onCreatePlanClick"
              @sendRequest="messageInputDialog.dialog = true"
              @addLocations="onAddLocationClick"
              @selectingMode="onSelectingMode"
            ></ChoosePlanDaySection>
          </v-flex>
          <v-flex v-if="!isLoggedIn"
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
                  :key="location.id"
                  mb-2 elevation-1 class="white">
            <LocationFullWidth v-bind="location">
              <template slot="action" v-if="isSelectingMode">
                <v-layout column>
                  <v-checkbox v-model="locationsCheckboxValues" :value="location.id">

                  </v-checkbox>
                </v-layout>
              </template>
            </LocationFullWidth>
          </v-flex>
          <v-flex style="height: 10vh">
            <!--HODLER -->
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
    <MessageInputDialog
      v-bind="messageInputDialog"
      v-model="messageInputDialog.messageInput"
      @confirm="onAddMessageConfirm"
      @close="messageInputDialog.dialog = false"
    ></MessageInputDialog>
  </v-content>
</template>
<script>
  import {
    AreaInput,
    ChoosePlanDialog,
    MessageInputDialog
  } from "../../common/input";
  import {
    LocationFullWidth,
  } from "../../common/block";
  import AddToPlanMixin from "./AddToPlanMixin";

  import ChoosePlanDaySection from "./ChoosePlanDaySection";
  import {mapGetters} from "vuex";
  import _ from "lodash";

  export default {
    name: "SearchView",
    mixins: [AddToPlanMixin],
    components: {
      AreaInput,
      ChoosePlanDialog,
      LocationFullWidth,
      ChoosePlanDaySection,
      MessageInputDialog
    },
    data() {
      return {
        searchInput: '',
        areaIdInput: '',

        lockAreaIdInput: false,

        result: {
          show: true,
          loading: false,
        },
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
      },
    },
    mounted() {
      if (this.context && this.context.areaId) {
        this.areaIdInput = this.context.areaId;
        this.lockAreaIdInput = true;
      }
    },
    methods: {
      onSearchClick() {
        this.$store.commit('searchContext', {
          areaId: this.areaId
        });
        this.$store.commit('previousSearchAreaId', {areaId: this.areaIdInput});
        this.$store.dispatch('search/fetchSearchResult', {
          search: this.searchInput,
          areaId: this.areaIdInput
        })
      },
      onAreaSelect(id){
        this.areaIdInput = id;
      }
    }

  }
</script>
