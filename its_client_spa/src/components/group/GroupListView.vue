<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Các nhóm của bạn"
    />
    <v-layout column>
      <v-flex>
        <v-btn color="success" :to="{name:'GroupCreate'}">
          <v-icon>group_add</v-icon>&nbsp; Tạo nhóm
        </v-btn>
      </v-flex>
      <v-flex v-for="n in 3"
              :key="n"
              elevation-2
              mt-2>
        <GroupFullWidth/>
      </v-flex>
      <v-flex style="height: 25vh">
        <!--Holder-->
      </v-flex>
    </v-layout>

  </v-content>
</template>

<script>
  import ParallaxHeader from "../shared/ParallaxHeader"
  import GroupFullWidth from "./GroupFullWidth"

  export default {
    name: "GroupListView",
    components: {
      ParallaxHeader,
      GroupFullWidth
    },

  }
</script>

<style scoped>

</style>
