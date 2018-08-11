<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Thông báo"
    />
    <v-container class="text-xs-center" v-if="pageLoading">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
    <v-container v-else pa-0 ma-0 fluid>
      <v-layout column class="white">
        <v-flex>
          <v-layout row wrap>
            <v-flex v-for="noti in notifications"
                    :key="noti.id"
                    my-2
                    xs12 lg6 px-2>
              <NotificationFullWidth v-bind="noti">
                <template slot="actions">
                  <v-layout row justify-end>
                    <v-btn color="green" dark small @click="acceptNotification(noti.id)">
                      <v-icon>check</v-icon>
                    </v-btn>
                    <v-btn small @click="denyNotification(noti.id)">
                      <v-icon>close</v-icon>
                    </v-btn>
                  </v-layout>
                </template>
              </NotificationFullWidth>
            </v-flex>
          </v-layout>
        </v-flex>

        <v-flex style="height: 25vh">
          <!--Holder-->
        </v-flex>
      </v-layout>
    </v-container>
  </v-content>
</template>

<script>
  import ParallaxHeader from "../../common/layout/ParallaxHeader"
  import NotificationFullWidth from "../../common/block/NotificationFullWidth"
  import {mapGetters} from "vuex"

  export default {
    name: "NotificationView",
    components: {
      ParallaxHeader,
      NotificationFullWidth
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
      acceptNotification(id) {
        this.$store.dispatch('request/acceptGroupInvitation', {id});
      },
      denyNotification(id) {
        this.$store.dispatch('request/denyGroupInvitation', {id});
      }
    }

  }
</script>

<style scoped>

</style>
