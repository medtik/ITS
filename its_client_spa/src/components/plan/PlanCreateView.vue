<template>
  <v-content>
    <v-toolbar class="light-blue" flat dark>
      <v-toolbar-title>
        Tạo chuyến đi
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-3>
      <v-flex my-3>
        <v-text-field
          label="Tên"
        />
        <v-text-field
          label="Ngày bắt đầu"
          type="date"
        />
        <v-text-field
          label="Ngày kết thúc"
          type="date"
        />
      </v-flex>
      <v-flex>
        <v-btn color="success"
               :loading="createBtnLoading"
               @click="onCreate">
          Tạo
        </v-btn>
        <v-btn color="secondary"
               @click="onCancel">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  export default {
    name: "PlanCreateView",
    data() {
      return {
        createBtnLoading: false
      }
    },
    methods: {
      onCreate() {
        this.createBtnLoading = true;
        setTimeout(() => {
            this.$router.push({name: "PlanDetail"})
          }
          , 2000);
      },
      onCancel() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
