<template>
  <v-content>
    <v-toolbar color="light-blue" dark flat>
      <v-toolbar-title>
        Tạo nhóm
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column my-3>
      <v-flex mx-3>
        <v-text-field label="Tên nhóm"/>
        <v-btn color="success"
               @click="$router.back()">
          Tạo
        </v-btn>
        <v-btn color="secondary"
               @click="$router.back()">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>
<!--TODO thêm chuyến đi-->
<script>
  export default {
    name: "CreateGroupView"
  }
</script>

<style scoped>

</style>
