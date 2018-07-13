<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Thông tin cá nhân"
    />
    <v-layout my-3 mx-2 column>
      <!--Separate edit view-->
      <v-flex>

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
        <v-layout row style="justify-content: space-around">
          <v-flex xs4>
            <v-btn color="success">Cập nhật</v-btn>
            d
          </v-flex>
          <v-flex xs4>
            <v-btn color="secondary">Đăng xuất</v-btn>
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex style="height: 30vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import PictureInput from "../shared/PictureInput";
  import ParallaxHeader from "../shared/ParallaxHeader";

  export default {
    name: "PersonalView",
    components: {
      ParallaxHeader,
      PictureInput
    },
    data() {
      return {
        editMode: false,
        input: {
          photo: undefined,
          email: undefined,
          phone: undefined,
          address: undefined,
          birthdate: undefined
        },
        user: {
          photo: "https://picsum.photos/400/400",
          email: "https://picsum.photos/400/400",
          phone: "https://picsum.photos/400/400",
          address: "https://picsum.photos/400/400",
          birthdate: "https://picsum.photos/400/400"
        }
      }
    }

  }
</script>

<style scoped>

</style>
