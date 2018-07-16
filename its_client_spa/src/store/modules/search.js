import _questions from "./Questions";

export default {
  namespaced: true,
  actions: {
    fetchAreas(context, payload) {
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          resolve({
            areas: ['Hà nội', 'TP.Hồ Chí Minh']
          });
        }, 2000)
      })
    },
    fetchQuestions(context, payload) {
      const {
        areaId
      } = payload;

      return new Promise((resolve, reject) => {
        setTimeout(() => {
          resolve({
            questions: _questions
          });
        }, 1500)
      })
    },
    fetchSuggestion(context, payload){
      const {
        answers,
      } = payload;

      return new Promise((resolve, reject) => {
        setTimeout(() => {
          resolve({
            suggestion: [] // <-- locations here
          });
        }, 2000)
      })
    }
  }
}
