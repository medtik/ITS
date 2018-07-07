<template>
  <v-dialog
    persistent
    v-model="dialog">
    <v-card>
      <!--<v-card-title class="headline green white&#45;&#45;text">-->
        <!--<span>Thêm tag</span>-->
      <!--</v-card-title>-->
      <!--<v-layout column headline>-->
        <!--<v-flex px-3 mt-3>-->
          <!--<v-text-field label="Tên tag" v-model="nameInput"></v-text-field>-->
          <!--<v-text-field label="Thể loại tag" v-model="categoryInput"></v-text-field>-->
        <!--</v-flex>-->
        <!--<v-flex px-2>-->
          <!--<v-btn color="primary"-->
                 <!--:loading="loading.createBtn">-->
            <!--Thêm mới-->
          <!--</v-btn>-->
        <!--</v-flex>-->
      <!--</v-layout>-->
      <!--<v-dialog v-model="editDialog" max-width="500px">-->
        <!--<v-card>-->
          <!--<v-card-title>-->
            <!--<span class="headline">Chỉnh sửa</span>-->
          <!--</v-card-title>-->

          <!--<v-card-text>-->
            <!--<v-container grid-list-md>-->
              <!--<v-layout wrap>-->
                <!--<v-flex xs12 sm6 md4>-->
                  <!--<v-text-field v-model="editedItem.name" label="Tên"></v-text-field>-->
                <!--</v-flex>-->
                <!--<v-flex xs12 sm6 md4>-->
                  <!--<v-text-field v-model="editedItem.category" label="Chỉnh sửa"></v-text-field>-->
                <!--</v-flex>-->
              <!--</v-layout>-->
            <!--</v-container>-->
          <!--</v-card-text>-->

          <!--<v-card-actions>-->
            <!--<v-spacer></v-spacer>-->
            <!--<v-btn color="blue darken-1" flat @click.native="onSaveEditDialog">Lưu</v-btn>-->
            <!--<v-btn color="blue darken-1" flat @click.native="editDialog = false">Hủy</v-btn>-->
          <!--</v-card-actions>-->
        <!--</v-card>-->
      <!--</v-dialog>-->
      <v-layout mt-5 wrap>
        <v-flex>
          <v-card-title class="headline green white--text">
            <span>Thêm mới</span>
          </v-card-title>
        </v-flex>
        <v-flex xs12 pa-3>
          <v-card-title>
            <v-spacer></v-spacer>
            <v-text-field
              v-model="searchInput"
              append-icon="search"
              label="Tìm"
              single-line
              hide-details
            ></v-text-field>
          </v-card-title>
          <v-data-table
            v-model="selected"
            :items="items"
            :pagination.sync="pagination"
            :headers="headers"
            :search="searchInput"
            item-key="id"
            :loading="loading.table">
            <template slot="items" slot-scope="props">
              <tr :active="props.selected" @click="props.selected = !props.selected">
                <td>
                  <v-checkbox
                    :input-value="props.selected"
                    primary
                    hide-details
                  ></v-checkbox>
                </td>
                <td>{{props.item.name}}</td>
                <td>{{props.item.category}}</td>
                <td class="justify-end layout px-0">
                  <v-icon
                    small
                    class="mr-2"
                    color="success"
                    @click="editItem(props.item)"
                  >
                    edit
                  </v-icon>
                  <v-icon
                    small
                    color="red"
                    @click="deleteItem(props.item)"
                  >
                    delete
                  </v-icon>
                </td>
              </tr>
            </template>
          </v-data-table>
        </v-flex>
        <v-flex xs12 mb-3>
          <v-btn dark color="green lighten-1"
                 v-on:click="onSaveClick">
            Lưu thay đổi
          </v-btn>
          <v-btn dark color="secondary"
                 v-on:click="onCancelClick">
            Hủy
          </v-btn>
        </v-flex>
      </v-layout>
    </v-card>
  </v-dialog>
</template>
<!--TODO sort by selected-->
<script>
  import _ from 'lodash';

  export default {
    name: "AddTagDialog",
    props: [
      'dialog'
    ],
    data() {
      return {
        nameInput: '',
        categoryInput: '',
        searchInput: '',
        loading: {
          table: true,
          saveBtn: false,
          // createBtn: false
        },
        items: [],
        headers: [
          {text: 'Chọn', value: 'selected', sortable: false},
          {text: 'Tên', value: 'name'},
          {text: 'Thể loại', value: 'category'},
          // {text: 'Hành động', value: 'id', sortable: false}
        ],
        pagination: {
          sortBy: 'id'
        },
        selected: [],
        // editDialog: false,
        // editedItem: {}
      }
    },
    created() {
      this.loadData();
    },
    methods: {
      loadData() {
        this.loading.table = true;
        this.$store.dispatch('tag/getAll')
          .then(value => {
            this.items = value.tags;
            this.loading.table = false
          })
          .catch(reason => {
            //TODO Error handling
          })
      },
      // onSaveEditDialog() {
      //   const originItem = this.items.find(value => value.id == this.editedItem.id);
      //   originItem.name = this.editedItem.name;
      //   originItem.category = this.editedItem.category;
      //   this.editDialog = false;
      // },
      // editItem(item) {
      //   this.editedItem = _.extend({}, item);
      //   this.editDialog = true;
      // },
      // deleteItem(item) {
      //
      // },
      onSaveClick() {

      },
      onCancelClick() {
        this.$emit('close');
      }
    }
  }
</script>

<style scoped>

</style>
