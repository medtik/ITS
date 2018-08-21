// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import store from "./store"
import {VietnameseLocate} from "./common/util";

Vue.use(Vuetify,{
  lang: {
    locales: { VietnameseLocate },
    current: 'vn'
  }
});

const localToken = store.getters['authenticate/getlocalToken'];
if (localToken) {
  store.commit('authenticate/setToken', {token: localToken})
}

Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  components: { App },
  template: '<App/>'
});
