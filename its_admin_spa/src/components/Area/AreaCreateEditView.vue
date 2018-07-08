<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title" v-if="mode == 'create'">Tạo mới khu vực</span>
        <span class="title" v-if="mode == 'edit'">Chỉnh sửa khu vực</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear indeterminate :loading="loading.page"></v-progress-linear>
        <v-layout column>
        </v-layout>
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
    name: "AreaCreateEditView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        mode: 'create',
        loading: {
          page: true
        },
        //DIALOG START
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
        //DIALOG END;
      }
    }
  }
</script>

<style scoped>

</style>
