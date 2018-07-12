<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Các nhóm của bạn"
    />
    <v-layout column>
      <v-flex v-for="n in 3"
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
  import GroupFullWidth from "../shared/GroupFullWidth"

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
