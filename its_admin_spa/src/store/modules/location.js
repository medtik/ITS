import _locations from "./mockdata/locations";
import axiosInstance from "../../axiosInstance";

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
        axiosInstance.get('api/Location', {
          params: {
            pageIndex: pagination.page,
            pageSize: pagination.rowsPerPage,
            sortBy: pagination.sortBy,
            searchValue: search
          }
        })
          .then(value => {
            resolve({
              locations: value.data.currentList,
              total: value.data.meta.totalElement,
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
    getById(context, payload) {
      return mockShell(() => {
        return {
          location: _locations.find(q => q.id == payload.id)
        };
      }, true)
    },
    create(context, payload) {

    },
    update(context, payload) {
      return mockShell(() => {
      })
    },
    delete(context, payload) {
      return mockShell(() => {

      })
    }

  }
}
