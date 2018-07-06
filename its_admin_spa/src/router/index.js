import Vue from 'vue'
import Router from 'vue-router'
import SigninView from '../components/SigninView'
import AccountListView from '../components/Account/AccountListView'
import AccountCreateEditView from '../components/Account/AccountCreateEditView'

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
    }
  ]
})
