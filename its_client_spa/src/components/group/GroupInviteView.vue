<template>
  <v-content>
    <v-toolbar dark flat color="light-blue">
      Mời vào nhóm ABC
    </v-toolbar>
    <v-container fluid>
      <v-layout column>
        <v-text-field label="Tìm"/>
        <v-list>
          <v-list-tile v-for="user in users"
                       :key="user.id"
                       @click="">
            <v-list-tile-avatar>
              <img :src="user.photo"/>
            </v-list-tile-avatar>
            <v-list-tile-content>
              {{user.name}}
            </v-list-tile-content>
            <v-list-tile-action>
              <v-btn flat color="success">
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
  export default {
    name: "GroupInviteView",
    data() {
      return {
        users: [
          {id: 1, name: 'Ai đó', photo: 'https://picsum.photos/100'},
          {id: 2, name: 'Ai đó 2', photo: 'https://picsum.photos/100'}
        ]
      }
    }
  }
</script>

<style scoped>

</style>
