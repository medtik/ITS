import axios from "axios";
import axiosInstance from "../util/axiosInstance";
import moment from "moment";

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
        const xhttp = new XMLHttpRequest();
        xhttp.open('POST', 'http://itssolution.azurewebsites.net/token', true);
        xhttp.send(`grant_type=password&username=${email}&password=${password}`);
        xhttp.onreadystatechange = function () {
          if (this.readyState == 4) {
            if (this.status == 200) {
              resolve(this.responseText);
            } else {
              let message = 'Có lỗi xẩy ra';
              if (this.status === 400) {
                message = 'Sai tên đăng nhập hoặc mật khẩu'
              }
              reject({
                message
              });
            }
          }
        };
      });
    }
  }
}
