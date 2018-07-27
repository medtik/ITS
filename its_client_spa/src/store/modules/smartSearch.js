import axiosInstance from "../../common/util/axiosInstance";


export default {
  namespaced: true,
  state: {
    selectedAreaID: undefined,
    loading: undefined,
    questions: undefined,
  },
  mutations: {
    setQuestions(state, payload) {
      state.questions = payload.questions;
    },
    setLoading(state, payload) {
      state.loading = payload.loading;
    }
  },
  actions: {
    getQuestionsByArea(context, payload) {
      const {
        areaId
      } = payload;

      context.commit('setLoading', {loading: true});
      axiosInstance.get('api/Question/QuestionsByArea', {
        params: {
          areaId
        }
      }).then(value => {
        context.commit('setQuestions', {questions: value.data});
        context.commit('setLoading', {loading: false});
      });
    },
    nullQuestions(context,payload){
      context.commit('setQuestions', {questions: null});
      context.commit('setLoading', {loading: true});
    },
    getSuggestion(context, payload) {
      const {
        answers
      } = payload;
      return new Promise(resolve => {
        setTimeout(()=>{resolve()},1500)
      })
    }
  }
}
