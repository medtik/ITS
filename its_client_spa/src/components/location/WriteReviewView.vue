<template>
  <v-content>
    <v-toolbar color="light-blue" dark flat>
      <v-toolbar-title>
        Viết bình luận
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-3 mt-2>
      <v-flex class="headline">
        <!--LOCATION TITLE-->
        Địa điểm ABC
      </v-flex>
      <v-flex my-3>
        <v-layout column>
          <vue-star-rating
            :star-size="35"
            :show-rating="false"
            v-model="ratingInput"
          />
          <v-text-field
            label="Tiêu đề"
            v-model="titleInput"
          />
          <v-textarea
            label="Mô tả"
            v-model="descriptionInput"
          />
          <v-flex>
            <MultiPhotoInput v-model="photosInput"/>
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex>
        <v-btn color="success" @click="onUpload" :loading="uploadBtnLoading">
          Đăng
        </v-btn>
        <v-btn color="secondary" @click="onCancel">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import VueStarRating from "vue-star-rating";
  import MultiPhotoInput from "../../common/input/MultiPhotoInput";

  export default {
    name: "WriteReviewView",
    components: {
      VueStarRating,
      MultiPhotoInput
    },
    data() {
      return {
        uploadBtnLoading: false,
        locationName: 'Địa điểm ABC',
        ratingInput: undefined,
        titleInput: '',
        descriptionInput: '',
        photosInput: []
      }
    },
    methods: {
      onUpload() {
        this.uploadBtnLoading = true;
        setTimeout(() => {
          this.$router.back();
        }, 2000)
      },
      onCancel() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
