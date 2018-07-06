<template>
  <v-dialog
    v-model="dialog"
    max-width="300">
    <v-card>
      <v-card-title class="headline" style="background-color: #E2747D; color: white">
        <v-spacer></v-spacer>
        <v-icon color="white" size="60" style="font-weight: 100">far fa-times-circle</v-icon>
        <v-spacer></v-spacer>
      </v-card-title>
      <v-card-text class="text-xs-center text-md-left">
        <span class="body-2">{{message}}</span>
      </v-card-text>
      <v-card-actions>
        <v-btn
          color="error"
          block
          flat
          @click="onClose">
          Đóng
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
    methods: {
      onClose(){
        this.$emit('close')
      }
    }
  }
</script>

<style scoped>

</style>
