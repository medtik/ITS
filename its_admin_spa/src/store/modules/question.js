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
          message: 'Có lỗi xẩy ra !'
        })
      }

    }, 1500 + (Math.random() * 1000))
  })
}

export default {
  namespaced: true,
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
        category,
        answers
      } = _.cloneDeep(payload);

      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Question', {
          content: text,
          categories: category,
          answers: _.map(answers, answer => {
            answer.tags = _.map(answer.tags, (tag) => {
              return tag.id
            });
            answer.answer = answer.text;
            answer.text = undefined;
            return answer;
          })
        })
          .then(value => {
            resolve()
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
    update(context, payload) {
      // put /api/Question
      const {
        id,
        content,
        categories,
        answers
      } = payload;
      axiosInstance.put('api/question', {}, {
        params: {
          id,
          content,
          categories,
          answers
        }
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
