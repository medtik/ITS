import _areas from "./mockdata/Areas";
import {axiosInstance} from "../../common/util";
import formatter from "../../formatter";
import _ from 'lodash';

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
          message: 'Có lỗi xảy ra !'
        })
      }

    }, 1500 + (Math.random() * 1000))
  })
}

export default {
  namespaced: true,
  state: {
    areas: [],
    featuredAreas: [],
    detailedArea: {},
    loading: {
      areas: true,
      detailedArea: true,
      featuredAreas: false,
      create: false,
      edit: false,
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
    setDetailedArea(state, payload) {
      state.detailedArea = payload.area;
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
            const response = formatter.getAllResponse(value.data);
            resolve(response);
          })
          .catch(reason => {
            context.commit('setLoading', {loading: {areas: false}});
            reject(reason.response);
          })
      })

    },
    getAllNoParam(context) {
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/area', {
          params: {
            pageSize: -1
          }
        })
          .then(value => {
            const response = formatter.getAllResponse(value.data);
            resolve(response);
          }).catch(reject)
      })
    },
    getById(context, payload) {
      // get /api/Area/DetailsAdmin
      const {
        id
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Area/DetailsAdmin', {
          params: {
            id
          }
        })
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response);
          })
      });

    },
    create(context, payload) {
      // post /api/Area

      const {
        name,
        questions
      } = payload;

      return new Promise((resolve, reject) => {
       axiosInstance.post('api/Area',{
         name,
         questions: _.map(questions, "id")
       })
         .then(value =>{
           resolve(value.data);
         })
         .catch(reason => {
           reject(reason.response);
         })
     })
    },
    update(context, payload) {
      const {
        id,
        name,
        questions
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.put('api/Area',{
          id,
          name,
          questions: _.map(questions, "id")
        })
          .then(value =>{
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response);
          })
      })
    },
    delete(contlext, payload) {
      return mockShell(() => {
        return payload;
      })
    }

  }
}
