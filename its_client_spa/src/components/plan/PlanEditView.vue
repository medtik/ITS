<template>
  <v-content>
    <v-toolbar class="light-blue darken-2" flat dark>
      <v-toolbar-title>
        Chỉnh sửa chuyến đi
      </v-toolbar-title>
    </v-toolbar>
    <v-container v-if="!pageLoading">
      <v-text-field v-model="input.name"
                    :error="!!input.nameError" :error-messages="input.nameError"
                    label="Tên">

      </v-text-field>
      <v-text-field v-model="input.startDate"
                    :error="!!input.startDateError" :error-messages="input.startDateError"
                    label="Ngày bắt đầu" type="date">

      </v-text-field>
      <v-text-field v-model="input.endDate"
                    :error="!!input.endDateError" :error-messages="input.endDateError"
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
        input: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
          //
          nameError: undefined,
          startDateError: undefined,
          endDateError: undefined,
        },

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
      validateBasicForm(){

      }
    }
  }
</script>

<style scoped>

</style>
