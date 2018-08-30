<template>
  <Request :user="user"
           title="Báo cáo bình luận"
           :status="status">
    <template slot="detail">
      <v-layout column>
        <v-flex ma-1 class="subheading">
          {{message}}
        </v-flex>
        <v-flex>
          <LocationReview v-bind="review"/>
        </v-flex>
      </v-layout>

    </template>
  </Request>
</template>

<script>
  import Request from "./Request";
  import {LocationReview} from "../../common/block";


  export default {
    name: "RequestReportReview",
    components: {
      Request,
      LocationReview
    },
    props: [
      'isOwner',
      'title',
      'user',
      'location',
      'status',
      'review',
      'message'
    ]
  }
</script>

<style scoped>

</style>
