import _question from "./mockdata/Questions";
import axiosInstance from "../../axiosInstance"
import _ from 'lodash'

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
      const {
        pagination,
        search,
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Question', {
          params: {
            pageIndex: pagination.page,
            pageSize: pagination.rowsPerPage,
            sortBy: pagination.sortBy,
            searchValue: search
          }
        })
          .then(value => {
            const data = value.data.currentList;
            const paging = value.data.meta;
            const questions = [];
            for (let question of data) {
              questions.push({
                id: question.id,
                text: question.content,
                category: question.categories,
                answerCount: question.answerCount,
              })
            }
            resolve({
              questions,
              total: paging.totalElement,
            })
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
      return mockShell(() => {
        return _question.find(q => q.id == payload.id);
      }, true)
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
      } = payload;

      return new Promise((resolve, reject) => {
        console.debug('payload');
        axiosInstance.post('api/Question', {
          content: text,
          categories: category,
          answers: answers.map(value => value.text)
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
      return mockShell(() => {
        const question = _question.find(q => q.id == payload.id);
        question.text = payload.text;
        question.category = payload.category;
        return question;
      })
    },
    delete(context, payload) {
      return mockShell(() => {
        _.remove(_question, q => q.id == payload.id)
      })
    }

  }
}
