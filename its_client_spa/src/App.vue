<template>
  <v-app id="v-app">
    <v-content>
      <router-view/>
    </v-content>
    <v-bottom-nav
      :value="true"
      fixed
      shift
      color="transparent"
    >
      <v-btn v-for="nav in navigation"
             color="teal"
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
    data() {
      return {
        navigation: [
          {text: 'Trang chính', icon: 'history', to: {name: 'Home'}},
          {text: 'Chuyến đi', icon: 'favorite', to: {name: 'MyPlanList'}},
          {text: 'Nhóm', icon: 'place', to: {name: 'GroupList'}},
          {text: 'Cá nhân', icon: 'place', to: {name: 'Personal'}},
        ]
      }
    },
    name: 'App'
  }
</script>
