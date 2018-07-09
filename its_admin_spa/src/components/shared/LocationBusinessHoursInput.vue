<template>
  <v-container fluid pa-0>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Chủ nhật</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day1.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day1.to"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Thứ 2</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day2.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day2.to"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Thứ 3</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day3.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day3.to"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Thứ 4</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day4.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day4.to"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Thứ 5</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day5.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day5.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Thứ 6</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day6.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day6.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
    <v-layout row style="align-items: baseline">
      <v-flex xs2>
        <v-label>Thứ 7</v-label>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Mở cửa"
          v-model="day7.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
      <v-flex>
        <v-text-field
          label="Đóng cửa"
          v-model="day7.from"
          @input="onInputChange"
          :readonly="readonly"
          placeholder="Giờ:phút"/>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  import _ from "lodash";

  export default {
    name: "LocationBusinessHoursInput",
    props: [
      'value',
      'readonly'
    ],
    data() {
      return {
        day1: {
          from: undefined,
          to: undefined
        },
        day2: {
          from: undefined,
          to: undefined
        },
        day3: {
          from: undefined,
          to: undefined
        },
        day4: {
          from: undefined,
          to: undefined
        },
        day5: {
          from: undefined,
          to: undefined
        },
        day6: {
          from: undefined,
          to: undefined
        },
        day7: {
          from: undefined,
          to: undefined
        },
      }
    },
    watch: {
      value(val, oldVal) {
        this.updateData(val);
      }
    },
    created() {
      this.updateData(this.value);
    },
    methods: {
      updateData(val) {
        this.day1 = val.day1;
        this.day2 = val.day2;
        this.day3 = val.day3;
        this.day4 = val.day4;
        this.day5 = val.day5;
        this.day6 = val.day6;
        this.day7 = val.day7;
      },
      onInputChange() {
        this.$emit('input', _.extend({}, this.$data));
      }
    }
  }
</script>

<style scoped>

</style>
