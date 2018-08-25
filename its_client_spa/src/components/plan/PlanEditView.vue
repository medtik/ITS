<template>
  <v-content>
    <v-toolbar class="light-blue darken-2" flat dark>
      <v-toolbar-title>
        Chỉnh sửa chuyến đi
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn color="success" :loading="confirmLoading" @click="onConfirmBtnClick">
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

      <v-flex class="title">
        Xếp lịch
      </v-flex>
      <v-flex v-for="(day,index) in plan.days"
              :key="day.key"
              class="grey lighten-5">
        <v-divider></v-divider>
        <v-flex class="title text-xs-center white" pb-2 pt-4>
          <span :id="'tab_item_'+day.key">
            <v-icon>far fa-calendar</v-icon> &nbsp; {{day.planDayText}}
          </span>
        </v-flex>
        <!--ITEMS-->
        <draggable :list="input.days[index]"
                   :options="{handle:'.handle-bar', group:'days'}"
                   style="padding-top: 5rem"
                   class="white">
          <v-flex py-2 mb-1
                  v-for="item in input.days[index]"
                  :key="item.id">
            <LocationFullWidth v-if="item.location"
                               v-bind="item.location">
              <template slot="handle">
                <v-layout class="handle-bar" justify-center align-center>
                  <v-icon>
                    fas fa-grip-vertical
                  </v-icon>
                </v-layout>
              </template>
            </LocationFullWidth>
            <NoteFullWidth v-else v-bind="item.note">
              <template slot="handle">
                <v-layout class="handle-bar" justify-center align-center>
                  <v-icon>
                    fas fa-grip-vertical
                  </v-icon>
                </v-layout>
              </template>
            </NoteFullWidth>
          </v-flex>

        </draggable>
      </v-flex>
    </v-container>
    <v-container class="text-xs-center" v-else>
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
  </v-content>
</template>

<script>
  import {mapState} from "vuex";
  import {LocationFullWidth} from "../../common/block";
  import NoteFullWidth from "./NoteFullWidth"
  import Raven from "raven-js";
  import moment from "moment" ;
  import draggable from 'vuedraggable';
  import _ from "lodash"

  export default {
    name: "PlanEditView",
    components: {
      LocationFullWidth,
      NoteFullWidth,
      draggable
    },
    data() {
      return {
        planId: undefined,
        confirmLoading: false,
        input: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
          days: []
        },
        formError: {
          name: undefined,
          startDate: undefined,
          endDate: undefined
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
            if (this.plan && this.plan.days) {
              this.input.name = this.plan.name;
              this.input.startDate = moment(this.plan.startDate).format('YYYY-MM-DD');
              this.input.endDate = moment(this.plan.endDate).format('YYYY-MM-DD');
              this.input.days = _.map(this.plan.days, (day) => {
                  if (!day.items) {
                    day.items = [];
                  }
                  return day.items;
              });
            } else {
              Raven.captureException(new Error("Invalid data"));
            }
          })
      },
      onConfirmBtnClick() {
        if (this.validate()) {
          this.$store.dispatch('plan/edit',{
            id: this.planId,
            ...this.input,
            ...this.getFormattedDays()
          })
        }
      },
      getFormattedDays(){
        let days = _.cloneDeep(this.input.days);

        const planLocation = [];
        const planNotes = [];

        for (let i = 0; i < days.length; i++){
          const items = days[i];
          for(let j = 0; j < items.length; j++){
            const item = items[j];
            const indexObj = {
              id: item.id,
              index: j,
              planDay: i
            };

            if(item.location){
              planLocation.push(indexObj);
            }else{
              planNotes.push(indexObj);
            }
          }
        }

        return {
          planNotes,
          planLocation
        };
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
