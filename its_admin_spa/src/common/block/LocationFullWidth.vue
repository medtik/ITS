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
    <v-layout slot="handle" px-1 v-if="$slots.handle" style="width: 100%; height: 100%">
      <slot name="handle"></slot>
    </v-layout>
    <v-layout slot="title" px-1>

      <v-icon v-if="locationIcon">
        {{locationIcon}}
      </v-icon>

      &nbsp;
      <v-flex class="title" pl-1>
        <router-link class="fakeLink"
                     tag="span"
                     :to="link">
          {{locationName}}
        </router-link>
      </v-flex>

    </v-layout>
    <v-layout slot="detail" column pl-2>
      <v-flex v-if="range"
              class="font-weight-black subheading">
        {{range}} km
      </v-flex>
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
    <v-layout slot="action" column align-end v-if="$slots.action">
      <slot name="action">

      </slot>
    </v-layout>
  </ListItemLayout>
</template>

<script>
  import StarRating from "vue-star-rating";
  import formatter from "../formatter";

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
      'categories',
      'name',
      'location',
      'rating',
      'reviewCount',
      'address',
      'reasons',
      'primaryPhoto',
      'range',
    ],
    computed: {
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
      locationIcon() {
        let type = this.type;
        if (!type) {
          type = this.categories;
        }

        return formatter.getCategoryIcon(type);
      },
      locationName() {
        if (this.name) {
          return this.name;
        } else {
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
