<template>
  <v-content>
    <v-toolbar v-if="!pageLoading" flat color="light-blue" dark>
      <v-toolbar-title>
        {{plan.name}}
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-title>
        {{plan.startDate}} - {{plan.endDate}}
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
        <v-btn flat @click="dialog.publishPlan = true">
          <v-icon large>publish</v-icon>
          <span v-if="!isSmallScreen">Đăng</span>
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>
    <v-layout v-if="!pageLoading" column class="white">
      <v-flex class="grey lighten-4">
        <v-btn flat :to="{name:'Search'}">
          <v-icon>add_location</v-icon>
          <span v-if="!isSmallScreen">Thêm địa điểm</span>
        </v-btn>
        <v-btn flat @click="dialog.addNote = true">
          <v-icon>note_add</v-icon>
          <span v-if="!isSmallScreen">Thêm ghi chú</span>
        </v-btn>
      </v-flex>
      <!--UNSCHEDULED-->
      <v-flex class="grey lighten-4"
              v-if="unScheduledItems && unScheduledItems.length > 0">
        <v-flex my-3>
          <v-layout row>
            <v-flex class="title">Chưa lên lịch</v-flex>
          </v-layout>
        </v-flex>
        <draggable v-model="unScheduledItems" :options="{handle:'.handle-bar', group:'items'}">
          <v-flex elevation-2 my-1
                  v-for="item in unScheduledItems"
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
      <v-flex v-for="(day,index) in days"
              v-if="day"
              :key="index"
              class="grey lighten-4">
        <v-flex class="title" my-2>
          Ngày {{index + 1}}
        </v-flex>
        <draggable v-model="days[index]"
                   :options="{handle:'.handle-bar', group:'items'}">
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
          <v-flex v-if="days[index].length <= 0" style="height: 50px">
            <!--Spacer-->
          </v-flex>
        </draggable>

      </v-flex>
      <v-layout class="title font-weight-bold" justify-center align-center pa-5>
        Chuyến đi của bạn chưa có địa điểm nào
      </v-layout>
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
    <v-layout v-if="pageLoading" column pa-5 justify-center align-center>
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-layout>
    <!--DIALOG-->
    <ChoosePlanDestinationDialog :dialog="dialog.choosePlanDestination"
                                 @select="dialog.choosePlanDestination = false"
                                 @close="dialog.choosePlanDestination = false"/>
  </v-content>
</template>

<script>
  import {mapGetters} from "vuex";
  import LocationFullWidth from "../../common/block/LocationFullWidth";
  import NoteFullWidth from "./NoteFullWidth";
  import ChoosePlanDestinationDialog from "../../common/input/ChoosePlanDestinationDialog";
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
        planId: undefined,
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
    mounted() {
      const {
        id
      } = this.$route.query;
      this.planId = id;
      this.$store.dispatch('plan/fetchPlanById', {
        id
      });
    },
    computed: {
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
      ...mapGetters('plan', {
        plan: 'detailedPlan',
        pageLoading: 'detailedPlanLoading'
      }),
      unScheduledItems() {
        return []
      },
      days() {
        return []
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
