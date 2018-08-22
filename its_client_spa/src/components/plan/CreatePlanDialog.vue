<template>
  <v-dialog v-model="dialog" max-width="550" persistent>
    <v-card>
      <v-card-title class="white--text light-blue darken-2 title">
        Tạo chuyến đi
      </v-card-title>
      <v-card-text>
        <v-layout column>
          <v-text-field label="Tên" v-model="input.name"
                        :error="!!formError.name" :error-messages="formError.name">

          </v-text-field>
          <v-text-field label="Ngày bắt đầu" type="date" v-model="input.startDate"
                        :error="!!formError.startDate" :error-messages="formError.startDate">

          </v-text-field>
          <v-text-field label="Ngày kết thúc" type="date" v-model="input.endDate"
                        :error="!!formError.endDate" :error-messages="formError.endDate">

          </v-text-field>
        </v-layout>
      </v-card-text>
      <v-card-actions>
        <v-btn color="success" @click="onConfirm">
          Xác nhận
        </v-btn>
        <v-btn color="secondary" @click="onClose">
          Hủy
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  import moment from "moment"
  import {AreaInput} from "../../common/input"

  export default {
    name: "CreatePlanDialog",
    components: {
      AreaInput
    },
    props: [
      'dialog',
      'areaId'
    ],
    data() {
      return {
        isOpen: undefined,
        input: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
        },
        formError: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
        }
      }
    },
    watch: {
      areaId: function (val) {
        this.input.areaId = val;
      }
    },
    methods: {
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
          if (endDate.isAfter(startDate, 'day')) {
            endDateError = "Ngày kết thức phải sau ngày bắt đầu";
          }
        }

        this.formError = {
          name: nameError,
          startDate: startDateError,
          endDate: endDateError
        };
        return nameError == undefined &&
          startDateError == undefined &&
          endDateError == undefined
      },
      onConfirm() {
        if (this.validate()) {
          this.$emit('confirm', this.input);
        }
      },
      onClose() {
        this.$emit('close');
        this.reset();
      },
      reset() {
        Object.assign(this.$data, this.$options.data())
      }
    }
  }
</script>

<style scoped>

</style>
