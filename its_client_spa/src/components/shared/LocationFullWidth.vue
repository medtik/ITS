<template>
  <router-link id="outer-link"
               tag="div"
               :to="{name:'LocationDetail', params:{id: this.id}}">
    <v-layout row>
      <v-flex style="flex-grow: 0">
        <v-avatar tile size="140">
          <img :src="primaryPhoto"/>
        </v-avatar>
      </v-flex>
      <v-flex pa-2>
        <v-layout column>
          <v-flex class="title">
            {{text}}
          </v-flex>
          <v-flex>
            <v-layout row>
              <StarRating
                read-only
                :rating="rating"
                :star-size="15"
                :increment="0.1"
                :show-rating="false"
                style="flex-grow: 0"
              />
            </v-layout>
          </v-flex>
          <v-flex
            mt-1
            class="caption font-weight-light"
            style="flex-grow: 0;">
            <v-icon small color="">comment</v-icon>
            <span>{{ratingCount}} bình luận</span>
          </v-flex>
          <v-flex
            mb-1
            class="caption font-weight-light">
            <v-icon small>place</v-icon>
            <span>{{address}}</span>
          </v-flex>
          <v-divider></v-divider>
          <v-flex class="body-1 font-weight-light">
            {{reason}}
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </router-link>
</template>

<script>
  import StarRating from "vue-star-rating";

  export default {
    name: "LocationSearchItem",
    components: {
      StarRating
    },
    props: [
      'id',
      'type',
      'text',
      'rating',
      'ratingCount',
      'address',
      'reason',
      'primaryPhoto'
    ]
  }
</script>

<style scoped>
  #outer-link:hover {
    cursor: pointer;
  }
</style>
