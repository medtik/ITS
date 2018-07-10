import _claimOwner from "./mockdata/ClaimOwnerRequests";
import _changeRequest from "./mockdata/LocationChangeRequests";
import _reportReview from "./mockdata/ReportReviewRequest";

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
        const _requests = _.concat(_changeRequest, _claimOwner, _reportReview);
        let total = _requests.length;
        // let requests = _requests.filter(request => {
        //   return (
        //     (request.title && request.title.indexOf(payload.search) >= 0)
        //   )
        // });
        let requests = _requests;

        if (payload.pagination.sortBy) {
          requests = requests.sort((a, b) => {
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
          requests = requests.slice((payload.pagination.page - 1) * payload.pagination.rowsPerPage, payload.pagination.page * payload.pagination.rowsPerPage)
        }

        return {
          requests,
          total
        }
      }, true);
    },
    getById(context, payload) {
      return mockShell(() => {
        const _requests = _.concat(_changeRequest, _claimOwner, _reportReview);
        return _requests.find(item => item.id == payload.id);
      }, true)
    },
    accept(context, payload) {
      const {
        id
      } = payload;

      return mockShell(() => {

      })
    },
    deny(context, payload) {
      const {
        id
      } = payload;

      return mockShell(() => {

      })
    },
  }
}
