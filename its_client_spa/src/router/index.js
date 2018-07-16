import Vue from 'vue'
import Router from 'vue-router'
import LandingView from '../components/LandingView'
import MyPlanListView from '../components/plan/MyPlanListView'
import GroupListView from '../components/group/GroupListView'
import PersonalView from '../components/account/PersonalView'
import NotificationView from '../components/notification/NotificationView'
import LocationDetailView from '../components/location/LocationDetailView'
import ReviewWritingView from '../components/location/ReviewWritingView'
import SigninView from '../components/account/SigninView'
import SignupView from '../components/account/SignupView'
import SmartSearchView from '../components/search/SmartSearchView'
import SmartSearchResultView from '../components/search/SearchResultView'

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
      path: '/group/list',
      name: 'GroupList',
      component: GroupListView
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
      path: '/location/review',
      name: 'ReviewWriting',
      component: ReviewWritingView
    },
    {
      path: '/search',
      name: 'SmartSearch',
      component: SmartSearchView
    },
    {
      path: '/search/result',
      name: 'SmartSearchResult',
      component: SmartSearchResultView
    }
  ]
})
