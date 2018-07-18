<template>
  <v-layout column pa-1>
    <v-flex class="subheading font-weight-medium">
      {{title}}
    </v-flex>
    <v-divider></v-divider>
    <v-flex class="body-1" px-2>
      {{text}}
    </v-flex>
  </v-layout>
</template>

<script>
  export default {
    name: "NoteFullWidth",
    props: [
      'title',
      'text'
    ]
  }
</script>

<style scoped>

</style>
