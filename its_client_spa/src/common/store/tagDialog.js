import axios from "axios";
import {axiosInstance} from "../util";
import formatter from "../formatter/index";


export default {
  namespaced: true,
  state: {
    items: undefined,
    loading: true,
  },
  mutations: {
    setItemsResult(state, payload) {
      state.items = payload.list;
    },
    setLoading(state, payload) {
      state.loading = payload.loading;
    }
  },
  actions: {
    getAll(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/tag', {
          params: {
            pageSize: -1,
          }
        })
          .then(value => {
            const data = formatter.getAllResponse(value.data);
            context.commit('setItemsResult', data);
            context.commit('setLoading', {loading: false})
          })
          .catch(reason => {
            reject(reason.response);
          })
      })

    },
  }
}
