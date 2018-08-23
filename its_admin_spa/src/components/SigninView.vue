<template>
  <v-container>
    <v-layout row justify-center style="margin-top: 10%">
      <v-flex xs12 md4>
        <v-card id="content">
          <v-layout column>
            <v-flex>
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
            </v-flex>
            <v-flex px-3 mb-1>
              <v-text-field label="Email"
                            v-model="emailInput"/>
              <v-text-field label="Mật khẩu"
                            v-model="passwordInput"
                            type="password"/>
              <v-btn color="primary" block @click="signIn" :loading="loading.signinBtn">
                Đăng nhập
              </v-btn>
              <v-alert
                v-model="error.show"
                dismissible
                type="error"
              >
                {{error.message}}
              </v-alert>
            </v-flex>
          </v-layout>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  export default {
    name: "SigninView",
    data() {
      return {
        loading: {
          signinBtn: false
        },
        emailInput: 'admin@bao.com',
        passwordInput: '123456',
        error: {
          show: false,
          message: ''
        }
      }
    },
    methods: {
      signIn() {
        this.loading.signinBtn = true;
        this.$store.dispatch('authenticate/fetchToken', {
          email: this.emailInput,
          password: this.passwordInput
        })
          .then(value => {
            if(value.role != "User"){
              this.$store.commit('authenticate/setToken', {token: value});
              this.$router.push({
                name: 'AccountList'
              });
            }else{
              this.error = {
                show: true,
                message: "Bạn không có quyền truy cập hệ thống"
              }
              this.loading.signinBtn = false;
            }
          })
          .catch(reason => {
            this.error = {
              show: true,
              message: reason.message
            };
            this.loading.signinBtn = false;
          })
      },
    }
  }
</script>
