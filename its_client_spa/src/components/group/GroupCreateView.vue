<template>
  <v-content>
    <v-toolbar color="light-blue" dark flat>
      <v-toolbar-title>
        Tạo nhóm
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column my-3>
      <v-flex mx-3>
        <v-text-field label="Tên nhóm" v-model="input.name"/>
        <v-btn color="success"
               :loading="loading"
               @click="createGroup">
          Tạo
        </v-btn>
        <v-btn color="secondary"
               @click="$router.back()">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>
<script>
  import {mapGetters} from "vuex";

  export default {
    name: "CreateGroupView",
    data(){
      return {
        input:{
          name
        }
      }
    },
    computed:{
      ...mapGetters('group', {
        loading: 'createLoading'
      })
    },
    methods:{
      createGroup(){
        this.$store.dispatch('group/create',{
          ...this.input
        })
          .then(value => {
            this.$router.back();
          })
      }
    }
  }
</script>

<style scoped>

</style>
