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
              <v-btn flat color="success" @click="onInviteUser">
                <v-icon small>
                  fas fa-user-plus
                </v-icon>
              </v-btn>
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
      </v-layout>
    </v-container>
  </v-content>
</template>

<script>
  import {mapGetters} from "vuex"

  export default {
    name: "GroupInviteView",
    data() {
      return {
        groupName: '',
        groupId: '',
        nameInput: ''
      }
    },
    computed: {
      ...mapGetters('user', {
        users: 'getUsers',
        usersLoading: 'getUsersLoading'
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
        this.$store.dispatch('user/fetchUsers',{nameInput:this.nameInput});
      },
      onInviteUser(userId){

      }
    }
  }
</script>

<style scoped>

</style>
