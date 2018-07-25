<template>
  <v-content>
    <v-toolbar dark flat color="light-blue">
      <v-toolbar-title>
        Báo cáo bình luận
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-2>
      <v-flex my-2>
        <v-text-field label="Lý do"/>
      </v-flex>
      <v-divider/>
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
<!--TODO show review detail on top-->
<script>
  export default {
    name: "ReportReviewView"
  }
</script>

<style scoped>

</style>
