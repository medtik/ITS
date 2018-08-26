<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Thông báo"
    />
    <v-container class="text-xs-center" v-if="pageLoading">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
    <v-container v-else fluid class="white">
      <v-layout row justify-center wrap>
        <v-flex v-if="groupInvitation" lg5 xs12 pa-1>
          <div class="title">
            Lời mời vào nhóm
          </div>
          <v-list two-line>
            <v-list-tile v-for="item of groupInvitation" :key="item.key">
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
                    <v-chip color="green" class="white--text">
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
        <v-flex shrink>
          <v-divider vertical></v-divider>
        </v-flex>
        <v-flex lg6 xs12 pa-1>
          <div class="title">
            Đề nghị thêm địa điểm
          </div>
          <v-list>
            <v-list two-line>
              <v-list-tile v-for="item of locationSuggestions" :key="item.key">
                <v-list-tile-content>
                  <v-list-tile-title>
                    <!--{{item.title}}&nbsp;<a>{{item.data.groupName}}</a>-->
                    {{item.title}}
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
                               @click="onAcceptLocationSuggestion(item.id)">
                          <v-icon> fas fa-check</v-icon>
                        </v-btn>
                      </v-flex>
                      <v-flex pa-2>
                        <v-btn color=secondary icon flat
                               @click="onDenyLocationSuggestion(item.id)">
                          <v-icon>fas fa-times</v-icon>
                        </v-btn>
                      </v-flex>
                    </template>
                    <template v-else-if="item.status == 1">
                      <v-chip color="green" class="white--text">
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
        groupInvitation: 'groupInvitations',
        locationSuggestions: 'locationSuggestions',
        pageLoading: 'notificationsLoading'
      })
    },
    mounted() {
      this.$store.dispatch('request/fetchNotifications')
    },
    methods: {
      onAcceptGroupInvitation(id) {
        this.$store.commit('request/changeStatusGroupInvitation', {
          id,
          status: 1
        });
        this.$store.dispatch('request/acceptGroupInvitation', {id});
      },
      onDenyGroupInvitation(id) {
        this.$store.commit('request/changeStatusGroupInvitation', {
          id,
          status: 2
        });
        this.$store.dispatch('request/denyGroupInvitation', {id});
      },

      onAcceptLocationSuggestion(id) {
        this.$store.commit('request/changeStatusLocationSuggestion', {
          id,
          status: 1
        });
        this.$store.dispatch('request/acceptLocationSuggestion', {id});
      },
      onDenyLocationSuggestion(id) {
        this.$store.commit('request/changeStatusLocationSuggestion', {
          id,
          status: 2
        });
        this.$store.dispatch('request/denyLocationSuggestion', {id});
      }
    }

  }
</script>

<style scoped>

</style>
