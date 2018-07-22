import axiosInstance from "../../axiosInstance";
import formatter from "../../formatter";
import _ from "lodash";

export default {
  namespaced: true,
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
    /***
     * @param context
     * @param payload {id}
     */
    getById(context, payload) {

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
