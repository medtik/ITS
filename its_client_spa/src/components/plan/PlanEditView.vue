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
    <v-flex v-for="(day,index) in plan.days"
            :key="day.key"
            class="grey lighten-5">
      <v-divider></v-divider>
      <v-flex class="title text-xs-center white" pb-2 pt-4>
        <span :id="'tab_item_'+day.key">{{day.planDayText}}</span>
        <v-flex v-if="!isPublic">
          <v-btn flat @click="onAddLocation(day)">
            <v-icon>add_location</v-icon>
            <span>Thêm địa điểm</span>
          </v-btn>
          <v-btn flat @click="onAddNote(day)" v-if="isOwnPlan">
            <v-icon>note_add</v-icon>
            <span>Thêm ghi chú</span>
          </v-btn>
        </v-flex>
      </v-flex>
      <!--ITEMS-->
      <v-flex py-2 mb-1
              v-for="item in day.items"
              :key="item.id"
              class="white">
        <LocationFullWidth v-if="item.location"
                           v-bind="item.location"
                           :isOwn="true"
                           @delete="onLocationDelete(item)">
          <template v-if="isOwnPlan" slot="action">
            <v-layout column align-center>
              <v-checkbox :value="item.id"
                          v-model="checkboxValues"
                          @change="onToggleLocation(item.id)">
              </v-checkbox>
              <v-btn icon flat color="red" @click="onLocationDelete(item)">
                <v-icon>
                  fas fa-trash
                </v-icon>
              </v-btn>
            </v-layout>
          </template>
        </LocationFullWidth>
        <NoteFullWidth v-else v-bind="item.note"
                       @delete="onNoteDelete(item,id)">
          <template v-if="isOwnPlan" slot="action">
            <v-layout column align-center>
              <v-checkbox :value="item.id"
                          v-model="checkboxValues"
                          @change="onToggleNote(item.id)">
              </v-checkbox>

              <v-btn icon flat color="red" @click="onNoteDelete(item)">
                <v-icon>
                  fas fa-trash
                </v-icon>
              </v-btn>
            </v-layout>
          </template>
        </NoteFullWidth>
      </v-flex>
      <!--SPACER-->
      <v-flex v-if="plan.days[index].length <= 0" style="height: 50px">
      </v-flex>
    </v-flex>
    <v-layout v-if="!plan.days"
              class="title font-weight-bold"
              justify-center align-center pa-5>
      Chuyến đi của bạn đang trống
    </v-layout>
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
