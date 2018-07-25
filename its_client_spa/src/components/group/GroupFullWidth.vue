<template>
  <v-layout column pa-2 class="white">
    <v-flex class="title" py-2>
      Nhóm ABC
    </v-flex>
    <v-divider></v-divider>
    <v-layout row wrap>
      <v-flex xs12 sm5 py-2 class="text-xs-center">
        <!--USERS-->
        <v-flex>
          <v-icon :size="45" color="primary">
            fas fa-users
          </v-icon>
        </v-flex>
        <v-flex class="subheading font-weight-medium">
          8 thành viên
        </v-flex>
      </v-flex>
      <v-flex d-flex justify-center>
        <v-divider vertical class="sm1"></v-divider>
      </v-flex>
      <v-flex xs12 sm5 py-2 flex class="text-xs-center">
        <!--TRIPS-->
        <v-flex>
          <v-icon :size="45" color="primary">
            fas fa-suitcase
          </v-icon>
        </v-flex>
        <v-flex class="subheading font-weight-medium">
          3 chuyến đi
        </v-flex>
      </v-flex>
    </v-layout>
  </v-layout>
</template>
<!--TODO More Info-->
<script>
  export default {
    name: "GroupFullWidth"
  }
</script>

<style scoped>

</style>
