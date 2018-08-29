<template>
  <section id="content">
    <v-container fluid v-if="!loading.page">
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
                  :error='!!formError["data.Name"]' :error-messages="formError['data.Name']"
                />
                <v-text-field
                  label="Địa chỉ"
                  v-model="input.addressInput"
                  :error="!!formError['data.Address']" :error-messages="formError['data.Address']"
                />
                <v-textarea
                  label="Mô tả"
                  v-model="input.descriptionInput"
                  :error="!!formError['data.Description']" :error-messages="formError['data.Description']"
                />
                <v-layout row wrap>
                  <v-flex xs12 md6>
                    <v-text-field
                      label="Vĩ độ"
                      v-model="input.latInput"
                      :error="!!formError['data.Latitude']" :error-messages="formError['data.Latitude']"
                    />
                  </v-flex>
                  <v-flex xs12 md6>
                    <v-text-field
                      label="Kinh độ"
                      v-model="input.longInput"
                      :error="!!formError['data.Longitude']" :error-messages="formError['data.Longitude']"
                    />
                  </v-flex>
                </v-layout>
                <v-text-field
                  label="Web"
                  v-model="input.websiteInput"
                  :error="!!formError['data.Website']" :error-messages="formError['data.Website']"
                />
                <v-text-field
                  label="Điện thoại"
                  v-model="input.phoneInput"
                  :error="!!formError['data.PhoneNumber']" :error-messages="formError['data.PhoneNumber']"

                />
                <v-text-field
                  label="Email"
                  v-model="input.emailInput"
                  :error="!!formError['data.EmailAddress']" :error-messages="formError['data.EmailAddress']"

                />
                <AreaInput
                  v-model="input.areaInput"
                  :error="!!formError['data.AreaId']" :errorMessages="formError['data.AreaId']"

                ></AreaInput>
                <LocationCategoryInput
                  v-model="input.category"
                  :error="!!formError['data.Category']" :error-messages="formError['data.Category']"

                />
              </v-flex>
            </v-flex>
            <v-flex my-3>
              <span class="subheading">Tình trạng</span>
              <v-flex pl-3>
                <v-switch label="Đã xác nhận" color="green" v-model="input.isVerifiedInput"></v-switch>
                <v-switch label="Ngừng kinh doanh" color="red" v-model="input.isCloseInput"></v-switch>
              </v-flex>
            </v-flex>
            <v-flex my-3>
              <span class="subheading">Thời gian hoạt động</span>
              <v-flex pl-3 mt-2>
                <LocationBusinessHoursInput
                  v-model="input.businessHoursInput"
                />
              </v-flex>
              <v-alert
                v-if="!!formError['']"
                :value="!!formError['']"
                type="error"
              >
               {{formError[''][0]}}
              </v-alert>
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
                <v-alert
                  :value="!!formError.reviews"
                  type="error"
                >
                  {{formError.reviews}}
                </v-alert>
              </v-flex>
            </v-flex>
            <v-flex my-3>
              <span class="subheading">Thẻ</span>
              <v-flex pl-3>
                <TagsInput
                  v-model="input.tagsInput"
                  :admin="true"
                  @create="createEditTag.dialog = true"
                />
                <v-alert
                  :value="!!formError.tags"
                  type="error"
                >
                  {{formError.tags}}
                </v-alert>
              </v-flex>
            </v-flex>
            <v-flex my-3>
              <span class="subheading">Hình ảnh</span>
              <v-layout column pl-3 mt-3>
                <v-label class="subheading">Ảnh bìa</v-label>
                <v-flex>
                  <PictureInput
                    v-model="input.primaryPhotoInput"
                    v-bind="{
                width:300,
                height:300,
                size:50,
                text: 'Ảnh bìa',
                center: false
              }"
                  />
                </v-flex>
                <v-alert
                  :value="!!formError.primaryPhoto"
                  type="error"
                >
                  {{formError.primaryPhoto}}
                </v-alert>
              </v-layout>
              <v-flex pl-3 mt-3>
                <v-label class="subheading">Ảnh thêm</v-label>
                <MultiPhotoInput v-model="input.secondaryPhotos"/>
                <v-alert
                  :value="!!formError.secondaryPhotos"
                  type="error"
                >
                  {{formError.primaryPhoto}}
                </v-alert>
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
      <TagCreateEditDialog v-bind="createEditTag" v-on:close="createEditTag.dialog = false" @create="onCreateTag"/>
      <ErrorDialog v-bind="error" v-on:close="error.dialog = false"/>
      <SuccessDialog v-bind="success" v-on:close="success.dialog = false"/>
    </v-container>
    <v-container class="text-xs-center" v-if="loading.page">
      <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
    </v-container>
  </section>
