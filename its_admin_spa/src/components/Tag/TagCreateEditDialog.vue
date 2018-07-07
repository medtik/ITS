<template>
  <v-dialog v-model="dialog" max-width="400" persistent>
    <v-card>
      <v-card-title :class="titleClass">
        <span class="title white--text">{{title}}</span>
      </v-card-title>
      <v-layout column py-2>
        <v-flex px-3>
          <v-text-field
            label="Tên"
            v-model="nameInput"
          />
        </v-flex>
        <v-flex px-3>
          <v-text-field
            label="Thể loại"
            v-model="categoryInput"
          />
        </v-flex>
        <v-flex mt-2>
          <v-btn dark color="primary"
                 :loading="loading.createBtn"
                 v-if="mode == 'create'"
                 v-on:click="onCreateClick">
            Tạo mới
          </v-btn>
          <v-btn dark color="success"
                 :loading="loading.editBtn"
                 v-if="mode == 'edit'"
                 v-on:click="onEditClick">
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

<script>
  import _ from "lodash";

  export default {
    name: "TagCreateEditDialog",
    props: [
      'dialog',
      'item'
    ],
    data() {
      return {
        nameInput: '',
        categoryInput: '',
        loading: {
          createBtn: false,
          editBtn: false,
        }
      }
    },
    computed: {
      titleClass() {
        if (this.mode == 'edit') return "green lighten-1";
        else return "primary";
      },
      title() {
        if (this.mode == 'edit') return 'Chỉnh sửa thẻ';
        else return 'Tạo thẻ';
      },
      mode() {
        if (this.item) return 'edit';
        else return 'create';
      }
    },
    watch: {
      dialog(val, oldVal) {
        if (val && this.item) {
          this.nameInput = this.item.name;
          this.categoryInput = this.item.category;
        }
      }
    },
    methods: {
      onCreateClick() {
        this.loading.createBtn = true;
        setTimeout(() => {
          this.loading.createBtn = false;
          this.$emit('create', _.extend({}, {
            name: this.nameInput,
            category: this.categoryInput
          }));
          this.resetInput();
        }, 1200)

      },
      onEditClick() {
        this.loading.editBtn = true;
        setTimeout(() => {
          this.loading.editBtn = false;
          this.$emit('edit', _.extend({}, {
            id: this.item.id,
            name: this.nameInput,
            category: this.categoryInput
          }));
          this.resetInput();
        }, 1200)

      },
      onCancelClick() {
        this.$emit('close');
        this.resetInput();
      },
      resetInput() {
        this.nameInput = '';
        this.categoryInput = '';
      }
    }
  }
</script>

<style scoped>

</style>
