<template>
  <v-content>
    <v-toolbar flat color="light-blue" dark>
      <v-layout justify-center>
        <v-toolbar-title>
          {{plan.title}}
        </v-toolbar-title>
      </v-layout>
    </v-toolbar>

  </v-content>
</template>

<script>
  export default {
    name: "PlanDetailView",
    data() {
      return {
        plan: {
          title: 'Plan ABC'
        }
      }
    }
  }
</script>

<style scoped>

</style>
