import Vue from "vue" ;
import Vuex from "vuex";
import moment from "moment";

Vue.use(Vuex);

import LocationModule from "./modules/location";
import AccountModule from "./modules/account";
import SearchModule from "./modules/search";
import AuthenticateModule from "./modules/authenticate";

const store = new Vuex.Store({
  modules:{
    account: AccountModule,
    authenticate: AuthenticateModule,
    location: LocationModule,
    search: SearchModule,
  },
});

export default store;
