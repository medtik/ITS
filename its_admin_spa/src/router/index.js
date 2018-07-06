import Vue from 'vue'
import Router from 'vue-router'
import SigninView from '../components/SigninView'
import AccountListView from '../components/Account/AccountListView'
import AccountCreateEditView from '../components/Account/AccountCreateEditView'
import QuestionListView from '../components/Question/QuestionListView'
import QuestionCreateEditView from '../components/Question/QuestionCreateEditView'

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
    },{
      path: '/question/create',
      name: 'QuestionCreate',
      component: QuestionCreateEditView
    },{
      path: '/question/edit',
      name: 'QuestionEdit',
      component: QuestionCreateEditView
    }


  ]
})
