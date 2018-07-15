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
              />
              <v-text-field
                label="Mật khẩu"
                v-model="input.password"
              />
              <v-text-field
                label="Tên"
                v-model="input.name"
              />
              <v-text-field
                label="Địa chỉ"
                v-model="input.address"
              />
              <v-text-field
                label="Điện thoại"
                v-model="input.phone"
              />
              <v-text-field
                type="date"
                label="Ngày sinh"
                v-model="input.birthdate"
              />
            </v-flex>
            <v-flex class="title">
              <v-btn color="success" @click="signup">
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
  </v-content>
</template>

<script>
  export default {
    name: "SignupView",
    data() {
      return {
        input: {
          email: undefined,
          password: undefined,
          name: undefined,
          address: undefined,
          phone: undefined,
          birthdate: undefined,
        }
      }
    },
    methods: {
      signup() {

      },
      cancel() {
        this.$router.push({
          name: 'Home'
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
    top: -10%;
  }
</style>
