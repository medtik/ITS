<template>
  <router-link id="outer-link"
               tag="div"
               :to="{name:'LocationDetail', params:{id: this.id}}">
    <v-layout row>
      <!--<editor-fold desc="Handle">-->
      <v-flex xs4
              sm2
              d-flex
              align-center
              justify-center
              v-if="$slots.handle"
              class="text-xs-center">
        <slot name="handle"></slot>
      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Photo">-->
      <v-flex style="flex-grow: 0">
        <v-avatar tile size="140">
          <img :src="primaryPhoto.url"/>
        </v-avatar>
      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Content">-->
      <v-flex pa-2 style="flex-grow: 0">
        <v-layout column>
          <v-flex class="title">
            <v-icon>{{locationIcon}}</v-icon>
            {{name}}
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
            <span>{{reviews.length}} bình luận</span>
          </v-flex>
          <v-flex
            mb-1
            class="caption font-weight-light">
            <v-icon small>place</v-icon>
            <span>{{address}}</span>
          </v-flex>
          <v-divider v-if="reason"></v-divider>
          <v-flex class="body-1 font-weight-light"
                  v-if="reason">
            {{reason}}
          </v-flex>
        </v-layout>
      </v-flex>
      <!--</editor-fold>-->
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
      'name',
      'rating',
      'reviews',
      'address',
      'reason',
      'primaryPhoto'
    ],
    computed: {
      locationIcon() {
        return 'restaurant';
      }
    }
  }
</script>

<style scoped>
  #outer-link:hover {
    cursor: pointer;
  }
</style>
