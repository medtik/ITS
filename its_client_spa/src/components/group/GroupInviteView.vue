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
                      v-model="nameInput"
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
              <v-btn flat color="success" @click="onInviteUserClick(user.id)">
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
    <ErrorDialog v-bind="errorDialog"
                 @close="errorDialog.dialog = false"
    ></ErrorDialog>
    <MessageInputDialog v-bind="messageInputDialog"
                        @confirm="onInviteUserConfirm"
                        v-model="inviteInput.message"
    ></MessageInputDialog>
  </v-content>
</template>

<script>
  import {mapGetters, mapState} from "vuex"
  import _ from "lodash";
  import {ErrorDialog} from "../../common/block";

  import {MessageInputDialog} from "../../common/input";

  export default {
    name: "GroupInviteView",
    components: {
      MessageInputDialog,
      ErrorDialog
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
        messageInputDialog: {
          dialog: false
        },
        errorDialog: {
          dialog: false,
          message: ''
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
        let invitee = _.find(this.users, (user) => {
          return user.id == userId;
        });

        this.messageInputDialog = {
          dialog: true,
          title: `Mời ${invitee.name} vào ${this.groupName}`,
          message: 'Để lại lời nhắn'
        };
      },
      onInviteUserConfirm() {
        this.messageInputDialog = _.assign(this.messageInputDialog, {
          dialog: false
        });
        this.sendInvitation();
      },
      sendInvitation() {
        this.$store.dispatch('group/sendGroupInvitationRequest', {
          ...this.inviteInput,
          groupId: this.groupId
        }).catch(response => {
          if (response.status == 400) {
            if (response.data.message) {
              this.errorDialog = {
                dialog: true,
                message: response.data.message
              }
            }
          }
        })
      }
    }
  }
</script>

<style scoped>

</style>
