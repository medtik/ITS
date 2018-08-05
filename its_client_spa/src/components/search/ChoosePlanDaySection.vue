<template>
  <v-layout column justify-center my-2>
    <v-flex v-if="plans && plans.length > 0">
      <v-select :items="plans"
                item-text="name"
                item-value="id"
                v-model='selectedPlan'
                label="Chuyến đi"
      ></v-select>
      <v-select v-if="selectedPlan"
                :items="days"
                item-text="text"
                item-value="planDay"
                v-model='selectedDay'
                label="Ngày"
      ></v-select>
    </v-flex>
    <v-flex class="text-xs-center">
      <v-btn color="success"
             v-if="selectingMode"
             @click="onConfirm">
        <v-icon small>
          fas fa-check
        </v-icon>
        &nbsp; Xác nhận chuyến đi
      </v-btn>
      <v-btn color="light-blue accent"
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
  import {ChoosePlanDialog} from "../../common/input";

  export default {
    name: "ChoosePlanDaySection",
    components: {
      ChoosePlanDialog
    },
    props: [
      'context'
    ],

    data() {
      return {
        selectedPlan: undefined,
        selectedDay: undefined,
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

        return [
          {planDay: 0, text: 'Chưa lên lịch'},
          {planDay: 1, text: '23/6/2018'}
        ]
      }
    },
    methods: {
      onAddToPlan() {
        this.selectingMode = true;
        this.$emit('selectingMode', true);
      },
      onConfirm(){

      }
    }
  }
</script>

<style scoped>

</style>
