<template>
  <ListItemLayout>
    <v-layout slot="photo">
      <v-flex style="flex-grow: 0">
        <router-link class="fakeLink"
                     tag="div"
                     :to="link">
          <img :src="primaryPhoto"/>
        </router-link>
      </v-flex>
    </v-layout>
    <v-layout slot="title">
      <v-flex class="title" pl-1>
        {{locationName}}
      </v-flex>
    </v-layout>
    <v-layout slot="detail" column pl-2>
      <StarRating
        read-only
        :rating="rating"
        :star-size="15"
        :increment="0.1"
        :show-rating="false"
        style="flex-grow: 0"
      />
      <v-flex
        mt-1
        class="caption font-weight-light"
        style="flex-grow: 0;">
        <v-icon small color="">comment</v-icon>
        <span>{{reviewCount}} bình luận</span>
      </v-flex>
      <v-flex
        v-if="!isSmallScreen"
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
    </v-layout>
    <v-layout slot="action" column align-end>
      <!--PLAN DETAIL-->
      <!--delete, check-->
      <template v-if="isOwn" >
        <v-checkbox color="success" v-model="isCheck">
        </v-checkbox>
        <v-btn icon flat color="red" small
              @click="$emit('delete',id)">
          <v-icon small>
            fas fa-trash
          </v-icon>
        </v-btn>
      </template>


      <!--PLAN EDIT-->
      <!--up, down, change date-->

      <!--SEARCH RESULT-->
      <!--save to plan-->
      <template v-if="isSearchResult">
        <v-btn icon flat color="success"
               @click="$emit('save',id)">
          <v-icon>
            fas fa-heart
          </v-icon>
        </v-btn>
      </template>

    </v-layout>
  </ListItemLayout>
</template>

<script>
  import StarRating from "vue-star-rating";
  import {ListItemLayout} from "../../common/layout";

  export default {
    name: "LocationSearchItem",
    components: {
      StarRating,
      ListItemLayout
    },
    props: [
      'id',
      'type',
      'name',
      'location',
      'rating',
      'reviewCount',
      'address',
      'reasons',
      'primaryPhoto',
      'isOwn',
      'isSearchResult',
      'isOwnEdit',
      'isCheck'
    ],
    computed: {
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
      locationIcon() {
        return 'restaurant';
      },
      locationName(){
        if(this.name){
          return this.name;
        }else{
          return this.location;
        }
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

<style scoped>
  img {
    max-width: 100%;
    max-height: 100%;
  }
</style>
