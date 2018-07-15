import Vue from "vue" ;
import Vuex from "vuex";

Vue.use(Vuex);


import LocationModule from "./modules/location";
import AccountModule from "./modules/account";

const store = new Vuex.Store({
  modules:{
    account: AccountModule,
    location: LocationModule,
  }
});

export default store;
