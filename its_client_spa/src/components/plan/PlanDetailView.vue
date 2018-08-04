<template>
  <v-content>
    <v-toolbar v-if="!pageLoading" flat
               color="light-blue darken-2" dark>
      <v-toolbar-title class="headline">
        {{plan.name}}
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-btn flat @click="dialog.choosePlanDestination = true"
               :loading="addPlanToGroupLoading">
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
      <v-toolbar-items slot="extension" v-if="plan.days.length > 1">
        <!--TABS-->
        <v-tabs show-arrows color="light-blue darken-2"
                next-icon="fas fa-angle-right"
                prev-icon="fas fa-angle-left"
                grow>
          <v-tabs-slider color="light-blue accent"></v-tabs-slider>
          <v-tab
            v-for="(day) in plan.days"
            :key="'tab_' + day.key"
          >
            {{day.planDayText}}
          </v-tab>
        </v-tabs>
      </v-toolbar-items>
    </v-toolbar>

    <v-layout v-if="!pageLoading" column class="white">
      <!--DAYS-->
      <v-flex v-for="(day,index) in plan.days"
              :key="day.key"
              class="light-blue lighten-5">
        <v-divider></v-divider>
        <v-flex class="title text-xs-center white" pb-2 pt-4>
          <span :id="'item_'+day.key">{{day.planDayText}}</span>
          <v-flex>
            <v-btn flat @click="onAddLocation(day)">
              <v-icon>add_location</v-icon>
              <span v-if="!isSmallScreen">Thêm địa điểm</span>
            </v-btn>
            <v-btn flat @click="dialog.addNote = true">
              <v-icon>note_add</v-icon>
              <span v-if="!isSmallScreen">Thêm ghi chú</span>
            </v-btn>
          </v-flex>
        </v-flex>
        <v-flex pb-3
                v-for="item in day.items"
                :key="item.id"
                class="white"
        >
          <LocationFullWidth v-if="item.location"
                             v-bind="item.location"
                             :isOwn="true">
            <v-icon slot="handle" class="handle-bar">reorder</v-icon>
          </LocationFullWidth>
          <NoteFullWidth v-else v-bind="item.note">
            <v-icon slot="handle" class="handle-bar">reorder</v-icon>
          </NoteFullWidth>
        </v-flex>
        <v-flex v-if="plan.days[index].length <= 0" style="height: 50px">
          <!--Spacer-->
        </v-flex>

      </v-flex>
      <v-layout v-if="!plan.days"
                class="title font-weight-bold"
                justify-center align-center pa-5>
        Chuyến đi của bạn đang trống
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
                                 @select="onAddToGroupSelected"
                                 @close="dialog.choosePlanDestination = false"/>
    <SearchMethodDialog
      :dialog="dialog.chooseSearchMethod"
      @select="onSearchMethodChoose"
      @close="dialog.chooseSearchMethod = false"
    ></SearchMethodDialog>
    <SuccessDialog
      v-bind="success"
      @close="success.dialog = false"
    ></SuccessDialog>
  </v-content>
</template>

<script>
  import {mapGetters} from "vuex";
  import _ from "lodash";

  import NoteFullWidth from "./NoteFullWidth";
  import SearchMethodDialog from "../search/SearchMethodDialog";

  import {ChoosePlanDestinationDialog} from "../../common/input";
  import draggable from 'vuedraggable'
  import moment from "moment";
  import {
    SuccessDialog,
    LocationFullWidth
  } from "../../common/block";


  export default {
    name: "PlanDetailView",
    components: {
      LocationFullWidth,
      NoteFullWidth,
      ChoosePlanDestinationDialog,
      draggable,
      SuccessDialog,
      SearchMethodDialog
    },
    data() {
      return {
        planId: undefined,
        loading: {
          createNoteBtn: false,
          publishBtn: false,
          searchMethod: false
        },
        success: {
          dialog: false,
          message: ''
        },
        dialog: {
          addNote: false,
          publishPlan: false,
          choosePlanDestination: false,
          chooseSearchMethod: false
        },
        items: [],
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
      ...mapGetters('group', {
        addPlanToGroupLoading: 'addPlanToGroupLoading'
      }),
      formattedStartDate() {
        return moment(this.plan.startDate).format('DD/MM/YYYY');
      },
      formattedEndDate() {
        return moment(this.plan.endDate).format('DD/MM/YYYY');
      }
    },
    methods: {
      onAddToGroupSelected(payload) {
        const {
          group
        } = payload;
        this.dialog.choosePlanDestination = false;
        this.$store.dispatch('group/addPlanToGroup', {
          planId: this.planId,
          groupId: group.id
        })
          .then((value) => {
            this.success = {
              dialog: true,
              message: `Thêm chuyến đi hiện tại vào nhóm ${group.name} thành công.`
            }
          })
      },
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
      },
      onAddLocation(day) {
        this.dialog.chooseSearchMethod = true;
        this.$store.commit('search/setSearchContext', {
          context: {
            plan: this.plan,
            day: day
          }
        });
      },
      onSearchMethodChoose(searchMethod) {
        if (searchMethod == 'smart') {

        } else if (searchMethod == 'normal') {

        }
      }
    }
  }
</script>
