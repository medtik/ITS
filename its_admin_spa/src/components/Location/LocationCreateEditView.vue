<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class=title v-if="mode == 'create'">Tạo mới địa điểm</span>
        <span class=title v-if="mode == 'edit'">Chỉnh sửa địa điểm</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear v-if="loading.page" color="primary" indeterminate></v-progress-linear>
        <v-layout column v-else>
          <v-flex my-3>
            <span class="subheading">Thông tin cơ bản</span>
            <v-flex pl-3>
              <v-text-field
                label="Tên"
                v-model="input.nameInput"
              />
              <v-text-field
                label="Địa chỉ"
                v-model="input.addressInput"
              />
              <v-textarea
                label="Mô tả"
                v-model="input.descriptionInput"
              />
              <v-text-field
                label="Kinh độ"
                v-model="input.longInput"
              />
              <v-text-field
                label="Vĩ độ"
                v-model="input.latInput"
              />
              <v-text-field
                label="Web"
                v-model="input.websiteInput"
              />
              <v-text-field
                label="Điện thoại"
                v-model="input.phoneInput"
              />
              <v-text-field
                label="Email"
                v-model="input.emailInput"
              />
              <v-select
                :items="areas"
                :loading="loading.areas"
                v-model="input.areaInput"
                label="Khu vực"
                item-text="name"
                item-value="id"
              />
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Tình trạng</span>
            <v-flex pl-3>
              <v-switch label="Đã xác nhận" color="green" v-model="input.isVerifiedInput"></v-switch>
              <v-switch label="Đóng cửa" color="red" v-model="input.isCloseInput"></v-switch>
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Thời gian hoạt động</span>
            <v-flex pl-3 mt-2>
              <LocationBusinessHoursInput
                v-model="input.businessHoursInput"
              />
            </v-flex>
          </v-flex>
          <v-flex my-3 v-if="mode == 'edit'">
            <span class="subheading">Đánh giá</span>
            <v-flex pl-3>
              <LocationReview
                v-for="review in input.reviewsInput"
                v-bind="review"
                v-on:delete="onReviewRemove"
                :editMode="true"
                :key="review.id"
              />
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Thẻ</span>
            <v-flex pl-3>
              <TagManageSection
                v-model="input.tagsInput"
                v-on:addTag="tagChooseDialog.dialog = true"
                v-on:close="tagChooseDialog.dialog = false"
              />
              <TagChooseDialog
                v-bind="tagChooseDialog"
                v-model="input.tagsInput"
                v-on:save="tagChooseDialog.dialog = false"
                v-on:close="tagChooseDialog.dialog = false"/>
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Hình ảnh</span>
            <v-flex pl-3 mt-3>
              <v-label class="subheading">Ảnh bìa</v-label>
              <PictureInput
                v-model="input.primaryPhotoInput"
                v-bind="{
                width:300,
                height:300,
                size:50,
                text: 'Ảnh bìa'
              }"
              />
            </v-flex>

            <v-flex pl-3 mt-3>
              <v-label class="subheading">Ảnh thêm</v-label>
              <v-flex pl-3>
                <v-layout v-if="input.secondaryPhotos"
                          mb-4
                          wrap
                          row>
                  <v-flex v-for="(photo,index) in input.secondaryPhotos"
                          ma-2
                          style="flex-grow: 0.05"
                          :key="photo.id">
                    <v-card>
                      <v-card-media>
                        <img :src="photo.url" width="200" height="200"/>
                      </v-card-media>
                      <v-card-actions>
                        <v-btn color="red" flat block
                               @click="removeSecondaryPhoto(index)">
                          <v-icon color="red"
                                  class="white--text">
                            delete
                          </v-icon>
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-flex>
                </v-layout>
                <PictureInput
                  v-model="input.secondaryPhotoInput"
                  v-bind="{
                width:200,
                height:200,
                size:40,
                text: 'Ảnh thêm'
              }">
                  <v-btn
                    color="success"
                    slot="extraAction"
                    slot-scope="props"
                    v-if="props.value"
                    @click="secondaryConfirm(props.value)"
                    block>
                    <v-icon class="white--text">fas fa-file-upload</v-icon>
                  </v-btn>
                </PictureInput>
              </v-flex>
            </v-flex>
          </v-flex>
          <v-divider/>
          <v-flex mt-3>
            <v-btn color="primary"
                   v-if="mode == 'create'"
                   :loading="this.loading.createBtn"
                   @click="onCreateClick">
              Tạo mới
            </v-btn>
            <v-btn color="success"
                   v-if="mode == 'edit'"
                   :loading="this.loading.updateBtn"
                   @click="onUpdateClick">
              Lưu thay đổi
            </v-btn>
            <v-btn color="secondary"
                   @click="onExitClick">
              Thoát
            </v-btn>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
    <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
    <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
  </v-container>
</template>

