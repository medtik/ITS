<template>
  <v-app id="v-app">
    <v-navigation-drawer app
                         :permanent="drawer"
                         v-if="navigateAble"
                         width="200"
                         v-model="drawer">
      <v-list>
        <v-list-group>
          <v-list-tile slot="activator">
            <v-list-tile-action>
              <v-icon>fas fa-user</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>Tài khoản</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
          <v-list-tile :to="{name:'AccountList'}">
            <v-list-tile-action>
              <!--Empty-->
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title v-text="'Danh sách'"></v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
          <v-list-tile :to="{name:'AccountCreate'}">
            <v-list-tile-action>
              <!--Empty-->
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title v-text="'Tạo mới'"></v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </v-list-group>
      </v-list>
    </v-navigation-drawer>
    <v-toolbar app flat color="white" v-if="navigateAble">
      <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
      <v-toolbar-title v-text="title"></v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-content>
      <router-view/>
    </v-content>
    <v-footer app>
      <span>&copy; 2017</span>
    </v-footer>
  </v-app>
</template>

<script>
  export default {
    data() {
      return {
        navigateAble: true,
        drawer: true,
        right: true,
        title: 'ITS'
      }
    },
    created() {
      this.navigateAble = this.$route.name !== "Signin"
    },
    watch:{
      $route (to, from){
        this.navigateAble = this.$route.name !== "Signin"
      }
    },
    name: 'App'
  }
</script>

<style scoped>
  #v-app {
    background-image: url("assets/budapest-hungary-magyarorszag-tajkepek-fotos-fenykepesz-fotograf-budapesta-stock-foto-kirandulas-travel-nature-photography-affarit-andras-ferencz-marosvasarhely-kolozsvar-1.jpg");
    background-position: center bottom;
  }
  #content {
    background-color: whitesmoke;
  }
</style>
