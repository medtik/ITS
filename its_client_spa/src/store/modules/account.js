import axiosInstance from "../../common/util/axiosInstance";
import moment from "moment";
import Raven from "raven-js";

import _ from "lodash";

export default {
  namespaced: true,
  state: {
    loading: {
      recoverPassword: false,
      changePassword: false
    }
  },
  mutations: {
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {

    signup(context, payload) {
      const {
        email,
        password,
        rePassword,
        name,
        address,
        phone,
        birthdate
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.post('/api/account', {
          emailAddress: email,
          password: password,
          rePassword: rePassword,
          fullName: name,
          address: address,
          phoneNumber: phone,
          birthdate: birthdate,
        })
          .then(value => resolve(value.data))
          .catch(reason => {
            const {
              data,
              status
            } = reason.response;

            let error = {};
            if (status === 400) {
              error = {
                email: data.modelState['register.EmailAddress'] &&
                  data.modelState['register.EmailAddress'][0],
                password: data.modelState['register.Password'] &&
                  data.modelState['register.Password'][0],
                rePassword: data.modelState['register.RePassword'] &&
                  data.modelState['register.RePassword'][0],
                name: data.modelState['register.FullName'] &&
                  data.modelState['register.FullName'][0],
                address: data.modelState['register.Address'] &&
                  data.modelState['register.Address'][0],
                phone: data.modelState['register.PhoneNumber'] &&
                  data.modelState['register.PhoneNumber'][0],
                birthdate: data.modelState['register.Birthdate'] &&
                  data.modelState['register.Birthdate'][0]
              }
            }

            reject({
              data,
              status,
              error
            })
          })
      });
    },
    recoverPassword(context, payload) {
      // post /api/Account/RecoverPassword
      const {
        email
      } = payload;

      context.commit('setLoading', {
        loading: {recoverPassword: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Account/RecoverPassword', {
          userRecover: {
            "email": email
          }
        })
          .then(() => {
            context.commit('setLoading', {
              loading: {recoverPassword: false}
            });
            resolve()
          })
          .catch((reason) => {
            context.commit('setLoading', {
              loading: {recoverPassword: false}
            });
            Raven.captureException(reason);
            reject(reason.response);
          })
      });
    },
    changePassword(context, payload) {
      const {
        password,
        rePassword
      } = payload;


    }
  }
}
