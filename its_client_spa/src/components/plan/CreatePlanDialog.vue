<template>
  <v-dialog v-model="dialog" max-width="550" persistent>
    <v-card>
      <v-card-title class="white--text light-blue darken-2 title">
        Tạo chuyến đi
      </v-card-title>
      <v-card-text>
        <v-layout column>
          <v-text-field label="Tên">

          </v-text-field>

          <v-text-field label="Ngày bắt đầu">

          </v-text-field>
          <v-text-field label="Ngày kết thúc">

          </v-text-field>
        </v-layout>
      </v-card-text>
      <v-card-actions>
        <v-btn color="success">
          Xác nhận
        </v-btn>
        <v-btn color="secondary">
          Hủy
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: "CreatePlanDialog",
    props: [
      'dialog'
    ]
  }
</script>

<style scoped>

</style>
