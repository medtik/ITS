<template>
  <v-dialog v-model="dialog" max-width="450" persistent>
    <v-card>
      <v-layout column>
        <v-flex>
          <v-layout column>
            <!--PERSONAL-->
            <v-list subheader avatar v-if="showPersonal">
              <v-subheader>
                Cá nhân
              </v-subheader>
              <v-list-tile @click="selectedGroup = -1">
                <v-list-tile-avatar>
                  <v-icon>
                    fas fa-user
                  </v-icon>
                </v-list-tile-avatar>
                <v-list-tile-content>
                  <v-list-tile-title>
                    Cá nhân
                  </v-list-tile-title>
                </v-list-tile-content>
                <v-list-tile-action>
                  <v-icon
                    v-if="selectedGroup == -1"
                    color="green">
                    check
                  </v-icon>
                  <v-icon v-else/>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <!--GROUP-->
            <v-list subheader avatar v-if="!myGroupsLoading && isAnyQualified">
              <v-subheader>
                Nhóm
              </v-subheader>
              <v-list-tile v-for="(group) in myGroups" :key="group.id"
                           v-if="isShowAll || group.isOwner"
                           @click="selectedGroup = group">
                <v-list-tile-content>
                  <v-list-tile-title>
                    {{group.name}}
                  </v-list-tile-title>
                </v-list-tile-content>
                <v-list-tile-action>
                  <v-icon
                    v-if="selectedGroup.id == group.id"
                    color="green">
                    check
                  </v-icon>
                  <v-icon v-else/>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-progress-linear v-if="myGroupsLoading" indeterminate color="primary"></v-progress-linear>
          </v-layout>
        </v-flex>
        <v-flex>
          <v-divider></v-divider>
        </v-flex>
        <v-flex>
          <v-btn color="success"
                 @click="onSelect">
            Chọn
          </v-btn>
          <v-btn color="secondary"
                 @click="onClose">
            Hủy
          </v-btn>
        </v-flex>
      </v-layout>
    </v-card>
  </v-dialog>
</template>

<script>
  import {mapGetters} from "vuex";
  import _ from "lodash";

  export default {
    name: "ChoosePlanDestinationDialog",
    props: [
      'dialog',
      'destinations',
      'showPersonal',
      'isShowAll'
    ],
    data() {
      return {
        selectedGroup: {},
      }
    },
    computed: {
      ...mapGetters('group', {
        myGroups: 'myGroups',
        myGroupsLoading: 'myGroupsLoading'
      }),
      isAnyQualified() {
        return _.some(this.myGroups, (group) => {
          return this.isShowAll || group.isOwner
        })
      }
    },
    mounted() {
      if (!this.myGroups || this.myGroups.length == 0) {
        this.$store.dispatch('group/fetchMyGroups');
      }
    },
    methods: {
      onSelect() {
        if(this.selectedGroup == -1){
          this.$emit('select',{group: undefined});
        }else{
          this.$emit('select', {group: _.cloneDeep(this.selectedGroup)});
        }
        this.selectedGroup = {};
      },
      onClose() {
        this.$emit('close');
      }
    }
  }
</script>

<style scoped>

</style>
