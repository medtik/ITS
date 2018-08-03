// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import store from './store'
import Vuetify from 'vuetify'
import * as VueGoogleMaps from 'vue2-google-maps'
import 'vuetify/dist/vuetify.min.css'
import '@fortawesome/fontawesome-free/css/all.css'


Vue.use(Vuetify, {
  iconfont: 'fa'
});
Vue.use(VueGoogleMaps, {
  load: {
    key: 'AIzaSyAxQfGcJgUa5iZSfXirufK8Lbb8i5qDnwQ',
    libraries: 'places, directions',
  }
});

const localToken = store.getters['authenticate/getlocalToken'];
if (localToken) {
  store.commit('authenticate/setToken', {token: localToken})
}

//--Store reliant
import "./router/guards";


Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  components: {App},
  template: '<App/>'
});
