<template>
  <v-layout column justify-center my-2>
    <v-flex v-if="plansLoading" style="width: 200px">
      <v-progress-linear indeterminate color="primary"></v-progress-linear>
    </v-flex>
    <template v-else-if="plans && plans.length > 0">
      <v-flex>
        <v-select :items="plans"
                  item-text="name"
                  item-value="id"
                  prepend-icon="fas fa-suitcase"
                  :readonly="lockSelect.plan"
                  :value='selectedPlanId'
                  @change="onPlanSelect"
                  label="Chuyến đi">
          <template slot="item" slot-scope="props">
            <v-layout column>
              <v-flex class="caption" v-if="props.item.groupName">
                {{props.item.groupName}}
              </v-flex>
              <v-flex>
                {{props.item.name}}
              </v-flex>
            </v-layout>
          </template>
        </v-select>
        <v-select v-if="selectedPlanId"
                  :items="days"
                  item-text="planDayText"
                  item-value="planDay"
                  :value='selectedDay'
                  prepend-icon="fas fa-calendar"
                  :readonly="lockSelect.planDay"
                  @change="onDaySelect"
                  label="Ngày"
        ></v-select>
      </v-flex>
      <v-flex class="text-xs-center">
        <v-layout v-if="isOwnSelectedPlan">
          <v-btn color="success"
                 v-if="selectingMode"
                 @click="$emit('addLocations')"
                 :disabled="!isConfirmable"
                 :loading="addLocationConfirmLoading">
            <v-icon small>
              fas fa-plus
            </v-icon>
            <span v-if="selectedLocationCount > 0">&nbsp; Thêm {{selectedLocationCount}} địa điểm</span>
            <span v-else>&nbsp; Thêm địa điểm</span>
          </v-btn>
          <v-btn color="primary"
                 v-if="selectingMode"
                 @click="onConfirm">
            <v-icon small>
              fas fa-check
            </v-icon>
            &nbsp; Hoàn tất
          </v-btn>onSigninClick
          <v-btn v-if="!selectingMode"
                 color="light-blue accent"
                 @click="onAddToPlan">
            <v-icon small>
              fas fa-plus
            </v-icon>
            &nbsp; Thêm vào chuyến đi
          </v-btn>
        </v-layout>
        <v-layout v-if="!isOwnSelectedPlan">
          <v-btn color="primary"
                 :disabled="!isConfirmable"
                 :loading="sendRequestConfirmLoading"
                 @click="onSendRequest">
            <v-icon small>
              fas fa-check
            </v-icon>
            &nbsp; Gửi yêu cầu thêm địa điểm
          </v-btn>
        </v-layout>
      </v-flex>
    </template>
    <template v-else>
      <v-btn color="light-blue accent"
             @click="onCreatePlanClick">
        <v-icon small dark>
          fas fa-plus
        </v-icon>
        &nbsp;
        Tạo chuyến đi mới
      </v-btn>
    </template>
  </v-layout>
</template>

<script>
  import {
    mapGetters
  } from "vuex";
  import moment from "moment";
  import _ from "lodash";
  import Raven from "raven-js";
  import formatter from "../../formatter";
  import {ChoosePlanDialog} from "../../common/input";

  export default {
    name: "ChoosePlanDaySection",
    components: {
      ChoosePlanDialog
    },
    props: [
      'addLocationConfirmLoading',
      'sendRequestConfirmLoading',
      'selectedLocationCount',
    ],
    data() {
      return {
        lockSelect: {
          plan: false,
          planDay: false
        },
        init: false,
        selectedPlanId: undefined,
        selectedPlan: undefined,
        selectedDay: 0,
        selectingMode: false,
        dialog: {
          choosePlan: false,
        },
      }
    },
    mounted() {
      this.$store.dispatch('plan/fetchVisiblePlans', {
        areaId: this.context.areaId
      });

      if (this.plans && this.context) {
        this.initValue();
      }
    },
    computed: {
      ...mapGetters('plan', {
        plans: 'myVisiblePlansFlattened',
        plansLoading: 'myVisiblePlansLoading'
      }),
      ...mapGetters({
        context: 'searchContext'
      }),
      isConfirmable() {
        return this.selectedLocationCount > 0 && !!this.selectedPlanId
      },
      isOwnSelectedPlan() {
        if (this.selectedPlan &&
          this.selectedPlan.groupName != "" &&
          this.selectedPlan.groupName != undefined) {
          return this.selectedPlan.isGroupOwner;
        } else {
          return true;
        }
      },
      days() {
        const planDays = [
          {planDay: 0, planDayText: "Chưa lên lịch"}
        ];
        if (this.selectedPlan) {
          const startDate = moment(this.selectedPlan.startDate);
          const endDate = moment(this.selectedPlan.endDate);
          const diffDays = endDate.diff(startDate, 'days');

          for (let i = 1; i < diffDays + 2; i++) {
            planDays.push(formatter.getDaysObj(i, this.selectedPlan.startDate));
          }
        }
        return planDays;
      },
    },
    watch: {
      plans: function (plans) {
        if (!this.selectedPlanId) {
          const lastPlan = _.last(plans);
          if (lastPlan) {
            this.onPlanSelect(lastPlan.id);
          }
        }
        if (plans && this.context) {
          this.initValue();
        }
      },
      context: function (context) {
        if (this.plans && context) {
          this.initValue();
        }
      }
    },
    methods: {
      initValue() {
        if (this.init) {
          return;
        }
        let context = this.$store.getters['searchContext'];
        Raven.captureBreadcrumb({
          message: "ChoosePlanDaySection",
          category: "watch-plans",
          data: {
            plansSize: this.plans.length,
            context: this.$store.getters['searchContext']
          }
        });
        if (context.plan && context.planDay != undefined) {
          this.selectedPlan = context.plan;
          this.selectedPlanId = context.plan.id;
          this.selectedDay = context.planDay;

          this.lockSelect = {
            plan: true,
            // planDay: true,
          };
          this.selectingMode = true;
          this.$emit('selectingMode');
          this.$emit('select', {
            planId: context.plan.id,
            planDay: context.planDay
          });
        }
        this.init = true;
      },
      onAddToPlan() {
        this.selectingMode = true;
        this.$emit('selectingMode');
      },
      onPlanSelect(planId) {
        this.selectedPlan = _.find(this.plans, plan => {
          return plan.id == planId;
        });
        this.$emit('select', {
          planId: planId,
          planDay: this.selectedDay,
          plan: this.selectedPlan
        });
        this.selectedPlanId = planId;
      },
      onDaySelect(planDay) {
        this.selectedDay = planDay;
        this.emitSelect();
      },
      emitSelect() {
        this.$emit('select', {
          planId: this.selectedPlanId,
          planDay: this.selectedDay,
          plan: this.selectedPlan
        });
      },
      onConfirm() {
        this.$emit('confirm');
      },
      onSendRequest() {
        this.$emit('sendRequest');
      },
      onCreatePlanClick() {
        this.$emit('create');
      }
    }
  }
</script>

<style scoped>

</style>
