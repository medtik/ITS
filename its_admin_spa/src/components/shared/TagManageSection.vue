<template>
  <v-container fluid pa-1>
    <v-layout row>
      <v-flex>
        <v-btn icon color="success" v-on:click="onAddTagClick">
          <v-icon small>fas fa-plus</v-icon>
        </v-btn>
        <v-chip close
                v-for="tag in tags"
                @input="onRemove(tag)"
                :key="tag.id">
          {{tag.name}}
        </v-chip>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  import _ from 'lodash';

  export default {
    name: "ManageTagSection",
    props: [
      'value'
    ],
    computed: {
      tags() {
        return this.value;
      }
    },
    methods: {

      onRemove(tag) {
        const tags = _.remove(this.tags, (val) => val.id !== tag.id);
        this.$emit('input', tags);
      },
      onAddTagClick() {
        this.$emit('addTag');
      }
    }
  }
</script>

<style scoped>

</style>
