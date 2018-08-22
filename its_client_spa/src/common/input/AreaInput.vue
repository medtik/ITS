<template>
  <v-select label="Chọn khu vực"
            @change="onSelect"
            :readonly="readonly"
            v-model="selectedAreaId"
            :items="items"
            item-text="name"
            item-value="id"
            :loading="loading"
            :dark="dark"
            no-data-text="Chưa có dữ liệu"
  >

  </v-select>
</template>

<script>
  import {StoreBoundSelectMixin} from "../mixin";

  export default {
    name: "AreaInput",
    props: [
      'dark',
      'readonly',
    ],
    mixins: [
      StoreBoundSelectMixin({
        alias: 'area',
        itemsPath: 'areas',
        loadingPath: 'areasLoading',
        getItemPath: 'getAll',
      })
    ],
    watch: {
      value(val) {
        this.selectedAreaId = val;
      }
    },
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
