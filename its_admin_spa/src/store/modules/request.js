import _claimOwner from "./mockdata/ClaimOwnerRequests";
import _changeRequest from "./mockdata/LocationChangeRequests";
import _reportReview from "./mockdata/ReportReviewRequest";
import {axiosInstance} from "../../common/util"
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
          message: 'Có lỗi xảy ra !'
        })
      }

    }, 1500 + (Math.random() * 1000))
  })
}

export default {
  namespaced: true,
  actions: {
    getChangeLocationRequest() {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Location/GetChangeRequests')
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response);
          })
      })
    },
    getReportReviewRequests(){
      // get /api/Location/Report
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Location/Report')
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response);
          })
      })
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
