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
    <router-link class="fakeLink"
                 tag="div"
                 :to="link">
      <v-layout slot="title" px-1>
        <v-icon v-if="locationIcon">
          {{locationIcon}}
        </v-icon>
        &nbsp;
        <v-flex class="title" pl-1>
          {{locationName}}
        </v-flex>
      </v-layout>
    </router-link>
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
      <slot name="action">

      </slot>
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
      'categories',
      'name',
      'location',
      'rating',
      'reviewCount',
      'address',
      'reasons',
      'primaryPhoto',
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

        switch (type) {
          case 'Ăn uống':
            return 'fas fa-utensils';
          case 'Nơi ở':
            return 'fas fa-hotel';
          case 'Mua sắm':
            return 'fas fa-shopping-cart';
          case 'Giải trí':
            return 'fas fa-gamepad';
          case 'Địa điểm thăm quan':
            return 'fas fa-university';
          case 'Dịch vụ':
            return 'fas fa-gas-pump';
          case 'Tiền tệ':
            return 'fas fa-credit-card';
          case 'Trụ sở ban ngành':
            return 'far fa-building';
          case 'Trạm xăng':
            return 'fas fa-gas-pump';
        }
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
