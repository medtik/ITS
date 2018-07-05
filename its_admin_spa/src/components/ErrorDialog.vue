<template>
  <v-dialog
    v-model="dialog"
    max-width="290"
  >
    <v-card color="error">
      <v-card-title class="headline">{{title}}</v-card-title>

      <v-card-text>
        {{message}}
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>

        <v-btn
          color="green darken-1"
          flat="flat"
          @click="this.$emit('ok')">
          OK
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: "ErrorDialog",
    props: [
      'dialog',
      'title',
      'message'
    ],

  }
</script>

<style scoped>

</style>
