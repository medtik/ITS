import axios from "axios";
import axiosInstance from "../../axiosInstance";
import moment from "moment";
import MockAdapter from "axios-mock-adapter";

// const mock = new MockAdapter(axios, {delayResponse: 1500});
//
// mock.onPost('/token').reply(200, {
//   access_token: 'access_token',
//   token_type: 'bearer',
//   expires_in: '86399',
//   '.issued': 'Tue, 17 Jul 2018 08:42:56 GMT',
//   '.expires': 'Wed, 17 Jul 2019 08:42:56 GMT'
// });

export default {
  namespaced: true,
  state: {
    token: undefined,
  },
  getters: {
    isLoggedIn(state) {
      return !!state.token;
    },
    authorizeHeader(state, getters) {
      if (getters.isLoggedIn) {
        return `${state.token.token_type} ${state.token.access_token}`
      }
    },
    getlocalToken() {
      const tokenStr = localStorage.getItem('token');
      if (tokenStr) {
        const token = JSON.parse(tokenStr);
        let expire = moment(token.expires);
        let now = moment();

        if (now.isBefore(expire)) {
          return token;
        }
      }
    },
  },
  mutations: {
    setToken(state, payload) {
      const {
        token
      } = payload;

      let formattedToken;
      if (token.issued && token.expires) {
        //Formatted
        formattedToken = token;
      } else {
        formattedToken = {
          access_token: token.access_token,
          token_type: token.token_type,
          expires_in: token.expires_in,
          issued: token['.issued'],
          expires: token['.expires'],
        }
      }
      state.token = formattedToken;
      localStorage.setItem('token', JSON.stringify(formattedToken));
      axiosInstance.defaults.headers.common['Authorization'] =
        `${state.token.token_type} ${state.token.access_token}`;
    },
    nullToken(state) {
      state.token = undefined;
      localStorage.removeItem('token');
      axiosInstance.defaults.headers.common['Authorization'] = undefined;
    },
  },
  actions: {
    fetchToken(context, payload) {
      const {
        email,
        password
      } = payload;

      return new Promise((resolve, reject) => {
        axios({
          method: 'post',
          url: '/token',
          baseURL: 'http://itssolution.azurewebsites.net',
          headers: {
            'Content-type': 'text/plan'
          },
          data: `grant_type=password&username=${email}&password=${password}`
        })
          .then((response) => {
            resolve(response.data);
          })
          .catch(reason => {
            let message = 'Có lỗi xẩy ra';
            if (reason.response.status === 400) {
              message = 'Sai tên đăng nhập hoặc mật khẩu'
            }
            reject({
              ...reason.response,
              message
            });
          });
      });
    }
  }
}
