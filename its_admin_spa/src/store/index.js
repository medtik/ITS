import Vuex from "vuex"
import Vue from 'vue'
import AuthenticateModule from "./modules/authenticate"
import AccountModule from "./modules/account"
import QuestionModule from "./modules/question"
import TagModule from "./modules/tag"
Vue.use(Vuex);

const store = new Vuex.Store({
  modules:{
    authenticate: AuthenticateModule,
    account: AccountModule,
    question: QuestionModule,
    tag: TagModule,
  }
});


export default store
