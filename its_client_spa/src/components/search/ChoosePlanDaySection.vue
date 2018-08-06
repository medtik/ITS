<template>
  <v-layout column justify-center my-2>
    <v-flex v-if="plans && plans.length > 0 && selectingMode">
      <v-select :items="plans"
                item-text="name"
                item-value="id"
                v-model='selectedPlanId'
                @change="onPlanSelect"
                label="Chuyến đi"
      ></v-select>
      <v-select v-if="selectedPlanId"
                :items="days"
                item-text="planDayText"
                item-value="planDay"
                v-model='selectedDay'
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
      'confirmLoading'
    ],

    data() {
      return {
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
      isHavePlan(){
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
          planId: this.selectedPlanId,
          planDay: this.selectedDay
        })
      },
      onDaySelect() {
        this.$emit('select', {
          planId: this.selectedPlanId,
          planDay: this.selectedDay
        })
      },
      onConfirm() {
        this.$emit('confirm');
      }
    }
  }
</script>

<style scoped>

</style>
