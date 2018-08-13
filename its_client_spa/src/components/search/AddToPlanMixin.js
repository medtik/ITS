import ChoosePlanDaySection from "./ChoosePlanDaySection";
import {MessageInputDialog} from "../../common/input";

import {mapState} from "vuex";
import _ from "lodash";

//required


export default {
  component: {
    ChoosePlanDaySection,
    MessageInputDialog
  },
  data: {
    locationsCheckboxValues: [],

    messageInputDialog: {
      dialog: false,
      messageInput: '',
      title: 'Ghi chú',
      message: "Lời nhắn cho chủ nhóm"
    },

    choosePlanSection: {
      loading: undefined,
      plans: [],
      selectedPlanId: undefined,
      selectedPlanDay: undefined,
    }
  },
  computed: {
    ...mapState({
      context: (state) => state.searchContext
    }),
    isSelectingMode() {
      return !!this.selectedPlanId
    },
    formattedAddLocationToPlanRequest() {
      return _.map(this.locationsCheckboxValues, (locationId) => {
        return {
          locationId: locationId,
          planId: this.choosePlanSection.selectedPlanId,
          planDay: this.choosePlanSection.selectedPlanDay
        }
      });
    }
  },
  methods: {
    onPlanIdSelect(planId) {
      this.selectedPlanId = planId;
    },

    onAddLocationClick() {
      let addLocationToPlanRequests = this.formattedAddLocationToPlanRequest();

      this.locationsCheckboxValues = [];
      let responses = [];
      for (let req of addLocationToPlanRequests) {
        let res = this.$store.dispatch('plan/addLocationToPlan', req);
        responses.push(res);
      }

      Promise.all(responses)
        .then(() => {
          this.locationsCheckboxValues = [];
          resolve();
        })
    },
    onSendRequestClick() {
      this.messageInputDialog.dialog = true;
    },
    onAddMessageConfirm() {
      this.locationsCheckboxValues = [];

      let addLocationToPlanRequests = this.formattedAddLocationToPlanRequest();

    },
    onCompleteClick() {
      this.$router.push({
        name: 'PlanDetail',
        query: {
          id: this.selectedPlanId
        }
      })
    },
    onSigninClick() {
      this.$store.commit('signinContext', {
        context: {
          returnRoute: {
            name: this.$route.name
          }
        }
      });

      this.$router.push({
        name: 'Signin'
      })
    },
    onCreatePlanClick() {
      this.$store.commit('createPlanContext', {
        context: {
          returnRoute: {
            name: this.$route.name
          }
        }
      });
      this.$router.push({
        name: "PlanCreate"
      })
    }
  }
}
