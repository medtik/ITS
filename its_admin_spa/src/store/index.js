import Vuex from "vuex"
import Vue from 'vue'
import {
  AuthenticateModule,
  TagDialogModule
} from "../common/store"
import AccountModule from "./modules/account"
import QuestionModule from "./modules/question"
import TagModule from "./modules/tag"
import LocationModule from "./modules/location"
import AreaModule from "./modules/area"
import RequestModule from "./modules/request"

Vue.use(Vuex);

const store = new Vuex.Store({
  modules: {
    authenticate: AuthenticateModule,
    account: AccountModule,
    question: QuestionModule,
    tag: TagModule,
    location: LocationModule,
    area: AreaModule,
    request: RequestModule,
    tagDialog: TagDialogModule
  }
});


export default store
