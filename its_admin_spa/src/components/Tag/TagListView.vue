<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí thẻ</span>
        <v-divider class="my-3"></v-divider>
        <!--Content start-->
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
          <v-btn color="primary" v-on:click="onCreateClick">
            Tạo mới
          </v-btn>
        </v-card-title>
        <v-data-table
          :items="items"
          :total-items="total"
          :pagination.sync="pagination"
          :headers="headers"
          :loading="loading">
          <template slot="items" slot-scope="props">
            <td>{{ props.item.name }}</td>
            <td>{{ props.item.category }}</td>
            <td>{{ props.item.locationCount}}</td>
            <td class="justify-center layout px-0">
              <a>
                <v-icon
                  small
                  class="mr-2"
                  color="green"
                  v-on:click="onEditClick(props.item)">
                  edit
                </v-icon>
              </a>
              <a>
                <v-icon
                  small
                  color="red"
                  v-on:click="onDeleteClick(item)">
                  delete
                </v-icon>
              </a>
            </td>
          </template>
        </v-data-table>
        <!--Content end-->
      </v-flex>
    </v-layout>
    <TagCreateEditDialog
      v-bind="createEditDialog"
      v-on:close="createEditDialog.dialog = false"
      v-on:edit="onDialogConfirmEdit"
      v-on:create="onDialogConfirmCreate"
    />
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>

<script>
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";
  import TagCreateEditDialog from "./TagCreateEditDialog";

  export default {
    name: "TagListView",
    components: {ErrorDialog, SuccessDialog, TagCreateEditDialog},
    data() {
      return {
        //TABLE
        loading: true,
        items: [],
        headers: [
          {text: 'Tên', value: 'name'},
          {text: 'Thể loại', value: 'category'},
          {text: 'Số địa điểm', value: 'locationCount'},
          {text: 'Hành động', value: 'id', sortable: false},
        ],
        pagination: {},
        total: undefined,
        searchText: '',
        //TABLE
        error: {
          dialog: false,
          title: '',
          message: ''
        },
        success: {
          dialog: false,
          title: '',
          message: ''
        },
        createEditDialog: {
          dialog: false,
          item: false
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
    mounted() {
      this.loadData();
    },
    methods: {
      loadData() {
        this.loading = true;
        this.$store.dispatch('tag/getAll', {
          search: this.searchText,
          pagination: this.pagination
        })
          .then(data => {
            this.items = data.tags;
            this.total = data.total;
            this.loading = false;
          })
          .catch(error => {
            this.error = {
              dialog: true,
              title: 'Chú ý',
              message: error.message
            };
          })
      },
      onSearchEnter() {
        this.pagination.page = 1;
        this.loadData();
      },
      onCreateClick() {
        this.createEditDialog = {
          dialog: true
        }
      },
      onEditClick(item) {
        this.createEditDialog = {
          dialog: true,
          item
        }
      },
      onDeleteClick(item) {
        this.loading = true;
        setTimeout(() => {
          this.loading = false;
        }, 1200)
      },
      onDialogConfirmCreate(item) {
        this.createEditDialog = {
          dialog: false
        }
      },
      onDialogConfirmEdit(item) {
        this.createEditDialog = {
          dialog: false
        }
      }
    }
  }
</script>

<style scoped>

</style>
