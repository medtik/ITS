<template>
  <v-content>
    <v-toolbar class="light-blue darken-2" flat dark>
      <v-toolbar-title>
        Tạo chuyến đi
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-3>
      <v-flex my-3>
        <v-text-field
          label="Tên"
          v-model="input.name"
          :error="!!formError.name" :error-messages="formError.name"
        />
        <AreaInput
          :readonly="lockAreaId"
          v-model="input.areaId"
          :error="formError.areaId" :errorMessages="formError.areaId"
        >

        </AreaInput>
        <v-text-field
          label="Ngày bắt đầu"
          type="date"
          v-model="input.startDate"
          :error="!!formError.startDate" :error-messages="formError.startDate"
        />
        <v-text-field
          label="Ngày kết thúc"
          type="date"
          v-model="input.endDate"
          :error="!!formError.endDate" :error-messages="formError.endDate"
        />
      </v-flex>
      <v-flex>
        <v-btn v-if="isHavingContext"
               color="primary"
               :loading="createLoading"
               @click="onCreate">
          Tiếp tục
        </v-btn>
        <v-btn v-else
               color="success"
               :loading="createLoading"
               @click="onCreate">
          Tạo
        </v-btn>
        <v-btn color="secondary"
               @click="onCancel">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import {mapGetters} from "vuex";
  import {AreaInput} from "../../common/input"

  export default {
    name: "PlanCreateView",
    components: {
      AreaInput
    },
    data() {
      return {
        createBtnLoading: false,
        input: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
          areaId: undefined,
        },
        formError:{
          name: undefined,
          startDate: undefined,
          endDate: undefined,
          areaId: undefined
        },
        lockAreaId: false
      }
    },
    computed: {
      ...mapGetters({
        context: 'createPlanContext',
        previousSearchAreaId: 'previousSearchAreaId'
      }),
      isHavingContext() {
        return !!this.context.returnRoute
      },
      ...mapGetters('plan', {
        createLoading: 'createLoading',
        // editLoading: 'editLoading'
      })
    },
    mounted() {
      if (this.isHavingContext) {
        if(!!this.previousSearchAreaId){
          this.input.areaId = this.previousSearchAreaId;
          this.lockAreaId = true;
        }
      }
    },
    methods: {
      onCreate() {
        if(this.validate()){
          this.$store.dispatch('plan/create', {
            ...this.input
          }).then((data) => {
            if (this.isHavingContext) {
              this.$store.commit('searchContext',{
                context:{
                  areaId: this.input.areaId,
                  planId: data.id
                }
              });
              this.$router.push(this.context.returnRoute);
            } else {
              this.$router.back();
            }

          })
        }
      },
      onCancel() {
        this.$router.back();
      },
      validate() {
        let nameError = undefined;
        let startDateError = undefined;
        let endDateError = undefined;
        let areaIdError = undefined;

        nameError = !this.input.name ? 'Tên không được trống' : undefined;
        startDateError = !this.input.startDate ? 'Ngày bắt đầu không được trống' : undefined;
        console.log(this.input.startDate, !this.input.startDate, startDateError);
        endDateError = !this.input.endDate ? 'Ngày kết thúc không được trống' : undefined;
        areaIdError = !this.input.areaId ? "Khu vực không được trống" : undefined;

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
            endDateError = "Ngày kết thức phải sau ngày bắt đầu";
          }
        }

        if (!!this.input.startDate && !!this.input.endDate) {
          const startDate = moment(this.input.startDate);
          const endDate = moment(this.input.endDate);
          if (endDate.diff(startDate, 'days') > 30) {
            endDateError = "Chuyến đi không quá 30 ngày";
          }
        }
        this.formError.name = nameError;
        this.formError.startDate = startDateError;
        this.formError.endDate = endDateError;
        this.formError.areaId = areaIdError;

        return nameError == undefined &&
          startDateError == undefined &&
          endDateError == undefined &&
          areaIdError == undefined
      },
    }
  }
</script>

<style scoped>

</style>
