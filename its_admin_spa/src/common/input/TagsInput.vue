<template>
  <v-container fluid pa-1>
    <v-layout column>
      <v-flex>
        <v-btn icon
               v-if="readonly != ''"
               @click="dialog = true"
               color="success">
          <v-icon small>fas fa-plus</v-icon>
        </v-btn>
        <v-chip :close="readonly != ''"
                v-for="tag in tags"
                @input="onRemove(tag)"
                :key="tag.id">
          {{tag.name}}
        </v-chip>
      </v-flex>
      <v-flex v-if="error">
        <v-alert type="error" dismissible>
          {{error}}
        </v-alert>
      </v-flex>
    </v-layout>
    <TagChooseDialog :dialog="dialog"
                     :value="value"
                     :admin="admin"
                     @create="$emit('create')"
                     @input="onAddTagConfirm"
                     @save="dialog = false"
                     @close="dialog = false">
    </TagChooseDialog>
  </v-container>
</template>

<script>
  import _ from 'lodash';
  import TagChooseDialog from "./TagChooseDialog";

  export default {
    name: "ManageTagSection",
    components: {
      TagChooseDialog
    },
    props: [
      'value',
      'readonly',
      'admin',
      'error'
    ],
    data() {
      return {
        dialog: false
      }
    },
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

      onAddTagConfirm(tags) {
        this.$emit('input', tags)
      }
    }
  }
</script>

<style scoped>

</style>
