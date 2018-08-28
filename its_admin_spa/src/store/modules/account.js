import _accounts from './mockdata/Accounts.json';
import _ from "lodash"
import {axiosInstance} from "../../common/util"
import formatter from "../../common/formatter"
import Raven from "raven-js"
import moment from "moment"
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
      // return mockShell(() => {
      //   let total = _accounts.length;
      //   let accounts = _accounts.filter(account => {
      //     return (
      //       (account.name && account.name.indexOf(payload.search) >= 0) ||
      //       (account.email && account.email.indexOf(payload.search) >= 0) ||
      //       (account.phone && account.phone.indexOf(payload.search) >= 0) ||
      //       (account.birthdate && account.birthdate.indexOf(payload.search) >= 0) ||
      //       (account.address && account.address.indexOf(payload.search) >= 0)
      //     )
      //   });
      //
      //   if (payload.pagination.sortBy) {
      //     accounts = accounts.sort((a, b) => {
      //       const sortA = a[payload.pagination.sortBy];
      //       const sortB = b[payload.pagination.sortBy];
      //
      //       if (payload.pagination.descending) {
      //         if (sortA < sortB) return 1;
      //         if (sortA > sortB) return -1;
      //         return 0
      //       } else {
      //         if (sortA < sortB) return -1;
      //         if (sortA > sortB) return 1;
      //         return 0
      //       }
      //     })
      //   }
      //
      //   if (payload.pagination.rowsPerPage > 0) {
      //     accounts = accounts.slice((payload.pagination.page - 1) * payload.pagination.rowsPerPage, payload.pagination.page * payload.pagination.rowsPerPage)
      //   }
      //
      //   return {
      //     accounts,
      //     total
      //   }
      // },true);
      // get /api/User

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/User', {
          params: formatter.getAllRequest(payload)
        })
          .then(value => {
            const accounts = _.map(value.data.currentList,
              (value) => {

                let account = _.mapKeys(value, (value, key) => {
                  switch (key) {
                    default:
                      return key;
                  }
                });
                account.birthdate = moment(account.birthdate).format('l')
                return account;
              }
            );

            resolve({
              accounts,
              total: value.data.meta.totalElement,
            })
          })
          .catch(error => {
            Raven.captureException(error);
          })
      })
    },
    getById(context, payload) {
      return mockShell(() => {
        return _accounts.find(acc => payload.id == acc.id);
      }, true);

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
    create(context, payload) {
      return mockShell(() => {
        let account = {
          ...payload
        };
        account.id = _accounts.length + 1;

        _accounts.push(account);
        return account;
      })
    },
    ban(context, payload) {
      return mockShell(() => {
        let account = _accounts.find(acc => payload.id == acc.id);
        account.ban = true;
        return account;
      })
    },
    unBan(context, payload) {
      return mockShell(() => {
        let account = _accounts.find(acc => payload.id == acc.id);
        account.ban = false;
        return account;
      })
    },
    upgradeAccount() {

    }
  }
};
