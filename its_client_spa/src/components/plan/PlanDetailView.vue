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
        <v-layout row reverse>
          <v-btn icon color="success" flat large :to="{name:'Search'}">
            <v-icon large>add_location</v-icon>
          </v-btn>
          <v-btn icon color="success" flat large>
            <v-icon large>note_add</v-icon>
          </v-btn>
          <v-btn icon color="success" flat large>
            <v-icon large>publish</v-icon>
          </v-btn>
        </v-layout>
      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Unscheduled">-->
      <v-flex>
        <v-flex class="title" my-2>
          Chưa lên lịch
        </v-flex>
        <draggable v-model="plan.unScheduledItems" :options="{handle:'.handle-bar'}">
          <v-flex elevation-2 my-1
                  v-for="item in plan.unScheduledItems"
                  :key="item.id">
            <LocationFullWidth v-if="item.location" v-bind="item.location">
              <v-icon slot="handle" class="handle-bar">reorder</v-icon>
            </LocationFullWidth>
            <NoteFullWidth v-else v-bind="item">
              <v-icon slot="handle" class="handle-bar">reorder</v-icon>
            </NoteFullWidth>
          </v-flex>
          <v-flex v-if="plan.unScheduledItems <= 0" style="height: 50px">
            <!--Spacer-->
          </v-flex>
        </draggable>
      </v-flex>
      <!--</editor-fold>-->
      <!--<editor-fold desc="Days">-->
      <v-flex v-for="(day,index) in plan.days" v-if="day" :key="index">
        <v-flex class="title" my-2>
          Ngày {{index + 1}}
        </v-flex>
        <draggable v-model="plan.days[index]" :options="{handle:'.handle-bar'}">
          <v-flex elevation-2 my-1
                  v-for="item in day"
                  :key="item.id">
            <LocationFullWidth v-if="item.location" v-bind="item.location">
              <v-icon slot="handle" class="handle-bar">reorder</v-icon>
            </LocationFullWidth>
            <NoteFullWidth v-else v-bind="item">
              <v-icon slot="handle" class="handle-bar">reorder</v-icon>
            </NoteFullWidth>
          </v-flex>
          <v-flex v-if="plan.days[index].length <= 0" style="height: 50px">
            <!--Spacer-->
          </v-flex>
        </draggable>

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
  import draggable from 'vuedraggable'

  export default {
    name: "PlanDetailView",
    components: {
      LocationFullWidth,
      NoteFullWidth,
      draggable
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
              done: false,
            },
            {
              id: 'ed71c0e4-e311-43d2-95e2-ce186542db3f',
              title: 'title 2',
              text: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer non eros ut dui mollis aliquam. Donec vel nibh tempus, aliquet dolor vitae, mattis massa',
              done: false,
            },
            {
              id: 'be8c4ace-1edc-480e-862f-2aef1c1cc4a7',
              location: _locations[0],
              comment: '',
              done: true
            },
          ],
          days: [
            [
              {
                id: '3e1e96c4-e78b-4224-8721-020ad869f0c7',
                location: _locations[2],
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
