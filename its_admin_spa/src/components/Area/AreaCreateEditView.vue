<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title" v-if="mode == 'create'">Tạo mới khu vực</span>
        <span class="title" v-if="mode == 'edit'">Chỉnh sửa khu vực</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear
          v-if="loading.page"
          indeterminate/>
        <v-layout column v-else>
          <v-flex>
            <v-text-field
              label="Tên"
              v-model="nameInput"/>
          </v-flex>
          <v-flex mt-4>
            <span class="subheading">Câu hỏi cho khu vực</span>
            <v-layout row style="justify-items: end">
              <v-flex>
                <v-select label="Tìm câu hỏi"
                          :items="allQuestions"/>
              </v-flex>
              <v-flex d-flex style="flex-grow: 0;align-items: center">
                <v-btn flat color="green">
                  <v-icon color="green">fas fa-plus</v-icon>
                </v-btn>
              </v-flex>
            </v-layout>
            <!--<v-flex>-->
              <!--<v-btn v-if="mode == 'create'" color="primary">-->
                <!--Tạo-->
              <!--</v-btn>-->

              <!--<v-btn v-if="mode == 'edit'" color="success">-->
                <!--Xác nhận-->
              <!--</v-btn>-->

              <!--<v-btn color="secondary">-->
                <!--Hủy-->
              <!--</v-btn>-->
            <!--</v-flex>-->
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>

<script>
  import {
    ErrorDialog,
    SuccessDialog
  } from "../../common/block";
  import mapState from "vuex"

  export default {
    name: "AreaCreateEditView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        mode: 'create',
        area: undefined,
        nameInput: undefined,
        choosenQuestions: [],
        loading: {
          page: true
        },
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
        //DIALOG END;
      }
    },
    created() {
      const {
        name,
        query
      } = this.$route;
      if (name == 'AreaEdit') {
        this.mode = 'edit';
        this.$store.dispatch('area/getById', {...query})
          .then(value => {
            this.area = value;
            this.setInput(value);
            this.loading.page = false;
          })
          .catch(reason => {
            this.error = {
              dialog: true,
              ...reason
            };
            this.loading.page = false;
          })
      } else {
        this.loading.page = false;
      }
    },
    methods: {
      setInput(area) {
        this.nameInput = area.name;
        this.choosenQuestions = area.questions;
      }
    }
  }
</script>

<style scoped>

</style>
