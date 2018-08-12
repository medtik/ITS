<template>
  <v-card>
    <v-card-media>
      <v-flex class="v-center-flex" mt-2>
        <v-avatar size="80">
          <img :src="avatar"/>
        </v-avatar>
      </v-flex>
      <slot name="actionBtn">
      </slot>
    </v-card-media>
    <v-card-text>
      <v-layout column>
        <v-flex style="text-align: center">
          <span class=subheading>{{name}}</span>
        </v-flex>
        <v-flex style="text-align: center">
          <v-label>{{phoneNumber}}</v-label>
        </v-flex>
        <v-flex style="text-align: center">
          <v-label>{{emailAddress}}</v-label>
        </v-flex>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
  export default {
    name: "AccountCard",
    props: [
      'avatar',
      'name',
      'phoneNumber',
      'emailAddress',
    ]
  }
</script>

<style scoped>
  .v-center-flex {
    display: flex;
    justify-content: center;
  }
</style>
