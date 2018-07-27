import axiosInstance from "../../common/util/axiosInstance";

export default {
  namespaced: true,
  state: {
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
        axiosInstance.post('/api/account', {
          email: email,
          password: password,
          rePassword: rePassword,
          name: name,
          address: address,
          phone: phone,
          birthdate: birthdate,
        })
          .then(value => resolve(value.data))
          .catch(reason => {
            const {
              data,
              status
            } = reason.response;

            let error = {};
            if(status === 400){
              error = {
                email: data.modelState['register.EmailAddress'] &&
                  data.modelState['register.EmailAddress'][0],
                password: data.modelState['register.Password'] &&
                  data.modelState['register.Password'][0],
                rePassword: data.modelState['register.RePassword'] &&
                  data.modelState['register.RePassword'][0],
                name: data.modelState['register.FullName'] &&
                  data.modelState['register.FullName'][0],
                address: data.modelState['register.Address'] &&
                  data.modelState['register.Address'][0],
                phone: data.modelState['register.PhoneNumber'] &&
                  data.modelState['register.PhoneNumber'][0],
                birthdate: data.modelState['register.Birthdate'] &&
                  data.modelState['register.Birthdate'][0]
              }
            }

            reject({
              data,
              status,
              error
            })
          })
      });
    },
  }
}
