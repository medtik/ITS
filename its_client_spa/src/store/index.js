import Vue from "vue" ;
import Vuex from "vuex";

Vue.use(Vuex);

import LocationModule from "./modules/location";
import AccountModule from "./modules/account";
import SmartSearchModule from "./modules/smartSearch";
import AreaModule from "./modules/area";
import SearchModule from "./modules/search";
import TagModule from "./modules/tag"
import PlanModule from "./modules/plan"
import GroupModule from "./modules/group"
import {
  AuthenticateModule,
  TagDialogModule
} from "../common/store";


const store = new Vuex.Store({
  state:{
    searchContext:{
      plan: undefined,
      planDay: undefined,
      area: undefined,
    }
  },
  getters:{
    searchContext(state){
      return state.searchContext;
    }
  },
  mutations:{
    searchContext(state,payload){
      state.searchContext = _.assign(state.searchContext, payload.context);
    }
  },
  modules: {
    account: AccountModule,
    authenticate: AuthenticateModule,
    location: LocationModule,
    smartSearch: SmartSearchModule,
    area: AreaModule,
    search: SearchModule,
    tag: TagModule,
    tagDialog: TagDialogModule,
    plan: PlanModule,
    group: GroupModule
  },
});

export default store;
