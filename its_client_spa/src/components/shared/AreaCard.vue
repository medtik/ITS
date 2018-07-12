<template>
  <v-card>
    <v-card-media style="width: 300px; height: 300px">
      <img :src='photo'/>
    </v-card-media>
    <v-card-text class="">
      <v-layout column>
        <v-flex class="text-xs-center" pb-0>
          <span class="title">{{name}}</span>
        </v-flex>
        <v-flex>
          <v-divider></v-divider>
          <v-layout row>
            <v-flex xs6 style="text-align: center">
              <span class="title">{{locationCount}}</span><br/>
              <v-label>Địa điểm</v-label>
            </v-flex>
            <v-flex xs6 style="text-align: center">
              <span class="title">{{planCount}}</span><br/>
              <v-label>Chuyến đi</v-label>
            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>

    </v-card-text>
  </v-card>
</template>

<script>
  export default {
    name: "AreaCard",
    data() {
      return {
        name: 'Thành phố Hồ Chí Minh',
        photo: 'https://picsum.photos/400/400',
        locationCount: 22,
        planCount: 67
      }
    }
  }
</script>

<style scoped>

</style>
