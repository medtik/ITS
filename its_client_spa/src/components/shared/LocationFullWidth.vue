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
                   :to="{name:'LocationDetail', params:{id: this.id}}">
        <v-avatar tile size="140">
          <img :src="primaryPhoto.url"/>
        </v-avatar>
      </router-link>
    </v-flex>
    <!--FIELD-->
    <v-flex pa-2 style="flex-grow: 0">
      <router-link class="fakeLink"
                   tag="div"
                   :to="{name:'LocationDetail', params:{id: this.id}}">
        <v-layout column justify-around>
          <v-layout row>
            <v-flex class="title">
              <v-icon>{{locationIcon}}</v-icon>
              {{name}}
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
            <span>{{reviews.length}} bình luận</span>
          </v-flex>
          <v-flex
            mb-1
            class="caption font-weight-light">
            <v-icon small>place</v-icon>
            <span>{{address}}</span>
          </v-flex>
          <v-divider v-if="reason"></v-divider>
          <v-flex v-if="reason" class="body-1 font-weight-light">
            Bởi vì bạn chọn "Sang trong", "Phong cách Ý"
            <br/>
            <v-chip>
              Ẩm thực ý
            </v-chip>
            <v-chip>
              5 sao
            </v-chip>
            <v-chip>
              Giá cao
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
</style>
