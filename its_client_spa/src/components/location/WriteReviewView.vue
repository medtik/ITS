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
        {{locationName}}
      </v-flex>
      <v-flex my-3>
        <v-layout column>
          <vue-star-rating
            :star-size="35"
            :show-rating="false"
            v-model="input.rating"
          />
          <v-text-field
            label="Tiêu đề"
            v-model="input.title"
          />
          <v-textarea
            label="Mô tả"
            v-model="input.description"
          />
          <v-flex>
            <MultiPhotoInput v-model="input.photos"/>
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex>
        <v-btn color="success" @click="onUpload" :loading="uploadBtnLoading">
          Đăng bình luận
        </v-btn>
        <v-btn color="secondary" @click="onCancel">
          Hủy
        </v-btn>
      </v-flex>
      <v-container>
        <!--HOLDER-->
      </v-container>
    </v-layout>
  </v-content>
</template>

<script>
  import VueStarRating from "vue-star-rating";
  import MultiPhotoInput from "../../common/input/MultiPhotoInput";
  import {mapState} from "vuex"

  export default {
    name: "WriteReviewView",
    components: {
      VueStarRating,
      MultiPhotoInput
    },
    data() {
      return {
        uploadBtnLoading: false,
        locationName: '',
        input: {
          rating: undefined,
          title: undefined,
          description: undefined,
          photos: undefined
        }
      }
    },
    created() {
      const {
        id,
        name
      } = this.$route.query;

      this.locationName = name;
      this.locationId = id;
    },
    methods: {
      onUpload() {
        this.uploadBtnLoading = true;
        // setTimeout(() => {
        //   this.$router.back();
        // }, 2000)
        this.$store.dispatch('location/review', {
          locationId: this.locationId,
          ...this.input
        }).then(() => {
          this.$router.back();
        })
      },
      onCancel() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
