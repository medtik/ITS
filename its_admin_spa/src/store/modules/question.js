import _question from "./mockdata/Questions";
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
      return mockShell(() => {
        let total = _question.length;
        let questions = _question.filter(_question => {
          return (
            (_question.text && _question.text.indexOf(payload.search) >= 0) ||
            (_question.category && _question.category.indexOf(payload.search) >= 0)
          )
        });

        if (payload.pagination.sortBy) {
          questions = questions.sort((a, b) => {
            const sortA = a[payload.pagination.sortBy];
            const sortB = b[payload.pagination.sortBy];

            if (payload.pagination.descending) {
              if (sortA < sortB) return 1;
              if (sortA > sortB) return -1;
              return 0
            } else {
              if (sortA < sortB) return -1;
              if (sortA > sortB) return 1;
              return 0
            }
          })
        }

        if (payload.pagination.rowsPerPage > 0) {
          questions = questions.slice((payload.pagination.page - 1) * payload.pagination.rowsPerPage, payload.pagination.page * payload.pagination.rowsPerPage)
        }

        return {
          questions,
          total
        }
      },true);
    },
    getById(context, payload) {
      return mockShell(() => {
        return _question.find(q => q.id == payload.id);
      },true)
    },
    getCategories(context, payload) {
      return mockShell(() => {
        const searchString = payload.search ? payload.search : '';
        return _question
          .filter(q => q.category && q.category.indexOf(searchString) >= 0)
          .map(q => q.category)
      }, true)
    },
    create(context, payload) {
      return mockShell(() => {
        const question = {
          id: _question.length + 1,
          text: payload.text,
          category: payload.category
        };
        _question.push(question);
        return question;
      })
    },
    update(context, payload) {
      return mockShell(() => {
        const question = _question.find(q => q.id == payload.id);
        question.text = payload.text;
        question.category = payload.category;
        return question;
      })
    },
    delete(context,payload){
      return mockShell(()=>{
        _.remove(_question,q => q.id == payload.id)
      })
    }

  }
}
