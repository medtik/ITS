import {axiosInstance} from "../../common/util"
import Raven from "raven-js"
import moment from "moment";
import _ from "lodash"

export default {
  namespaced: true,
  state: {
    searchUsers: [],
    current: {},
    mobileToken: undefined,
    loading: {
      searchUsers: false,
      currentUser: false,
    }
  },
  getters: {
    getSearchUsersLoading(state) {
      return state.loading.searchUsers;
    }
  },
  mutations: {
    setSearchUsers(state, payload) {
      state.searchUsers = _.filter(payload.users, (user) => {
        if (state.current.id) {
          return user.id != payload.currentUserId
        } else {
          Raven.captureException(new Error('setSearchUsers: missing current user'));
          return true;
        }
      });
    },
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
    },
    setMobileToken(state, payload){
      const {
        token
      } = payload;

      state.mobileToken = token;
    }
  },
  actions: {
    updateMobileToken(context, payload) {
      const {
        token
      } = payload;
      // put /api/User/SetMobileToken
      if(context.rootGetters['authenticate/isLoggedIn']){
        axiosInstance.put('api/User/SetMobileToken?token=' + token);
      }
      context.commit('setMobileToken',{
        token: token
      });
    },
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
            resolve(value.data);
          })
          .catch(reason => {
            Raven.captureException(reason);
            context.commit('setLoading', {
              loading: {currentUser: false}
            });
            reject(reason.response)
          })
      });
    },
    fetchUsers(context, payload) {
      // get /api/User
      Raven.captureBreadcrumb(
        {
          category: 'action',
          message: 'fetchUsers',
          data: {
            payload: payload
          }
        }
      );
      const {
        nameInput
      } = payload;

      context.commit('setLoading', {
        loading: {searchUsers: true}
      });

      const usersPromise = axiosInstance.get('api/user', {
        params: {
          nameSearchValue: nameInput
        }
      });

      let currentUserPromise;
      if (context.state.current) {
        currentUserPromise = context.dispatch('fetchCurrentInfo');
      }


      return new Promise((resolve, reject) => {
        Promise.all([usersPromise, currentUserPromise])
          .then(values => {
            context.commit('setLoading', {
              loading: {searchUsers: false}
            });
            context.commit('setSearchUsers', {
              users: values[0].data.currentList,
              currentUserId: values[1].id
            });
            resolve(values[0].data);
          })
          .catch(reason => {
            Raven.captureException(reason);
            context.commit('setLoading', {
              loading: {searchUsers: false}
            });
            reject(reason.response);
          });
      })
    }
  }
}
