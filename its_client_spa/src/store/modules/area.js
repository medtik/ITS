import axiosInstance from "../../common/util/axiosInstance";

export default {
  namespaced: true,
  state: {
    areas: [],
    featuredArea: [],
    loading: {
      areas: true,
      featuredArea: false
    },
  },
  getters: {
    areas(state) {
      return state.areas;
    },
    areasLoading(state) {
      return state.loading.areas;
    },
    featuredArea() {
      return state.featuredArea
    }
  },
  mutations: {
    setAreas(state, payload) {
      state.areas = payload.areas;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    getAll(context) {
      context.commit('setLoading', {loading: {areas: true}});
      return new Promise((resolve, reject) => {
        axiosInstance.get('/api/Area', {
          params: {
            pageIndex: 1,
            pageSize: -1,
          }
        })
          .then(value => {
            context.commit('setAreas', {
              areas: value.data.currentList
            });
            context.commit('setLoading', {loading: {areas: false}});
            resolve();
          })
          .catch(reason => {
            context.commit('setLoading', {loading: {areas: false}});
            reject(reason.response);
          })
      })

    },

    getFeatured() {
      axiosInstance.get('api/GetFeaturedArea');
    }
  }
}
