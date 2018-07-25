<template>
  <v-content>
    <v-toolbar dark flat color="light-blue">
      <v-toolbar-title>
        {{title}}
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column>
      <v-flex class="grey lighten-4">
        <v-layout row wrap px-1 my-2>
          <v-flex xs12 px-2 my-1 class="white">
            <!--USERS-->
            <v-layout column>
              <v-flex class="text-md-center title" ma-3>
                <v-icon>fas fa-user-friends</v-icon>
                Thành viên
              </v-flex>
              <v-flex class="text-md-center title">
                <v-btn icon flat color="primary" large>
                  <v-icon>fas fa-user-plus</v-icon>
                </v-btn>
              </v-flex>
              <v-layout row wrap my-2>
                <v-flex xs3 lg2 pa-1
                        v-for="account in accounts"
                        :key="account.id">
                  <AccountCard v-bind="account"/>
                </v-flex>
              </v-layout>
            </v-layout>
          </v-flex>
          <v-flex xs12 px-2 my-1 class="white">
            <!--GROUPS-->
            <v-layout column>
              <v-flex class="text-md-center title" xs12 my-2>
                <v-icon>fas fa-suitcase</v-icon>
                Chuyến đi
              </v-flex>
              <v-flex xs12 lg6 my-3
                      v-for="plan in plans"
                      :key="plan.id">
                <PlanFullWidth v-bind="plan"/>
              </v-flex>
            </v-layout>

          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import PlanFullWidth from "../shared/PlanFullWidth";
  import AccountCard from "../shared/AccountCard";

  export default {
    name: "GroupFullWidth",
    components: {
      PlanFullWidth,
      AccountCard
    },
    data() {
      return {
        title: 'Nhóm ABC',
        accounts: [
          {
            id: 1,
            photo: "http://lorempixel.com/400/400/people/",
            name: "Nguyễn văn A",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 2,
            photo: "http://lorempixel.com/400/400/people/",
            name: "Nguyễn văn C",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 3,
            photo: "http://lorempixel.com/400/400/people/",
            name: "Nguyễn văn A",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 4,
            photo: "http://lorempixel.com/400/400/people/",
            name: "Nguyễn văn B",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 5,
            photo: "http://lorempixel.com/400/400/people/",
            name: "Nguyễn văn A",
            phone: "0909152644",
            email: "tlphong@tlp.com"
          },
          {
            id: 6,
            photo: "http://lorempixel.com/400/400/people/",
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
