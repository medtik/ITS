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
    <v-layout slot="title" px-1>
      <v-icon v-if="locationIcon">
        {{locationIcon}}
      </v-icon>
      &nbsp;
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
      <template v-if="isOwn">
        <v-checkbox color="success" :v-model="localIsChecked">
        </v-checkbox>
        <v-btn icon flat color="secondary" small
               @click="$emit('delete',id)">
          <v-icon small>
            fas fa-trash
          </v-icon>
        </v-btn>
      </template>

      <!--PLAN EDIT-->
      <!--up, down, change date-->
      <!--<template v-if="isOwnEdit">-->
      <!--<v-btn icon flat color="success"-->
      <!--@click="$emit('save',id)">-->
      <!--<v-icon>-->
      <!--fas fa-heart-->
      <!--</v-icon>-->
      <!--</v-btn>-->
      <!--</template>-->

      <!--SEARCH RESULT-->
      <!--save to plan-->
      <template v-if="isCheckable">
        <v-btn v-if="localIsChecked"
               icon flat color="success"
               @click="onCheck">
          <v-icon>
            fas fa-plus-circle
          </v-icon>
        </v-btn>
        <v-btn v-if="!localIsChecked"
               icon flat
               @click="onCheck">
          <v-icon>
            fas fa-plus-circle
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
    data() {
      return {
        'localIsChecked': false
      }
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
      'isOwn',
      'isSearchResult',
      'isOwnEdit',
      'isCheck',
      'isCheckable'
    ],
    watch: {
      isCheck(val) {
        this.localIsChecked = val;
      }
    },
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
    },
    methods: {
      onCheck() {
        this.$emit('save', {
          id: this.id,
          check: !this.localIsChecked
        });
        this.localIsChecked = !this.localIsChecked;
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
