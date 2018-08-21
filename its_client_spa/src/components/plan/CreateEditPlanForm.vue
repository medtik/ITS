<template>
  <v-container>
    <v-text-field
      label="Tên"
      v-model="value.name"
    />
    <AreaInput
      :readonly="lockArea"
      v-model="value.areaId">
    </AreaInput>
    <v-text-field
      label="Ngày bắt đầu"
      type="date"
      v-model="value.startDate"
    />
    <v-text-field
      label="Ngày kết thúc"
      type="date"
      v-model="value.endDate"
    />
  </v-container>
</template>

<script>
  import {AreaInput} from "../../common/input";

  export default {
    name: "CreatePlanSection",
    props: [
      'value',
      'lockArea'
    ]
  }
</script>

<style scoped>

</style>
