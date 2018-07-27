import axiosInstance from "../../common/util/axiosInstance";

export default {
  namespaced: true,
  state: {
    areas: [],
    areasLoading: true
  },
  mutations: {
    setAreas(state, payload) {
      state.areas = payload.areas;
    },
    setLoading(state, payload) {
      state.areasLoading = payload.loading;
    }
  },
  actions: {
    getAll(context, payload) {
      context.commit('setLoading', {loading: true});
      return new Promise((resolve, reject) => {
        axiosInstance.get('/api/Area',{
          params: {
            pageIndex: 1,
            pageSize: -1,
          }
        })
          .then(value => {
            context.commit('setAreas', {
              areas: value.data.currentList
            });
            context.commit('setLoading', {loading: false});
            resolve();
          })
          .catch(reason => {
            context.commit('setLoading', {loading: false});
            reject(reason.response);
          })
      })

    }
  }
}
