import _locations from "./mockdata/locations";
import {axiosInstance} from "../../common/util";

import formatter from "../../formatter";

import _ from "lodash";


function mockShell(bodyFunc, noFail) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (noFail || Math.random() > 0.1) {
        //Success
        let result = bodyFunc();
        resolve(result);
      } else {
        //error
        reject({
          message: 'Có lỗi xẩy ra !'
        })
      }

    }, 1500 + (Math.random() * 1000))
  })
}


export default {
  namespaced: true,
  actions: {
    getAll(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Location', {
          params: formatter.getAllRequest(payload)
        })
          .then(value => {

            const locations = _.map(value.data.currentList,
              (value, key, colelction) => {
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
        payload.businessHoursInput = _.map(payload.businessHoursInput,
          (value, key, collection) => {
            return {
              day: key,
              from: value.from,
              to: value.to,
            }
          });

        console.log('create', payload.secondaryPhotos, payload);
        payload.secondaryPhotos = _.map(payload.secondaryPhotos,
          (value, key, collection) => {
            console.debug('payload.secondaryPhotos', value, key);
            return value.url;
          });

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

        axiosInstance.post('api/location', formatted)
          .then(resolve)
          .catch(reject)
      })
    },
    update(context, payload) {
      return mockShell(() => {
      })
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
    }
  }
}
