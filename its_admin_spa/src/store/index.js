import Vuex from "vuex"
import Vue from 'vue'
import AuthenticateModule from "./modules/authenticate"
import AccountModule from "./modules/account"
Vue.use(Vuex);

const store = new Vuex.Store({
  modules:{
    authenticate: AuthenticateModule,
    account: AccountModule
  }
});


export default store
