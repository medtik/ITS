<template>
  <v-dialog v-model="dialog">
    <v-card>
      <v-card-text>
        <v-layout column>
          <v-flex class="text-xs-center">
            <v-select
              item-text="text"
              item-value="value"
            ></v-select>
          </v-flex>
          <v-divider></v-divider>
          <v-flex class="grey lighten-4 text-xs-center">
            <v-btn color="primary">
              <v-icon>
                fas fa-search
              </v-icon>
              Tìm
            </v-btn>
          </v-flex>
        </v-layout>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: "SearchMethodDialog",
    props: [
      'dialog'
    ],
    data() {
      return {
        items: [
          {text: 'Tìm thông minh', value: 'smart'},
          {text: 'Tìm thường', value: 'normal'}
        ]
      }
    },
    methods:{
      onSelect(value){
        this.$emit('select', value);
      }
    }
  }
</script>
