import ChoosePlanDaySection from "./ChoosePlanDaySection";
import {MessageInputDialog} from "../../common/input";

import {mapState, mapGetters} from "vuex";
import _ from "lodash";

export default {
  component: {
    ChoosePlanDaySection,
    MessageInputDialog
  },
  data() {
    return {
      locationsCheckboxValues: [],
      selectingMode: false,
      messageInputDialog: {
        dialog: false,
        messageInput: '',
        title: 'Ghi chú',
        message: "Lời nhắn cho chủ nhóm"
      },
      choosePlanSection: {
        loading: undefined,
        selectedPlanId: undefined,
        selectedPlanDay: undefined,
      }
    }
  },
  computed: {
    ...mapState({
      context: (state) => state.searchContext
    }),
    ...mapGetters('authenticate', {
      isLoggedIn: 'isLoggedIn'
    }),
    isSelectingMode() {
      return this.selectingMode;
    },
    selectedLocationCount() {
      return this.locationsCheckboxValues.length;
    },
  },
  methods: {
    onPlanIdSelect(planId) {
      this.selectedPlanId = planId;
    },

    onAddLocationClick() {
      let addLocationToPlanRequests = _.map(this.locationsCheckboxValues,
        (locationId) => {
          return {
            locationId: locationId,
            planId: this.choosePlanSection.selectedPlanId,
            planDay: this.choosePlanSection.selectedPlanDay
          }
        });

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
    onSelect({planId, planDay, plan}) {
      this.choosePlanSection = {
        selectedPlanId: planId,
        selectedPlanDay: planDay
      }
    },
    onSendRequestClick() {
      this.messageInputDialog.dialog = true;
    },
    onAddMessageConfirm() {
      let locationRequest = {
        locations: this.locationsCheckboxValues,
        planId: this.choosePlanSection.selectedPlanId,
        planDay: this.choosePlanSection.selectedPlanDay,
        message: this.messageInputDialog.messageInput
      };

      this.$store.dispatch('request/createLocationSuggestion', locationRequest);
      this.locationsCheckboxValues = [];
    },
    onCompleteClick() {
      this.$router.push({
        name: 'PlanDetail',
        query: {
          id: this.choosePlanSection.selectedPlanId
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
    },
    onSelectingMode(){
      this.selectingMode = true
    }
  }
}
