<template>
  <v-layout column
            class="white"
            style="width: 100%">
    <v-flex class="text-xs-center" my-2>
      <span class="headline">{{title}}</span>
      <v-divider/>
    </v-flex>
    <v-flex>
      <v-layout row wrap px-1>
        <v-flex xs12 lg6 px-2 my-1>
          <v-layout row wrap>
            <v-flex class="text-lg-center title" xs12 mb-2>
              <v-icon>fas fa-user-friends</v-icon> Thành viên
            </v-flex>
            <v-flex xs6 lg4 v-for="account in accounts" :key="account.id">
              <AccountCard v-bind="account"/>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex xs12 lg6 px-2 my-1>
          <v-layout column>
            <v-flex class="text-lg-center title" xs12 my-2>
              <v-icon>fas fa-suitcase</v-icon> Kế hoạch
            </v-flex>
            <v-flex xs12 lg6>
              <PlanFullWidth v-for="plan in plans"
                             v-bind="plan"
                             :key="plan.id"/>
            </v-flex>
          </v-layout>

        </v-flex>
      </v-layout>
    </v-flex>
  </v-layout>
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
