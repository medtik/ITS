import axiosInstance from "../../common/util/axiosInstance";
import _ from "lodash";


export default {
  namespaced: true,
  state: {
    selectedAreaID: undefined,
    questions: undefined,
    searchResult: undefined,
    loading: {
      questions: false,
      searchResult: false,
    }
  },
  getters: {
    questions(state) {
      return state.questions;
    },
    searchResult(state) {
      return state.searchResult;
    },
    questionsLoading(state) {
      return state.loading.questions;
    },
    searchResultLoading(state) {
      return state.loading.searchResult;
    }
  },
  mutations: {
    setQuestions(state, payload) {
      state.questions = payload.questions;
    },
    setSearchResult(state, payload) {
      state.searchResult = payload.result;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    getQuestionsByArea(context, payload) {
      const {
        areaId
      } = _.cloneDeep(payload);
      context.commit('previousSearchAreaId', {areaId}, {root: true});
      context.commit('setLoading', {loading: {questions: true}});

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Question/QuestionsByArea', {
          params: {
            areaId
          }
        }).then(value => {
          context.commit('setQuestions', {questions: value.data});
          context.commit('setLoading', {loading: {questions: false}});
          resolve(value.data);
        }).catch(reason => {
          context.commit('setLoading', {loading: {questions: false}});
          reject(reason.response);
        });
      });

    },
    nullQuestions(context, payload) {
      context.commit('setQuestions', {questions: null});
      context.commit('setLoading', {loading: true});
    },
    getSuggestion(context, payload) {
      const {
        answers
      } = _.cloneDeep(payload);

      context.commit('setLoading', {loading: {searchResult: true}});

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/test', {
          params: {
            list: answers
          }
        })
          .then(value => {
            context.commit('setSearchResult', {
              result: value.data
            });
            context.commit('setLoading', {loading: {searchResult: false}});
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {loading: {searchResult: false}});
            reject(reason.response);
          })
      });

    }
  }
}
