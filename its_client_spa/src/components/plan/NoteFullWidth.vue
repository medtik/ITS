<template>
  <ListItemLayout>
    <v-layout slot="handle" px-1 v-if="$slots.handle" style="width: 100%; height: 100%">
      <slot name="handle"></slot>
    </v-layout>
    <v-layout slot="photo" column align-center justify-center
              style="height: 100%">
      <v-icon size="60">
        far fa-sticky-note
      </v-icon>
    </v-layout>
    <v-layout slot="title">
      <v-flex class="title font-weight">
        {{name}}
      </v-flex>
    </v-layout>
    <v-layout slot="detail">
      <v-flex class="body-1" v-html="description"
              style="overflow-y: auto; max-height: 200px">
      </v-flex>
    </v-layout>
    <v-layout slot="action" column align-end>
      <slot name="action">

      </slot>
    </v-layout>
  </ListItemLayout>
</template>

<script>
  import {ListItemLayout} from "../../common/layout";

  export default {
    name: "NoteFullWidth",
    components: {
      ListItemLayout
    },
    props: [
      'id',
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
