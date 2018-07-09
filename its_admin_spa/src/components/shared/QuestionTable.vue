<template>
  <dialog :dialog="dialog">
    <v-data-table
      :items="items"
      :total-items="total"
      :pagination.sync="pagination"
      :headers="headers"
      :loading="loading">
      <template slot="items" slot-scope="props">
        <td>{{ props.item.text }}</td>
        <td>{{ props.item.category }}</td>
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
  </dialog>
</template>

<script>
  export default {
    name: "QuestionTable",
    props: [
      'searchText',
      'dialog'
    ],
    data() {
      return {
        //TABLE
        loading: true,
        items: [],
        headers: [
          {text: 'Nội dung', value: 'text'},
          {text: 'Thể loại', value: 'category'},
          {text: 'Số câu trả lời', value: 'answers.length'},
          {text: 'Hành động', value: 'id', sortable: false},
        ],
        pagination: {},
        total: undefined,
        //TABLE
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
    }
  }
</script>

<style scoped>

</style>
