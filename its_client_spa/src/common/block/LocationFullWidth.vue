<template>

  <v-layout row>
    <!--HANDLE-->
    <v-flex xs3
            sm1
            d-flex
            align-center
            justify-center
            v-if="$slots.handle"
            class="text-xs-center">
      <slot name="handle"></slot>
    </v-flex>
    <!--PHOTO-->
    <v-flex style="flex-grow: 0">
      <router-link class="fakeLink"
                   tag="div"
                   :to="link">
        <v-avatar tile size="140">
          <img :src="primaryPhoto"/>
        </v-avatar>
      </router-link>
    </v-flex>
    <!--FIELD-->
    <v-flex pa-2 style="flex-grow: 0">
      <router-link class="fakeLink"
                   tag="div"
                   :to="link">
        <v-layout column justify-around>
          <v-layout row>
            <v-flex class="title">
              <!--<v-icon>{{locationIcon}}</v-icon>-->
              {{location}}
            </v-flex>
          </v-layout>
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
            <span>{{reviewCount}} bình luận</span>
          </v-flex>
          <v-flex
            mb-1
            class="caption font-weight-light">
            <v-icon small>place</v-icon>
            <span>{{address}}</span>
          </v-flex>
          <v-divider v-if="reasons"></v-divider>
          <v-flex v-if="reasons" class="body-1 font-weight-light">
            <v-chip v-for="(reason, index) in reasons" :key="`r${index}`">
              {{reason}}
            </v-chip>
          </v-flex>
        </v-layout>
      </router-link>
    </v-flex>
  </v-layout>
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
      'location',
      'rating',
      'reviewCount',
      'address',
      'reasons',
      'primaryPhoto'
    ],
    computed: {
      locationIcon() {
        return 'restaurant';
      },
      link() {
        return {
          name: 'LocationDetail',
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
