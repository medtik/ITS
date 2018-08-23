<template>
  <v-combobox
    label="Thể loại"
    v-model="categoryInput"
    :items="items"
    :loading="loading"
    :readonly="readonly"
    @change="onSelect"
  >
  </v-combobox>
</template>

<script>
  import {StoreBoundSelectMixin} from "../mixin";

  export default {
    name: "LocationCategoryInput",
    mixins: [
      StoreBoundSelectMixin({
        alias: 'location',
        itemsPath: 'categories',
        loadingPath: 'categoriesLoading',
        getItemPath: 'fetchCategories',
      })
    ],
    props: [
      'dark',
      'readonly',
    ],
    data() {
      return {
        categoryInput: undefined
      }
    },
    watch: {
      value(val) {
        this.categoryInput = val;
      }
    },
    mounted() {
      this.categoryInput = this.value;
    }
  }
</script>

<style scoped>

</style>
