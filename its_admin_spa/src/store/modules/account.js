import _accounts from './Accounts.json';

function mockShell(bodyFunc) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (Math.random() > 0.3) {
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
      let total = _accounts.length;
      let accounts = _accounts.filter(account => {
        return (
          (account.name && account.name.indexOf(payload.search) >= 0) ||
          (account.email && account.email.indexOf(payload.search) >= 0) ||
          (account.phone && account.phone.indexOf(payload.search) >= 0) ||
          (account.birthdate && account.birthdate.indexOf(payload.search) >= 0) ||
          (account.address && account.address.indexOf(payload.search) >= 0)
        )
      });

      if (payload.pagination.sortBy) {
        accounts = accounts.sort((a, b) => {
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
        accounts = accounts.slice((payload.pagination.page - 1) * payload.pagination.rowsPerPage, payload.pagination.page * payload.pagination.rowsPerPage)
      }

      return mockShell(() => {
        return {
          accounts,
          total
        }
      });
    },
    getById(context, payload) {
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          let account = _accounts.find(acc => payload.id == acc.id);
          resolve(account);
        }, 1000)
      })
    },
    update(context, payload) {
      return mockShell(() => {
        let account = _accounts.find(acc => payload.id == acc.id);
        account.photo = payload.photo;
        account.name = payload.name;
        account.email = payload.email;
        account.phone = payload.phone;
        account.birthdate = payload.birthdate;
        account.address = payload.address;
        return account;
      })
    },
    create(context,payload) {
      return mockShell(() => {
        let account = {
          ...payload
        };
        account.id = _accounts.length + 1;

        _accounts.push(account);
        return account;
      })
    }
  }
};
