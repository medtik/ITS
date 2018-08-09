<template>
  <v-dialog v-model="isOpen" max-width="550" persistent>
    <v-card>
      <v-card-title class="light-blue darken-2 white--text title">
        {{title}}
      </v-card-title>
      <v-card-text>
        <v-layout column px-2>
          <v-flex>
            {{message}}
          </v-flex>
          <v-flex>
            <v-textarea v-model="messageInput">

            </v-textarea>
          </v-flex>
        </v-layout>
      </v-card-text>
      <v-card-actions>
        <v-btn color="success" @click="onConfirm">
          Xác nhận
        </v-btn>
        <v-btn color="secondary" @click="onCancel">
          Hủy
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: "MessageInputDialog",
    props: [
      'dialog',
      'title',
      'value',
      'message'
    ],
    data() {
      return {
        isOpen: false,
        messageInput: undefined,
      }
    },
    watch: {
      value(val) {
        this.messageInput = val;
      },
      dialog(dialog) {
        this.isOpen = dialog;
      },
      isOpen(isOpen) {
        if (!isOpen) {
          this.messageInput = '';
        }
      }
    },
    methods: {
      onConfirm() {
        this.isOpen = false;
        this.$emit('input', this.messageInput);
        this.$emit('confirm', this.messageInput);
      },
      onCancel() {
        this.isOpen = false;
        this.$emit('close');
      }
    }
  }
</script>

<style scoped>

</style>
