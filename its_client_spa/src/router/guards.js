import router from "./routes";
import store from "../store";
import Raven from "raven-js";

import _ from "lodash";

//PlanDetail
const requireLoggedInRoute = [
  'MyPlanList',
  'PlanCreate',
  'PlanEdit',

  'GroupList',
  'GroupCreate',
  'GroupDetail',

  'Personal',
  'Notification',
  'ReviewWrite',
  'ReviewReport',
  'LocationChangeRequest',
  'LocationClaimRequest',
  'GroupDetail',
  'GroupInvite'
];

function getDataRedirect(to) {
  switch (to.name) {
    case 'SmartSearchResult': {
      const searchResult = store.getters['smartSearch/searchResult'];
      if (!searchResult) {
        return {
          name: 'SmartSearch'
        };
      }
      break;
    }
    case 'NearbyLocationList':{
      const {
        long,
        lat
      }= to.query;

      if(long == null || lat == null){
        Raven.captureException(new Error('Missing long, lat on NearbyLocationList'))
      }
    }

  }
}

function isAuthenticateAllow(to) {
  const isLoggedIn = store.getters['authenticate/isLoggedIn'];

  if (isLoggedIn) {
    return true;
  } else {
    return !_.includes(requireLoggedInRoute, to.name);
  }
}

router.beforeEach((to, from, next) => {
  if (!isAuthenticateAllow(to)) {
    store.commit('signinContext', {
      context: {
        returnRoute: to
      }
    });
    next({
      name: 'Signin'
    });
    return;
  }

  let dataRedirect = getDataRedirect(to);
  if (dataRedirect) {
    next(dataRedirect);
  } else {
    next();
  }
});
