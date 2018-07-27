<template>
  <v-card>
    <v-card-media>
      <v-flex class="v-center-flex" mt-2>
        <v-avatar>
          <img :src="photo"/>
        </v-avatar>
      </v-flex>
      <v-icon small
              color="red"
              class="fakeLink"
              style="position: absolute;top:5%; right: 2%">
        fas fa-user-minus
      </v-icon>
    </v-card-media>
    <v-card-text>
      <v-layout column>
        <v-flex style="text-align: center">
          <span class=subheading>{{name}}</span>
        </v-flex>
        <v-flex style="text-align: center">
          <v-label>{{phone}}</v-label>
        </v-flex>
        <v-flex style="text-align: center">
          <v-label>{{email}}</v-label>
        </v-flex>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
  export default {
    name: "AccountCard",
    props: [
      'photo',
      'name',
      'phone',
      'email',
    ]
  }
</script>

<style scoped>
  .v-center-flex {
    display: flex;
    justify-content: center;
  }
</style>
