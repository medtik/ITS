<template>
  <v-card>
    <v-card-media style="width: 300px; height: 300px">
      <img :src='photo'/>
    </v-card-media>
    <v-card-text>
      <v-layout column>
        <v-flex style="text-align: center">
          <span></span>
        </v-flex>
        <v-flex>
          <v-layout column>
            <v-flex style="text-align: start" pt-1 pb-0>
              <span class="title">{{title}}</span>
            </v-flex>
            <v-flex style="text-align: start" pt-1 pb-1>
              <span class="subheading">Kéo dài {{dayCount}} ngày</span>
            </v-flex>
            <v-flex pt-1 pb-1>
              <span class="subheading">Với {{voteCount}} điểm</span>
            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
  export default {
    name: "PlanCard",
    data() {
      return {
        title: 'Chuyến đi ABC',
        photo: 'https://picsum.photos/400/600',
        dayCount: 6,
        voteCount: 68
      }
    }
  }
</script>

<style scoped>

</style>
