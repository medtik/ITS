import _question from "./Questions";

function mockShell(bodyFunc) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (Math.random() > 0.1) {
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
      });
    },
    getById(context, payload){

    }
  }
}
