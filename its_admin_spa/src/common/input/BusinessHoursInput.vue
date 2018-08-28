<template>
  <v-layout row>
    <v-flex xs2>
      <v-select label="Ngày"
                :items="days"
                v-model="input.day"
                @input="emitInput"
                item-text="text"
                item-value="value"
      ></v-select>
    </v-flex>
    <v-flex v-if="!allDayCheck">
      <v-text-field
        label="Mở cửa"
        v-model="input.from"
        @input="emitInput"
        :readonly="readonly"
        placeholder="Giờ:phút"/>
    </v-flex>
    <v-flex v-if="!allDayCheck">
      <v-text-field
        label="Đóng cửa"
        v-model="input.to"
        @input="emitInput"
        :readonly="readonly"
        placeholder="Giờ:phút"/>
    </v-flex>
    <v-flex style="flex-grow: 0">
      <v-checkbox label="Cả ngày" v-model='allDayCheck' @input="emitInput">

      </v-checkbox>
    </v-flex>
    <v-layout style="flex-grow: 0" align-center>
      <v-btn color="red" icon flat @click="$emit('delete')">
        <v-icon small>
          fas fa-trash
        </v-icon>
      </v-btn>
    </v-layout>
  </v-layout>
</template>
<!--TODO ADD FORM VALIDATION-->
<!--TODO READONLY-->
<script>
  import _ from "lodash"
  import moment from "moment"
  export default {
    name: "BusinessHoursInput",
    props: ['readonly', 'value'],
    data() {
      return {
        days: [
          {text: 'Thứ 2', value: "day2"},
          {text: 'Thứ 3', value: "day3"},
          {text: 'Thứ 4', value: "day4"},
          {text: 'Thứ 5', value: "day5"},
          {text: 'Thứ 6', value: "day6"},
          {text: 'Thứ 7', value: "day7"},
          {text: 'Chủ nhật', value: "day1"},
        ],
        input: {
          day: undefined,
          from: undefined,
          to: undefined,
        },
        allDayCheck: false
      }
    },
    watch: {
      value: {
        immediate: true,
        handler(val, oldVal) {
          let zeroTime = moment("00:00","HH:mm");
          if (moment(val.from).isSame(zeroTime) &&
            moment(val.to).isSame(zeroTime)) {
            this.allDayCheck = true;
          }
          this.input = val;
        }
      }
    },
    methods: {
      emitInput() {
        const returnVal = _.cloneDeep(this.input);
        if (this.allDayCheck) {
          returnVal.from = "00:00";
          returnVal.to = "00:00";
        }
        this.$emit('input', returnVal);
      }
    }
  }
</script>

<style scoped>

</style>
