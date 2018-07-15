<template>
  <v-content id="content-holder">
    <v-layout justify-center align-center id="layout" >
      <v-dialog value="true" max-width="650" hide-overlay persistent>
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
            <v-layout column>
              <v-flex
                v-for="question in questions"
                :key="question.id"
              >
                <Question v-bind="question"/>
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
      </v-dialog>
    </v-layout>
  </v-content>
</template>


<script>
  import ParallaxHeader from "../shared/ParallaxHeader";
  import Question from "./Question";

  import _questions from "./Questions";

  export default {
    name: "SmartSearchView",
    components: {
      ParallaxHeader,
      Question
    },
    data() {
      return {
        loading: {
          finishBtn: false
        },
        questions: _questions
      }
    },
    methods: {
      submit() {

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

  #dialog{
    max-width: 550px;
  }

  /*#layout {*/
    /*height: 100vh;*/
  /*}*/

  /*#form {*/
    /*width: 100%;*/
    /*max-width: 500px;*/
  /*}*/

  /*#card-content {*/
    /*max-height: 60%;*/
    /*overflow-y: auto;*/
  /*}*/
</style>
