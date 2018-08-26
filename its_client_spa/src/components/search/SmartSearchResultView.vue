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

            <v-flex my-3>
              <v-btn @click="onCreateSuggestedPlanClick" color="primary" :loading="suggestedBtnLoading">
                Tạo chuyến đi tự động
              </v-btn>
            </v-flex>
          </v-flex>
          <v-flex v-if="!isLoggedIn"
                  shrink my-3
                  class="text-xs-center title">
            Bạn cần đăng nhập để thêm các để sử dụng chuyến đi
            <v-btn color="success" @click="onSigninClick">
              Đăng nhập
            </v-btn>
          </v-flex>
        </v-layout>

        <!--RESULT-->
        <v-flex v-for="location in locations"
                :key="location.id"
                mb-2 elevation-1 class="white">
          <LocationFullWidth v-bind="location">
            <template slot="action" v-if="isSelectingMode">
              <v-checkbox v-model="locationsCheckboxValues" :value="location.id">

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
    <!--DIALOG-->
    <MessageInputDialog
      v-bind="messageInputDialog"
      v-model="messageInputDialog.messageInput"
      @confirm="onAddMessageConfirm"
      @close="messageInputDialog.dialog = false"
    ></MessageInputDialog>
    <CreatePlanDialog
      :dialog="createPlanDialog.dialog"
      :areaId="createPlanDialog.areaId"
      @close="createPlanDialog.dialog = false"
      @confirm="onCreateSuggestedPlanConfirm">
    </CreatePlanDialog>
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
  import AddToPlanMixin from "./AddToPlanMixin";
  import CreatePlanDialog from "../plan/CreatePlanDialog"
  import {mapGetters, mapState} from "vuex";

  export default {
    name: "SmartSearchResultView",
    mixins: [AddToPlanMixin],
    components: {
      LocationFullWidth,
      PlanFullWidth,
      SuccessDialog,
      ChoosePlanDaySection,
      MessageInputDialog,
      CreatePlanDialog
    },
    data() {
      return {
        selectingMode: false,
        messageInputDialog: {},

        createPlanDialog: {
          dialog: false,
          areaId: undefined,
        },

        selectedAreaId: undefined,
        selectedAnswers: undefined,
      }
    },
    computed: {
      ...mapGetters('smartSearch', {
        locations: 'searchResult'
      }),
      ...mapState('plan',{
        suggestedBtnLoading: 'createSuggestedPlan'
      })
    },
    created() {
      const {
        areaId,
        answers
      } = this.$route.params;

      this.selectedAreaId = areaId;
      this.selectedAnswers = answers;
    },
    methods: {
      onCreateSuggestedPlanClick() {
        this.createPlanDialog = {
          dialog: true,
          areaId: this.$store.state.previousSearchAreaId
        }
      },
      onCreateSuggestedPlanConfirm(inputs) {
        const {
          name,
          startDate,
          endDate
        } = inputs;

        this.createPlanDialog = {
          dialog: false
        };
        this.$store.dispatch("plan/createSuggestedPlan", {
          name,
          startDate,
          endDate,
          areaId: this.selectedAreaId,
          answers: this.selectedAnswers
        }).then(value => {
          this.$router.push({
            name: 'PlanDetail',
            query: value
          })
        })
      },
    }
  }
</script>

<style scoped>

</style>
