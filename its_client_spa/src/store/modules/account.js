import superagent from "superagent"
import config from "../../superagentConfig"

export default {
  namespaced: true,
  state: {
    token: undefined,
    current: {
      "id": 1,
      "name": "Stephanus Culbert",
      "address": "0571 Cody Alley",
      "phone": "952-133-2547",
      "email": "sculbert0@deliciousdays.com",
      "birthdate": "1951-04-21",
      "photo": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAHwSURBVDjLpZM9a1RBFIafM/fevfcmC7uQjWEjUZKAYBHEVEb/gIWFjVVSWEj6gI0/wt8gprPQykIsTP5BQLAIhBVBzRf52Gw22bk7c8YiZslugggZppuZ55z3nfdICIHrrBhg+ePaa1WZPyk0s+6KWwM1khiyhDcvns4uxQAaZOHJo4nRLMtEJPpnxY6Cd10+fNl4DpwBTqymaZrJ8uoBHfZoyTqTYzvkSRMXlP2jnG8bFYbCXWJGePlsEq8iPQmFA2MijEBhtpis7ZCWftC0LZx3xGnK1ESd741hqqUaqgMeAChgjGDDLqXkgMPTJtZ3KJzDhTZpmtK2OSO5IRB6xvQDRAhOsb5Lx1lOu5ZCHV4B6RLUExvh4s+ZntHhDJAxSqs9TCDBqsc6j0iJdqtMuTROFBkIcllCCGcSytFNfm1tU8k2GRo2pOI43h9ie6tOvTJFbORyDsJFQHKD8fw+P9dWqJZ/I96TdEa5Nb1AOavjVfti0dfB+t4iXhWvyh27y9zEbRRobG7z6fgVeqSoKvB5oIMQEODx7FLvIJo55KS9R7b5ldrDReajpC+Z5z7GAHJFXn1exedVbG36ijwOmJgl0kS7lXtjD0DkLyqc70uPnSuIIwk9QCmWd+9XGnOFDzP/M5xxBInhLYBcd5z/AAZv2pOvFcS/AAAAAElFTkSuQmCC",
      "ban": true
    }
  },
  getters: {
    currentAccount(state, getters, rootState) {
      return state.current;
    }
  },
  actions: {
    updateAccountInfo(context, payload) {
      console.debug('updateAccountInfo', payload);
      return Promise.resolve();
    },
    signin(context, payload) {
      const {
        email,
        password
      } = payload;

      return new Promise((resolve, reject) => {
        superagent.post(config.root + '/token')
          .set('Content-type', 'text/plan')
          .send(`grant_type=password&username${email}&password=${password}`)
          .then((response) => {
            const body = response.body;
            resolve(body)
          })
          .catch(reason => {
            reject(reason);
          })
      });
    },
    signup(context, payload) {

      const {
        email,
        password,
        rePassword,
        name,
        address,
        phone,
        birthdate
      } = payload;

      return new Promise((resolve, reject) => {
        superagent.post(config.root + '/api/account')
          .use(config.prefix)
          .send({
            emailAddress: email,
            password: password,
            rePassword: rePassword,
            fullName: name,
            address: address,
            phoneNumber: phone,
            birthdate: birthdate,
          })
          .then((response) => {
            const body = response.body;
            resolve(body)
          })
          .catch(reason => {
            reject(reason);
          })
      });
    },
    signout(context, payload) {
      console.debug('signout', payload);
      return Promise.resolve();
    }
  }
}
