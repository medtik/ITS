<template>
  <v-content id="content-holder">
    <v-layout column justify-center align-center id="layout">
      <v-flex style="height: 5vh">
        <!--Holder-->
      </v-flex>
      <v-card id="form">
        <v-card-title class="light-blue white--text">
          <v-layout column>
            <div class="display-2 font-weight-black font-italic text-xs-center">
              ITS
            </div>
            <v-divider class="my-2"/>
            <div class="title font-weight-medium text-xs-center">
              Tìm kiếm thông minh
            </div>
          </v-layout>
        </v-card-title>
        <v-card-text id="card-content">
          <v-select
            label="Chọn khu vực"
            @change="onAreaSelect"
            v-model="selectedArea"
            :items="areas"
            item-text="name"
            item-value="id"
            :loading="loading.areaSelect || areasLoading"
          />
          <v-progress-linear
            v-if="loading.questions"
            indeterminate/>
          <v-layout column
                    v-if="questions">
            <v-flex
              v-for="question in questions"
              :key="question.id"
            >
              <v-layout column>
                <v-flex class="title font-weight-bold" mb-2>{{question.text}}</v-flex>
                <v-flex v-for="answer in question.answers"
                        :key="answer.id"
                        mx-3>
                  <v-checkbox
                    class="my-0"
                    v-model="selectedAnswers"
                    :label="answer.text"
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
              <v-divider/>
            </v-flex>
            <v-layout justify-center>
              <v-flex style="flex-grow: 0">
                <v-btn color="success"
                       @click="onSubmit"
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
  import ParallaxHeader from "../shared/ParallaxHeader";
  import {mapState} from "vuex"
  export default {
    name: "SmartSearchView",
    components: {
      ParallaxHeader
    },
    data() {
      return {
        loading: {
          finishBtn: false,
          areaSelect: false,
          questions: false
        },
        questions: [],
        selectedArea: undefined,
        selectedAnswers: [],
        error: {}
      }
    },
    computed:{
      ...mapState('area',{
        areas: state => state.areas,
        areasLoading: state=>state.areasLoading
      })
    },
    mounted() {
      if(!this.areas || this.areas.length <= 0){
        this.$store.dispatch('area/getAll')
          .catch(reason =>{
            console.debug('mounted', reason)
          })
      }
      // this.$store.dispatch('search/fetchAreas')
      //   .then((value) => {
      //     const {
      //       areas
      //     } = value;
      //     this.areas = areas;
      //     this.loading.areaSelect = false;
      //   })
      //   .catch((reason) => {
      //     this.error = {
      //       ...reason
      //     };
      //     this.loading.areaSelect = false;
      //   })
    },
    methods: {
      onAreaSelect() {
        this.loading.questions = true;
        this.$store.dispatch('search/fetchQuestions', {
          areaId: this.selectedArea
        })
          .then(value => {
            const {
              questions
            } = value;
            this.questions = questions;
            this.loading.questions = false;
          })
          .catch(reason => {
            this.error = {
              ...reason
            };
            this.loading.questions = false;
          })

      },
      onSubmit() {
        this.loading.finishBtn = true;
        this.$store.dispatch('search/fetchSuggestion', {
          answers: this.selectedAnswers
        })
          .then(value => {
            this.$router.push({
              name: 'SmartSearchResult',
              params: value
            });
            this.loading.finishBtn = false;
          })
          .catch(reason => {
            this.error = {
              ...reason
            };
            this.loading.finishBtn = false;
          })
      }
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
