<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Thông tin cá nhân"
    />
    <v-layout my-3 mx-2 column>
      <!--Separate edit view-->
      <v-flex>
        <PictureInput
          v-model="input.photo"
          width="300"
          height="300"
          size="50"
          text="Ảnh đại diện"
        />
      </v-flex>
      <v-flex>
        <v-layout column>
          <v-text-field label="Tên"
                        :readonly="!editMode"
                        v-model="input.name"/>
          <v-text-field label="Email"
                        :readonly="!editMode"
                        v-model="input.email"/>
          <v-text-field label="Điện thoại"
                        :readonly="!editMode"
                        v-model="input.phone"/>
          <v-text-field label="Địa chỉ"
                        :readonly="!editMode"
                        v-model="input.address"/>
          <v-text-field label="Ngày sinh"
                        type="date"
                        :readonly="!editMode"
                        v-model="input.birthdate"/>
        </v-layout>
      </v-flex>
      <v-flex>
        <v-btn color="success">Cập nhật</v-btn>
        <v-btn color="success">Đổi mật khẩu</v-btn>
        <v-btn color="secondary" @click="signout">Đăng xuất</v-btn>
      </v-flex>
      <v-flex style="height: 30vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import PictureInput from "../../sharedComponents/input/PictureInput";
  import ParallaxHeader from "../../sharedComponents/layout/ParallaxHeader";
  import {mapGetters, mapActions} from 'vuex'

  export default {
    name: "PersonalView",
    components: {
      ParallaxHeader,
      PictureInput
    },
    data() {
      return {
        editMode: true,
        loading: {
          page: true
        },
        input: {
          photo: undefined,
          name: undefined,
          email: undefined,
          phone: undefined,
          address: undefined,
          birthdate: undefined
        }
      }
    },
    computed: {
      ...mapGetters({
        currentAccount: 'account/currentAccount'
      })
    },
    mounted() {
      this.setInputs(this.currentAccount);
      this.loading.page = false
    },
    methods: {
      setInputs(account) {
        this.input.photo = account.photo;
        this.input.name = account.name;
        this.input.email = account.email;
        this.input.phone = account.phone;
        this.input.address = account.address;
        this.input.birthdate = account.birthdate;
      },
      signout() {
        this.$store.commit('authenticate/nullToken');
        this.$router.push({
          name: 'Home'
        });
      }
    }
  }
</script>

<style scoped>

</style>
