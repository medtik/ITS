<template>
  <v-content>
    <v-toolbar dark flat color="light-blue">
      <v-toolbar-title>
        Báo cáo bình luận
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-2>
      <v-flex my-2>
        <v-textarea label="Mô tả thêm"/>
      </v-flex>
      <v-flex my-2>
        <v-btn color="success"
               @click="$router.back()">
          Gửi
        </v-btn>
        <v-btn color="secondary"
               @click="onCancel">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>
<!--TODO show review detail on top-->
<script>
  export default {
    name: "ReportReviewView",
    data(){
      return {
        reviewId: undefined
      };
    },
    created(){
      const {
        reviewId,
      } = this.$route.query;

      this.reviewId = reviewId;
    },
    methods:{
      onSendBtnClick(){

      },
      onCancel(){
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
