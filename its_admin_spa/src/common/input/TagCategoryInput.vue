<template>
  <v-autocomplete label="Thể loại"
                  @change="onSelect"
                  v-model="selectedCategory"
                  :items="items"
                  :loading="loading"
  >
  </v-autocomplete>
</template>

<script>
  import {StoreBoundSelectMixin} from "../mixin";

  export default {
    name: "CategoryInput",
    mixins: [
      StoreBoundSelectMixin({
        alias: 'tag',
        itemsPath: 'tagCategories',
        loadingPath: 'tagCategoryLoading',
        getItemPath: 'fetchCategories',
      })
    ],
    data() {
      return {
        selectedCategory: undefined
      }
    },
    mounted() {
      this.selectedCategory = this.value;
    }
  }
</script>

<style scoped>

</style>
