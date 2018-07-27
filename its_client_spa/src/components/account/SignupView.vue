<template>
  <v-content id="content-holder">
    <v-layout justify-center align-center id="layout">
      <v-card id="form">
        <v-card-title class="light-blue white--text">
          <v-layout column>
            <div class="display-2 font-weight-black font-italic text-xs-center">
              ITS
            </div>
            <v-divider class="my-3"/>
            <div class="title font-weight-medium text-xs-center">
              Hệ thống hướng dẫn du lịch thông minh
            </div>
          </v-layout>
        </v-card-title>
        <v-card-text>
          <v-layout column>
            <v-flex class="title">
              <v-text-field
                required
                label="Email"
                v-model="input.email"
                :error-messages="errorMessages.email"
              />
              <v-text-field
                label="Mật khẩu"
                v-model="input.password"
                type="password"
                :error-messages="errorMessages.password"
              />
              <v-text-field
                label="Nhập lại mật khẩu"
                v-model="input.rePassword"
                type="password"
                :error-messages="errorMessages.rePassword"
              />
              <v-text-field
                label="Tên"
                v-model="input.name"
                :error-messages="errorMessages.name"
              />
              <v-text-field
                label="Địa chỉ"
                v-model="input.address"
                :error-messages="errorMessages.address"
              />
              <v-text-field
                label="Điện thoại"
                v-model="input.phone"
                :error-messages="errorMessages.phone"
              />
              <v-text-field
                type="date"
                label="Ngày sinh"
                v-model="input.birthdate"
                :error-messages="errorMessages.birthdate"
              />
            </v-flex>
            <v-flex class="title">
              <v-btn color="success"
                     @click="signup"
                     :loading="loading.signupBtn">
                Đăng ký
              </v-btn>
              <v-btn color="secondary" @click="cancel">
                Hủy
              </v-btn>
            </v-flex>
          </v-layout>
        </v-card-text>
      </v-card>
    </v-layout>
    <SuccessDialog v-bind="successDialog" @close="successDialog.dialog = false"/>
    <ErrorDialog v-bind="errorDialog" @close="errorDialog.dialog = false"/>
  </v-content>
</template>

<script>
  import SuccessDialog from "../../common/block/SuccessDialog"
  import ErrorDialog from "../../common/block/ErrorDialog"

  export default {
    name: "SignupView",
    components: {
      SuccessDialog,
      ErrorDialog
    },
    data() {
      return {
        loading: {
          signupBtn: false
        },
        input: {
          email: undefined,
          password: undefined,
          rePassword: undefined,
          name: undefined,
          address: undefined,
          phone: undefined,
          birthdate: undefined,
        },
        errorMessages: {
          email: undefined,
          password: undefined,
          rePassword: undefined,
          name: undefined,
          address: undefined,
          phone: undefined,
          birthdate: undefined,
        },
        successDialog: {},
        errorDialog: {}
      }
    },
    methods: {
      signup() {
        this.loading.signupBtn = true;
        this.$store.dispatch('account/signup', {
          ...this.input
        })
          .then(value => {
            this.successDialog = {
              dialog: true,
              message: 'Tạo tài khoản thành công !'
            };
            this.loading.signupBtn = false;
          })
          .catch(reason => {
            if (reason) {
              switch (reason.status) {
                case 400:
                  this.errorMessages = {
                    ...reason.error
                  };
                  break;
                default:
                  this.errorDialog = {
                    dialog: true,
                    message: 'Có lỗi xẩy ra'
                  };
                  break;
              }
              this.loading.signupBtn = false;
            }
          })
      },
      cancel() {
        this.$router.push({
          name: 'Home'
        })
      },
      clearErrorMessages() {
        for (let key in this.errorMessages) {
          if (this.errorMessages.hasOwnProperty(key)) {
            this.errorMessages[key] = undefined;
          }
        }
      }
    }
  }
</script>

<style scoped>
  #content-holder {
    background-image: url("../../../static/pexels-photo-239520.jpeg");
    background-size: cover;
    background-position: center center;
  }

  #layout {
    height: 100vh;
  }

  #form {
    width: 100%;
    max-width: 500px;
    position: relative;
    top: -10%;
  }
</style>
