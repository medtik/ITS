import axiosInstance from "../../common/util/axiosInstance";
import {RavenStatic as Raven} from "raven-js";

export default {
  namespaced: true,
  state: {
    areas: [],
    featuredAreas: [],
    detailedArea: undefined,
    loading: {
      areas: true,
      detailedArea: true,
      featuredAreas: false
    },
  },
  getters: {
    areas(state) {
      return state.areas;
    },
    areasLoading(state) {
      return state.loading.areas;
    },
    featuredAreas(state) {
      return state.featuredAreas;
    },
    featuredAreasLoading(state) {
      return state.loading.featuredAreas;
    }
  },
  mutations: {
    setAreas(state, payload) {
      state.areas = payload.areas;
    },
    setFeaturedAreas(state, payload) {
      state.featuredAreas = payload.areas;
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

    getFeatured(context) {
      context.commit('setLoading', {loading: {featuredAreas: true}});
      axiosInstance.get('api/GetFeaturedArea')
        .then(value => {
          context.commit('setLoading', {loading: {featuredAreas: false}});
          context.commit('setFeaturedAreas', {areas: value.data})
        })
        .catch(reason => {
          context.commit('setLoading', {loading: {featuredAreas: false}});
          Raven.captureException(reason);
        })
    }
  }
}
