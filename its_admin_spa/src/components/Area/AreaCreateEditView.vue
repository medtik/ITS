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
                          :loading="allQuestionsLoading"
                          item-text="content"
                          item-value="id"
                          v-model="choosenQuestionId"
                          :items="allQuestions"/>
              </v-flex>
              <v-flex d-flex style="flex-grow: 0;align-items: center">
                <v-btn flat color="green" @click="addQuestion">
                  <v-icon color="green">fas fa-plus</v-icon>
                </v-btn>
              </v-flex>
            </v-layout>
            <v-list>
              <v-list-tile v-for="(question, index) in this.choosenQuestions" :key="question">
                <v-list-tile-title>{{question.content}}</v-list-tile-title>
                <v-list-tile-action>
                  <v-btn color="red" flat @click="removeQuestion(question.id)">
                    <v-icon>
                      fas fa-trash
                    </v-icon>
                  </v-btn>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
          </v-flex>
          <v-flex>
            <v-btn v-if="mode == 'create'" color="primary" @click="onCreateBtnClick">
              Tạo
            </v-btn>
            <v-btn v-if="mode == 'edit'" color="success" @click="onEditBtnClick">
              Xác nhận
            </v-btn>
            <v-btn  color="secondary" @click="onCancel">
              Hủy
            </v-btn>
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
  import {mapState} from "vuex"
  import _ from "lodash"
  export default {
    name: "AreaCreateEditView",
    components: {ErrorDialog, SuccessDialog},
    data() {
      return {
        mode: 'create',
        area: undefined,
        nameInput: undefined,
        choosenQuestions: [],
        choosenQuestionId: undefined,
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
    computed:{
      ...mapState('question',{
        allQuestionsLoading: state => state.loading.allQuestions,
        allQuestions: state => state.allQuestions
      })
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
    mounted(){
      this.$store.dispatch('question/GetAllWithoutParams')
    },
    methods: {
      setInput(area) {
        this.nameInput = area.name;
        this.choosenQuestions = area.questions;
      },
      addQuestion(){
        const q = _.find(this.allQuestions, (q) => {return q.id == this.choosenQuestionId});
        const isDupped = _.some(this.choosenQuestions, q => {return q.id == this.choosenQuestionId});
        if(!!q && !isDupped){
          this.choosenQuestions.push(q);
        }
      },
      removeQuestion(questionId){
        this.choosenQuestions = _.filter(this.choosenQuestions, q =>{
          return q.id != questionId
        })
      },
      onCreateBtnClick(){
        this.$store.dispatch('area/create', {
          name: this.nameInput,
          questions: this.choosenQuestions
        })
      },
      onEditBtnClick(){
        this.$store.dispatch('area/update',{
          name: this.nameInput,
          questions: this.choosenQuestions
        })
      },
      onCancel(){
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
