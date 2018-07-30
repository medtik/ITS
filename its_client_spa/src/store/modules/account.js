import axiosInstance from "../../common/util/axiosInstance";
import moment from "moment";

import _ from "lodash";

export default {
  namespaced: true,
  state: {
    current: {
      name: undefined,
      address: undefined,
      phoneNumber: undefined,
      emailAddress: undefined,
      birthdate: undefined,
      photo: undefined
    },
    loading: {
      currentUser: true
    }
  },
  getters: {
    currentAccount(state) {
      return state.current;
    }
  },
  mutations: {
    setCurrentUser(state, payload) {
      const {
        name,
        address,
        phoneNumber,
        emailAddress,
        birthdate,
        photo
      } = _.cloneDeep(payload.user);
      const birthdateFormatted = moment(birthdate).format('YYYY-MM-DD');

      state.current = {
        name,
        address,
        phoneNumber,
        emailAddress,
        birthdate: birthdateFormatted,
        photo
      };
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    updateAccountInfo(context, payload) {
      console.debug('updateAccountInfo', payload);
      return Promise.resolve();
    },
    fetchCurrentInfo(context) {
      context.commit('setLoading', {
        loading: {currentUser: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/CurrentUser')
          .then(value => {
            context.commit('setCurrentUser', {
              user: value.data
            });
            context.commit('setLoading', {
              loading: {currentUser: false}
            });
            resolve();
          })
          .catch(reason => {
            console.debug(reason.response);
            context.commit('setLoading', {
              loading: {currentUser: false}
            });
            reject(reason.response)
          })
      });
    },
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
          email: email,
          password: password,
          rePassword: rePassword,
          name: name,
          address: address,
          phone: phone,
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
  }
}
