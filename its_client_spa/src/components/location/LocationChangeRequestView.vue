<template>
  <v-content>
    <v-toolbar flat dark color="light-blue">
      <v-toolbar-title>
        Cập nhật thông tin địa điểm
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-2>
      <v-flex>
        <v-text-field
          label="Tên"
        />
        <v-text-field
          label="Địa chỉ"
        />
        <v-text-field
          label="Mô tả"
        />
        <v-text-field
          label="Điện thoại"
        />
        <v-text-field
          label="Email"
        />
        <v-flex my-2>
          <div class="subheading">
            Giờ hoạt động
          </div>
          <LocationBusinessHoursInput v-model="input.businessHours"/>
        </v-flex>
        <v-flex y-2>
          <div class="subheading">
            Thẻ
          </div>
          <TagManageSection v-model="input.tags"/>
        </v-flex>
      </v-flex>
      <v-divider></v-divider>
      <v-flex>
        <v-btn color="success"
               @click="$router.back()">
          Gửi yêu cầu thay đổi
        </v-btn>
        <v-btn color="secondary"
               @click="$router.back()">
          Thoát
        </v-btn>
      </v-flex>
      <v-flex style="height: 15vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import LocationBusinessHoursInput from "../../common/input/LocationBusinessHoursInput";
  import TagManageSection from "../../common/input/TagsInput";

  export default {
    name: "LocationChangeRequestView",
    components: {
      LocationBusinessHoursInput,
      TagManageSection
    },
    data() {
      return {
        input: {
          businessHours: {
            day1: {
              from: undefined,
              to: undefined
            },
            day2: {
              from: undefined,
              to: undefined
            },
            day3: {
              from: undefined,
              to: undefined
            },
            day4: {
              from: undefined,
              to: undefined
            },
            day5: {
              from: undefined,
              to: undefined
            },
            day6: {
              from: undefined,
              to: undefined
            },
            day7: {
              from: undefined,
              to: undefined
            },
          },
          tags: []
        }
      }
    }
  }
</script>

<style scoped>

</style>
