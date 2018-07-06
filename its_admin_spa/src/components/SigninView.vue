<template>
  <v-container>
    <v-layout row justify-center style="margin-top: 25%">
      <v-flex xs12 md4>
        <v-card id="content">
          <v-layout column>
            <v-flex>
              <v-card-title class="title">
                ITS
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
            <!--<v-divider></v-divider>-->
            <!--<v-flex id="button-container" pa-2>-->
            <!--<v-btn id="facebookBtn" dark>-->
            <!--<v-icon dark left>fab fa-facebook</v-icon>-->
            <!--Đăng nhập Facebook-->
            <!--</v-btn>-->
            <!--<v-btn id="googleBtn" dark>-->
            <!--<v-icon dark left>fab fa-google</v-icon>-->
            <!--Đăng nhập Google-->
            <!--</v-btn>-->
            <!--</v-flex>-->
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
        emailInput: 'admin@tlp.com',
        passwordInput: '',
        error: {
          show: false,
          message: ''
        }
      }
    },
    methods: {
      signIn() {
        this.loading.signinBtn = true;
        this.$store.dispatch('authenticate/signInEmail', {
          email: this.emailInput,
          password: this.passwordInput
        })
          .then(value => {
            this.$router.push({
              name: 'AccountList'
            });
            this.loading.signinBtn = false;
          })
          .catch(reason => {
            this.error = {
              show: true,
              message: reason.message
            };
            this.loading.signinBtn = false;
          })
      }
    }
  }
</script>

<style scoped>
  #button-container {
    display: flex;
    flex-direction: column;
  }

  #googleBtn {
    background-color: #FF4031;
  }

  #facebookBtn {
    background-color: #3B5998;
  }
</style>
