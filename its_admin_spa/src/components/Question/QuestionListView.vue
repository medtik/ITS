<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí câu hỏi</span>
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
          <v-btn color="primary"
                 :to="{name:'QuestionCreate'}">
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
            <td>{{ props.item.content }}</td>
            <td>{{ props.item.categories }}</td>
            <td>{{ props.item.answerCount }}</td>
            <td class="justify-center layout px-0">
              <router-link :to="{name:'QuestionEdit', query:{id:props.item.id}}">
                <v-icon
                  small
                  class="mr-2"
                  color="green">
                  edit
                </v-icon>
              </router-link>
              <a>
                <v-icon
                  small
                  color="red"
                  @click="onRemove(props.item)">
                  delete
                </v-icon>
              </a>
            </td>
          </template>
        </v-data-table>
      </v-flex>
    </v-layout>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>
<script>
  import {ErrorDialog, SuccessDialog} from "../../common/block";


  export default {
    name: "QuestionListView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        //TABLE
        loading: true,
        items: [],
        headers: [
          {text: 'Nội dung', value: 'content'},
          {text: 'Thể loại', value: 'categories'},
          {text: 'Số câu trả lời', value: 'answerCount'},
          {text: 'Hành động', value: 'id', sortable: false},
        ],
        pagination: {},
        total: undefined,
        searchInput: '',
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
        this.$store.dispatch('question/getAll', {
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
              message: error.message
            };
            this.loading = false;
          })
      },
      onSearchEnter() {
        this.pagination.page = 1;
        this.loadData();
      },
      onRemove(question) {
        this.loading = true;
        this.$store.dispatch('question/delete', {
          id: question.id
        })
          .then(value => {
            this.pagination.page = 1;
            this.loadData();
            this.success = {
              dialog: true,
              message: 'Xóa thành công'
            }
          })
          .catch(reason => {
            this.error = {
              dialog: true,
              message: reason.message
            };
            this.loading = false;
          })
      }
    }
  }
</script>

<style scoped>

</style>
