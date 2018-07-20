import Vue from "vue" ;
import Vuex from "vuex";

Vue.use(Vuex);

import LocationModule from "./modules/location";
import AccountModule from "./modules/account";
import SearchModule from "./modules/search";
import AuthenticateModule from "./modules/authenticate";
import AreaModule from "./modules/area";

const store = new Vuex.Store({
  modules:{
    account: AccountModule,
    authenticate: AuthenticateModule,
    location: LocationModule,
    search: SearchModule,
    area: AreaModule,
  },
});

export default store;
