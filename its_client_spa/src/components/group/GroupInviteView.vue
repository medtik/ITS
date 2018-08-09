<template>
  <v-content>
    <v-toolbar dark flat color="light-blue darken-2">
      <span class="title">
        Mời vào {{groupName}}
      </span>
    </v-toolbar>
    <v-container fluid>
      <v-layout column>
        <v-text-field :loading="usersLoading"
                      label="Tìm"
                      v-on:keyup.enter="onSearchEnter"/>
        <v-list>
          <v-list-tile v-for="user in users"
                       :key="user.id"
                       @click="">
            <v-list-tile-avatar>
              <img :src="user.avatar"/>
            </v-list-tile-avatar>
            <v-list-tile-content>
              {{user.name}}
            </v-list-tile-content>
            <v-list-tile-action>
              <v-btn flat color="success" @click="onInviteUserClick">
                <v-icon small>
                  fas fa-user-plus
                </v-icon>
              </v-btn>
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
      </v-layout>
    </v-container>
    <!--DIALOG-->
    <MessageInputDialog :dialog="dialog.messageInputDialog"
                        title="Mời bạn"
                        message="Để lại lời nhắn"
                        @confirm="onInviteUserConfirm"
                        v-model="inviteInput.message"
    ></MessageInputDialog>
  </v-content>
</template>

<script>
  import {mapGetters, mapState} from "vuex"
  import {MessageInputDialog} from "../../common/input";

  export default {
    name: "GroupInviteView",
    components: {
      MessageInputDialog
    },
    data() {
      return {
        groupName: '',
        groupId: '',
        nameInput: '',
        inviteInput: {
          inviteUserId: undefined,
          message: undefined
        },
        dialog: {
          messageInputDialog: false,
        }
      }
    },
    computed: {
      ...mapGetters('user', {
        usersLoading: 'getSearchUsersLoading'
      }),
      ...mapState('user', {
        users: 'searchUsers'
      })
    },
    created() {
      const {
        groupId,
        groupName
      } = this.$route.query;

      this.groupName = groupName;
      this.groupId = groupId;
    },
    methods: {
      onSearchEnter() {
        this.$store.dispatch('user/fetchUsers', {nameInput: this.nameInput});
      },
      onInviteUserClick(userId) {
        this.inviteInput.userId = userId;
      },
      onInviteUserConfirm() {
        this.sendInvitation();
      },
      sendInvitation() {
        this.$store.dispatch('group/sendGroupInvitationRequest', {
          ...this.inviteInput,
          groupId: this.groupId
        })
      }
    }
  }
</script>

<style scoped>

</style>
