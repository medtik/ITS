export default {
  getAllResponse({meta, currentList}) {
    return {
      list: currentList,
      total: meta.totalElement
    }
  },

  getAllRequest({pagination, search, sortByStr}) {
    if (!sortByStr) {
      sortByStr = pagination.sortBy;
    }
    const descStr = pagination.descending ? 'desc' : 'asc';
    const orderBy = `${sortByStr}_${descStr}`;
    console.debug(search);
    return {
      orderBy,
      pageIndex: pagination.page,
      pageSize: pagination.rowsPerPage,
      searchValue: search
    };
  }

}
