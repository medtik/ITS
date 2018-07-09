<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí Yêu cầu</span>
        <v-divider class="my-3"></v-divider>
        <v-card-title>
          <v-text-field
            v-model="searchText"
            v-on:keyup.enter="onSearchEnter"
            append-icon="search"
            label="Tìm"
            single-line
            hide-details>
          </v-text-field>
          <v-spacer></v-spacer>
          <v-layout column>
            <v-flex mb-1>
              <v-label>Lọc yêu cầu</v-label>
            </v-flex>
            <v-flex ml-3  >
              <v-checkbox label="Thay đổi thông tin địa điểm" v-model="filter.changeLocationInfo"/>
              <v-checkbox label="Làm chủ địa điểm" v-model="filter.claimOwner"/>
              <v-checkbox label="Báo cáo đánh giá" v-model="filter.reportReview"/>
              <v-checkbox label="Đã xử lí" v-model="filter.done"/>
            </v-flex>

          </v-layout>
        </v-card-title>
        <v-layout row wrap>
          <v-flex pa-1 sm12 md8 lg6
                  v-for="request in requests"
                  :key="request.id">
            <RequestReportReview v-bind="request"/>
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
  import RequestClaimOwner from "./RequestClaimOwner";
  import RequestReportReview from "./RequestReportReview";
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";
  import _requests from '../../store/modules/mockdata/ReportReviewRequest'

  export default {
    name: "RequestListView",
    components: {
      ErrorDialog,
      SuccessDialog,
      RequestChangeLocationInfo,
      RequestClaimOwner,
      RequestReportReview
    },
    data() {
      return {
        requests: _requests,
        filter: {
          changeLocationInfo: true,
          claimOwner: true,
          reportReview: true,
          done: false
        },
        loading: true,
        searchText: '',
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
    },
    methods: {
      onSearchEnter() {

      }
    }
  }
</script>

<style scoped>

</style>
