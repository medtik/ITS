<template>
  <Request :user="user" :status="status" :title="title">
    <template slot="detail">
      <v-textarea
        readonly
        v-model="description"
      />
    </template>
  </Request>
</template>

<script>
  import Request from "./Request";

  export default {
    name: "ClaimOwnerRequest",
    components: {Request},
    props: [
      'user',
      'location',
      'title',
      'status',
      'description'
    ],

  }
</script>

<style scoped>

</style>
