<template>
  <v-content>
    <v-toolbar dark flat color="light-blue">
      <v-toolbar-title>
        Gửi yêu sở hữu địa điểm
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-2>
      <v-flex my-2>
        <v-textarea label="Thông tin thêm"/>
      </v-flex>
      <v-flex my-2>
        <v-btn color="success"
               @click="$router.back()">
          Gửi
        </v-btn>
        <v-btn color="secondary"
               @click="$router.back()">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
    export default {
        name: "ClaimLocationView"
    }
</script>

<style scoped>

</style>
