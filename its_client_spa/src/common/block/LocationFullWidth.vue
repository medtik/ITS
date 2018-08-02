<template>
  <v-layout row pa-1 style="max-height: 200px; height: 100px">
    <!--PHOTO-->
    <v-flex xs4 lg2 style="width: 100px">
       <router-link class="fakeLink"
                    tag="div"
                    :to="link">
         <v-flex>
           <img :src="primaryPhoto" style="max-width: 100%; max-height: 100%"/>
         </v-flex>
       </router-link>
    </v-flex>
    <v-flex shrink>
      <v-divider vertical></v-divider>
    </v-flex>
    <!--FIELDS-->
    <v-flex row layout wrap px-1>
      <!--SHORT-TOP FIELD-->
      <v-layout column xs12>
        <v-flex class="title">
          <!--<v-icon>{{locationIcon}}</v-icon>-->
          {{location}}
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
          <span>{{reviewCount}} bình luận</span>
        </v-flex>
      </v-layout>
      <!--LONG-BOT FIELD-->
      <v-flex column layout xs12>
        <v-flex
          mb-1
          class="caption font-weight-light">
          <v-icon small>place</v-icon>
          <span>{{address}}</span>
        </v-flex>

        <v-flex v-if="reasons" class="body-1 font-weight-light">
          <v-divider></v-divider>
          <v-chip v-for="(reason, index) in reasons" :key="`r${index}`">
            {{reason}}
          </v-chip>
        </v-flex>

      </v-flex>
    </v-flex>
  </v-layout>
</template>
<!--Plan detail actions-->
<!--chuyển ngày, lên xuống, xóa-->
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
      imageSize() {
        if (this.isSmallScreen) {
          return 100
        } else {
          return 150
        }
      },
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
      locationIcon() {
        return 'restaurant';
      },
      link() {
        return {
          name: 'LocationDetail',
          query: {
            id: this.id
          }
        }
      }
    }
  }
</script>
