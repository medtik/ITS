import {axiosInstance} from "../../common/util";
import _ from "lodash";

export default {
  namespaced: true,
  state: {
    detailedLocation: undefined,
    loading: {
      detailedLocation: true,
    }
  },
  getters: {
    detailedLocation(state) {
      return state.detailedLocation;
    },
    detailedLocationLoading(state) {
      return state.loading.detailedLocation;
    }
  },
  mutations: {
    setDetailedLocation(state, payload) {
      state.detailedLocation = payload.location;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    addImage(context, payload) {
      const {
        locationId,
        photo
      } = payload;

      return Promise.resolve();
    },

    getDetails(context,payload) {
      const {
        id
      } = payload;
      context.commit('setLoading', {
        loading: {detailedLocation: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/details',{
          params:{
            id: id
          }
        })
          .then(value => {
            context.commit('setDetailedLocation',{
              location: value.data.locationDetail
            });
            context.commit('setLoading', {
              loading: {detailedLocation: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {detailedLocation: false}
            });
            reject(reason.response);
          })
      })
    }
  }
}
