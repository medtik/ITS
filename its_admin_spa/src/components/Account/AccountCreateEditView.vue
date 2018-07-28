<template>
  <v-container id="content" fluid>
    <v-layout row pa-3 style="background-color: white" elevation-4>
      <v-flex xs12>
        <span class=title v-if="mode == 'create'">Tạo mới tài khoản</span>
        <span class=title v-if="mode == 'edit'">Chỉnh sửa tài khoản</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear v-if="loading.page" color="primary" indeterminate></v-progress-linear>
        <v-layout column v-else>
          <v-flex>
            <PictureInput
              v-model="photoInput"
              v-bind="{
                width:300,
                height:300,
                size:50,
                text: 'Ảnh đại diện'
              }"
            />
          </v-flex>
          <v-flex style="width: 25rem">
            <v-text-field label="Tên" v-model="nameInput"></v-text-field>
            <v-text-field label="Email" v-model="emailInput"></v-text-field>
            <v-text-field label="Điện thoại" v-model="phoneInput"></v-text-field>
            <v-text-field label="Địa chỉ" v-model="addressInput"></v-text-field>
            <v-text-field label="Ngày sinh" v-model="birthdateBrowerFormat"
                          type="date"></v-text-field>
          </v-flex>

          <v-flex>
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
  import {PictureInput} from '../../common/input';
  import {
    ErrorDialog,
    SuccessDialog
  } from "../../common/block";


  export default {
    name: "AccountCreateEditView",
    components: {
      PictureInput,
      SuccessDialog,
      ErrorDialog
    },
    data() {
      return {
        loading: {
          page: true,
          updateBtn: false,
          createBtn: false
        },
        mode: 'create',
        accountId: '',
        account: undefined,
        nameInput: '',
        emailInput: '',
        addressInput: '',
        phoneInput: '',
        birthdateInput: '',
        photoInput: '',
        // set image to this when want to add picture
        photoPrefill: undefined,
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
    computed: {
      birthdateBrowerFormat: {
        get: function () {
          return moment(this.birthdateInput, 'DD/MM/YYYY').format('YYYY-MM-DD');
        },
        set: function (newValue) {
          this.birthdateInput = moment(newValue, 'YYYY-MM-DD').format('DD/MM/YYYY');
        }
      }
    },
    created() {
      if (this.$route.name === 'AccountEdit') {
        if (this.$route.query) {
          this.accountId = this.$route.query.id;
          this.mode = 'edit';
          this.$store.dispatch('account/getById', {
            id: this.accountId
          })
            .then(value => {
              this.account = value;
              this.fillInputs()
                .then(() => {
                  this.loading.page = false;
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
      } else {
        this.loading.page = false;
      }
    },
    methods: {
      async fillInputs() {
        return new Promise((resolve, reject) => {
          if (this.mode == 'edit' && this.account) {
            this.nameInput = this.account.name;
            this.emailInput = this.account.email;
            this.addressInput = this.account.address;
            this.birthdateInput = this.account.birthdate;
            this.phoneInput = this.account.phone;
            this.photoInput = this.account.photo;
            resolve();
          } else {
            reject();
          }
        })
      },
      onCreateClick() {
        this.loading.createBtn = true;
        this.$store.dispatch('account/create', {
          name: this.nameInput,
          email: this.emailInput,
          phone: this.phoneInput,
          address: this.addressInput,
          birthdate: this.birthdateInput,
          photo: this.photoInput
        })
          .then(value => {
            this.success = {
              dialog: true,
              message:'Tạo mới thành công'
            };

            this.loading.createBtn = false;
          })
          .catch(error => {
            this.loading.createBtn = false;
            this.error = {
              dialog: true,
              message: error.message
            }
          })
      },
      onUpdateClick() {
        this.loading.updateBtn = true;
        this.$store.dispatch('account/update', {
          id: this.accountId,
          name: this.nameInput,
          email: this.emailInput,
          phone: this.phoneInput,
          address: this.addressInput,
          birthdate: this.birthdateInput,
          photo: this.photoInput
        })
          .then(value => {
            this.success = {
              dialog: true,
              message:'Cập nhật thành công'
            };

            this.loading.updateBtn = false;
          })
          .catch(error => {
            this.loading.updateBtn = false;
            this.error = {
              dialog: true,
              message: error.message
            }
          })
      },
      onExitClick() {
        this.$router.back();
      },
      base64toFile(url, filename = 'randomname', mimeType) {

        mimeType = mimeType || (url.match(/^data:([^;]+);/) || '')[1];
        return (fetch(url)
            .then(function (res) {
              return res.arrayBuffer();
            })
            .then(function (buf) {
              return new File([buf], filename, {type: mimeType});
            })
        );
      }
    }
  }
</script>

<style scoped>

</style>
