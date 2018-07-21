import axiosInstance from "../../axiosInstance";


export default {
  namespaced: true,
  state: {
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
    getSuggestion(context, payload) {
      const {
        answers
      } = payload;
      console.debug('getSuggestion', answers);

    }
  }
}
