import axiosInstance from "../../axiosInstance";

export default {
  namespaced: true,
  state: {
    //Listing page
    tagTableItems: [],
    tagTableItemsTotal: undefined,
    tagTableLoading: true,
    tagTableSearchValue: undefined,
    tagTablePagination: {},
    error: {
      notified: false,
      message: undefined
    },
    success: {
      notified: false,
      message: undefined
    }
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
    setTagTableLoading(state, payload) {
      state.tagTableLoading = payload.loading;
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
      context.commit('setTagTableLoading', {
        loading: true
      });
      axiosInstance.get('api/tag', {
        params: context.getters.requestGetAll
      })
        .then(value => {
          context.commit('setTagTableData', value.data);
          context.commit('setTagTableLoading', {
            loading: false
          });
        })
        .catch(reason => {
          context.commit('setError', reason.response);
          context.commit('setTagTableLoading', {
            loading: false
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

    },
    update(context, payload) {

    },
    delete(context, payload) {

    }
  }
};
