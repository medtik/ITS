import {axiosInstance} from "../../common/util"

export default {
  namespaced: true,
  state: {
    loading: {
      search: false,
    },
    searchResult: {
      locations: undefined
    }
  },
  getters: {
    searchResultArea(state) {
      return state.searchResult.area;
    },
    searchResultLocations(state) {
      return state.searchResult.locations
    },
    searchResultLoading(state) {
      return state.loading.search;
    }
  },
  mutations: {
    setSearchResultLocations(state, payload) {
      state.searchResult.locations = payload.locations;
    },
    setSearchResultArea(state, payload) {
      state.searchResult.area = payload.area;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    fetchSearchResult(context, payload) {
      const {
        search,
        areaId
      } = payload;

      context.commit('setLoading', {
        loading: {search: true}
      });
      return new Promise((resolve, reject) => {

        axiosInstance.get('api/location', {
          params: {
            searchValue: search,
            pageSize: -1,
            areaId
          }
        }).then(value => {
          context.commit('setSearchResultLocations', {
            locations: value.data.currentList
          });
          context.commit('setLoading', {
            loading: {search: false}
          });
          resolve(value.data);
        })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {search: false}
            });
            reject(reason.response)
          });
      })
    }
  }
}
