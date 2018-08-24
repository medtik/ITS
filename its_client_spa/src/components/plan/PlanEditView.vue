<template>
  <v-content>
    <v-toolbar class="light-blue darken-2" flat dark>
      <v-toolbar-title>
        Chỉnh sửa chuyến đi
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn color="success" :loading="confirmLoading">
        <v-icon>fas fa-check</v-icon>
        &nbsp; Xác nhận
      </v-btn>
    </v-toolbar>
    <v-container v-if="!pageLoading">
      <v-text-field v-model="input.name"
                    :error="!!formError.name" :error-messages="formError.name"
                    label="Tên">
      </v-text-field>
      <v-text-field v-model="input.startDate"
                    :error="!!formError.startDate" :error-messages="formError.startDate"
                    label="Ngày bắt đầu" type="date">

      </v-text-field>
      <v-text-field v-model="input.endDate"
                    :error="!!formError.endDate" :error-messages="formError.endDate"
                    label="Ngày kết thúc" type="date">
      </v-text-field>
    </v-container>
    <v-container class="text-xs-center" v-else>
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
  </v-content>
</template>

<script>
  import {mapState} from "vuex";
  import Raven from "raven-js";
  import moment from "moment" ;
  export default {
    name: "PlanEditView",
    data() {
      return {
        planId: undefined,
        confirmLoading: false,
        input: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
        },
        formError:{
          name,
          startDate,
          endDate
        }
      }
    },
    computed: {
      ...mapState('plan', {
        plan: state => state.detailedPlan,
        pageLoading: state => state.loading.detailedPlan
      })
    },
    created() {
      const {
        id
      } = this.$route.query;

      this.planId = id;
    },
    mounted() {
      this.loadData();
    },
    methods: {
      loadData() {
        this.$store.dispatch('plan/fetchPlanById', {id: this.planId})
          .then(() => {
            if(this.plan && this.plan.days){
              this.input.name = this.plan.name;
              this.input.startDate = moment(this.plan.startDate).format('YYYY-MM-DD');
              this.input.endDate = moment(this.plan.endDate).format('YYYY-MM-DD');
            }else{
              Raven.captureException(new Error("Invalid data"));
            }
          })
      },
      onConfirmBtnClick(){
        if(this.validate()){

        }
      },
      validate() {
        let nameError = undefined;
        let startDateError = undefined;
        let endDateError = undefined;

        nameError = !this.input.name ? 'Tên không được trống' : undefined;
        startDateError = !this.input.startDate ? 'Ngày bắt đầu không được trống' : undefined;
        endDateError = !this.input.endDate ? 'Ngày kết thúc không được trống được trống' : undefined;

        if (!!this.input.startDate) {
          const now = moment();
          const startDate = moment(this.input.startDate);
          if (startDate.isBefore(now, 'day')) {
            startDateError = "Ngày bắt đầu không được trong quá khứ";
          }
        }

        if (!!this.input.startDate && !!this.input.endDate) {
          const startDate = moment(this.input.startDate);
          const endDate = moment(this.input.endDate);
          if (endDate.isBefore(startDate, 'day')) {
            endDateError = "Ngày kết thúc phải sau ngày bắt đầu";
          }
        }

        this.formError = {
          name: nameError,
          startDate: startDateError,
          endDate: endDateError
        };

        return nameError == undefined &&
          startDateError == undefined &&
          endDateError == undefined;
      },
    }
  }
</script>

<style scoped>

</style>
