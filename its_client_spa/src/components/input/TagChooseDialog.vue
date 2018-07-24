<template>
  <v-dialog
    persistent
    v-model="dialog">
    <v-card>
      <v-layout wrap>
        <v-flex>
          <v-card-title class="headline green white--text">
            <span>Chọn thẻ</span>
          </v-card-title>
        </v-flex>
        <v-flex xs12 pa-3>
          <v-card-title>
            <v-spacer></v-spacer>
            <v-text-field
              v-model="searchInput"
              v-on:keyup.enter="onSearchEnter"
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
            :total="total"
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
                <td>{{props.item.categories}}</td>
              </tr>
            </template>
          </v-data-table>
        </v-flex>
        <v-flex xs12 mb-3>
          <v-btn dark color="green lighten-1"
                 v-on:click="onSaveClick">
            Thêm
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
  export default {
    name: "AddTagDialog",
    props: [
      'dialog',
      'value'
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
        total: undefined,
        headers: [
          {text: 'Chọn', value: 'selected', sortable: false},
          {text: 'Tên', value: 'name'},
          {text: 'Thể loại', value: 'categories'},
          // {text: 'Hành động', value: 'id', sortable: false}
        ],
        pagination: {
          sortBy: 'id'
        },
        selected: [],
      }
    },
    created() {
      this.loadData();
    },
    watch: {
      value(val,oldVal) {
        this.selected = val;
      }
    },
    methods: {
      loadData() {
        this.loading.table = true;
        this.$store.dispatch('tag/getAll', {
          pagination: this.pagination,
          search: this.searchInput
        })
          .then(value => {
            this.items = value.list;
            this.total = value.total;
            this.loading.table = false
          })
          .catch(reason => {
            //TODO Error handling
          })
      },
      onSearchEnter() {
        this.pagination.page = 1;
        this.loadData()
      },
      onSaveClick() {
        this.$emit('input', this.selected);
        this.$emit('save', this.selected);
      },
      onCancelClick() {
        this.$emit('close');
      }
    }
  }
</script>

<style scoped>

</style>
