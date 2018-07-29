<template>
  <v-app id="v-app">
    <v-navigation-drawer app
                         :permanent="drawer"
                         v-if="navigateAble"
                         width="200"
                         v-model="drawer">
      <v-list>
        <v-list-group v-for="group in navigation" :key="group.text">
          <v-list-tile slot="activator">
            <v-list-tile-action>
              <v-icon>{{group.icon}}</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>{{group.text}}</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
          <v-list-tile v-for="item in group.items" :to="item.to" :key="item.text">
            <v-list-tile-action>
              <v-icon>{{item.icon}}</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title v-text="item.text"></v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </v-list-group>
      </v-list>
    </v-navigation-drawer>
    <v-toolbar app dark v-if="navigateAble" style="background-color: #03a9f4">
      <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
      <v-toolbar-title v-text="title"></v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-content>
      <router-view :key="$route.fullPath"/>
    </v-content>
    <v-footer app>
      <span>ITS &copy; 2018</span>
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
        title: 'ITS',
        navigation: [
          {
            text: 'Tài khoản',
            icon: 'fas fa-user',
            items: [
              {
                text: 'Danh sách',
                icon: 'fas fa-bars',
                to: {name: 'AccountList'}
              },
              {
                text: 'Tạo mới',
                icon: 'fas fa-plus',
                to: {name: 'AccountCreate'}
              }
            ]
          },
          {
            text: 'Câu hỏi',
            icon: 'fas fa-question',
            items: [
              {
                text: 'Danh sách',
                icon: 'fas fa-bars',
                to: {name: 'QuestionList'}
              },
              {
                text: 'Tạo mới',
                icon: 'fas fa-plus',
                to: {name: 'QuestionCreate'}
              }
            ]
          },
          {
            text: 'Thẻ',
            icon: 'fas fa-tag',
            items: [
              {
                text: 'Danh sách',
                icon: 'fas fa-bars',
                to: {name: 'TagList'}
              }
            ]
          },
          {
            text: 'Địa điểm',
            icon: 'fas fa-map-marked-alt',
            items: [
              {
                text: 'Danh sách',
                icon: 'fas fa-bars',
                to: {name: 'LocationList'}
              },
              {
                text: 'Tạo mới',
                icon: 'fas fa-plus',
                to: {name: 'LocationCreate'}
              }
            ]
          },
          {
            text: 'Khu vực',
            icon: 'fas fa-map',
            items: [
              {
                text: 'Danh sách',
                icon: 'fas fa-bars',
                to: {name: 'AreaList'}
              },
              {
                text: 'Tạo mới',
                icon: 'fas fa-plus',
                to: {name: 'AreaCreate'}
              }
            ]
          },
          {
            text: 'Yêu cầu',
            icon: 'fas fa-ticket-alt',
            items: [
              {
                text: 'Danh sách',
                icon: 'fas fa-bars',
                to: {name: 'RequestList'}
              }
            ]
          }
        ]
      }
    },
    created() {
      this.navigateAble = this.$route.name !== "Signin"
    },
    watch: {
      $route() {
        this.navigateAble = this.$route.name !== "Signin"
      }
    },
    name: 'App'
  }
</script>

<style>
  #v-app {
    background: url("assets/budapest-hungary-magyarorszag-tajkepek-fotos-fenykepesz-fotograf-budapesta-stock-foto-kirandulas-travel-nature-photography-affarit-andras-ferencz-marosvasarhely-kolozsvar-1.jpg") center bottom;
  }

  #content {
    background-color: whitesmoke;
  }

  .md-column {
    min-width: 15rem;
  }

  .lg-column {
    min-width: 25rem;
  }
</style>
