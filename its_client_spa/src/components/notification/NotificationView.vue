<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Thông báo"
    />
    <v-container class="text-xs-center" v-if="pageLoading">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
    <v-container v-else fluid>
      <v-layout row class="white" justify-center>
        <v-flex v-if="notifications" lg6 xs12>
          <div class="title">
            Lời mời vào nhóm
          </div>
          <v-list two-line>
            <v-list-tile v-for="item of notifications">
              <v-list-tile-content>
                <v-list-tile-title>
                  {{item.title}}&nbsp;<a>{{item.data.groupName}}</a>
                </v-list-tile-title>
                <v-list-tile-sub-title>
                  {{item.message}}
                </v-list-tile-sub-title>
              </v-list-tile-content>
              <v-list-tile-action>
                <v-layout row justify-center>
                  <template v-if="item.status == 0">
                    <v-flex pa-2>
                      <v-btn color="success" icon flat
                             @click="onAcceptGroupInvitation(item.id)">
                        <v-icon> fas fa-check</v-icon>
                      </v-btn>
                    </v-flex>
                    <v-flex pa-2>
                      <v-btn color=secondary icon flat
                             @click="onDenyGroupInvitation(item.id)">
                        <v-icon>fas fa-times</v-icon>
                      </v-btn>
                    </v-flex>
                  </template>
                  <template v-else-if="item.status == 1">
                    <v-chip color="green">
                      {{item.statusText}}
                    </v-chip>
                  </template>
                  <template v-else-if="item.status == 2">
                    <v-chip color="red" class="white--text">
                      {{item.statusText}}
                    </v-chip>
                  </template>
                </v-layout>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
        </v-flex>
      </v-layout>
    </v-container>
    <v-flex style="height: 25vh">
      <!--Holder-->
    </v-flex>
  </v-content>
</template>

<script>
  import ParallaxHeader from "../../common/layout/ParallaxHeader"
  import {mapGetters} from "vuex"

  export default {
    name: "NotificationView",
    components: {
      ParallaxHeader
    },
    computed: {
      ...mapGetters('request', {
        notifications: 'notifications',
        pageLoading: 'notificationsLoading'
      })
    },
    mounted() {
      this.$store.dispatch('request/fetchNotifications')
    },
    methods: {
      onAcceptGroupInvitation(id) {
        this.$store.dispatch('request/acceptGroupInvitation', {id});
      },
      onDenyGroupInvitation(id) {
        this.$store.dispatch('request/denyGroupInvitation', {id});
      }
    }

  }
</script>

<style scoped>

</style>
