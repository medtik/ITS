import _question from "./mockdata/Questions";
import {axiosInstance} from "../../common/util";
import formatter from "../../formatter";
import Raven from "raven-js";


function mockShell(bodyFunc, noFail) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (noFail || Math.random() > 0.1) {
        //Success
        let result = bodyFunc();
        resolve(result);
      } else {
        //error
        reject({
          message: 'Có lỗi xảy ra !'
        })
      }

    }, 1500 + (Math.random() * 1000))
  })
}

export default {
  namespaced: true,
  state: {
    allQuestions: [],
    loading: {
      allQuestions: false
    }
  },
  mutations: {
    setAllQuestions(state, payload) {
      state.allQuestions = payload.questions;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    getAll(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Question', {
          params: formatter.getAllRequest(payload)
        })
          .then(value => {
            resolve(formatter.getAllResponse(value.data));
          })
          .catch(reason => {
            let error = [];

            reject({
              ...reason.response,
              error
            })
          });
      })
    },
    GetAllWithoutParams(context) {
      context.commit('setLoading', {loading: {allQuestions: true}});
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Question', {
          params: {
            pageSize: -1
          }
        })
          .then(value => {
            context.commit('setAllQuestions', {questions: value.data.currentList});
            context.commit('setLoading', {loading: {allQuestions: false}});
          })
          .catch(reason => {
            let error = [];

            reject({
              ...reason.response,
              error
            })
          });
      })
    },
    getById(context, payload) {
      // get /api/Question/Detail
      const {
        id
      } = payload;
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Question/Detail', {
          params: {
            id
          }
        })
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject();
          })
      })
    },
    getCategories(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Question/categories')
          .then(value => {
            resolve({
              categories: value.data
            })
          })
          .catch(reason => {
            let error = [];

            reject({
              ...reason.response,
              error
            })
          })
      });
    },
    create(context, payload) {
      const {
        text,
        categories,
        answers
      } = _.cloneDeep(payload);

      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Question', {
          content: text,
          categories: categories,
          answers: _.map(answers, answer => {
            answer.tags = _.map(answer.tags, 'id');
            answer.answer = answer.content;
            return answer;
          })
        })
          .then(value => {
            resolve()
          })
          .catch(reason => {
            reject(reason);
          })
      });


    },
    update(context, payload) {
      // put /api/Question
      const {
        id,
        content,
        categories,
        answers
      } = _.cloneDeep(payload);

      return new Promise((resolve, reject) => {
        axiosInstance.put('api/question', {
          "id": id,
          "content": content,
          "categories": categories,
          "answers": _.map(answers, answer => {
            answer.tags = _.map(answer.tags, 'id');
            answer.answer = answer.content;
            return answer;
          })
        })
          .then(value => {
            resolve(value.data)
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject(reason);
          })
      })
    },
    delete(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/Question', {
          params: {
            questionId: payload.id
          }
        })
          .then(value => {
            resolve();
          })
          .catch(reason => {
            let error = [];

            reject({
              ...reason.response,
              error
            })
          })
      })

    }

  }
}
