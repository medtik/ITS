<template>
  <v-layout column>
    <v-flex pb-2>
      <span class="subheading">{{title}}</span>
    </v-flex>
    <v-flex>
      <v-label>
        {{startDate}} - {{endDate}}
      </v-label>
    </v-flex>
    <v-flex>
      <v-layout style="overflow-y: auto;" >
        <v-flex v-for="n in 9" :key="n" mx-2>
          <LocationCard/>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-layout>
</template>

<script>
  import LocationCard from "../shared/LocationCard"

  export default {
    name: "PlanFullWidth",
    components:{
      LocationCard
    },
    data() {
      return {
        title: 'Plan abc',
        startDate: '20/7/2018',
        endDate: '25/7/2018'
      }
    }
  }
</script>

<style scoped>

</style>
