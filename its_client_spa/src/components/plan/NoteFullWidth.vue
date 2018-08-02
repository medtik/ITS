<template>
  <v-layout row style="max-height: 200px; height: 100px">
    <!--PHOTO-->
    <v-flex style="flex-grow: 0" xs4 lg2>
      <v-layout shrink align-center column>
        <v-flex>
          <v-avatar :size="65">
            <img src="https://picsum.photos/200">
          </v-avatar>
        </v-flex>
        <v-flex mt-1>
          Phong
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex shrink>
      <v-divider vertical></v-divider>
    </v-flex>
    <!--CONTENT-->
    <v-layout column px-1>
      <v-flex class="title font-weight-medium" my-2>
        {{name}}
      </v-flex>
      <v-flex class="body-1" pa-1>
        {{description}}
      </v-flex>
    </v-layout>
  </v-layout>

</template>

<script>
  export default {
    name: "NoteFullWidth",
    props: [
      'name',
      'description'
    ],
    computed: {
      imageSize() {
        if (this.isSmallScreen) {
          return 520
        } else {
          return 150
        }
      },
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
    },
    methods: {}
  }
</script>

<style scoped>

</style>
