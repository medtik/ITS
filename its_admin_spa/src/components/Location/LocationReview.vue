<template>
  <v-container>
    <v-layout row>
      <v-flex xs2 class="green lighten-4">
        <v-layout column class="green lighten-3">
          <v-flex class="amber lighten-5" d-flex justify-center>
            <v-avatar size="60">
              <img src="https://picsum.photos/200/200"/>
            </v-avatar>
          </v-flex>
          <v-flex class="amber lighten-3" d-flex justify-center>
            Justin
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex xs12 class="blue lighten-1">
        <v-layout column>
          <v-flex class="blue lighten-4">
            <StarRating :star-size="25" v-model="rating" read-only></StarRating>
          </v-flex>
          <v-flex class="blue lighten-2">
            <span class="subheading">
              Some title
            </span>
          </v-flex>
          <v-flex class="blue">
            <p class="body-1">
              dk;akd;laskdl;akdl;asdk;lka
            </p>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  import StarRating from "vue-star-rating";

  export default {
    name: "LocationReview",
    components: {
      StarRating
    },
    props:[
      'rating',
      'title',
      'description '
    ],
  }
</script>

<style scoped>

</style>
