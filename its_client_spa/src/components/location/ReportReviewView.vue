<template>
  <v-content>
    <v-toolbar dark flat color="light-blue">
      <v-toolbar-title>
        Báo cáo bình luận
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-2>
      <v-flex my-2>
        <v-textarea label="Mô tả thêm" v-model="commentInput"/>
      </v-flex>
      <v-flex my-2>
        <v-btn color="success" :loading="btnLoading" @click="onSendBtnClick">
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
        btnLoading : false,
        reviewId: undefined,
        commentInput: undefined
      };
    },
    created(){
      const {
        id,
      } = this.$route.query;

      this.reviewId = id;
    },
    methods:{
      onSendBtnClick(){
        this.btnLoading = true;
        this.$store.dispatch('request/sendReportReview',{
          reviewId: this.reviewId,
          commentInput: this.commentInput
        })
          .then(value => {
            this.btnLoading = false;
            this.$router.back();
          })
          .catch(reason => {
            this.btnLoading = false;
            this.$router.back();
          })
      },
      onCancel(){
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
