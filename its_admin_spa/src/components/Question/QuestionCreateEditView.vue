<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí câu hỏi</span>
        <v-divider class="my-3"></v-divider>
        <span class="display-1">Nội dung</span>
      </v-flex>
    </v-layout>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>

<script>
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";

  export default {
    name: "QuestionCreateEditView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        loading: true,

        error: {
          dialog: false,
          title: '',
          message: ''
        },
        success: {
          dialog: false,
          title: '',
          message: ''
        }
      }
    },
    watch: {
      pagination: {
        //Do not use arrow funcs
        handler: function () {
          this.loadData();
        },
        deep: true
      }
    },
  }
</script>

<style scoped>

</style>
