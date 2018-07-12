<template>
  <v-card>
    <v-card-media style="width: 300px; height: 300px">
      <img :src='photo'/>
    </v-card-media>
    <v-card-text>
      <v-layout column>
        <v-flex class="title" style="text-align: center">
          {{name}}
        </v-flex>
        <v-flex>
          <StarRating
            :star-size="25"
            v-model="rating"
            read-only
            :show-rating="false"
            style="display: flex;justify-content: center;"
          />
        </v-flex>
        <v-flex>
          <v-label>
            9159 Lake View St.
            Clifton, NJ 07011
          </v-label>
        </v-flex>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
  import StarRating from "vue-star-rating"

  export default {
    name: "Locationcard",
    components: {
      StarRating
    },
    data() {
      return {
        name: 'Địa điểm abc',
        rating: 4,
        photo: 'https://picsum.photos/400/400',
      }
    }
  }
</script>

<style scoped>

</style>
