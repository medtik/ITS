<template>
  <router-link :to="routerLink" tag="div" class="fakeLink">
    <v-layout column pa-2 class="white">
      <v-flex py-2>
        <v-layout class="title" align-center justify-space-between>
          {{name}}
          <v-btn color="red" flat icon
                 @click.stop="$emit('delete',id)">
            <v-icon>
              fas fa-trash
            </v-icon>
          </v-btn>
        </v-layout>
      </v-flex>
      <v-divider></v-divider>
      <v-layout row wrap>
        <v-flex xs12 sm5 py-2 class="text-xs-center">
          <!--USERS-->
          <v-flex>
            <v-icon :size="45" color="primary">
              fas fa-users
            </v-icon>
          </v-flex>
          <v-flex class="subheading font-weight-medium">
            {{memberCount}} thành viên
          </v-flex>
        </v-flex>
        <v-flex d-flex justify-center>
          <v-divider vertical class="sm1"></v-divider>
        </v-flex>
        <v-flex xs12 sm5 py-2 flex class="text-xs-center">
          <!--TRIPS-->
          <v-flex>
            <v-icon :size="45" color="primary">
              fas fa-suitcase
            </v-icon>
          </v-flex>
          <v-flex class="subheading font-weight-medium">
            {{planCount}} chuyến đi
          </v-flex>
        </v-flex>
      </v-layout>
    </v-layout>
  </router-link>
</template>
<!--TODO More Info-->
<script>
  export default {
    name: "GroupFullWidth",
    props:[
      'id',
      'name',
      'memberCount',
      'planCount'
    ],
    computed: {
      routerLink() {
        return {
          name: "GroupDetail",
          params:{
            'id': this.id
          }
        }
      }
    },

  };
</script>

<style scoped>
</style>
