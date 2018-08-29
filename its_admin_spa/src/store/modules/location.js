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
      // get /api/Details
      const {
        id
      } = payload;
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Details', {
          params: {
            id
          }
        })
          .then(value => {
            let formatted = _(value.data)
              .mapKeys((value, key) => {
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
              .mapValues((value,key)=>{
                if(key == "area"){
                  return Number(value);
                }
                switch (key) {
                  case 'area':
                    return Number(value);
                  default:
                    return value;
                }
              })
              .value();
            resolve(formatted);
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject();
          })
      })
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

        if(formatted.phoneNumber == ""){
          formatted.phoneNumber = undefined;
        }

        if(!!formatted.longitude){
          formatted.longitude = Number(formatted.longitude);
        }

        if(!!formatted.latitude){
          formatted.latitude = Number(formatted.latitude);
        }

        console.debug("createlocation/formatted", formatted);

        return new Promise((resolve1, reject1) => {
          axiosInstance.post('api/location', formatted)
            .then(value => {
              resolve();
            })
            .catch(reason => {
              Raven.captureException(reason);
              reject(reason.response.data.modelState)
            })
        })

      })
    },
    update(context, payload) {
      const {
        id,
        nameInput,
        addressInput,
        descriptionInput,
        phoneInput,
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


      const data = {
        "id": id,
        "name": nameInput,
        "address": addressInput,
        "desription": descriptionInput,
        "Latitude": latInput,
        "Longitude": longInput,
        "web": websiteInput,
        "phone": phoneInput,
        "email": emailInput,
        "areaId": areaInput,
        "category": category,
        "isVerified": isVerifiedInput,
        "isClosed": isCloseInput,
        "tags": _.map(tagsInput, 'id'),
        "reviews": reviewsInput,
        "primaryPhoto": primaryPhotoInput,
        "otherPhotos": secondaryPhotos,
        "days": businessHoursInput
      };

      if(data["phone"] == ""){
        data["phone"] = undefined;
      }

      if(!!data["lat"]){
        data["lat"] = Number(data["lat"]);
      }

      if(!!data["long"]){
        data["long"] = Number(data["long"]);
      }

      return new Promise((resolve, reject) => {
        axiosInstance.put('api/location', data)
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response.data.modelState)
          })
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
