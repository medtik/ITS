<template>
  <v-content>
    <ParallaxHeader src="static/pexels-photo-490411.jpeg" text="Các chuyến đi của bạn"/>
    <v-layout column class="grey lighten-4">
      <v-flex>
        <v-btn color="success" :to="{name:'PlanCreate'}">
          <v-icon>
            fas fa-plus
          </v-icon>
          &nbsp;
          Tạo chuyến đi
        </v-btn>
      </v-flex>
      <v-flex v-for="plan in myPlans"
              :key="plan.id"
              my-2
              py-2
              elevation-1
              class="white">
        <PlanFullWidth @save="dialog.choosePlanDestination = true"
                       @delete="deletePlan"
                       v-bind="plan"/>
      </v-flex>
    </v-layout>
    <v-flex style="height: 30vh">
      <!--Holder-->
    </v-flex>
    <ChoosePlanDestinationDialog :dialog="dialog.choosePlanDestination"
                                 @select="dialog.choosePlanDestination = false"
                                 @close="dialog.choosePlanDestination = false"
    />
  </v-content>
</template>

<script>
  import ParallaxHeader from "../../common/layout/ParallaxHeader";
  import PlanFullWidth from "../../common/block/PlanFullWidth";
  import ChoosePlanDestinationDialog from "../../common/input/ChoosePlanDestinationDialog";
  import {mapGetters} from "vuex";


  export default {
    name: "MyPlanListView",
    components: {
      ParallaxHeader,
      PlanFullWidth,
      ChoosePlanDestinationDialog
    },
    data() {
      return {
        dialog: {
          choosePlanDestination: false,
        }
      }
    },
    computed: {
      ...mapGetters('plan', {
        myPlans: 'myPlans',
        myPlansLoading: 'myPlansLoading'
      })
    },
    mounted() {
      this.$store.dispatch('plan/fetchMyPlans')
    },
    methods: {
      deletePlan(id) {
        this.$store.dispatch('plan/delete', {
          id
        })
          .then(() => {
            this.$store.dispatch('plan/fetchMyPlans')
          })
      }
    }
  }
</script>

<style scoped>

</style>
