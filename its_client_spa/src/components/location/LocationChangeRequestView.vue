<template>
  <v-content>
    <v-toolbar flat dark color="light-blue">
      <v-toolbar-title>
        Cập nhật thông tin địa điểm
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-2>
      <v-flex>
        <v-text-field
          label="Tên"
        />
        <v-text-field
          label="Địa chỉ"
        />
        <v-text-field
          label="Mô tả"
        />
        <v-text-field
          label="Điện thoại"
        />
        <v-text-field
          label="Email"
        />
        <v-text-field
          label="Giờ hoạt động"
        />
        <v-text-field
          label="thẻ"
        />


      </v-flex>
      <v-flex style="height: 15vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  export default {
    name: "LocationChangeRequestView"
  }
</script>

<style scoped>

</style>
