<template>
  <v-container fluid pa-0>
    <v-layout column>
      <BusinessHoursInput v-for="(input, index) in businessHours"
                          :key="`B_${index}`"
                          @input="onInputChange"
                          v-model="businessHours[index]"/>
      <v-btn color icon color="success" @click="onAddBusinessHoursClick">
        <v-icon small>fas fa-plus</v-icon>
      </v-btn>
    </v-layout>
  </v-container>
</template>

<script>
  import _ from "lodash";
  import BusinessHoursInput from "./BusinessHoursInput";

  export default {
    name: "LocationBusinessHoursInput",
    components:{
      BusinessHoursInput
    },
    props: [
      'value',
      'readonly'
    ],
    data() {
      return {
        businessHours:[]
      }
    },
    watch: {
      value(val, oldVal) {
        this.businessHours = val;
      }
    },
    methods: {
      onInputChange() {
        this.$emit('input', this.businessHours);
      },
      onAddBusinessHoursClick(){
        this.businessHours.push({});
        this.onInputChange();
      }
    }
  }
</script>

<style scoped>

</style>
