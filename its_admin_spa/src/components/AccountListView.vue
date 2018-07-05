<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12 >
        <span class="title">Quản lí tài khoản</span>
        <v-divider class="my-3"></v-divider>
        <v-card-title>
          <v-text-field
            v-model="searchText"
            v-on:keyup.enter="onSearchEnter"
            append-icon="search"
            label="Search"
            single-line
            hide-details>
          </v-text-field>
          <v-spacer></v-spacer>
          <v-btn color="primary"
                 :to="{name:'AccountCreate'}">
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
            <td>{{ props.item.email }}</td>
            <td>{{ props.item.phone }}</td>
            <td>{{ props.item.birthdate }}</td>
            <td>{{ props.item.address }}</td>
            <td class="justify-center layout px-0">
              <router-link :to="{name:'AccountEdit', query:{id:props.item.id}}">
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
                  @click="deleteItem(props.item)">
                  delete
                </v-icon>
              </a>
            </td>
          </template>
        </v-data-table>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  export default {
    name: "AccountListView",
    data() {
      return {
        loading: true,
        items: [],
        headers: [
          {text: 'Tên', value: 'name'},
          {text: 'Email', value: 'email'},
          {text: 'Điện thoại', value: 'phone'},
          {text: 'Ngày sinh', value: 'birthdate'},
          {text: 'Địa chỉ', value: 'address'},
          {text: 'Hành động', value: 'id', sortable: false},
        ],
        pagination: {},
        total: undefined,
        searchText: ''
      }
    },
    watch: {
      pagination: {
        //Do not use arrow funcs
        handler: function () {
          this.loading = true;
          this.$store.dispatch('account/getAll', {
            search: this.searchText,
            pagination: this.pagination
          })
            .then(data => {
              this.items = data.accounts;
              this.total = data.total;
              this.loading = false;
            })
        },
        deep: true
      }
    },
    mounted() {
      this.loading = true;
      this.$store.dispatch('account/getAll', {
        search: this.searchText,
        pagination: this.pagination
      })
        .then(data => {
          this.items = data.accounts;
          this.total = data.total;
          this.loading = false;
        })
    },
    methods: {
      onSearchEnter() {
        this.loading = true;
        this.$store.dispatch('account/getAll', {
          search: this.searchText,
          pagination: this.pagination
        })
          .then(data => {
            this.items = data.accounts;
            this.total = data.total;
            this.loading = false;
          })
      },
      deleteItem(item) {

      },
    }
  }
</script>
