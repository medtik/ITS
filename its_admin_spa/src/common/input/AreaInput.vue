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
  import {StoreBoundSelectMixin} from "../mixin";

  export default {
    name: "AreaInput",
    props:[
      'dark'
    ],
    mixins: [
      StoreBoundSelectMixin({
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
