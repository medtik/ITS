<template>
  <v-dialog v-model="dialog" max-width="450" persistent>
    <v-card>
      <v-layout column>
        <v-flex>
          <!--CONTENT-->
          <v-list subheader avatar v-for="n in 3 " :key="`n${n}`">
            <v-subheader>
              Nhóm {{n}}
            </v-subheader>
            <v-list-tile v-for="(i,index) in 3" :key="n*i"
                         @click="selectedIndex = n*(i+4)">
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
                  v-if="selectedIndex === n*(i+4)"
                  color="green">
                  check
                </v-icon>
                <v-icon v-else/>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
        </v-flex>
        <v-divider/>
        <v-flex>
          <!--ACTIONS-->
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
  import {mapGetters} from "vuex"

  export default {
    name: "ChoosePlanDialog",
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
    computed: {
      ...mapGetters('plan', {
        groupedPlans: 'myVisiblePlanGrouped',
        loading: 'myVisiblePlansLoading'
      })
    },
    mounted(){
      if(!this.groupedPlans || !this.groupedPlans.length > 0){
        this.$store.dispatch('plan/fetchVisiblePlans');
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
