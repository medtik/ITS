import Vuex from "vuex"
import Vue from 'vue'
import AuthenticateModule from "./modules/authenticate"
import AccountModule from "./modules/account"
import QuestionModule from "./modules/question"
import TagModule from "./modules/tag"
import LocationModule from "./modules/location"
Vue.use(Vuex);

const store = new Vuex.Store({
  modules:{
    authenticate: AuthenticateModule,
    account: AccountModule,
    question: QuestionModule,
    tag: TagModule,
    location: LocationModule,
  }
});


export default store
