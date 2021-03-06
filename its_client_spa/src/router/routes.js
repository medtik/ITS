import Vue from 'vue'
import Router from 'vue-router'

import LandingView from '../components/LandingView'
import MyPlanListView from '../components/plan/MyPlanListView'
import GroupListView from '../components/group/GroupListView'
import PersonalView from '../components/account/PersonalView'
import NotificationView from '../components/notification/NotificationView'
import LocationDetailView from '../components/location/LocationDetailView'
import SigninView from '../components/account/SigninView'
import SignupView from '../components/account/SignupView'
import SmartSearchView from '../components/search/SmartSearchView'
import SmartSearchResultView from '../components/search/SmartSearchResultView'
import PlanDetailView from '../components/plan/PlanDetailView'
import LocationOnMapView from '../components/location/LocationOnMapView'
import AreaDetailView from '../components/area/AreaDetailView'
import SearchView from "../components/search/SearchView";
import WriteReviewView from "../components/location/WriteReviewView";
import ReportReviewView from "../components/location/ReportReviewView";
import PlanCreateView from "../components/plan/PlanCreateView";
import LocationChangeRequestView from "../components/location/LocationChangeRequestView";
import GroupDetailView from "../components/group/GroupDetailView";
import GroupCreateView from "../components/group/GroupCreateView";
import GroupInviteView from "../components/group/GroupInviteView";
import LocationClaimView from "../components/location/LocationClaimView";
import NearbyLocationList from "../components/location/NearbyLocation";
import PlanEditView from "../components/plan/PlanEditView"

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: LandingView
    },
    {
      path: '/signin',
      name: 'Signin',
      component: SigninView
    },
    {
      path: '/signup',
      name: 'Signup',
      component: SignupView
    },
    {
      path: '/plan/list',
      name: 'MyPlanList',
      component: MyPlanListView
    },
    {
      path: '/plan/detail',
      name: 'PlanDetail',
      component: PlanDetailView
    },
    {
      path: '/plan/create',
      name: 'PlanCreate',
      component: PlanCreateView
    },
    {
      path: '/plan/edit',
      name: 'PlanEdit',
      component: PlanEditView
    },
    {
      path: '/group/list',
      name: 'GroupList',
      component: GroupListView
    },
    {
      path: '/group/create',
      name: 'GroupCreate',
      component: GroupCreateView
    },
    {
      path: '/personal',
      name: 'Personal',
      component: PersonalView
    },
    {
      path: '/notification',
      name: 'Notification',
      component: NotificationView
    },
    {
      path: '/location/detail',
      name: 'LocationDetail',
      component: LocationDetailView
    },
    {
      path: '/smartsearch',
      name: 'SmartSearch',
      component: SmartSearchView
    },
    {
      path: '/smartsearch/result',
      name: 'SmartSearchResult',
      component: SmartSearchResultView
    },
    {
      path: '/search',
      name: 'Search',
      component: SearchView
    },
    {
      path: '/location/map',
      name: 'NearbyOnMap',
      component: LocationOnMapView
    },
    {
      path: '/review/write',
      name: 'ReviewWrite',
      component: WriteReviewView
    },
    {
      path: '/review/report',
      name: 'ReviewReport',
      component: ReportReviewView
    },
    {
      path: '/area/detail',
      name: 'AreaDetail',
      component: AreaDetailView
    },
    {
      path: '/location/changeRequest',
      name: 'LocationChangeRequest',
      component: LocationChangeRequestView
    },
    {
      path: '/location/claimOwner',
      name: 'LocationClaimRequest',
      component: LocationClaimView
    },
    {
      path: '/location/nearby',
      name: 'LocationNearbyList',
      component: NearbyLocationList
    },
    {
      path: '/group/detail',
      name: 'GroupDetail',
      component: GroupDetailView
    },
    {
      path: '/group/invite',
      name: 'GroupInvite',
      component: GroupInviteView
    }
  ]
})
