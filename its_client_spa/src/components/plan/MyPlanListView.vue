<template>
  <v-content>
    <ParallaxHeader src="static/pexels-photo-490411.jpeg" text="Các chuyến đi của bạn"/>
      <v-layout column class="grey lighten-4">
        <v-flex>
          <v-btn color="success" :to="{name:'PlanCreate'}">
            <v-icon>
              fas fa-plus
            </v-icon>
            &nbsp;
            Tạo chuyến đi
          </v-btn>
        </v-flex>
        <v-flex v-for="n in 4" :key="n"
                my-2
                py-2
                elevation-1
                class="white">
          <PlanFullWidth/>
        </v-flex>
      </v-layout>
    <v-flex style="height: 30vh">
      <!--Holder-->
    </v-flex>
  </v-content>
</template>

<script>
  import ParallaxHeader from "../shared/ParallaxHeader"
  import PlanFullWidth from "../shared/PlanFullWidth"

  export default {
    name: "MyPlanListView",
    components: {
      ParallaxHeader,
      PlanFullWidth
    }
  }
</script>

<style scoped>

</style>
