import axiosInstance from "../../axiosInstance";
import _ from "lodash";

export default {
  namespaced: true,
  state: {
    loading: {
      table: true,
      createBtn: false
    },
    //Listing page
    tagTableItems: [],
    tagTableItemsTotal: undefined,
    tagTableSearchValue: undefined,
    tagTablePagination: {},
    error: {
      notified: false,
      message: undefined
    },
    success: {
      notified: false,
      message: undefined
    },
  },
  getters: {
    orderBy(state, getters) {
      const {
        sortBy,
        descending,
      } = state.tagTablePagination;

      const sortByStr = sortBy;
      const descStr = descending ? 'desc' : 'asc';
      return `${sortByStr}_${descStr}`;
    },
    requestGetAll(state, getters) {
      const {
        page,
        rowsPerPage
      } = state.tagTablePagination;

      return {
        searchValue: state.tagTableSearchValue,
        orderBy: getters.orderBy,
        pageIndex: page,
        pageSize: rowsPerPage,
      }
    }
  },
  mutations: {
    setPagination(state, payload) {
      state.tagTablePagination = payload.pagination
    },
    setSearchValue(state, payload) {
      state.tagTableSearchValue = payload.search;
    },
    setTagTableData(state, payload) {
      const {
        meta,
        currentList
      } = payload;

      state.tagTableItemsTotal = meta.totalElement;
      state.tagTableItems = currentList;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload);
    },
    setError(state, payload) {
      state.error = {
        notified: false,
        message: 'Có lỗi sẩy ra'
      }
    },
    setSuccess(state, payload) {

    }
  },
  actions: {
    search(context, payload) {
      const data = {
        ...context.state.tagTablePagination
      };
      data.page = 1;
      context.commit('setSearchValue', {...payload});
      context.commit('setPagination', {pagination: data});
      context.dispatch('getAll');
    },
    getAll(context, payload) {
      context.commit('setLoading', {
        table: true
      });
      axiosInstance.get('api/tag', {
        params: context.getters.requestGetAll
      })
        .then(value => {
          context.commit('setTagTableData', value.data);
          context.commit('setLoading', {
            table: false
          });
        })
        .catch(reason => {
          context.commit('setError', reason.response);
          context.commit('setLoading', {
            table: false
          });
        })
    },
    /***
     * @param context
     * @param payload {id}
     */
    getById(context, payload) {

    },
    create(context, payload) {
      axiosInstance.post('api/tag', {
        name: payload.tag.name,
        categories: payload.tag.categories,
      })
        .then(value => context.dispatch("getAll"))
    },
    update(context, payload) {

    },
    delete(context, payload) {
      context.commit('setLoading', {
        table: true
      });
      axiosInstance.delete('api/tag', {
        params: {
          tagId: payload.id,
        }
      })
        .then(value => context.dispatch("getAll"))
    }
  }
};
