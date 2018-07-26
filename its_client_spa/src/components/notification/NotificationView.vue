<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Thông báo"
    />
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
                  <v-btn color="green" dark small>
                    <v-icon>check</v-icon>
                  </v-btn>
                  <v-btn small>
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
  </v-content>
</template>

<script>
  import ParallaxHeader from "../../sharedComponents/layout/ParallaxHeader"
  import NotificationFullWidth from "../../sharedComponents/block/NotificationFullWidth"

  export default {
    name: "NotificationView",
    components: {
      ParallaxHeader,
      NotificationFullWidth
    },
    data() {
      return {
        notifications: [
          {
            id: 1,
            message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam"
          },
          {
            id: 2,
            message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam"
          },
          {
            id: 3,
            message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam"
          },
          {
            id: 4,
            message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam"
          },
          {
            id: 5,
            message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam"
          },
          {
            id: 6,
            message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam"
          },
        ]
      }
    }
  }
</script>

<style scoped>

</style>
