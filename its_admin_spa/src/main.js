// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import store from "./store"

const vn = {
  dataIterator: {
    rowsPerPageText: 'Vật phẩm mỗi trang:',
    rowsPerPageAll: 'Toàn bộ',
    pageText: '{0}-{1} trên {2}',
    noResultsText: 'Không có bản ghi phù hợp',
    nextPage: 'Trang tiếp',
    prevPage: 'Trang trước'
  },
  dataTable: {
    rowsPerPageText: 'Bản ghi mỗi trang:'
  },
  noDataText: 'Không có dữ liệu'
};

// noinspection JSUnresolvedFunction
Vue.use(Vuetify,{
  lang: {
    locales: { vn },
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
