<template>
  <v-container id="content" fluid>
    <v-layout row pa-3>
      <v-flex xs12>
        <span class=title v-if="mode == 'create'">Tạo mới địa điểm</span>
        <span class=title v-if="mode == 'edit'">Chỉnh sửa địa điểm</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear v-if="loading.page" color="primary" indeterminate></v-progress-linear>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  export default {
    name: "LocationCreateEditView",
    data() {
      return {
        mode: 'create',
        loading:{
          page: true
        }
      }
    }
  }
</script>

<style scoped>

</style>
