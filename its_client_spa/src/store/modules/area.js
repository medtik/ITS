import axiosInstance from "../../axiosInstance";

export default {
  namespaced: true,
  state: {
    areas: [],
    areasLoading: true
  },
  mutations: {
    setAreas(state, payload) {
      state.areas = payload.areas;
      state.areasLoading = false;
    }
  },
  actions: {
    getAll(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('/api/Area')
          .then(value => {
            context.commit('setAreas', {
              areas: value.data
            });
            resolve();
          })
          .catch(reason => {
            reject(reason.response);
          })
      })

    }
  }
}
