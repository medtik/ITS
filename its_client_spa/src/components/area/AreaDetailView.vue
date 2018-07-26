<template>
<v-content>
  <ParallaxHeader
    src="https://picsum.photos/1200/900/?blur"
    :text="area.name"
  />
  <v-layout column mx-2>
    <v-flex>
      <v-toolbar flat light>
        <v-toolbar-items style="width: 100%"
                         class="d-flex justify-center">
          <v-btn flat color="light-blue" @click="$vuetify.goTo('#hotel',{offset:-50})">
            <v-icon large>
              hotel
            </v-icon>
          </v-btn>
          <v-btn flat color="light-blue" @click="$vuetify.goTo('#restaurant',{offset:-50})">
            <v-icon large>
              restaurant
            </v-icon>
          </v-btn>
          <v-btn flat color="light-blue" @click="$vuetify.goTo('#activity',{offset:-50})">
            <v-icon large>
              local_activity
            </v-icon>
          </v-btn>
        </v-toolbar-items>
      </v-toolbar>
    </v-flex>
    <v-flex my-4>
      <div class="title" >
        Địa điểm nổi bật
      </div>
      <v-layout style="overflow-y: auto;">
        <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
          <LocationCard/>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex my-4 >
      <div class="title" >
        Chuyến đi nổi bật
      </div>
      <v-layout style="overflow-y: auto;">
        <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
          <PlanCard/>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex my-4 id="hotel">
      <div class="title" >
        Nơi ở
      </div>
      <v-layout style="overflow-y: auto;">
        <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
          <LocationCard/>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex my-4 id="restaurant">
      <div class="title">
        Khách sạn
      </div>
      <v-layout style="overflow-y: auto;">
        <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
          <LocationCard/>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex my-4 id="activity">
      <div class="title">
        Giải trí
      </div>
      <v-layout style="overflow-y: auto;">
        <v-flex v-for="n in 5" :key="n" mx-2 mt-2>
          <LocationCard/>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex style="height: 15vh">
      <!--Holder-->
    </v-flex>
  </v-layout>
</v-content>
</template>

<script>
  import ParallaxHeader from "../../sharedComponents/layout/ParallaxHeader";
  import PlanCard from "../../sharedComponents/block/PlanCard";
  import LocationCard from "../../sharedComponents/block/LocationCard";

  export default {
    name: "AreaDetail",
    components: {
      ParallaxHeader,
      LocationCard,
      PlanCard
    },
    data(){
      return {
        area:{
          name: 'Thành phố Hồ Chí Minh'
        }
      }
    },

  }
</script>

<style scoped>

</style>
