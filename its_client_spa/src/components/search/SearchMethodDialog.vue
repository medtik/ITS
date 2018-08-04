<template>
  <v-dialog v-model="dialog" max-width="450" persistent>
    <v-card>
      <v-card-text>
        <v-layout column>
          <v-flex class="text-xs-center">
            <v-select
              value="smart"
              :items="items"
              v-model="selectedValue"
              item-text="text"
              item-value="value"
            ></v-select>
          </v-flex>
          <v-divider></v-divider>
          <v-layout row>
            <v-btn color="primary" @click="onSelect">
              <v-icon small>
                fas fa-search
              </v-icon>
              Tiếp tục
            </v-btn>
            <v-btn color="secondary"
                   @click="$emit('close')">
              Hủy
            </v-btn>
          </v-layout>
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
        selectedValue: 'smart',
        items: [
          {text: 'Tìm thông minh', value: 'smart'},
          {text: 'Tìm thường', value: 'normal'}
        ]
      }
    },
    methods: {
      onSelect() {
        this.$emit('select', this.selectedValue);
      }
    }
  }
</script>
