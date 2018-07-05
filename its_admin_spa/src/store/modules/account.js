export default {
  namespaced: true,
  actions: {
    getAll(context, payload) {
      let accounts = [{
        "id": 1,
        "name": "Lou Bolderoe",
        "email": "lbolderoe0@msu.edu",
        "phone": "453-765-1305",
        "birthdate": "04/15/1950",
        "address": "9 Artisan Court aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
      }, {
        "id": 2,
        "name": "Leighton Berringer",
        "email": "lberringer1@pcworld.com",
        "phone": "390-924-2762",
        "birthdate": "07/11/1976",
        "address": "4 Arizona Alley"
      }, {
        "id": 3,
        "name": "Leanor Everix",
        "email": "leverix2@youtu.be",
        "phone": "413-875-6910",
        "birthdate": "04/18/1983",
        "address": "2948 Rutledge Point"
      }, {
        "id": 4,
        "name": "Maryjo Vallance",
        "email": "mvallance3@flavors.me",
        "phone": "240-572-8041",
        "birthdate": "07/23/1969",
        "address": "87883 Lighthouse Bay Pass"
      }, {
        "id": 5,
        "name": "Helaine Molyneux",
        "email": "hmolyneux4@domainmarket.com",
        "phone": "309-569-9369",
        "birthdate": "07/17/1948",
        "address": "01917 Sunnyside Court"
      }, {
        "id": 6,
        "name": "Liliane Tamsett",
        "email": "ltamsett5@storify.com",
        "phone": "656-793-8138",
        "birthdate": "05/08/1974",
        "address": "8963 Sunfield Terrace"
      }, {
        "id": 7,
        "name": "Ty Witherow",
        "email": "twitherow6@networkadvertising.org"
      }, {
        "id": 8,
        "name": "Ric Harback",
        "email": "rharback7@tumblr.com"
      }, {
        "id": 9,
        "name": "Felic Cannings",
        "email": "fcannings8@google.pl",
        "phone": "599-566-9321",
        "birthdate": "07/06/1985",
        "address": "165 Karstens Court"
      }, {
        "id": 10,
        "name": "Rip Willshire",
        "email": "rwillshire9@berkeley.edu",
        "phone": "991-473-9616",
        "birthdate": "08/01/1978",
        "address": "4 Drewry Hill"
      }, {
        "id": 11,
        "name": "Yanaton Huie",
        "email": "yhuiea@domainmarket.com",
        "phone": "862-836-6455",
        "birthdate": "07/14/1976",
        "address": "78 Basil Park"
      }, {
        "id": 12,
        "name": "Hilary Sanford",
        "email": "hsanfordb@chron.com",
        "phone": "197-604-7814",
        "birthdate": "02/28/1968",
        "address": "0 Valley Edge Junction"
      }, {
        "id": 13,
        "name": "Jesselyn Swiffin",
        "email": "jswiffinc@wired.com",
        "phone": "296-213-7700",
        "birthdate": "10/11/1992",
        "address": "5644 Emmet Place"
      }];

      let total = accounts.length;

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
            total
          });
        }, 1500)
      })
    }
  }
};