<script>
  import moment from 'moment';
  import _ from 'lodash';
  import LocationBusinessHoursInput from "../shared/LocationBusinessHoursInput";
  import PictureInput from '../shared/PictureInput'
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";
  import LocationReview from "../shared/LocationReview";
  import TagManageSection from "../shared/TagManageSection";
  import TagChooseDialog from "../shared/TagChooseDialog";
  import {mapState} from "vuex";


  export default {
    name: "LocationCreateEditView",
    components: {
      LocationReview,
      ErrorDialog,
      SuccessDialog,
      PictureInput,
      TagManageSection,
      TagChooseDialog,
      LocationBusinessHoursInput
    },
    data() {
      return {
        mode: 'create',
        loading: {
          page: false,
          areas: true,
          createBtn: false,
          updateBtn: false
        },
        areas: undefined,
        location: {},
        //Basic Inputs
        input: {
          nameInput: undefined,
          addressInput: undefined,
          descriptionInput: undefined,
          longInput: undefined,
          latInput: undefined,
          websiteInput: undefined,
          phoneInput: undefined,
          emailInput: undefined,
          areaInput: undefined,
          isVerifiedInput: undefined,
          isCloseInput: undefined,
          tagsInput: [],
          reviewsInput: [],
          //BusinessHours inputs
          businessHoursInput: {
            day1: {
              from: undefined,
              to: undefined
            },
            day2: {
              from: undefined,
              to: undefined
            },
            day3: {
              from: undefined,
              to: undefined
            },
            day4: {
              from: undefined,
              to: undefined
            },
            day5: {
              from: undefined,
              to: undefined
            },
            day6: {
              from: undefined,
              to: undefined
            },
            day7: {
              from: undefined,
              to: undefined
            },
          },
          primaryPhotoInput: undefined,
          secondaryPhotoInput: undefined,
          secondaryPhotos: undefined,
        },
        //Dialog
        error: {
          dialog: false,
          title: '',
          message: ''
        },
        success: {
          dialog: false,
          title: '',
          message: ''
        },
        tagChooseDialog: {
          dialog: false
        }
      }
    },
    created() {
      const {
        name,
        query
      } = this.$route;

      if (name == "LocationEdit" && query.id) {
        if (query.id) {
          this.mode = 'edit';
          this.loading.page = true;

          this.$store.dispatch('location/getById', {
            ...query
          })
            .then(value => {
              this.location = value.location;
              this.setInput(value.location)
                .then(() => {
                  this.loading.page = false
                })
            })
            .catch(reason => {
              this.error = {
                dialog: true,
                message: reason.message
              }
            })
        } else {
          this.error = {
            dialog: true,
            message: 'Đường dẫn không hợp lệ'
          }
        }
      }
    },
    mounted() {
      if (!this.areas) {
        this.$store.dispatch('area/getAllNoParam')
          .then(value => {
            this.loading.areas = false;
            this.areas = value.list;
          });
      }
    },
    methods: {
      setInput(location) {
        return new Promise((resolve, reject) => {
          this.input.nameInput = location.name;
          this.input.addressInput = location.address;
          this.input.descriptionInput = location.description;
          this.input.longInput = location.long;
          this.input.latInput = location.lat;
          this.input.websiteInput = location.website;
          this.input.phoneInput = location.phone;
          this.input.emailInput = location.email;
          this.input.areaInput = location.area;
          this.input.tagsInput = location.tags;
          this.input.businessHoursInput.day1 = _.extend({}, location.businessHours[0]);
          this.input.businessHoursInput.day2 = _.extend({}, location.businessHours[1]);
          this.input.businessHoursInput.day3 = _.extend({}, location.businessHours[2]);
          this.input.businessHoursInput.day4 = _.extend({}, location.businessHours[3]);
          this.input.businessHoursInput.day5 = _.extend({}, location.businessHours[4]);
          this.input.businessHoursInput.day6 = _.extend({}, location.businessHours[5]);
          this.input.businessHoursInput.day7 = _.extend({}, location.businessHours[6]);
          this.input.isVerifiedInput = location.isVerified;
          this.input.isCloseInput = location.isClose;
          this.input.primaryPhotoInput = location.primaryPhoto.url;
          this.input.secondaryPhotos = location.photos;
          this.input.reviewsInput = location.reviews;
          resolve();
        })
      },
      secondaryConfirm(val) {
        if (!this.input.secondaryPhotos) {
          this.input.secondaryPhotos = [];
        }
        this.input.secondaryPhotos.push({
          url: val
        });
        this.input.secondaryPhotoInput = undefined;
      },
      removeSecondaryPhoto(index) {
        this.input.secondaryPhotos.splice(index, 1);
      },
      onReviewRemove(reviewId) {
        this.input.reviewsInput = this.input.reviewsInput
          .filter(review => review.id != reviewId)
      },
      onUpdateClick() {

      },
      onCreateClick() {
        this.$store.dispatch('location/create', {...this.input})
          .then(value => {
            this.success = {
              dialog: true,
              message: "Tạo mới địa điểm thành công"
            }
          })
      },
      onExitClick() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
