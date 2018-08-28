<template>
  <router-link :to="to" tag="section" class="fakeLink">
    <v-card-title class="light-blue white--text">
      <v-layout column>
        <div class="display-2 font-weight-black font-italic text-xs-center">
          ITS
        </div>
        <v-divider class="my-3"/>
        <div class="title font-weight-medium text-xs-center">
          Hệ thống hướng dẫn du lịch thông minh
        </div>
      </v-layout>
    </v-card-title>
  </router-link>
</template>

<script>
  export default {
    name: "AppCardTitle",
    data() {
      return {
        to: {
          name: 'Home'
        }
      }
    }
  }
</script>

<style scoped>

</style>
