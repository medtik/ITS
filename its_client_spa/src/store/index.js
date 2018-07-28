import Vue from "vue" ;
import Vuex from "vuex";

Vue.use(Vuex);

import LocationModule from "./modules/location";
import AccountModule from "./modules/account";
import SmartSearchModule from "./modules/smartSearch";
import AreaModule from "./modules/area";
import SearchModule from "./modules/search";
import TagModule from "./modules/tag"
import {
  AuthenticateModule,
  TagDialogModule
} from "../common/store";


const store = new Vuex.Store({
  modules: {
    account: AccountModule,
    authenticate: AuthenticateModule,
    location: LocationModule,
    smartSearch: SmartSearchModule,
    area: AreaModule,
    search: SearchModule,
    tag: TagModule,
    tagDialog: TagDialogModule
  },
});

export default store;
