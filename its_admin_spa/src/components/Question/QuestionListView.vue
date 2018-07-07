<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí câu hỏi</span>
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
          <v-btn color="primary"
                 :to="{name:'QuestionCreate'}">
            Tạo mới
          </v-btn>
        </v-card-title>
        <!--Content start-->
        <v-data-table
          :items="items"
          :total-items="total"
          :pagination.sync="pagination"
          :headers="headers"
          :loading="loading">
          <template slot="items" slot-scope="props">
            <td>{{ props.item.text }}</td>
            <td>{{ props.item.category }}</td>
            <td>{{ props.item.area }}</td>
            <td>{{ props.item.answers.length }}</td>
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
        <!--Content end-->
      </v-flex>
    </v-layout>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>
<!--
TODO
text
category

ADVANCE
answer
tag
-->
<script>
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";

  export default {
    name: "QuestionListView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        //TABLE
        loading: true,
        items: [],
        headers: [
          {text: 'Nội dung', value: 'text'},
          {text: 'Thể loại', value: 'category'},
          {text: 'Khu vực', value: 'area'},
          {text: 'Số câu trả lời', value: 'answers.length'},
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
    methods:{
      loadData() {
        this.loading = true;
        this.$store.dispatch('question/getAll', {
          search: this.searchText,
          pagination: this.pagination
        })
          .then(data => {
            this.items = data.questions;
            this.total = data.total;
            this.loading = false;
          })
          .catch(error => {
            this.error = {
              dialog: true,
              title: 'Chú ý',
              message: error.message
            };
            this.loading = false;
            // console.error('loadData', error);
          })
      },
      onSearchEnter(){
        this.pagination.page = 1;
        this.loadData();
      },
      onRemove(question){
        this.loading = true;
        this.$store.dispatch('question/delete', {
          id:question.id
        })
          .then(value => {
            this.pagination.page = 1;
            this.loadData();
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
