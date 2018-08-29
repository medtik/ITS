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
            <v-text-field label="Nội dung câu hỏi"
                          v-model="textInput"
                          :error='!!formError["data.Content"]' :error-messages="formError['data.Content']"
            ></v-text-field>
            <v-combobox
              v-model="categoryInput"
              :items="categories"
              label="Thể loại"
              :loading="loading.categories"
              :error='!!formError["data.Categories"]' :error-messages="formError['data.Categories']"
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
                    <v-flex pa-2>
                      <v-layout row style="align-items: center">
                        <v-text-field
                          label="Câu trả lời"
                          v-model="answerTextInput"
                        ></v-text-field>
                        <v-btn icon flat color="green"
                               v-on:click="onAddAnswerClick">
                          <v-icon>fas fa-plus</v-icon>
                        </v-btn>
                      </v-layout>
                    </v-flex>
                    <v-divider></v-divider>
                    <v-flex v-if="answersInput && answersInput.length > 0"
                            v-for="(answer, index) in answersInput" :key="index"
                            px-2>
                      <v-layout row py-2>
                        <v-flex xs11>
                          <v-layout column>
                            <v-flex ml-2>
                              <span class="subheading">{{answer.content}}</span>
                            </v-flex>
                            <v-flex mt-2>
                              <TagsInput
                                v-model="answer.tags"
                                :admin="true"
                                @create="createEditDialog.dialog = true"
                              />
                            </v-flex>
                          </v-layout>
                        </v-flex>
                        <v-flex xs1 style="text-align: end;">
                          <v-btn icon flat color="success"
                                 @click="onEditAnswer(index)">
                            <v-icon>edit</v-icon>
                          </v-btn>
                          <v-btn icon flat color="red" @click="onRemoveAnswer(index)">
                            <v-icon>delete</v-icon>
                          </v-btn>
                        </v-flex>
                      </v-layout>
                      <v-divider v-if="(index + 1) < answersInput.length"></v-divider>
                    </v-flex>
                    <v-alert
                      :value="!!formError['data.Answers']"
                      type="error"
                    >
                      {{formError['data.Answers']}}
                    </v-alert>
                  </v-layout>
                </v-card>
              </v-flex>
            </v-layout>
          </v-flex>

          <!--Actions-->
          <v-flex mt-2>
            <v-btn color="success"
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
            <v-text-field label="Nội dung" v-model="editedAnswer.content"></v-text-field>
          </v-flex>
          <v-flex>
            <v-btn color="green" dark v-on:click="onSaveEditAnswer">Lưu thay đổi</v-btn>
            <v-btn color="secondary" dark v-on:click="editAnswerDialog = false">Hủy</v-btn>
          </v-flex>
        </v-layout>
      </v-card>
    </v-dialog>
    <TagCreateEditDialog v-bind="createEditDialog"
                         v-on:close="createEditDialog.dialog = false"
                         v-on:create="onDialogConfirmCreate"/>
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
  import {FormRuleMixin} from "../../common/mixin";
  import TagCreateEditDialog from "../Tag/TagCreateEditDialog" ;
  import _ from "lodash";

  export default {
    name: "QuestionCreateEditView",
    mixins: [FormRuleMixin],
    components: {
      ErrorDialog,
      SuccessDialog,
      TagsInput,
      TagCreateEditDialog
    },
    data() {
      return {
        refs: {
          question: 'question',
          answerText: 'answerText',
        },
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
        tagInputErrorMessage: "",
        createEditDialog: {
          dialog: false,
        },
        error: {
          dialog: false,
        },
        success: {
          dialog: false,
        },
        editAnswerDialog: false,
        editAnswerIndex: undefined,
        editedAnswer: {},
        formError: {
          ['data.Content']: undefined,
          ['data.Categories']: undefined,
          ['data.Answers']: undefined,
        }
      }
    },
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
              };

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
        this.textInput = this.question.content;
        this.categoryInput = this.question.category;
        this.answersInput = this.question.answer;
        return Promise.resolve();
      },
      onAddAnswerClick() {
        if (this.answerTextInput) {
          this.answersInput.push({
            content: this.answerTextInput
          });
          this.answerTextInput = '';
          // this.content[this.refs.answerText].reset()
        }
      },
      onCreateClick() {
        this.loading.createBtn = true;
        this.$store.dispatch('question/create', {
          text: this.textInput,
          category: this.categoryInput,
          answers: this.answersInput
        }).then(() => {
          this.success = {
            dialog: true,
            message: `Tạo mới thành công câu hỏi`
          };
          this.loading.createBtn = false;
        }).catch(reason => {
          this.loading.createBtn = false;
          this.formError = reason.response.data.modelState
        })
      },
      onUpdateClick() {
        this.loading.updateBtn = true;
        this.$store.dispatch('question/update', {
          id: this.questionId,
          content: this.textInput,
          categories: this.categoryInput,
          answers: this.answersInput
        }).then(() => {
          this.$router.push({
            name: 'QuestionList'
          });
        })
          .catch(reason =>{
            this.formError = reason.response.data.modelState
          })
      },
      onDialogConfirmCreate(item) {
        this.$store.dispatch('tag/create', {tag: item})
          .then(() => {
            this.$store.dispatch('tagDialog/getAll');
          })
          .catch(reason => {
            console.debug('onDialogConfirmCreate-catch', reason);
            this.error = {
              dialog: true,
              message: 'Có lỗi xảy ra'
            }
          });
        this.createEditDialog = {
          dialog: false
        }
      },
      onRemoveAnswer(removeIndex) {
        this.answersInput = _.filter(this.answersInput, (val, index) => {
          return index != removeIndex;
        })
      },
      onEditAnswer(editIndex) {
        this.editedAnswer = _(this.answersInput)
          .filter((val, index) => {
            return index == editIndex;
          })
          .head();
        this.editAnswerDialog = true;
        this.editAnswerIndex = editIndex;
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
