import axiosInstance from "../../common/util/axiosInstance";
import formatter from "../../formatter";
import _ from "lodash"

export default {
  namespaced: true,
  state: {
    allCategories: undefined,
    loading: {
      allCategories: true
    }
  },
  getters: {
    tagCategories(state) {
      return state.allCategories;
    },
    tagCategoryLoading(state) {
      return state.loading.allCategories;
    }
  },
  mutations: {
    setAllCategories(state, payload) {
      const {
        categories
      } = _.cloneDeep(payload);

      state.allCategories = _.uniq(categories);
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    getAll(context, payload) {
      const {
        pagination,
        search
      } = payload;

      const reqData = formatter.getAllRequest({
        search,
        pagination,
      });

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/tag', {
          params: reqData
        }).then(value => {
          resolve(formatter.getAllResponse(value.data));
        }).catch(reason => {
          reject(reason.response);
        })
      })

    },
    getAllCategories(context) {
      //get /api/Tag/categories
      context.commit('setLoading', {
        loading: {allCategories: true}
      });

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/tag/categories')
          .then(value => {
            context.commit('setAllCategories', {
              categories: value.data
            });
            context.commit('setLoading', {
              loading: {allCategories: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            console.error('tag/getAllCategories', reason.response);
            context.commit('setLoading', {
              loading: {allCategories: false}
            });
            reject(reason.response);
          })
      })
    },
    create(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.post('api/tag', {
          name: payload.tag.name,
          categories: payload.tag.categories,
        })
          .then(value => resolve(value.data))
          .catch(reason => reject(reason.response))
      })
    },
    update(context, payload) {

    },
    delete(context, payload) {
      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/tag', {
          params: {
            tagId: payload.id,
          }
        }).then(value => resolve(value.data))
          .catch(reason => reject(reason.response))
      });
    }
  }
};

