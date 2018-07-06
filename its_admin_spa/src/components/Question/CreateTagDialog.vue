<template>
  <v-dialog
    persistent
    v-model="dialog"
    max-width="300">
    <v-card>
      <v-card-title class="headline">
        <span>Tạo tag mới</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text class="text-xs-center text-md-left">
        <span class="body-2">{{message}}</span>
      </v-card-text>
      <v-card-actions>
        <v-btn
          color="success"
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
    name: "CreateTagDialog",
    props: [
      'dialog',
    ],
    methods: {
      onClose() {
        this.$emit('close')
      }
    }
  }
</script>

<style scoped>

</style>
