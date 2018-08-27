import _locations from "./mockdata/locations";
import {axiosInstance} from "../../common/util";

import formatter from "../../formatter";
import Raven from "raven-js"
import _ from "lodash";


export default {
  namespaced: true,
  state: {
    categories: [],
    loading: {
      categories: false
    }
  },
  getters: {
    categoriesLoading(state) {
      return state.loading.categories;
    },
    categories(state) {
      return state.categories;
    }
  },
  mutations: {
    setCategories(state, payload) {
      state.categories = payload.categories;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    getAll(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Location', {
          params: formatter.getAllRequest(payload)
        })
          .then(value => {

            const locations = _.map(value.data.currentList,
              (value) => {
                return _.mapKeys(value, (value, key) => {
                  switch (key) {
                    case 'phoneNumber':
                      return 'phone';
                    case 'emailAddress':
                      return 'email';
                    case 'areaName':
                      return 'area';
                    default:
                      return key;
                  }
                })
              }
            );

            resolve({
              locations: locations,
              total: value.data.meta.totalElement,
            })
          })
          .catch(reason => {
            let error = [];
            reject({
              ...reason.response,
              error
            })
          })
      });
    },
    getById(context, payload) {

      return mockShell(() => {
        return {
          location: _locations.find(q => q.id == payload.id)
        };
      }, true)
    },
    create(context, payload) {
      return new Promise((resolve, reject) => {
        payload.secondaryPhotos = _.map(payload.secondaryPhotos,
          (value, key, collection) => {
            console.debug('payload.secondaryPhotos', value, key);
            return value;
          });

        console.debug("createlocation", payload);

        payload.tagsInput = _.map(payload.tagsInput,
          (value, key, collection) => {
            return value.id;
          });

        const formatted = _.mapKeys(payload, (value, key) => {
          switch (key) {
            case 'nameInput':
              return 'name';
            case 'addressInput':
              return 'address';
            case 'descriptionInput':
              return 'description';
            case 'longInput':
              return 'longitude';
            case 'latInput':
              return 'latitude';
            case 'websiteInput':
              return 'website';
            case 'phoneInput':
              return 'phoneNumber';
            case 'emailInput':
              return 'emailAddress';
            case 'isVerifiedInput':
              return 'isVerified';
            case 'isCloseInput':
              return 'isClosed';
            case 'areaInput':
              return 'areaId';
            case 'primaryPhotoInput':
              return 'primaryPhoto';
            case 'secondaryPhotos':
              return 'otherPhotos';
            case 'businessHoursInput':
              return 'days';
            case 'tagsInput':
              return 'tags';
            default:
              return key;
          }
        });

        console.debug("createlocation/formatted", formatted);

        axiosInstance.post('api/location', formatted)
          .then(resolve)
          .catch(reject)
      })
    },
    update(context, payload) {
      const {
        nameInput,
        addressInput,
        descriptionInput,
        longInput,
        latInput,
        websiteInput,
        emailInput,
        areaInput,
        category,
        isVerifiedInput,
        isCloseInput,
        tagsInput,
        reviewsInput,
        businessHoursInput,
        primaryPhotoInput,
        secondaryPhotos,
      } = payload;

      axiosInstance.patch('api/location', {
        "name": nameInput,
        "address": addressInput,
        "desription": descriptionInput,
        "lat": latInput,
        "long": longInput,
        "web": websiteInput,
        "phone": phoneInput,
        "email": emailInput,
        "areaId": areaInput,
        "category": category,
        "isVerified": isVerifiedInput,
        "isClosed": isCloseInput,
        "tags": tagsInput,
        "primaryPhoto": primaryPhotoInput,
        "otherPhotos": secondaryPhotos,
        "days": businessHoursInput
      });
    },
    delete(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/location', {
          params: {
            locationId: payload.id
          }
        })
          .then(resolve)
          .catch(reject)
      })
    },
    fetchCategories(context) {
      //GET api/location/categories

      context.commit('setLoading', {loading: {categories: true}});
      axiosInstance.get('api/location/categories')
        .then(value => {
          context.commit('setCategories', {categories: value.data});
          context.commit('setLoading', {loading: {categories: false}});
        })
        .catch(reason => {
          context.commit('setLoading', {loading: {categories: false}});
          Raven.captureException(reason);
        })
    }
  }
}
