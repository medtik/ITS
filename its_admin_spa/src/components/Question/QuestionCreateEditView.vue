<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class="title">Quản lí câu hỏi</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear v-if="loading.page" color="primary" indeterminate></v-progress-linear>
        <v-layout column v-else>
          <!--Basic input-->
          <v-flex style="width: 25rem">
            <v-text-field label="Nội dung câu hỏi" v-model="textInput"></v-text-field>
            <v-combobox
              v-model="categoryInput"
              :items="categories"
              label="Thể loại"
              :loading="loading.categories"
            ></v-combobox>
          </v-flex>
          <!--Answer-->
          <v-flex my-3>
            <v-layout row>
              <v-flex xs12>
                <v-card elevation-5>
                  <v-toolbar flat dark color="blue darken-1">
                    <v-toolbar-title>Câu trả lời</v-toolbar-title>
                  </v-toolbar>
                  <v-layout column>
                    <v-flex px-2>
                      <v-layout row style="align-items: center">
                        <v-text-field
                          label="Câu trả lời"
                          v-model="answerTextInput"
                          single-line
                        ></v-text-field>
                        <v-btn icon flat color="green"
                               v-on:click="onAddAnswerClick">
                          <v-icon>fas fa-plus</v-icon>
                        </v-btn>
                      </v-layout>
                    </v-flex>
                    <v-divider></v-divider>
                    <v-flex px-2 v-for="(answer,index) in answersInput" :key="index">
                      <v-layout row py-2>
                        <v-flex xs11>
                          <v-layout column>
                            <v-flex ml-2>
                              <span class="subheading">{{answer.text}}</span>
                            </v-flex>
                            <v-flex mt-2>
                              <TagsInput
                                v-model="answer.tags"
                              />
                            </v-flex>
                          </v-layout>
                        </v-flex>
                        <v-flex xs1 style="text-align: end;">
                          <v-btn icon flat color="success"
                                 v-on:click="editAnswerDialog = true">
                            <v-icon>edit</v-icon>
                          </v-btn>
                          <v-btn icon flat color="red">
                            <v-icon>delete</v-icon>
                          </v-btn>
                        </v-flex>
                      </v-layout>
                      <v-divider v-if="(index + 1) < question.answers.length"></v-divider>
                    </v-flex>
                  </v-layout>
                </v-card>

              </v-flex>
            </v-layout>
          </v-flex>
          <!--Actions-->
          <v-flex mt-2>
            <v-btn color="primary"
                   v-if="mode == 'create'"
                   :loading="loading.createBtn"
                   @click="onCreateClick">
              Tạo mới
            </v-btn>
            <v-btn color="success"
                   v-if="mode == 'edit'"
                   :loading="loading.updateBtn"
                   @click="onUpdateClick">
              Lưu thay đổi
            </v-btn>
            <v-btn color="secondary"
                   @click="onExitClick">
              Thoát
            </v-btn>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
    <v-dialog v-model="editAnswerDialog" max-width="400">
      <v-card>
        <v-card-title class="green">
          <span class="title white--text">Chỉnh sửa câu trả lời</span>
        </v-card-title>
        <v-layout column pa-4>
          <v-flex>
            <v-text-field label="Tên" v-model="editedAnswer.name"></v-text-field>
          </v-flex>
          <v-flex>
            <v-btn color="green" dark v-on:click="onSaveEditAnswer">Lưu thay đổi</v-btn>
            <v-btn color="secondary" dark v-on:click="editAnswerDialog = false">Hủy</v-btn>
          </v-flex>
        </v-layout>
      </v-card>
    </v-dialog>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>

<script>
  import {
    SuccessDialog,
    ErrorDialog
  } from "../../common/block";

  import {TagsInput} from "../../common/input";


  export default {
    name: "QuestionCreateEditView",
    components: {
      ErrorDialog,
      SuccessDialog,
      TagsInput
    },
    data() {
      return {
        loading: {
          page: true,
          categories: true,
          createBtn: false,
          updateBtn: false
        },
        mode: 'create',
        questionId: undefined,
        question: undefined,
        textInput: undefined,
        categoryInput: undefined,
        answerTextInput: undefined,
        answersInput: [],
        categories: [],
        //boiler plane
        error: {
          dialog: false,
          title: '',
          message: ''
        },
        success: {
          dialog: false,
          title: '',
          message: ''
        },
        editAnswerDialog: false,
        editedAnswer: {},
      }
    },
    computed: {},
    created() {
      if (this.$route.name === 'QuestionEdit') {
        if (this.$route.query) {
          this.questionId = this.$route.query.id;
          this.mode = 'edit';
          this.$store.dispatch('question/getById', {
            id: this.questionId
          })
            .then(value => {
              this.question = value;
              this.fillInputs()
                .then(() => {
                  this.loading.page = false;
                })
            })
            .catch(reason => {
              this.error = {
                dialog: true,
                message: reason.message
              }
            })
        } else {
          this.error = {
            dialog: true,
            message: 'Đường dẫn không hợp lệ'
          }
        }
      } else {
        this.question = {
          answers: []
        };
        this.loading.page = false;
      }
      this.updateCategories();
    },
    methods: {
      updateCategories() {
        this.loading.categories = true;
        this.$store.dispatch('question/getCategories', {
          search: this.categoryInput
        })
          .then(value => {
            this.categories = value.categories;
            this.loading.categories = false;
          })
      },
      fillInputs() {
        this.textInput = this.question.text;
        this.categoryInput = this.question.category;
        this.answersInput = this.question.answers;
        return Promise.resolve();
      },
      onAddAnswerClick() {
        this.answersInput.push({
          text: this.answerTextInput
        })
      },
      onCreateClick() {
        this.loading.createBtn = true;
        this.$store.dispatch('question/create', {
          text: this.textInput,
          category: this.categoryInput,
          answers: this.answersInput
        }).then(question => {
          this.success = {
            dialog: true,
            message: `Tạo mới thành công câu hỏi`
          };
          this.loading.createBtn = false;
        }).catch(reason => {
          this.error = {
            dialog: true,
            message: reason.message
          };
          this.loading.createBtn = false;
        })
      },
      onUpdateClick() {
        this.loading.updateBtn = true;
        this.$store.dispatch('question/update', {
          id: this.questionId,
          text: this.textInput,
          category: this.categoryInput
        }).then(question => {
          this.success = {
            dialog: true,
            message: `Cập nhật thành công câu hỏi ${question.text}`
          };
          this.loading.updateBtn = false;
        }).catch(reason => {
          this.error = {
            dialog: true,
            message: reason.message
          };
          this.loading.updateBtn = false;
        })
      },
      onSaveEditAnswer() {
        this.editAnswerDialog = false;
      },
      onExitClick() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
