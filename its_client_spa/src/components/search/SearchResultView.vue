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
          {
            id: 1,
            text: 'abvc',
            primaryPhoto: 'https://picsum.photos/600/600',
            rating: 4.4,
            ratingCount: 12,
            address: '263/11 gì đó gì đó',
            reason: 'Vì bạn chọn "Đi phượt"'
          },
          {
            id: 2,
            text: 'abdsavc',
            primaryPhoto: 'https://picsum.photos/600/600',
            rating: 3.5,
            ratingCount: 6,
            address: '263/11 gì đó gì đó',
            reason: 'Vì bạn chọn "Đi phượt"'
          },
          {
            id: 3,
            text: 'abzvc',
            primaryPhoto: 'https://picsum.photos/600/600',
            rating: 4.0,
            ratingCount: 7,
            address: '263/11 gì đó gì đó',
            reason: 'Vì bạn chọn "Đi phượt"'
          },
        ]
      }
    }
  }
</script>

<style scoped>

</style>
