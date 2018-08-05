<template>
  <v-content>
    <v-container class="text-xs-center" v-if="pageLoading">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
    <v-toolbar v-if="!pageLoading" flat
               dense fixed
               color="light-blue darken-2" dark>
      <!--TOOL BAR TOP ROW-->
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
      <!--TABS-->
      <v-toolbar-items slot="extension" v-if="plan.days.length > 1">
        <v-tabs show-arrows color="light-blue darken-2"
                next-icon="fas fa-angle-right"
                prev-icon="fas fa-angle-left"
                grow>
          <v-tabs-slider color="light-blue accent"></v-tabs-slider>
          <v-tab
            v-for="(day) in plan.days"
            :key="'tab_' + day.key"
            @click="$vuetify.goTo('#tab_item_'+day.key,{offset:-150})"
          >
            {{day.planDayText}}
          </v-tab>
        </v-tabs>
      </v-toolbar-items>
    </v-toolbar>
    <v-flex style="height: 10vh">
      <!--Holder-->
    </v-flex>

    <v-layout v-if="!pageLoading" column class="white" mt-5>
      <v-layout my-3
                column
                class="text-xs-center white">
        <span class="caption">
          khu vực
        </span>
        <span class="headline">
          {{plan.areaName}}
        </span>
      </v-layout>
      <!--DAYS-->
      <v-flex v-for="(day,index) in plan.days"
              :key="day.key"
              class="grey lighten-5">
        <v-divider></v-divider>
        <v-flex class="title text-xs-center white" pb-2 pt-4>
          <span :id="'tab_item_'+day.key">{{day.planDayText}}</span>
          <v-flex>
            <v-btn flat @click="onAddLocation(day)">
              <v-icon>add_location</v-icon>
              <span v-if="!isSmallScreen">Thêm địa điểm</span>
            </v-btn>
            <v-btn flat @click="onAddNote(day)">
              <v-icon>note_add</v-icon>
              <span v-if="!isSmallScreen">Thêm ghi chú</span>
            </v-btn>
          </v-flex>
        </v-flex>
        <!--ITEMS-->
        <v-flex py-2 mb-1
                v-for="item in day.items"
                :key="item.id"
                class="white">
          <LocationFullWidth v-if="item.location"
                             v-bind="item.location"
                             :isOwn="true"
                             @delete="onLocationDelete(item)">

          </LocationFullWidth>
          <NoteFullWidth v-else v-bind="item.note"
                         @delete="onNoteDelete(item)">
          </NoteFullWidth>
        </v-flex>
        <!--SPACER-->
        <v-flex v-if="plan.days[index].length <= 0" style="height: 50px">
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

    <!--DIALOGS-->
    <v-dialog v-model="addNoteDialog.dialog" max-width="450" persistent>
      <!--ADD NOTE-->
      <v-card>
        <v-card-title class="light-blue title white--text">
          Thêm ghi chú
        </v-card-title>
        <v-card-text>
          <v-layout column>
            <v-flex>
              <v-form ref="addNoteForm">
                <v-text-field label="Tiêu đề" v-model="addNoteDialog.titleInput" :rules="[rules.title]"/>
                <v-textarea label="Nội dung" v-model="addNoteDialog.descriptionInput"/>
              </v-form>
            </v-flex>
            <v-divider/>
            <v-flex>
              <v-btn color="success"
                     :loading="loading.createNoteBtn"
                     @click="onAddNoteConfirm">
                Tạo
              </v-btn>
              <v-btn color="secondary"
                     @click="addNoteDialog.dialog = false">
                Hủy
              </v-btn>
            </v-flex>
          </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
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
  import {FormRuleMixin} from "../../common/mixin"
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
    mixins: [FormRuleMixin],
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
        addNoteDialog: {
          dialog: false,
          day: undefined,
          titleInput: undefined,
          descriptionInput: undefined
        },
        items: [],
      }
    },
    mounted() {
      const {
        id
      } = this.$route.query;
      this.planId = id;
      this.loadData();
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
      loadData() {
        this.$store.dispatch('plan/fetchPlanById', {
          id: this.planId
        });
      },
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
      onAddNote(day) {
        this.addNoteDialog = {
          dialog: true,
          day: day
        }
      },
      onAddNoteConfirm() {
        this.loading.createNoteBtn = true;
        if (!this.$refs.addNoteForm.validate()) {
          return;
        }

        this.$store.dispatch('plan/addNoteToPlan', {
          title: this.addNoteDialog.titleInput,
          content: this.addNoteDialog.descriptionInput,
          planDay: this.addNoteDialog.day.planDay,
          planId: this.plan.id
        })
          .then(() => {
            this.loadData();
          })


      },
      onNoteDelete(noteId) {
        this.$store.dispatch('plan/removeNoteFromPlan',{
          id: noteId
        })
          .then(() => {
            this.loadData();
          })
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
        this.$store.commit('searchContext', {
          context: {
            plan: this.plan,
            day: day
          }
        });
      },
      onLocationDelete(item) {
        this.$store.dispatch('plan/removeLocationFromPlan', {
          itemId: item.id
        });
      },
      onSearchMethodChoose(searchMethod) {
        if (searchMethod == 'smart') {
          this.$router.push({
            name: "SmartSearch"
          });
        } else if (searchMethod == 'normal') {
          this.$router.push({
            name: "Search"
          });
        }
      }
    }
  }
</script>
