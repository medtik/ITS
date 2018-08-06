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
import RequestModule from "./modules/request"
import {
  AuthenticateModule,
  TagDialogModule
} from "../common/store";


const store = new Vuex.Store({
  state: {
    searchContext: {
      plan: undefined,
      planDay: undefined,
      areaId: undefined,
      planName: undefined,
    },
    createPlanContext: {
      returnRoute: undefined,
    },
    previousSearchAreaId: undefined
  },
  getters: {
    searchContext(state) {
      return state.searchContext;
    },
    createPlanContext(state){
      return state.createPlanContext;
    },
    previousSearchAreaId(state){
      return state.previousSearchAreaId;
    }
  },
  mutations: {
    searchContext(state, payload) {
      state.searchContext = _.assign(state.searchContext, payload.context);
    },
    createPlanContext(state,payload){
      state.createPlanContext = _.assign(state.createPlanContext, payload.context);
    },
    previousSearchAreaId(state,payload){
      state.previousSearchAreaId = payload.areaId;
    },
    consumeSearchContext(state){
      state.searchContext = {};
    },
    consumeCreatePlanContext(state){
      state.createPlanContext = {};
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
    group: GroupModule,
    request: RequestModule
  },
});

export default store;
