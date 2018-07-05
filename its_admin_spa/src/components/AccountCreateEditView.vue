<template>
  <v-container id="content" fluid>
    <v-layout row pa-3>
      <v-flex xs12>
        <span class=title>Account create edit page</span>

      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  export default {
    name: "AccountCreateEditView",
    data() {
      return {
        mode: 'create',
        accountId: ''
      }
    },
    created() {
      if (this.$route.name === 'AccountEdit') {
        if (this.$route.query) {
          this.accountId = this.$route.query.id;
          this.mode = 'edit'
        }else{
          //TODO some error here when no id / wrong id
        }
      }
    }
  }
</script>

<style scoped>

</style>
