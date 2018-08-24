import axiosInstance from "../util/axiosInstance";
import RNMsgChannel from 'react-native-webview-messaging';
import moment from "moment";
import Raven from "raven-js"

export default {
  namespaced: true,
  state: {
    facebookAppId: "266318357470729",
    token: undefined,
    facebookStatus: undefined,
    facebookInstance: undefined
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
          role: token.role,
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
    setFacebookAuthentication(state, payload){
      state.facebookStatus = payload.status;
    },
    setFacebookInstance(state, payload){
      state.facebookInstance = payload.instance;
    }
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
              const responseObj = JSON.parse(this.responseText);
              resolve(responseObj);
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
    },
    getTokenUsingFacebook (context, payload){
      const {
        status,
        authResponse
      } = payload.response;

      if(status == "connected"){
        axiosInstance.post('', {
          accessToken: authResponse.accessToken,
          useId: authResponse.userID,
          appId: context.state.facebookAppId
        })
      }
    },
    signinFacebook(context, payload){
      const FB = context.state.facebookInstance;

      FB.login(function(response){
        Raven.captureMessage("FB login status - after logged in", {
          extra:{
            response
          }
        });
        if(response.status != 'unknown'){
          //reload or redirect once logged in...
          window.location.reload();
        }
      });
    },
    logout(context){
      const {
        facebookInstance
      } = context.state;

      if(context.state.facebookInstance){
        facebookInstance.logout();
      }
    }
  }
}
