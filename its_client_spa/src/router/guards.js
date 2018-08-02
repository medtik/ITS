import router from "./index";
import store from "../store";
import _ from "lodash";

//PlanDetail
const requireLoggedInRoute = [
  'MyPlanList',
  'PlanCreate',
  'PlanEdit',
  'GroupList',
  'GroupCreate',
  'Personal',
  'Notification',
  'ReviewWrite',
  'ReviewReport',
  'LocationChangeRequest',
  'LocationClaimRequest',
  'GroupDetail',
  'GroupInvite'
];

function isAllow(to){
  const isLoggedIn = store.getters['authenticate/isLoggedIn'];

  if(isLoggedIn){
    return true;
  }else {
    return !_.includes(requireLoggedInRoute, to.name);
  }
}

router.beforeEach((to, from, next) => {
  if(isAllow(to)){
    next()
  }else{
    next({
      name: 'Signin'
    })
  }
});
