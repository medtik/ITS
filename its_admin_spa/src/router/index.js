import Vue from 'vue'
import Router from 'vue-router'
import SigninView from '../components/SigninView'

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Signin',
      component: SigninView
    }
  ]
})
