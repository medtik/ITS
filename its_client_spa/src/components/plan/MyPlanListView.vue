<template>
  <v-content>
    <ParallaxHeader src="static/pexels-photo-490411.jpeg" text="Các chuyến đi của bạn"/>
    <section>
      <v-layout column>
        <v-flex>
          <v-btn color="success" :to="{name:'PlanCreate'}">
            <v-icon>
              fas fa-plus
            </v-icon>
            &nbsp;
            Tạo chuyến đi
          </v-btn>
        </v-flex>
        <v-flex my-4 mx-2 v-for="n in 4" :key="n">
          <PlanFullWidth/>
        </v-flex>
      </v-layout>
    </section>
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
