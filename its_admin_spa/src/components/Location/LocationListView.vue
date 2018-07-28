<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class=title>Danh sách địa điểm</span>
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
          <v-btn color="primary"
                 :to="{name:'LocationCreate'}">
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
            <td>{{ props.item.address }}</td>
            <td>{{ props.item.website }}</td>
            <td>{{ props.item.phone }}</td>
            <td>{{ props.item.email }}</td>
            <td>
              <v-chip outline disabled label
                      v-if="props.item.isVerified"
                      color="green">
                Đã xác nhận
              </v-chip>
              <v-chip outline disabled label
                      v-else
                      color="red">
                Chưa xác nhận
              </v-chip>
            </td>
            <td>
              <v-chip outline disabled label
                      v-if="props.item.isClosed"
                      color="red">
                Ngừng kinh doanh
              </v-chip>
              <v-chip outline disabled label
                      v-else
                      color="green">
                Còn hoạt động
              </v-chip>
            </td>
            <td>{{ props.item.area }}</td>

            <td class="justify-center layout px-0">
              <router-link :to="{name:'LocationEdit', query:{id:props.item.id}}">
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
                  @click="onDeleteClick(props.item)">
                  delete
                </v-icon>
              </a>
            </td>
          </template>
        </v-data-table>
        <!--Content end-->
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  import {
    ErrorDialog,
    SuccessDialog
  } from "../../common/block";

  export default {
    name: "LocationListView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        //TABLE
        loading: true,
        items: [],
        headers: [
          {text: 'Tên', value: 'name'},
          {text: 'Địa chỉ', value: 'address'},
          {text: 'Web', value: 'website'},
          {text: 'Điện thoại', value: 'phone'},
          {text: 'Email', value: 'email'},
          {text: 'Xác thực', value: 'isVerified'},
          {text: 'Đóng cửa', value: 'isClosed'},
          {text: 'Khu vực', value: 'area'},
          {text: 'Hành động', value: 'id', sortable: false},
        ],
        pagination: {},
        total: undefined,
        searchInput: '',
        //TABLE
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
        //DIALOG END
      };
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
        this.$store.dispatch('location/getAll', {
          search: this.searchInput,
          pagination: this.pagination
        })
          .then(data => {
            this.items = data.locations;
            this.total = data.total;
            this.loading = false;
          })
          .catch(error => {
            this.error = {
              dialog: true,
              title: 'Chú ý',
              message: error.message
            };
            // console.error('loadData', error);
          })
      },
      onSearchEnter() {
        this.pagination.page = 1;
        this.loadData();
      },
      onDeleteClick(item) {
        this.$store.dispatch('location/delete', {
          id: item.id
        }).then(value => this.loadData());
      }
    }
  }
</script>

<style scoped>

</style>
