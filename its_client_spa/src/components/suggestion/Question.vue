<template>
  <v-layout column>
    <v-flex class="title font-weight-bold">{{text}}</v-flex>
    <v-flex v-for="answer in answers"
            :key="answer.id"
            mx-3>
      <v-checkbox
        :label="answer.text"
      />
    </v-flex>
  </v-layout>
</template>

<script>
  export default {
    name: "Question",
    props: [
      'text',
      'answers'
    ]
  }
</script>

<style scoped>
</style>
