import router from "./routes";
import store from "../store";
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
      if(!searchResult){
        return {
          name: 'SmartSearch'
        };
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
    next({
      name: 'Signin'
    });
    return;
  }

  let dataRedirect = getDataRedirect(to);
  if(dataRedirect){
    next(dataRedirect);
  }else{
    next();
  }
});
