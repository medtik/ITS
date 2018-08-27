<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí Yêu cầu</span>
        <v-divider class="my-3"></v-divider>
        <v-card-title>
          <v-text-field
            v-model="searchInput"
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
            <v-flex ml-3>
              <v-checkbox label="Thay đổi thông tin địa điểm" v-model="filter.changeLocationInfo"/>
              <v-checkbox label="Làm chủ địa điểm" v-model="filter.claimOwner"/>
              <v-checkbox label="Đã xử lí" v-model="filter.done"/>
            </v-flex>

          </v-layout>
        </v-card-title>
        <v-layout row wrap>
          <v-progress-linear indeterminate v-if="this.loading"/>
          <v-flex pa-1 sm12 md8 lg6
                  v-for="request in requests"
                  :key="request.id">
            <RequestReportReview v-if="request.type == 'reportReview'" v-bind="request"/>
            <RequestChangeLocationInfo v-if="request.type == 'locationChangeRequest'" v-bind="request"/>
          </v-flex>
        </v-layout>
        <v-layout v-if="total">
          <v-flex text-xs-center>
            <v-pagination
              v-bind="paginationModel"
              @input="paginationUpdate"
            />
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

  import {
    SuccessDialog,
    ErrorDialog
  } from "../../common/block";


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
        requests: undefined,
        filter: {
          locationChangeRequest: true,
          claimOwner: true,
          reportReview: true,
          done: false
        },
        loading: true,
        searchInput: '',
        pagination: {
          page: 1,
          rowsPerPage: 6
        },
        total: undefined,
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
    computed: {
      paginationModel() {
        const length = Math.ceil(this.total / this.pagination.rowsPerPage);
        return {
          value: this.pagination.page,
          length: length ? length : 0
        }
      }
    },
    created() {
      this.loadData();
    },
    methods: {
      loadData() {
        this.loading = true;
        this.$store.dispatch('request/getAll', {
          search: this.searchInput,
          pagination: this.pagination,
          filter: this.filter
        }).then(value => {
          this.requests = value.requests;
          this.total = value.total;
          this.loading = false
        }).catch(reason => {
          this.error = {
            dialog: true,
            ...reason
          }
        })
      },
      onSearchEnter() {
        this.pagination.page = 1;
        this.loadData();
      },
      paginationUpdate(page) {
        this.pagination.page = page;
        this.loadData();
      }
    }
  }
</script>

<style scoped>

</style>
