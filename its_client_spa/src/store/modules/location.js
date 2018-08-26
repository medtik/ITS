import {axiosInstance} from "../../common/util";
import _ from "lodash";
import Raven from "raven-js";

export default {
  namespaced: true,
  state: {
    detailedLocation: undefined,
    nearbyLocations: undefined,
    loading: {
      detailedLocation: true,
      nearbyLocations: true
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
    setNearbyLocations(state, payload) {
      state.nearbyLocations = payload.locations;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    addImage(context, payload) {
      //post /api/Location/AddImageToLocation
      const {
        locationId,
        photo
      } = payload;


      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Location/AddImageToLocation', {
          params: {
            locationId,
            photo
          }
        })
      })
    },

    review(context, payload) {
      // put /api/Location/AddReview
      const {
        locationId,
        title,
        description,
        rating,
        photos
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.put('api/Location/AddReview', {
          "locationId": locationId,
          "title": title,
          "description": description,
          "rating": rating,
          "photos": photos
        })
          .then(value => {
            resolve(value.data)
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject();
          })
      });

    },

    getDetails(context, payload) {
      const {
        id,
        name
      } = payload;
      context.commit('setLoading', {
        loading: {detailedLocation: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/details', {
          params: {
            id: id
          }
        })
          .then(value => {
            context.commit('setDetailedLocation', {
              location: value.data
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
      });
    },
    fetchNearbyLocations(context, payload) {
      // get /api/Location/NearbyLocation
      const {
        long, lat, radius
      } = payload;

      context.commit('setLoading', {loading: {nearbyLocations: true}});
      axiosInstance.get('api/Location/NearbyLocation', {
        params: {
          longitude: long,
          latitude: lat,
          radius: (radius/1000)
        }
      })
        .then(value => {
          context.commit('setLoading', {loading: {nearbyLocations: false}});
        })
        .catch(reason => {
          Raven.captureException(reason);
          context.commit('setLoading', {loading: {nearbyLocations: false}});
        })
    }
  }
}
