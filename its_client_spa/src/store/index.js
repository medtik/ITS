import Vue from "vue" ;
import Vuex from "vuex";
import moment from "moment";

Vue.use(Vuex);

import LocationModule from "./modules/location";
import AccountModule from "./modules/account";
import SearchModule from "./modules/search";

const store = new Vuex.Store({
  modules:{
    account: AccountModule,
    location: LocationModule,
    search: SearchModule,
  },
});

if (typeof(Storage) !== "undefined") {
  const tokenStr = localStorage.getItem('token');
  if(tokenStr){
    const token = JSON.parse(tokenStr);
    let expire = moment(token.expires);
    let now = moment();
    if(now.isBefore(expire)){
      store.commit('account/setToken',token);
    }
  }
}
export default store;