</template>

<script>
  import TagCreateEditDialog from "../Tag/TagCreateEditDialog"
  import _ from 'lodash';
  import {
    ErrorDialog,
    SuccessDialog,
    LocationReview,
  } from "../../common/block";

  import {
    PictureInput,
    TagsInput,
    LocationBusinessHoursInput,
    MultiPhotoInput,
    LocationCategoryInput,
    AreaInput
  } from "../../common/input";

  export default {
    name: "LocationCreateEditView",
    components: {
      LocationReview,
      ErrorDialog,
      SuccessDialog,
      PictureInput,
      TagsInput,
      LocationBusinessHoursInput,
      TagCreateEditDialog,
      MultiPhotoInput,
      LocationCategoryInput,
      AreaInput
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
          category: undefined,
          isVerifiedInput: undefined,
          isCloseInput: undefined,
          tagsInput: [],
          reviewsInput: [],
          businessHoursInput: undefined,
          primaryPhotoInput: undefined,
          secondaryPhotoInput: undefined,
          secondaryPhotos: undefined,
        },
        formError: {
          ['data.Name']: undefined,
          ['data.Address']: undefined,
          ['data.Description']: undefined,
          ['data.Longitude']: undefined,
          ['data.Latitude']: undefined,
          ['data.Website']: undefined,
          ['data.PhoneNumber']: undefined,
          ['data.EmailAddress']: undefined,
          ['data.AreaId']: undefined,
          ['data.Category']: undefined,
          ['data.Tags']: undefined,
          ['data.PrimaryPhoto']: undefined,
          ['data.OtherPhotos']: undefined,
          ['']: undefined,
        },
        //Dialog
        createEditTag: {
          dialog: false,
        },
        error: {
          dialog: false,
          title: '',
          message: ''
        },
        success: {
          dialog: false,
          title: '',
          message: ''
        }
      }
    },
    mounted() {
      const {
        name,
        query
      } = this.$route;

      if (name == "LocationEdit" && query.id) {
        if (query.id) {
          this.loadingId = query.id;
          this.mode = 'edit';
          this.loading.page = true;

          this.$store.dispatch('location/getById', {
            ...query
          })
            .then(location => {
              this.setInput(location)
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
          this.input.category = location.category;
          this.input.descriptionInput = location.description;
          this.input.longInput = location.long;
          this.input.latInput = location.lat;
          this.input.websiteInput = location.website;
          this.input.phoneInput = location.phone;
          this.input.emailInput = location.emailAddress;
          this.input.areaInput = location.area;
          this.input.tagsInput = location.tags;
          this.input.businessHoursInput = location.businessHours;
          this.input.isVerifiedInput = location.isVerified;
          this.input.isCloseInput = location.isClose;
          this.input.primaryPhotoInput = location.primaryPhoto;
          this.input.secondaryPhotos = location.otherPhotos;
          this.input.reviewsInput = location.comments;
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
      onCreateTag(item) {
        this.$store.dispatch('tag/create', {tag: item})
          .then(value => {
            this.$store.dispatch('tagDialog/getAll');
          })
          .catch(reason => {
            console.debug('onDialogConfirmCreate-catch', reason);
            this.error = {
              dialog: true,
              message: 'Có lỗi xảy ra'
            }
          });
        this.createEditTag.dialog = true;
      },
      removeSecondaryPhoto(index) {
        this.input.secondaryPhotos.splice(index, 1);
      },
      onReviewRemove(reviewId) {
        this.input.reviewsInput = this.input.reviewsInput
          .filter(review => review.id != reviewId)
      },
      onUpdateClick() {
        this.$store.dispatch('location/update', {...this.input, id: this.loadingId})
          .then(value => {
            this.success = {
              dialog: true,
              message: "Cập nhật địa điểm thành công"
            }
          })
          .catch(errors => {
            this.formError = _.cloneDeep(errors);
          })
      },
      onCreateClick() {
        this.$store.dispatch('location/create', {...this.input})
          .then(value => {
            this.success = {
              dialog: true,
              message: "Tạo mới địa điểm thành công"
            }
          })
          .catch(errors => {
            this.formError = _.cloneDeep(errors);
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
