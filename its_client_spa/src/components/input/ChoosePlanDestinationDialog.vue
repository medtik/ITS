<template>
  <v-dialog v-model="dialog" max-width="450" persistent>
    <v-card>
      <v-layout column>
        <v-flex>
          <v-layout column>
            <v-list subheader avatar>
              <v-subheader>
                Cá nhân
              </v-subheader>
              <v-list-tile @click="selectedIndex = -1">
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
                    v-if="selectedIndex === -1"
                    color="green">
                    check
                  </v-icon>
                  <v-icon v-else/>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-list subheader avatar>
              <v-subheader>
                Nhóm của bạn
              </v-subheader>
              <v-list-tile v-for="(n,index) in 3" :key="n"
                           @click="selectedIndex = index">
                <v-list-tile-avatar>
                  <img src="https://picsum.photos/200"/>
                </v-list-tile-avatar>
                <v-list-tile-content>
                  <v-list-tile-title>
                    Nhóm ABC
                  </v-list-tile-title>
                </v-list-tile-content>
                <v-list-tile-action>
                  <v-icon
                    v-if="selectedIndex === index"
                    color="green">
                    check
                  </v-icon>
                  <v-icon v-else/>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
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
  export default {
    name: "ChoosePlanDestinationDialog",
    props: [
      'dialog',
      'value',
      'destinations'
    ],
    data() {
      return {
        selectedIndex: undefined
      }
    },
    watch: {
      value(val, oldVal) {
        // this.selectedIndex = val;
      }
    },
    methods: {
      onSelect() {
        this.$emit('input', this.selected);
        this.$emit('select', this.selected);
      },
      onClose() {
        this.$emit('close');
      }
    }
  }
</script>

<style scoped>

</style>
