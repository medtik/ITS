import _areas from "./mockdata/Areas";
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
        let total = _areas.length;
        let areas = _areas.filter(_area => {
          return (
            (_area.name && _area.name.indexOf(payload.search) >= 0)
          )
        });

        if (payload.pagination.sortBy) {
          areas = areas.sort((a, b) => {
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
          areas = areas.slice((payload.pagination.page - 1) * payload.pagination.rowsPerPage, payload.pagination.page * payload.pagination.rowsPerPage)
        }

        return {
          areas,
          total
        }
      },true);
    },
    getById(context, payload) {
      return mockShell(() => {
        return _areas.find(item => item.id == payload.id);
      },true)
    },
    create(context, payload) {
      return mockShell(() => {

      })
    },
    update(context, payload) {
      return mockShell(() => {

      })
    },
    delete(context,payload){
      return mockShell(()=>{
        return payload;
      })
    }

  }
}
