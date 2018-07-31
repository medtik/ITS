<template>
  <router-link :to="routerLink" tag="span" class="fakeLink">
    <v-card style="width: 300px;">
      <v-card-media>
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
  </router-link>
</template>

<script>
  export default {
    name: "AreaCard",
    props: [
      'id',
      'name',
      'locationCount',
      'planCount',
      'photo'
    ],
    computed: {
      routerLink() {
        return {
          name: 'AreaDetail',
          params: {
            id: this.id
          }
        }
      }
    }
  }
</script>

<style scoped>

</style>
