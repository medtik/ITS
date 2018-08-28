<template>
  <v-content id="content-holder">
    <v-layout column justify-center align-center id="layout">
      <v-flex style="height: 5vh">
        <!--Holder-->
      </v-flex>
      <v-card id="form">
        <AppCardTitle/>
        <v-card-text id="card-content">
          <!--AREA-->
          <AreaSelect
            v-if="!lockAreaSelect"
            v-model="selectedAreaId"
            @change="onAreaSelect"
            alias="area"
            itemsPath="areas"
            loadingPath="areasLoading"
            getItemPath="getAll"
          />
          <v-layout v-if="lockAreaSelect"
                    column align-center mb-5>
            <span class="caption">
              Tại
            </span>
            <span class="headline font-weight-bold">
              {{context.plan.areaName}}
            </span>
          </v-layout>

          <v-progress-linear
            v-if="questionsLoading"
            indeterminate>
          </v-progress-linear>
          <v-layout column
                    v-if="questions">
            <v-flex
              v-for="question in questions"
              :key="question.id"
            >
              <v-layout column>
                <v-flex class="title font-weight-bold" mb-2>{{question.content}}</v-flex>
                <v-flex v-for="answer in question.answer"
                        :key="answer.id"
                        mx-3>
                  <v-checkbox
                    class="my-0"
                    v-model="selectedAnswers"
                    :label="answer.content"
                    :value="answer.id"
                  />
                </v-flex>
              </v-layout>
            </v-flex>
          </v-layout>
        </v-card-text>
        <v-card-actions>
          <v-layout column>
            <v-flex my-2>
              <v-divider></v-divider>
            </v-flex>
            <v-layout align-center column>
              <v-flex py-2>
              </v-flex>
              <v-flex>
                <v-btn color="success"
                       @click="onSubmit"
                       :disabled="!questions || !isHaveAnswer || loading.createSuggestedPlan"
                       :loading="loading.finishBtn">
                  <v-icon>check</v-icon>
                  &nbsp;&nbsp;
                  Hoàn thành
                </v-btn>
              </v-flex>
            </v-layout>
          </v-layout>
        </v-card-actions>
      </v-card>
      <v-flex style="height: 15vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>


<script>
  import {ParallaxHeader} from "../../common/layout";
  import {AreaInput} from "../../common/input/";
  import CreatePlanDialog from "../plan/CreatePlanDialog"
  import {mapGetters} from "vuex"
  import {AppCardTitle} from "../../common/block";


  export default {
    name: "SmartSearchView",
    components: {
      ParallaxHeader,
      AreaSelect: AreaInput,
      CreatePlanDialog,
      AppCardTitle
    },
    data() {
      return {
        loading: {
          finishBtn: false,
          areaSelect: false,
          createSuggestedPlan: false,
        },
        lockAreaSelect: false,
        selectedAreaId: undefined,
        selectedAnswers: [],
        error: {},
        //
        createPlanDialog: {
          dialog: false,
          areaId: undefined,
        }
      }
    },
    computed: {
      ...mapGetters('smartSearch', {
        questions: 'questions',
        questionsLoading: 'questionsLoading'
      }),
      ...mapGetters('authenticate',{
        isLoggedIn: 'isLoggedIn'
      }),
      ...mapGetters({
        context: 'searchContext'
      }),
      isHaveAnswer() {
        return this.selectedAnswers && this.selectedAnswers.length > 0;
      }
    },
    mounted() {
      if (this.context && this.context.areaId) {
        this.lockAreaSelect = true;
        this.onAreaSelect(this.context.areaId);
      }
    },
    beforeDestroy() {
      this.$store.dispatch('smartSearch/nullQuestions');
    },
    methods: {
      onAreaSelect(id) {
        let areaId = id;
        if (!areaId) {
          areaId = this.selectedAreaId
        }

        this.loading.questions = true;
        this.$store.dispatch('smartSearch/getQuestionsByArea', {
          areaId
        }).catch(reason => {
          this.error = {
            dialog: true,
            message: 'Có Lỗi xẩy ra',
          };
          console.error('onAreaSelect', reason);
        })
      },
      onSubmit() {
        this.loading.finishBtn = true;
        this.$store.commit('previousSearchAnswers', {answers: this.selectedAnswers});
        this.$store.commit('previousSearchAreaId', {areaId: this.selectedAreaId});
        this.$store.dispatch('smartSearch/getSuggestion', {
          answers: this.selectedAnswers,
          areaId: this.selectedAreaId || this.context.areaId
        })
          .then(value => {
            this.$router.push({
              name: 'SmartSearchResult',
              params: {
                ...value,
              }
            });
            this.loading.finishBtn = false;
          })
          .catch(reason => {
            this.error = {
              ...reason
            };
            this.loading.finishBtn = false;
          })
      },
      onCreateSuggestedPlanClick() {
        this.createPlanDialog = {
          dialog: true,
        }
      },
      onCreateSuggestedPlanConfirm(inputs) {
        const {
          name,
          startDate,
          endDate
        } = inputs;

        this.createPlanDialog = {
          dialog: false
        };
        this.loading.createSuggestedPlan = true;
        this.$store.dispatch("plan/createSuggestedPlan", {
          name,
          startDate,
          endDate,
          areaId: this.selectedAreaId,
          answers: this.selectedAnswers
        }).then(value => {
          this.$router.push({
            name: 'PlanDetail',
            query: value
          })
        })
      },
      onSigninClick() {
        this.$store.commit('signinContext', {
          context: {
            returnRoute: {
              name: this.$route.name
            }
          }
        });

        this.$router.push({
          name: 'Signin'
        })
      },
    }
  }
</script>
<style scoped>
  #content-holder {
    background-image: url("../../../static/pexels-photo-273886.jpeg");
    background-size: cover;
    background-position: center center;
  }
</style>
