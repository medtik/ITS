<template>
  <v-content>
    <v-layout column>
      <v-flex class="headline font-weight-black">
        Kết quả tìm kiếm
      </v-flex>
      <v-flex v-for="item in items"
              :key="item.id"
              my-2>
        <v-divider/>
        <SearchResult v-bind="item"/>
        <v-divider/>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import SearchResult from "./LocationSearchItem";

  export default {
    name: "SmartSearchResultView",
    components: {
      SearchResult
    },
    data() {
      return {
        items: [
          {id: 1, text: 'abvc'},
          {id: 2, text: 'abvcd'},
          {id: 3, text: 'abv1c'},
          {id: 4, text: 'abccvc'},
        ]
      }
    }
  }
</script>

<style scoped>

</style>
