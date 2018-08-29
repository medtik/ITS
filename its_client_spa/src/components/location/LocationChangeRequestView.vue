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
          v-model="input.name"
        />
        <v-text-field
          label="Địa chỉ"
          v-model="input.address"
        />
        <v-textarea
          label="Mô tả"
          v-model="input.description"
        />
        <v-text-field
          label="Điện thoại"
          v-model="input.phone"
        />
        <v-text-field
          label="Email"
          v-model="input.email"
        />
        <v-text-field
          label="Website"
          v-model="input.website"
        ></v-text-field>
        <!--<v-flex my-2>-->
        <!--<div class="subheading">-->
        <!--Giờ hoạt động-->
        <!--</div>-->
        <!--<LocationBusinessHoursInput v-model="input.businessHours"/>-->
        <!--</v-flex>-->
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
               @click="createLocationChangeRequest">
          Gửi yêu cầu thay đổi
        </v-btn>
        <v-btn color="secondary"
               @click="$router.back()">
          Thoát
        </v-btn>
      </v-flex>
      <v-flex style="height: 15vh">
        <!--Holder-->
        <SuccessDialog v-bind="successDialog" @close="$router.back()"/>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import LocationBusinessHoursInput from "../../common/input/LocationBusinessHoursInput";
  import TagManageSection from "../../common/input/TagsInput";
  import {SuccessDialog} from "../../common/block"
  import {mapState} from "vuex"

  export default {
    name: "LocationChangeRequestView",
    components: {
      LocationBusinessHoursInput,
      TagManageSection,
      SuccessDialog
    },
    data() {
      return {
        input: {
          name: undefined,
          address: undefined,
          description: undefined,
          phone: undefined,
          email: undefined,
          website: undefined,
          tags: undefined,
        },
        successDialog:{
          dialog: false,
          message: undefined
        }
      }
    },
    computed:{
      ...mapState('location',{
        location: state => state.detailedLocation
      })
    },
    mounted(){
      this.setInputs(this.location);
    },
    methods: {
      setInputs(location){
        this.input = {
          name: location.name,
          address: location.address,
          description: location.description,
          phone: location.phoneNumber,
          email: location.email,
          website: location.website,
          tags: location.tags
        };
      },
      createLocationChangeRequest() {
        this.$store.dispatch('request/createChangeRequest', {
          id: this.location.id,
          ...this.input
        })
          .then(value => {
            this.successDialog = {
              dialog: true,
              message: "Xin cám ơn bạn đã đóng góp thông tin"
            }
          })
      }
    }
  }
</script>

<style scoped>

</style>
