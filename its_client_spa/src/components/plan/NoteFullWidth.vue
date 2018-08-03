<template>
  <ListItemLayout>
    <v-layout slot="photo" column align-center>
      <v-flex>
        <v-avatar :size="65">
          <img src="https://picsum.photos/200">
        </v-avatar>
      </v-flex>
      <v-flex mt-1 class="subheading">
        Phong
      </v-flex>
    </v-layout>
    <v-layout slot="title">
      <v-flex class="title font-weight">
        {{name}}
      </v-flex>
    </v-layout>
    <v-layout slot="detail">
      <v-flex class="body-1">
        {{description}}
      </v-flex>
    </v-layout>
  </ListItemLayout>
</template>

<script>
  import {ListItemLayout} from "../../common/layout";

  export default {
    name: "NoteFullWidth",
    components:{
      ListItemLayout
    },
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
