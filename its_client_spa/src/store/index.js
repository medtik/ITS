import Vue from "vue" ;
import Vuex from "vuex";

Vue.use(Vuex);


import LocationModule from "./modules/location";

const store = new Vuex.Store({
  modules:{
    location: LocationModule
  }
});

export default store;
