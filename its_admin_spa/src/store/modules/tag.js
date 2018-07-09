import _tags from './mockdata/Tags'

function mockShell(bodyFunc) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (Math.random() > 0.1) {
        //Success
        let result = bodyFunc();
        resolve(result);
      } else {
        //error
        reject({
          message: 'Có lỗi xẩy ra'
        })
      }

    }, 1500 + (Math.random() * 1000))
  })
}

export default {
  namespaced: true,
  actions: {
    /***
     *
     * @param context
     * @param payload {pagination}
     */
    getAll(context, payload) {
      return mockShell(() => {
        if(!payload) return {tags: _tags};

        let total = _tags.length;
        let tags = _tags.filter(tag => {
          return (
            (tag.name && tag.name.indexOf(payload.search) >= 0) ||
            (tag.category && tag.category.indexOf(payload.search) >= 0)
          )
        });

        if (payload.pagination.sortBy) {
          tags = tags.sort((a, b) => {
            const sortA = a[payload.pagination.sortBy];
            const sortB = b[payload.pagination.sortBy];

            if (payload.pagination.descending) {
              if (sortA < sortB) return 1;
              if (sortA > sortB) return -1;
              return 0
            } else {
              if (sortA < sortB) return -1;
              if (sortA > sortB) return 1;
              return 0
            }
          })
        }

        if (payload.pagination.rowsPerPage > 0) {
          tags = tags.slice((payload.pagination.page - 1) * payload.pagination.rowsPerPage, payload.pagination.page * payload.pagination.rowsPerPage)
        }

        return {
          tags,
          total
        }
      },true);
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
