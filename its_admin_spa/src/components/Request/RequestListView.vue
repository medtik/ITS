<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí Yêu cầu</span>
        <v-divider class="my-3"></v-divider>
        <v-layout column>
          <v-flex my-1
                  v-for="request in requests"
                  :key="request.id">
            <RequestChangeLocationInfo v-bind="request"/>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>

<script>
  import RequestChangeLocationInfo from "./RequestChangeLocationInfo";
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";
  import _requests from '../../store/modules/mockdata/LocationChangeRequests'

  export default {
    name: "RequestListView",
    components: {
      ErrorDialog,
      SuccessDialog,
      RequestChangeLocationInfo
    },
    data() {
      return {
        requests: _requests,
        loading: true,
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
