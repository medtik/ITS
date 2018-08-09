<template>
  <ListItemLayout>
    <v-layout slot="photo" column align-center>
      <v-icon large>
        far fa-sticky-note
      </v-icon>
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
    <v-layout slot="action" justify-end>
      <v-btn icon flat color="secondary" small
             @click="$emit('delete',id)">
        <v-icon small>
          fas fa-trash
        </v-icon>
      </v-btn>
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
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
    },
    methods: {}
  }
</script>

<style scoped>

</style>
