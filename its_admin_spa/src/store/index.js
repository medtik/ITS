import Vuex from "vuex"
import Vue from 'vue'
import AuthenticateModule from "./modules/authenticate"
Vue.use(Vuex);

const store = new Vuex.Store({
  modules:{
    authenticate: AuthenticateModule
  }
});


export default store
