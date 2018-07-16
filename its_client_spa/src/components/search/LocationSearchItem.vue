<template>
  <v-layout row>
    <v-flex style="flex-grow: 0">
      <v-avatar tile size="100">
        <img :src="primaryPhoto"/>
      </v-avatar>
    </v-flex>
    <v-flex pa-2>
      <v-layout column>
        <v-flex class="title">
          {{text}}
        </v-flex>
      </v-layout>
    </v-flex>
  </v-layout>
</template>

<script>
  export default {
    name: "LocationSearchItem",
    props: [
      'text'
    ],
    data() {
      return {
        primaryPhoto: 'https://picsum.photos/500/500'
      }
    }
  }
</script>

<style scoped>

</style>
