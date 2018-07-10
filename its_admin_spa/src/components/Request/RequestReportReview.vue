<template>
  <Request :user="user"
           :isOwner="isOwner"
           :title="title"
           :status="status">
    <template slot="detail">
      <LocationReview v-bind="review"/>
    </template>
  </Request>
</template>

<script>
  import Request from "./Request";
  import LocationReview from "../shared/LocationReview";

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
      'review'
    ]
  }
</script>

<style scoped>

</style>
