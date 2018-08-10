<template>
  <v-app id="v-app">
    <router-view/>
    <v-bottom-nav
      v-show="showAppBar"
      :value="true"
      fixed
      shift
      color="light-blue darken-1">

      <v-btn v-for="nav in navigation"
             dark
             flat
             exact
             :to="nav.to"
             :value="nav.text"
             :key="nav.text">
        <span>{{nav.text}}</span>
        <v-icon>{{nav.icon}}</v-icon>
      </v-btn>
    </v-bottom-nav>
  </v-app>
</template>

<script>

  export default {
    name: 'App',
    data() {
      return {
        navigation: [
          {text: 'Trang chính', icon: 'home', to: {name: 'Home'}},
          {text: 'Chuyến đi', icon: 'map', to: {name: 'MyPlanList'}},
          {text: 'Nhóm', icon: 'group', to: {name: 'GroupList'}},
          {text: 'Thông báo', icon: 'notifications', to: {name: 'Notification'}},
          {text: 'Cá nhân', icon: 'person', to: {name: 'Personal'}},
        ]
      }
    },
    mounted(){

    },
    computed: {
      showAppBar() {
        switch (this.$route.name) {
          case 'Signup':
          case 'Signin':
            return false;
          default:
            return true;
        }
      }
    }
  }
</script>

<style global>
  .fakeLink:hover {
    cursor: pointer;
  }
</style>
