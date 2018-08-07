<template>
  <v-layout column justify-center my-2>
    <template v-if="plans && plans.length > 0 ">
      <v-flex v-if="selectingMode">
        <v-select :items="plans"
                  item-text="name"
                  item-value="id"
                  prepend-icon="fas fa-suitcase"
                  :readonly="lockSelect.plan"
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
                  :readonly="lockSelect.planDay"
                  @change="onDaySelect"
                  label="Ngày"
        ></v-select>
      </v-flex>
      <v-flex class="text-xs-center">
        <v-layout>
          <v-btn color="success"
                 v-if="selectingMode"
                 :disabled="!confirmable"
                 :loading="confirmLoading">
            <v-icon small>
              fas fa-plus
            </v-icon>
            &nbsp; Thêm
          </v-btn>
          <v-btn color="light-blue accent"
                 v-if="selectingMode"
                 :disabled="!confirmable"
                 :loading="confirmLoading"
                 @click="onConfirm">
            <v-icon small>
              fas fa-check
            </v-icon>
            &nbsp; Hoàn tất
          </v-btn>
        </v-layout>
        <v-btn v-if="!selectingMode"
               color="light-blue accent"
               @click="onAddToPlan">
          <v-icon small>
            fas fa-plus
          </v-icon>
          &nbsp; Thêm vào chuyến đi
        </v-btn>
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
      'confirmable',
      'confirmLoading',
      'selectedLocations',
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
      if (!this.plans || this.plans.length < 1) {
        this.$store.dispatch('plan/fetchVisiblePlans')
      }

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
      // context(context){
      //   if(context.planId){
      //     this.onAddToPlan();
      //   }
      // }
    },
    methods: {
      initValue() {
        if(this.init){
          return;
        }
        console.group('innitValue');
        let context = this.$store.getters['searchContext'];
        console.debug('outer',context);
        Raven.captureBreadcrumb({
          message: "ChoosePlanDaySection",
          category: "watch-plans",
          data: {
            plans: this.plans,
            context: this.$store.getters['searchContext']
          }
        });
        if (context.plan && context.planDay != undefined) {
          console.debug('inner-1');
          this.selectedPlan = context.plan;
          this.selectedPlanId = context.plan.id;
          this.selectedDay = context.planDay;

          this.lockSelect = {
            plan: true,
            // planDay: true,
          };
          console.debug('inner-2');
          this.selectingMode = true;
          this.$emit('selectingMode');
          this.$emit('select', {
            planId: context.plan.id,
            planDay: context.planDay
          });
          console.debug('inner-3');
        }
        console.groupEnd()
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
      },
      onCreatePlanClick() {
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
