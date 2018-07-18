<template>
  <v-content>
    <v-toolbar flat color="light-blue" dark>
      <v-layout justify-center>
        <v-toolbar-title>
          {{plan.title}}
        </v-toolbar-title>
      </v-layout>
    </v-toolbar>
    <v-layout column>
      <!--<editor-fold desc="Actions">-->
      <v-flex>

      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Unscheduled">-->
      <v-flex>
        <v-flex class="title" my-2>
          Chưa lên lịch
        </v-flex>
        <v-flex elevation-2 my-1
                v-for="item in plan.unScheduledItems"
                :key="item.id">
          <LocationFullWidth v-if="item.location" v-bind="item.location"/>
          <NoteFullWidth v-else v-bind="item"/>
        </v-flex>
      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Days">-->
      <v-flex v-for="(day,index) in plan.days" v-if="day" :key="index">
        <v-flex class="title" my-2>
          Ngày {{index + 1}}
        </v-flex>
        <v-flex elevation-2 my-1
                v-for="item in day"
                :key="item.id">
          <LocationFullWidth v-if="item.location" v-bind="item.location"/>
          <NoteFullWidth v-else v-bind="item"/>
        </v-flex>
      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Holder">-->
      <v-flex style="height: 15vh">
        <!--Holder-->
      </v-flex>
      <!--</editor-fold>-->
    </v-layout>
  </v-content>
</template>

<script>
  import _locations from "../location/Locations";
  import LocationFullWidth from "../shared/LocationFullWidth";
  import NoteFullWidth from "./NoteFullWidth";


  export default {
    name: "PlanDetailView",
    components: {
      LocationFullWidth,
      NoteFullWidth
    },
    data() {
      return {
        plan: {
          title: 'Plan ABC',
          locations: _locations,
          unScheduledItems: [
            {
              id: '4f36ed66-f87c-4b9d-b338-147e20f56c4a',
              title: 'title',
              text: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer non eros ut dui mollis aliquam. Donec vel nibh tempus, aliquet dolor vitae, mattis massa',
              index: 1,
              done: false,

            },
          ],
          days: [
            [
              {
                id: '3e1e96c4-e78b-4224-8721-020ad869f0c7',
                location: _locations[0],
                comment: '',
                done: true
              },
              {
                id: '3f30c5a1-7c84-4531-a0b7-2836444a6d49',
                title: 'Lorem ipsum dolor sit amet,',
                text: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer non eros ut dui mollis aliquam. Donec vel nibh tempus, aliquet dolor vitae, mattis massa',
                done: false,
              }
            ],
            [{
              id: '3e1e96c4-e78b-4224-8721-020ad869f0c7',
              location: _locations[1],
              comment: '',
              done: true
            }]
          ],
        },

      }
    }
  }
</script>

<style scoped>

</style>
