<template>
  <v-layout column justify-center my-2>
    <v-flex v-if="plans && plans.length > 0 && selectingMode">
      <v-select :items="plans"
                item-text="name"
                item-value="id"
                prepend-icon="fas fa-suitcase"
                :readonly="lockSelect"
                :value='selectedPlanId'
                @change="onPlanSelect"
                label="Chuyến đi"
      ></v-select>
      <v-select v-if="selectedPlanId"
                :items="days"
                item-text="planDayText"
                item-value="planDay"
                :value='selectedDay'
                prepend-icon="fas fa-calendar"
                :readonly="lockSelect"
                @change="onDaySelect"
                label="Ngày"
      ></v-select>
    </v-flex>
    <v-flex class="text-xs-center">
      <v-btn color="success"
             v-if="selectingMode"
             :disabled="!confirmable"
             :loading="confirmLoading"
             @click="onConfirm">
        <v-icon small>
          fas fa-check
        </v-icon>
        &nbsp; Xác nhận
      </v-btn>
      <v-btn v-if="!selectingMode"
             color="light-blue accent"
             @click="onAddToPlan">
        <v-icon small>
          fas fa-plus
        </v-icon>
        &nbsp; Thêm vào chuyến đi
      </v-btn>
    </v-flex>
  </v-layout>
</template>

<script>
  import {
    mapGetters
  } from "vuex";
  import moment from "moment";

  import formatter from "../../formatter";

  import {ChoosePlanDialog} from "../../common/input";

  export default {
    name: "ChoosePlanDaySection",
    components: {
      ChoosePlanDialog
    },
    props: [
      'context',
      'confirmable',
      'confirmLoading',
      'initValue'
    ],

    data() {
      return {
        lockSelect: false,
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
      if (!this.plans || this.plans.length < 1) {
        this.$store.dispatch('plan/fetchVisiblePlans');
      }
      let context = this.$store.getters['searchContext'];
      if (context) {
        this.selectedPlan = context.plan;
        this.selectedPlanId = context.plan.id;
        this.selectedDay = context.planDay;
        this.lockSelect = true;

        this.$emit('select', {
          planId: context.plan.id,
          planDay: context.planDay
        });
      }
    },
    computed: {
      ...mapGetters('plan', {
        plans: 'myVisiblePlansFlattened',
        plansLoading: 'myVisiblePlansLoading'
      }),
      days() {
        const planDays = [
          {planDay: 0, planDayText: "Chưa lên lịch"}
        ];
        if (this.selectedPlan) {
          const startDate = moment(this.selectedPlan.startDay);
          const endDate = moment(this.selectedPlan.endDate);
          const diffDays = endDate.diff(startDate, 'days');

          for (let i = 1; i < diffDays + 2; i++) {
            planDays.push(formatter.getDaysObj(i, this.selectedPlan.startDay));
          }
        }
        return planDays;
      },
      isHavePlan() {
        //TODO
      }
    },
    methods: {
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
          planDay: this.selectedDay
        });
        this.selectedPlanId = planId;
      },
      onDaySelect(planDay) {
        this.$emit('select', {
          planId: this.selectedPlanId,
          planDay: planDay
        });
        this.selectedDay = planDay;
      },
      onConfirm() {
        this.$emit('confirm');
      }
    }
  }
</script>

<style scoped>

</style>
