import axiosInstance from "../util/axiosInstance";
import RNMsgChannel from 'react-native-webview-messaging';
import moment from "moment";
import Raven from "raven-js"
import firebase from "firebase";

const root = "https://itssolutiong9.azurewebsites.net/";
// const root = "http://localhost:59728/";

var config = {
  apiKey: "AIzaSyCouzeKTc_xf3r7QJZjCjyEr7rceMB7rgA",
  authDomain: "its-g8.firebaseapp.com",
  databaseURL: "https://its-g8.firebaseio.com",
  projectId: "its-g8",
  storageBucket: "its-g8.appspot.com",
  messagingSenderId: "917708153355"
};

firebase.initializeApp(config);

var googleProvider = new firebase.auth.GoogleAuthProvider();
var facebookProvider = new firebase.auth.FacebookAuthProvider();





export default {
  namespaced: true,
  state: {
    facebookAppId: "266318357470729",
    token: undefined,
  },
  getters: {
    isLoggedIn(state) {
      return !!state.token;
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
    setFacebookAuthentication(state, payload) {
      state.facebookStatus = payload.status;
    },
    setFacebookInstance(state, payload) {
      state.facebookInstance = payload.instance;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
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
        xhttp.open('POST', `${root}/token`, true);
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
    signinFacebook() {
      // post /RegisterExternal
      firebase.auth().signInWithPopup(facebookProvider).then(function(result) {
        // This gives you a Facebook Access Token. You can use it to access the Facebook API.
        var token = result.credential.accessToken;
        // The signed-in user info.
        var user = result.user;


        axiosInstance.post('RegisterExternal',{
          ...user
        });

        console.debug(user);
        Raven.captureMessage("signinFacebook",{
          extra:{
            token,
            user,
          }
        })
      }).catch(function(error) {
        Raven.captureException(error);
      });
    },
    signinGoogle(){
      // post /RegisterExternal
      firebase.auth().signInWithPopup(googleProvider).then(function(result) {
        // This gives you a Facebook Access Token. You can use it to access the Facebook API.
        var token = result.credential.accessToken;
        var user = result.user;


        axiosInstance.post('RegisterExternal',{
          ...user
        });
        Raven.captureMessage("signinGoogle",{
          extra:{
            token,
            user,
          }
        });
        // ...
      }).catch(function(error) {
        Raven.captureException(error);
      });
    },
    logout(context) {
      context.dispatch('resetUserData',{}, {root: true});
      firebase.auth().signOut()
    }
  }
}
