<template>
  <v-content>
    <v-toolbar flat color="light-blue" dark>
      <v-toolbar-title>
        {{plan.title}}
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-title>
        {{plan.from}} - {{plan.to}}
      </v-toolbar-title>
      <v-toolbar-items slot="extension">
        <v-btn flat @click="dialog.choosePlanDestination = true">
          <v-icon large>fas fa-heart</v-icon>
          <span v-if="!isSmallScreen">&nbsp; Lưu</span>
        </v-btn>
        <v-btn flat :to="{name:'PlanEdit'}">
          <v-icon large>edit</v-icon>
          <span v-if="!isSmallScreen">Chỉnh sửa</span>
        </v-btn>
        <v-btn flat :to="{name:'Search'}">
          <v-icon large>add_location</v-icon>
          <span v-if="!isSmallScreen">Thêm địa điểm</span>
        </v-btn>
        <v-btn flat @click="dialog.addNote = true">
          <v-icon large>note_add</v-icon>
          <span v-if="!isSmallScreen">Thêm ghi chú</span>
        </v-btn>
        <v-btn flat @click="dialog.publishPlan = true">
          <v-icon large>publish</v-icon>
          <span v-if="!isSmallScreen">Đăng</span>
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>
    <v-layout column class="white">
      <!--UNSCHEDULED-->
      <v-flex class="grey lighten-4">
        <v-flex my-3>
          <v-layout row>
            <v-flex class="title">Chưa lên lịch</v-flex>
          </v-layout>
        </v-flex>
        <draggable v-model="plan.unScheduledItems" :options="{handle:'.handle-bar', group:'items'}">
          <v-flex elevation-2 my-1
                  v-for="item in plan.unScheduledItems"
                  :key="item.id"
                  class="white">
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
      <!--DAYS-->
      <v-flex v-for="(day,index) in plan.days" v-if="day" :key="index" class="grey lighten-4">
        <v-flex class="title" my-2>
          Ngày {{index + 1}}
        </v-flex>
        <draggable v-model="plan.days[index]" :options="{handle:'.handle-bar', group:'items'}">
          <v-flex elevation-2 my-1
                  v-for="item in day"
                  :key="item.id"
                  class="white"
          >
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
      <v-flex style="height: 15vh">
        <!--Holder-->
      </v-flex>
      <!--DIALOGS-->
      <v-dialog v-model="dialog.addNote" max-width="450">
        <!--ADD NOTE-->
        <v-card>
          <v-card-title class="light-blue title white--text">
            Thêm ghi chú
          </v-card-title>
          <v-card-text>
            <v-layout column>
              <v-flex>
                <v-text-field label="Tiêu đề"/>
                <v-textarea label="Nội dung"/>
              </v-flex>
              <v-divider/>
              <v-flex>
                <v-btn color="success"
                       :loading="loading.createNoteBtn"
                       @click="onAddNote">
                  Tạo
                </v-btn>
                <v-btn color="secondary"
                       @click="dialog.addNote = false">
                  Hủy
                </v-btn>
              </v-flex>
            </v-layout>
          </v-card-text>
        </v-card>
      </v-dialog>
      <v-dialog v-model="dialog.publishPlan" max-width="400">
        <!--PUBLISH-->
        <v-card>
          <v-card-title class="light-blue title white--text">
            Đăng chuyến đi
          </v-card-title>
          <v-card-text>
            <v-layout column>
              <v-flex my-3 class="body-2">
                Chuyến đi của bạn sẽ được đăng lên hệ thống để mọi người có thể cũng trải nhiệm chuyến đi của bạn
              </v-flex>
              <v-divider></v-divider>
              <v-flex>
                <v-btn color="success"
                       :loading="loading.publishBtn"
                       @click="onPublish">
                  Xác nhận
                </v-btn>
                <v-btn color="secondary"
                       @click="dialog.publishPlan = false">
                  Hủy
                </v-btn>
              </v-flex>
            </v-layout>
          </v-card-text>
        </v-card>
      </v-dialog>
    </v-layout>
    <!--DIALOG-->
    <ChoosePlanDestinationDialog :dialog="dialog.choosePlanDestination"
                                 @select="dialog.choosePlanDestination = false"
                                 @close="dialog.choosePlanDestination = false"/>
  </v-content>
</template>

<script>
  import _locations from "../location/Locations";
  import LocationFullWidth from "../../sharedComponents/block/LocationFullWidth";
  import NoteFullWidth from "./NoteFullWidth";
  import ChoosePlanDestinationDialog from "../../sharedComponents/input/ChoosePlanDestinationDialog";
  import draggable from 'vuedraggable'

  export default {
    name: "PlanDetailView",
    components: {
      LocationFullWidth,
      NoteFullWidth,
      ChoosePlanDestinationDialog,
      draggable
    },
    data() {
      return {
        plan: {
          title: 'Plan ABC',
          from: '22/6/2018',
          to: '25/6/2018',
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
        loading: {
          createNoteBtn: false,
          publishBtn: false,
        },
        dialog: {
          addNote: false,
          publishPlan: false,
          choosePlanDestination: false,
        },
      }
    },
    computed: {
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      }
    },
    methods: {
      onAddNote() {
        this.loading.createNoteBtn = true;
        setTimeout(() => {
          this.dialog.addNote = false;
          this.loading.createNoteBtn = false;
        }, 1500)
      },
      onPublish() {
        this.loading.publishBtn = true;
        setTimeout(() => {
          this.dialog.publishPlan = false;
          this.loading.publishBtn = false;
        }, 1500)
      }
    }
  }
</script>

<style scoped>

</style>
