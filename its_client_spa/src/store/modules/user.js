import {axiosInstance} from "../../common/util"
import Raven from "raven-js"
import _ from "lodash"

export default {
  namespaced: true,
  state: {
    users: [],
    loading: {
      users: false,
    }
  },
  getters: {
    getUsers(state) {
      return state.users;
    },
    getUsersLoading(state) {
      return state.loading.users;
    }
  },
  mutations: {
    setUser(state, payload) {
      state.users = payload.users;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
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

      context.setLoading({
        loading: {users: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/user', {
          params: {
            nameSearchValue: nameInput
          }
        }).then((value) => {
          context.setLoading({
            loading: {users: false}
          });
          resolve(value.data);
        }).catch((reason) => {
          context.setLoading({
            loading: {users: false}
          });
          Raven.captureException(reason);
          reject(reason.response);
        })
      })
    }
  }
}
