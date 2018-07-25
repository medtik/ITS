<template>
  <v-layout row>
    <!--HANDLE-->
    <v-flex xs3
            sm1
            d-flex
            align-center
            justify-center
            v-if="$slots.handle"
            class="text-xs-center">
      <slot name="handle"></slot>
    </v-flex>
    <!--CONTENT-->
    <v-layout column pa-1>
      <v-flex class="title font-weight-medium" my-2>
        {{title}}
      </v-flex>
      <v-divider></v-divider>
      <v-flex class="body-1" pa-1>
        {{text}}
      </v-flex>
    </v-layout>
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
