import Vue from "vue" ;
import Vuex from "vuex";

Vue.use(Vuex);

import LocationModule from "./modules/location";
import AccountModule from "./modules/account";
import SmartSearchModule from "./modules/smartSearch";
import AuthenticateModule from "./modules/authenticate";
import AreaModule from "./modules/area";
import SearchModule from "./modules/search";

const store = new Vuex.Store({
  modules:{
    account: AccountModule,
    authenticate: AuthenticateModule,
    location: LocationModule,
    smartSearch: SmartSearchModule,
    area: AreaModule,
    search: SearchModule
  },
});

export default store;
