<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Các nhóm của bạn"
    />

    <v-layout column class="grey lighten-4">
      <v-flex class="text-xs-right">
        <v-btn color="success" :to="{name:'GroupCreate'}">
          <v-icon>group_add</v-icon>&nbsp; Tạo nhóm
        </v-btn>
      </v-flex>
      <v-container class="text-xs-center" v-if="myGroupsLoading">
        <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
      </v-container>
      <v-flex v-else-if="groups && groups.length > 0"
        v-for="group in groups"
              :key="group.id"
              elevation-2
              mt-2>
        <GroupFullWidth v-bind="group"
                        @delete="deleteGroup"/>
      </v-flex>
      <v-flex v-else="group" class="title text-xs-center grey lighten-4" py-4>
        <span>Bạn chưa có nhóm nào</span>
      </v-flex>
      <v-flex style="height: 25vh">
        <!--Holder-->
      </v-flex>
    </v-layout>

  </v-content>
</template>

<script>
  import ParallaxHeader from "../../common/layout/ParallaxHeader"
  import GroupFullWidth from "./GroupFullWidth"
  import {mapGetters} from "vuex";

  export default {
    name: "GroupListView",
    components: {
      ParallaxHeader,
      GroupFullWidth
    },
    computed: {
      ...mapGetters('group', {
        groups: 'myGroups',
        myGroupsLoading: 'myGroupsLoading'
      })
    },
    mounted() {
      this.$store.dispatch('group/fetchMyGroups')
    },
    methods: {
      deleteGroup(id) {
        this.$store.dispatch('group/delete', {id})
          .then(() => {
            this.$store.dispatch('group/fetchMyGroups')
          })
      }
    }
  }
</script>

<style scoped>

</style>
