<template>
  <v-dialog
    persistent
    v-model="dialog">
    <v-card>
      <v-layout wrap>
        <v-flex>
          <v-card-title class="headline green white--text">
            <span>Chọn thẻ</span>
            <v-spacer></v-spacer>
            <v-btn v-if="admin" color="primary" @click="$emit('create')">
              Tạo mới
            </v-btn>
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
            :headers="headers"
            :search="searchInput"
            item-key="id"
            :loading="loadingTable">
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
          <!--<v-btn dark color="green lighten-1"-->
                 <!--v-on:click="onSaveClick">-->
            <!--Thêm-->
          <!--</v-btn>-->
          <v-btn dark color="secondary"
                 v-on:click="onCancelClick">
            Hủy
          </v-btn>
        </v-flex>
      </v-layout>
    </v-card>
  </v-dialog>
</template>
<script>
  import {mapState} from "vuex";

  export default {
    name: "AddTagDialog",
    props: [
      'dialog',
      'value',
      'admin'
    ],
    data() {
      return {
        nameInput: '',
        categoryInput: '',
        searchInput: '',
        loading: {
          saveBtn: false,
        },
        headers: [
          {text: 'Chọn', value: 'selected', sortable: false},
          {text: 'Tên', value: 'name'},
          {text: 'Thể loại', value: 'categories'},
        ],
        pagination: {
          sortBy: 'id'
        },
        selected: [],
      }
    },
    computed: {
      ...mapState('tagDialog', {
        items: state => state.items,
        loadingTable: state => state.loading
      })
    },
    watch: {
      value(val) {
        this.selected = val;
      },
      dialog(val) {
        if (val && !this.items) {
          this.loadData();
        }
      }
    },
    methods: {
      loadData() {
        this.loading.table = true;
        this.$store.dispatch('tagDialog/getAll')
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
