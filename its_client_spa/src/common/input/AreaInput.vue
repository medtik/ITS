<template>
  <v-select label="Chọn khu vực"
            @change="onSelect"
            v-model="selectedAreaId"
            :items="items"
            item-text="name"
            item-value="id"
            :loading="loading"
            :dark="dark"
  >

  </v-select>
</template>

<script>
  import storeBoundSelectMixin from "./StoreBoundSelectMixin.js";
  export default {
    name: "AreaInput",
    props:[
      'dark'
    ],
    mixins: [
      storeBoundSelectMixin({
        alias: 'area',
        itemsPath: 'areas',
        loadingPath: 'areasLoading',
        getItemPath: 'getAll',
      })
    ],
    data() {
      return {
        selectedAreaId: undefined
      }
    },
    mounted() {
      this.selectedAreaId = this.value;
    }
  }
</script>

<style scoped>

</style>
