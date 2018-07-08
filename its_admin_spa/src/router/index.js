import Vue from 'vue'
import Router from 'vue-router'
import SigninView from '../components/SigninView'
import AccountListView from '../components/Account/AccountListView'
import AccountCreateEditView from '../components/Account/AccountCreateEditView'
import QuestionListView from '../components/Question/QuestionListView'
import QuestionCreateEditView from '../components/Question/QuestionCreateEditView'
import TagListView from '../components/Tag/TagListView'
import LocationListView from '../components/Location/LocationListView'
import LocationCreateEditView from '../components/Location/LocationCreateEditView'
import AreaListView from '../components/Area/AreaListView'
import AreaCreateEditView from '../components/Area/AreaCreateEditView'
import RequestListView from '../components/Request/RequestListView'

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Signin',
      component: SigninView
    },
    {
      path: '/account/list',
      name: 'AccountList',
      component: AccountListView
    },
    {
      path: '/account/create',
      name: 'AccountCreate',
      component: AccountCreateEditView
    },
    {
      path: '/account/edit',
      name: 'AccountEdit',
      component: AccountCreateEditView
    },
    {
      path: '/question/list',
      name: 'QuestionList',
      component: QuestionListView
    },
    {
      path: '/question/create',
      name: 'QuestionCreate',
      component: QuestionCreateEditView
    },
    {
      path: '/question/edit',
      name: 'QuestionEdit',
      component: QuestionCreateEditView
    },
    {
      path: '/tag/list',
      name: 'TagList',
      component: TagListView
    },
    {
      path: '/location/list',
      name: 'LocationList',
      component: LocationListView
    },
    {
      path: '/location/create',
      name: 'LocationCreate',
      component: LocationCreateEditView
    },
    {
      path: '/location/edit',
      name: 'LocationEdit',
      component: LocationCreateEditView
    },
    {
      path: '/area/list',
      name: 'AreaList',
      component: AreaListView
    },
    {
      path: '/area/create',
      name: 'AreaCreate',
      component: AreaCreateEditView
    },
    {
      path: '/area/edit',
      name: 'AreaEdit',
      component: AreaCreateEditView
    },
    {
      path: '/request/list',
      name: 'RequestList',
      component: RequestListView
    }
  ]
})
