<template>
  <v-dialog
    persistent
    v-model="dialog"
    max-width="300">
    <v-card>
      <v-card-title>
        <v-card-title class="headline">
          <span>Thêm tag</span>
        </v-card-title>
        <v-divider></v-divider>
        <v-data-table>

        </v-data-table>
      </v-card-title>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: "AddTagDialog",
    data() {
      return {
        tagInput: '',
        tags: []
      }
    },
    created() {

    }
  }
</script>

<style scoped>

</style>
