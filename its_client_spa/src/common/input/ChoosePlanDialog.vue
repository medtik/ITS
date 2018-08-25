<template>
  <v-dialog v-model="dialog" max-width="450" persistent>
    <v-card>
      <v-layout column>
        <v-flex v-if="groupedPlans && groupedPlans.length > 0">
          <!--CONTENT-->
          <v-list subheader avatar
                  v-for="plans in groupedPlans"
                  v-if="plans[0].groupName != currentGroup.groupName"
                  :key="`planGroup_${plans ? plans[0].groupName : ''}`">

            <v-subheader v-if="plans && plans[0].groupName">
              {{plans[0].groupName}}
            </v-subheader>
            <v-subheader v-else>
              Chuyến đi cá nhân
            </v-subheader>

            <v-list-tile v-for="(plan) in plans" :key="`plan_${plan.id}`"
                         @click="selectedPlanId = plan.id">
              <v-list-tile-content>
                <v-list-tile-title>
                  <v-flex px-2>
                    {{plan.name}}
                  </v-flex>
                </v-list-tile-title>
              </v-list-tile-content>
              <v-list-tile-action>
                <v-icon
                  v-if="selectedPlanId === plan.id"
                  color="green">
                  check
                </v-icon>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
        </v-flex>
        <v-flex v-else
                class="title">
          Bạn chưa có chuyến đi nào
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
  import _ from "lodash"

  export default {
    name: "ChoosePlanDialog",
    props: [
      'dialog',
      'value',
      'destinations',
      'currentGroup',
    ],
    data() {
      return {
        selectedPlanId: undefined
      }
    },
    computed: {
      ...mapGetters('plan', {
        groupedPlans: 'myVisiblePlans',
        loading: 'myVisiblePlansLoading'
      }),
      ...mapGetters('authenticate', {
        isLoggedIn: "isLoggedIn"
      })
    },
    mounted() {
      if (this.isLoggedIn) {
        this.$store.dispatch('plan/fetchVisiblePlans');
      }
    },
    methods: {
      onSelect() {
        const selected = _(this.groupedPlans)
          .flatten()
          .find(plan => {
            return plan.id == this.selectedPlanId;
          });

        this.$emit('input', selected);
        this.$emit('select', selected);
      },
      onClose() {
        this.$emit('close');
      }
    }
  }
</script>

<style scoped>

</style>
