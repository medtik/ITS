import {axiosInstance} from "../../common/util";
import _ from "lodash";

export default {
  namespaced: true,
  state: {
    myGroups: [],
    loading: {
      myGroup: true,
    }
  },
  getters: {
    myGroups(state) {
      return state.myGroups;
    },
    myGroupsLoading(state) {
      return state.loading.myGroups;
    }
  },
  mutations: {
    setMyGroup(state, payload) {
      state.myGroups = payload.groups;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    fetchMyGroups(context) {
      context.commit('setLoading', {
        loading: {myGroup: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/GetGroups')
          .then(value => {
            context.commit('setMyGroup', {groups: value.data});
            context.commit('setLoading', {
              loading: {myGroup: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {myGroup: false}
            });
            reject(reason.response);
          })
      });
    }
  }
}
