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
            <v-flex>
              <v-text-field
                label="Email"
                v-model="emailInput"
              />
              <v-text-field
                label="Password"
                type="password"
                v-model="passwordInput"
              />
            </v-flex>
            <v-flex>
              <v-layout wrap row>
                <v-flex xs12>
                  <v-alert
                    v-model="signinAlert.show"
                    dismissible
                    type="error"
                  >
                    {{signinAlert.message}}
                  </v-alert>
                </v-flex>
                <v-flex>
                  <v-layout>
                    <v-flex xs6 d-flex class="justify-center">
                      <v-btn color="primary" @click="signin" :loading="loading.signinBtn">
                        Đăng nhập
                      </v-btn>
                    </v-flex>
                    <v-flex xs6 d-flex class="justify-center">
                      <v-btn color="success" :to="{name: 'Signup'}">
                        Đăng ký
                      </v-btn>
                    </v-flex>
                  </v-layout>
                </v-flex>
                <v-flex xs12>
                  <v-divider class="my-2"/>
                </v-flex>
                <v-flex xs12 lg6 d-flex class="justify-center">
                  <v-btn color="red" dark @click="signinGoogle">
                    <v-icon>fab fa-google</v-icon>
                    &nbsp;&nbsp;
                    Đăng nhập Google
                  </v-btn>
                </v-flex>
                <v-flex xs12 lg6 d-flex class="justify-center">
                  <v-btn color="primary" dark @click="signinFacebook">
                    <v-icon>fab fa-facebook</v-icon>
                    &nbsp;&nbsp;
                    Đăng nhập Facebook
                  </v-btn>
                </v-flex>
              </v-layout>
            </v-flex>
          </v-layout>
        </v-card-text>
      </v-card>
    </v-layout>
  </v-content>
</template>

<script>
  export default {
    name: "SigninView",
    data() {
      return {
        loading: {
          signinBtn: false
        },
        emailInput: undefined,
        passwordInput: undefined,
        error: {},
        signinAlert: {}
      }
    },
    methods: {
      signin() {
        this.loading.signinBtn = true;
        this.$store.dispatch('authenticate/fetchToken', {
          email: this.emailInput,
          password: this.passwordInput
        })
          .then(value => {
            this.$store.commit('authenticate/setToken', {token: value});
            this.$router.push({
              name: 'Home'
            });
          })
          .catch(reason => {
            this.signinAlert = {
              show: true,
              ...reason
            };
            this.loading.signinBtn = false;
          })
      },
      signinGoogle() {
        this.$store.dispatch('account/signinGoogle')
          .then(value => {
            this.$router.push({
              name: 'Home'
            })
          })
          .catch(reason => {
            this.error = {...reason};
          })
      },
      signinFacebook() {
        this.$store.dispatch('account/signinFacebook')
          .then(value => {
            this.$router.push({
              name: 'Home'
            })
          })
          .catch(reason => {
            this.error = {...reason};
          })
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
    top: -15%;
  }
</style>
