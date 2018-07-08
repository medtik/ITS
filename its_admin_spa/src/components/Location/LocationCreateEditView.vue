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
                v-model="nameInput"
              />
              <v-text-field
                label="Địa chỉ"
                v-model="addressInput"
              />
              <v-textarea
                label="Mô tả"
                v-model="descriptionInput"
              />
              <v-text-field
                label="Kinh độ"
                v-model="longInput"
              />
              <v-text-field
                label="Vĩ độ"
                v-model="latInput"
              />
              <v-text-field
                label="Web"
                v-model="websiteInput"
              />
              <v-text-field
                label="Điện thoại"
                v-model="phoneInput"
              />
              <v-text-field
                label="Email"
                v-model="emailInput"
              />
              <v-select
                :items="['Hà nội','Tp.Hồ chí minh']"
                v-model="areaInput"
                label="Khu vực"
              />
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Tình trạng</span>
            <v-flex pl-3>
              <v-switch label="Đã xác nhận" color="green" v-model="isVerifiedInput"></v-switch>
              <v-switch label="Đóng cửa" color="red" v-model="isCloseInput"></v-switch>
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Thời gian hoạt động</span>
            <v-flex pl-3 mt-2>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Chủ nhật</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day1.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day1.to"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Thứ 2</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day2.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day2.to"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Thứ 3</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day3.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day3.to"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Thứ 4</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day4.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day4.to"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Thứ 5</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day4.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day4.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Thứ 6</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day6.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day6.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
              <v-layout row style="align-items: baseline">
                <v-flex xs2>
                  <v-label>Thứ 7</v-label>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Mở cửa"
                    v-model="day7.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
                <v-flex>
                  <v-text-field
                    label="Đóng cửa"
                    v-model="day7.from"
                    placeholder="Giờ:phút"/>
                </v-flex>
              </v-layout>
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Đánh giá</span>
            <v-flex pl-3>
              <LocationReview
                v-for="review in reviewsInput"
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
                v-model="tagsInput"
                v-bind="tagChooseDialog"
                v-on:addTag="tagChooseDialog.dialog = true"
              />
            </v-flex>
          </v-flex>
          <v-flex my-3>
            <span class="subheading">Hình ảnh</span>
            <v-flex pl-3 mt-3>
              <v-label class="subheading">Ảnh bìa</v-label>
              <PictureInput
                v-model="primaryPhotoInput"
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
                <v-layout v-if="secondaryPhotos"
                          mb-4
                          wrap
                          row>
                  <v-flex v-for="photo in secondaryPhotos"
                          ma-2
                          style="flex-grow: 0.05"
                          :key="photo.id">
                    <v-card>
                      <v-card-media>
                        <img :src="photo.url" width="200" height="200"/>
                      </v-card-media>
                      <v-card-actions>
                        <v-btn color="red" flat block>
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
                  v-model="secondaryPhotoInput"
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
  import PictureInput from '../shared/PictureInput'
  import ErrorDialog from "../shared/ErrorDialog";
  import SuccessDialog from "../shared/SuccessDialog";
  import LocationReview from "./LocationReview";
  import TagManageSection from "../shared/TagManageSection";

  export default {
    name: "LocationCreateEditView",
    components: {
      LocationReview,
      ErrorDialog,
      SuccessDialog,
      PictureInput,
      TagManageSection
    },
    data() {
      return {
        mode: 'create',
        loading: {
          page: false,
          createBtn: false,
          updateBtn: false
        },
        location: {},
        //Basic Inputs
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
        primaryPhotoInput: undefined,
        secondaryPhotoInput: undefined,
        secondaryPhotos: undefined,
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
    methods: {
      setInput(location) {
        return new Promise((resolve, reject) => {
          this.nameInput = location.name;
          this.addressInput = location.address;
          this.descriptionInput = location.description;
          this.longInput = location.long;
          this.latInput = location.lat;
          this.websiteInput = location.website;
          this.phoneInput = location.phone;
          this.emailInput = location.email;
          this.areaInput = 'Hà nội';
          this.tagsInput = location.tags;
          this.day1 = _.extend({}, location.businessHours[0]);
          this.day2 = _.extend({}, location.businessHours[1]);
          this.day3 = _.extend({}, location.businessHours[2]);
          this.day4 = _.extend({}, location.businessHours[3]);
          this.day5 = _.extend({}, location.businessHours[4]);
          this.day6 = _.extend({}, location.businessHours[5]);
          this.day7 = _.extend({}, location.businessHours[6]);
          this.isVerifiedInput = location.isVerified;
          this.isCloseInput = location.isClose;
          this.primaryPhotoInput = location.primaryPhoto.url;
          this.secondaryPhotos = location.photos;
          this.reviewsInput = location.reviews;
          this.nameInput = location.name;
          resolve();
        })
      },
      secondaryConfirm(val) {
        this.secondaryPhotos.push({
          url: val
        });
        this.secondaryPhotoInput = undefined;
      },
      onReviewRemove(reviewId) {
        this.reviewsInput = this.reviewsInput
          .filter(review => review.id != reviewId)
      },
      onUpdateClick() {

      },
      onCreateClick() {

      },
      onExitClick() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
