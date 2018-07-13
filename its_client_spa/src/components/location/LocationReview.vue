<template>
  <v-layout row pt-3 elevation-2 class="white">
    <v-flex xs3>
      <v-layout column>
        <v-flex d-flex justify-center>
          <v-avatar size="60">
            <img :src="user.photo"/>
          </v-avatar>
        </v-flex>
        <v-flex d-flex class="text-xs-center">
          <span>{{user.name}}</span>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex :class="{xs9: !editMode, xs8: editMode}">
      <v-layout column>
        <v-flex mb-1>
          <StarRating :star-size="25" v-model="rating" read-only></StarRating>
        </v-flex>
        <v-divider/>
        <v-flex my-1>
            <span class="title font-weight-medium">
              {{title}}
            </span>
        </v-flex>
        <v-flex pa-1>
          <p class="body-1">
            {{description}}
          </p>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex v-if="editMode" class="xs2" style="text-align: center">
      <v-btn icon v-on:click="onDelete">
        <v-icon color="red">
          delete
        </v-icon>
      </v-btn>
    </v-flex>
  </v-layout>
</template>

<script>
  import StarRating from "vue-star-rating";

  export default {
    name: "LocationReview",
    components: {
      StarRating
    },
    props: [
      'id',
      'rating',
      'title',
      'description',
      'user',
      'editMode'
    ],
    methods: {
      onDelete() {
        this.$emit('delete', this.id);
      }
    }
  }
</script>

<style scoped>

</style>
