<template>
  <v-content>
    <v-toolbar color="light-blue" dark flat>
      <v-toolbar-title>
        Tạo nhóm
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column my-3>
      <v-flex mx-3>
        <v-text-field label="Tên nhóm" v-model="input.name" :error="!!formError.name" :error-messages="formError.name"/>
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
        },
        formError:{
          name: undefined
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
        if(!!this.input.name){
          this.$store.dispatch('group/create',{
            ...this.input
          })
            .then(value => {
              this.$router.back();
            })
        }else{
          this.formError = {
            name: "Tên không được trống"
          }
        }
      }
    }
  }
</script>

<style scoped>

</style>
