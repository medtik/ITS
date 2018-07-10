<template>
  <v-app>
    <v-app></v-app>
    <v-bottom-nav
      :value="true"
      fixed
      shift
      color="transparent"
    >
      <v-btn v-for="nav in navigation"
             color="teal"
             flat
             :value="nav.text">
        <span>{{nav.text}}</span>
        <v-icon>{{nav.icon}}</v-icon>
      </v-btn>
    </v-bottom-nav>
    <v-content>
      <router-view/>
    </v-content>
  </v-app>
</template>

<script>
  export default {
    data() {
      return {
        navigation: [
          {text: 'Recent', icon: 'history', to: undefined},
          {text: 'Favorites', icon: 'favorite', to: undefined},
          {text: 'Nearby', icon: 'place', to: undefined},
        ]
      }
    },
    name: 'App'
  }
</script>
