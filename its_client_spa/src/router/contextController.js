import router from "./routes";
import store from "../store";


router.afterEach((to, from) => {
  if (from.name == 'SmartSearch') {
    if (to.name != 'SmartSearchResult') {
      store.commit('consumeSearchContext');
    }
  }

  if (from.name == 'Search') {
    store.commit('consumeSearchContext');
  }

  if (from.name == 'PlanCreate') {
    store.commit('consumeCreatePlanContext');
  }
});

router.afterEach(() => {

})
