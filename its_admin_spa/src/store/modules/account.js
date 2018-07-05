export default {
  namespaced: true,
  actions: {
    getAll(context, payload) {
      let accounts = [{
        "name": "Vonnie Donaldson",
        "email": "vdonaldson0@rakuten.co.jp",
        "phone": "189-590-2167",
        "birthdate": "3/16/1947",
        "address": "9698 Oriole Hill"
      }, {
        "name": "Hendrick Jacop",
        "email": "hjacop1@businesswire.com",
        "phone": "639-193-4932",
        "birthdate": "6/28/1979",
        "address": "75 Mandrake Pass"
      }, {
        "name": "Beaufort Belson",
        "email": "bbelson2@sina.com.cn",
        "phone": "554-783-5757",
        "birthdate": "9/15/1995",
        "address": "606 Arizona Court"
      }, {
        "name": "Durant Huet",
        "email": "dhuet3@ovh.net",
        "phone": "537-350-6583",
        "birthdate": "12/16/1976",
        "address": "7 Express Pass"
      }, {
        "name": "Jilleen Rathborne",
        "email": "jrathborne4@ucsd.edu",
        "phone": "747-927-3459",
        "birthdate": "8/7/1984",
        "address": "221 Buhler Junction"
      }, {
        "name": "Franny Davidovich",
        "email": "fdavidovich5@reference.com",
        "phone": "874-834-2519",
        "birthdate": "4/22/1986",
        "address": "282 Huxley Circle"
      }, {
        "name": "Benetta Hatherill",
        "email": "bhatherill6@about.com",
        "phone": "858-890-2841",
        "birthdate": "1/6/1961",
        "address": "19 Bay Parkway"
      }, {
        "name": "Betteann Chestnutt",
        "email": "bchestnutt7@pbs.org",
        "phone": "600-391-1848",
        "birthdate": "2/27/1991",
        "address": "54765 Melody Crossing"
      }, {
        "name": "Zora Lawrance",
        "email": "zlawrance8@hostgator.com",
        "phone": "693-365-3545",
        "birthdate": "5/28/1970",
        "address": "890 Farwell Crossing"
      }, {
        "name": "Rosella Dilloway",
        "email": "rdilloway9@fema.gov",
        "phone": "838-250-8269",
        "birthdate": "10/3/1928",
        "address": "8 Nova Plaza"
      }];

      accounts = accounts.filter(account => {
        return account.name.indexOf(payload.search) >= 0 ||
          account.email.indexOf(payload.search) >= 0 ||
          account.phone.indexOf(payload.search) >= 0 ||
          account.birthdate.indexOf(payload.search) >= 0 ||
          account.address.indexOf(payload.search) >= 0;
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

      return new Promise((resolve, reject) => {
        setTimeout(() => {
          resolve({
            accounts,
            total: 10
          });
        }, 1500)
      })
    }
  }
};
