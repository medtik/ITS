<template>
  <v-content>
    <v-toolbar dark flat color="light-blue" dense>
      <v-toolbar-title>
        {{title}}
      </v-toolbar-title>
      <v-tabs
        fixed-tabs
        slot="extension"
        color="light-blue"
        slider-color="white"
        v-model="tab"
        grow
      >
        <v-tab touchless key="members">
          <v-flex class="text-md-center" ma-3>
            <v-icon>fas fa-user-friends</v-icon>
            Thành viên
          </v-flex>
        </v-tab>
        <v-tab touchless key="trips">
          <v-flex class="text-md-center" xs12 my-2>
            <v-icon>fas fa-suitcase</v-icon>
            Chuyến đi
          </v-flex>
        </v-tab>
      </v-tabs>
    </v-toolbar>
    <v-tabs-items v-model="tab">
      <v-tab-item key="members">
        <v-flex xs12 px-2 py-1 class="grey lighten-4">
          <!--USERS-->
          <v-layout column>
            <v-flex>
              <v-btn color="success"
              :to="{name: 'GroupInvite'}">
                <v-icon>fas fa-user-plus</v-icon>
                &nbsp;
                Mời thành viên
              </v-btn>
            </v-flex>
            <v-layout row wrap my-2>
              <v-flex xs6 sm4 lg2 pa-1
                      v-for="account in accounts"
                      :key="account.id">
                <AccountCard v-bind="account"/>
              </v-flex>
            </v-layout>
          </v-layout>
        </v-flex>
      </v-tab-item>
      <v-tab-item key="trips">
        <v-flex xs12 px-2 py-1 class="grey lighten-4">
          <v-layout column>
            <v-flex>
              <v-btn color="success"
                     @click="dialog.choosePlan = true">
                <v-icon>fas fa-user-plus</v-icon>
                &nbsp;
                Thêm chuyến đi
              </v-btn>
            </v-flex>
          </v-layout>
          <!--GROUPS-->
          <v-layout column>
            <v-flex xs12 lg6 my-2 pa-2
                    class="white"
                    v-for="plan in plans"
                    :key="plan.id">
              <PlanFullWidth v-bind="plan"
                             @save="dialog.choosePlanDestination = true"/>
            </v-flex>
          </v-layout>
        </v-flex>
      </v-tab-item>
    </v-tabs-items>
    <v-flex style="height: 15vh">

    </v-flex>
    <ChoosePlanDestinationDialog :dialog="dialog.choosePlanDestination"
                                 @select="dialog.choosePlanDestination = false"
                                 @close="dialog.choosePlanDestination = false"
    />
    <ChoosePlanDialog
      :dialog="dialog.choosePlan"
      @select="dialog.choosePlan = false"
      @close="dialog.choosePlan = false"
    />

  </v-content>
</template>

<script>
  import PlanFullWidth from "../shared/PlanFullWidth";
  import AccountCard from "../shared/AccountCard";
  import ChoosePlanDestinationDialog from "../input/ChoosePlanDestinationDialog";
  import ChoosePlanDialog from "../input/ChoosePlanDialog";

  export default {
    name: "GroupFullWidth",
    components: {
      AccountCard,
      PlanFullWidth,
      ChoosePlanDialog,
      ChoosePlanDestinationDialog
    },
    data() {
      return {
        tab: undefined,
        dialog: {
          choosePlanDestination: false,
          choosePlan: false
        },
        title: 'Nhóm ABC',
        accounts: [
          {
            id: 1,
            photo: "http://lorempixel.com/100/100/people/",
            name: "Nguyễn văn A",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 2,
            photo: "http://lorempixel.com/100/100/people/",
            name: "Nguyễn văn C",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 3,
            photo: "http://lorempixel.com/100/100/people/",
            name: "Nguyễn văn A",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 4,
            photo: "http://lorempixel.com/100/100/people/",
            name: "Nguyễn văn B",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 5,
            photo: "http://lorempixel.com/100/100/people/",
            name: "Nguyễn văn A",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 6,
            photo: "http://lorempixel.com/100/100/people/",
            name: "Nguyễn văn D",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          }
        ],
        plans: [
          {id: 1, title: "Plan ABC", startDate: "20/4/2018", endDate: "27/4/2018"},
          {id: 2, title: "Plan ABC", startDate: "20/4/2018", endDate: "27/4/2018"}
        ]
      }
    }
  }
</script>

<style scoped>

</style>
