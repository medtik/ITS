<template>
  <v-layout column elevation-2>
    <v-flex pa-2>
      <div class="body-1">{{message}}</div>
    </v-flex>
    <v-flex v-if="$slots.actions">
      <v-divider/>
      <slot name="actions"></slot>
    </v-flex>
  </v-layout>
</template>

<script>
  export default {
    name: "NotificationFullWidth",
    props: [
      'title',
      'message'
    ]
  }
</script>

<style scoped>

</style>
