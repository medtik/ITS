<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí thẻ</span>
        <v-divider class="my-3"></v-divider>
        <!--Content start-->
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
            <td>{{ props.item.categories}}</td>
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
                  v-on:click="onDeleteClick(props.item)">
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
  import TagCreateEditDialog from "./TagCreateEditDialog";
  import {ErrorDialog, SuccessDialog} from "../../common/block";


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
          {text: 'Thể loại', value: 'categories'},
          {text: 'Số địa điểm', value: 'locationCount'},
          {text: 'Hành động', value: 'id', sortable: false},
        ],
        pagination: {},
        total: undefined,
        searchInput: '',
        formError:{
          ['data.Name']: undefined,
          ['data.Categories']: undefined
        },
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
    methods: {
      loadData() {
        this.loading = true;
        this.$store.dispatch('tag/getAll', {
          search: this.searchInput,
          pagination: this.pagination
        })
          .then(data => {
            this.items = data.list;
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
        this.$store.dispatch('tag/delete', {id: item.id})
          .then(value => {
            this.loadData();
          })
          .catch(reason => {
            console.debug('onDialogConfirmCreate-catch', reason);
            this.error = {
              dialog: true,
              message: 'Có lỗi xẩy ra'
            }
          })
      },
      onDialogConfirmCreate(item) {
        this.$store.dispatch('tag/create', {tag: item})
          .then(() => {
            this.loadData();
          })
          .catch(reason => {
            this.error = {
              dialog: true,
              message: 'Có lỗi xảy ra'
            }
          });
        this.createEditDialog = {
          dialog: false
        };
        this.formError = reason.data.modelState;
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
