<template>
<v-content>
  <ParallaxHeader
    src="https://picsum.photos/1200/900/?blur"
    :text="area.name"
  />
  <v-layout column>
    <v-flex>
      <v-toolbar flat light>
        <v-toolbar-items style="width: 100%"
                         class="d-flex justify-center">
          <v-btn flat color="light-blue">
            <v-icon large>
              star
            </v-icon>
          </v-btn>
          <v-btn flat color="light-blue">
            <v-icon large>
              hotel
            </v-icon>
          </v-btn>
          <v-btn flat color="light-blue">
            <v-icon large>
              restaurant
            </v-icon>
          </v-btn>
          <v-btn flat color="light-blue">
            <v-icon large>
              local_activity
            </v-icon>
          </v-btn>

        </v-toolbar-items>
      </v-toolbar>
    </v-flex>
    <v-flex style="height: 15vh">
      <!--Holder-->
    </v-flex>
  </v-layout>
</v-content>
</template>

<script>
  import ParallaxHeader from "../shared/ParallaxHeader";

  export default {
    name: "AreaDetail",
    data(){
      return {
        area:{
          name: 'Thành phố Hồ Chí Minh'
        }
      }
    },
    components: {
      ParallaxHeader
    }
  }
</script>

<style scoped>

</style>
