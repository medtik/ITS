<template>
  <v-dialog :value="dialog" persistent width="500">
    <v-layout column class="white" pa-3>
      <v-flex d-flex justify-center pa-4 style="text-align: center">
        <PictureInput
          v-if="dialog"
          width="400"
          height="400"
          size="50"
          :text="text"
          v-model="photoInput"
        />
      </v-flex>
      <v-flex>
        <v-layout row justify-end>
          <v-btn color="success" @click="onConfirm">
            Xác nhận
          </v-btn>
          <v-btn color="secondary" @click="onClose">
            Hủy
          </v-btn>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-dialog>
</template>

<script>
  import PictureInput from "../input/PictureInput";

  export default {
    name: "ChooseImageDialog",
    props: [
      'dialog',
      'text'
    ],
    components: {
      PictureInput
    },
    data() {
      return {
        photoInput: undefined
      }
    },
    methods: {
      onClose() {
        this.$emit('close');
      },
      onConfirm() {
        this.$emit('confirm', this.photoInput);
      }
    }
  }
</script>

<style scoped>

</style>
