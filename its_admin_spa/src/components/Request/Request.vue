<template>
  <v-layout class="request-layout" elevation-2 pa-2>
    <!--TODO find a way to separate each request more clearly-->
    <v-layout d-flex column
              style="align-items: center; grid-area: user">
      <v-flex>
        <v-avatar>
          <img :src="user.photo">
        </v-avatar>
      </v-flex>
      <v-flex>
        <v-label>{{user.name}}</v-label>
      </v-flex>
      <v-flex>
        <v-chip v-if="isOwner" label outline color="primary">Chủ địa điểm này</v-chip>
      </v-flex>
    </v-layout>
    <v-layout style="grid-area: summary" column>
      <span class="subheading">
        {{title}}
      </span>
      <v-flex d-flex style="align-items: center">
        <v-divider/>
      </v-flex>
    </v-layout>
    <v-layout column
              style="align-items: center; grid-area: status">

      <v-label>Trạng thái yêu cầu</v-label>
      <v-chip v-if="status == 1"
              label
              outline color="blue">
        <span>Đang chờ</span>
      </v-chip>
      <v-chip v-if="status == 2"
              label
              outline color="green">
        <span>Chấp nhận</span>
      </v-chip>
      <v-chip v-if="status == 3"
              label
              outline color="red">
        <span>Từ chối</span>
      </v-chip>
    </v-layout>
    <v-layout column
              style="align-items: center; grid-area: action">
      <v-label>Hành động</v-label>
      <v-flex>
        <v-btn icon flat color="green">
          <v-icon color="green">check</v-icon>
        </v-btn>
        <v-btn icon flat color="red">
          <v-icon color="red">delete</v-icon>
        </v-btn>
      </v-flex>
    </v-layout>
    <v-layout style="grid-area: detail" mt-2>
      <v-card flat style="width: 100%">
        <v-card-title style="padding: 0">
          <v-btn block flat v-on:click="openDetail = !openDetail">
            <v-label>Thông tin chi tiết</v-label>
          </v-btn>
        </v-card-title>
        <v-card-text v-show="openDetail">
          <slot name="detail"></slot>
        </v-card-text>
      </v-card>
    </v-layout>
  </v-layout>
</template>

<script>
  export default {
    name: "Request",
    props: [
      'user',
      'title',
      'status',
      'isOwner'
    ],
    data(){
      return {
        openDetail: false
      }
    }
  }
</script>

<style scoped>
  .request-layout {
    display: grid;
    grid-template-columns: 25% 40% 35%;
    grid-template-rows: 4rem 4rem auto;
    grid-template-areas: "user summary summary" "user status action" "detail detail detail";
    grid-row-gap: 0.5rem;
  }
</style>
