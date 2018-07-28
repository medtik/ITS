<template>
  <Request :user="user"
           :isOwner="isOwner"
           :title="title"
           :status="status">
    <template slot="detail">
      <v-text-field
        label="Tên"
        v-model="input.nameInput"
        readonly
      />
      <v-text-field
        label="Địa chỉ"
        v-model="input.addressInput"
        readonly
      />
      <v-textarea
        label="Mô tả"
        v-model="input.descriptionInput"
        readonly
      />
      <v-text-field
        label="Website"
        v-model="input.websiteInput"
        readonly
      />
      <v-text-field
        label="Điện thoại"
        v-model="input.phoneInput"
        readonly
      />
      <v-text-field
        label="Email"
        v-model="input.emailInput"
        readonly
      />
      <!--Business hours-->
      <LocationBusinessHoursInput v-model="input.businessHoursInput" readonly/>
      <!--Tags-->
      <TagsInput v-model="input.tagsInput" readonly/>
    </template>
  </Request>
</template>

<script>
  import Request from './Request'

  import {
    LocationBusinessHoursInput,
    TagsInput
  } from "../../common/input";


  export default {
    name: "RequestChangeLocationInfo",
    components: {
      Request,
      LocationBusinessHoursInput,
      TagsInput
    },
    props: [
      'isOwner',
      'user',
      'title',
      'location',
      'status',
      'name',
      'address',
      'description',
      'website',
      'phone',
      'email',
      'businessHours',
      'tags'
    ],
    data() {
      return {
        input:{
          nameInput: undefined,
          addressInput: undefined,
          descriptionInput: undefined,
          websiteInput: undefined,
          phoneInput: undefined,
          emailInput: undefined,
          businessHoursInput: {},
          tagsInput: []
        },
      }
    },
    created(){
      this.setInputs();
    },
    methods:{
      setInputs(){
        this.input.nameInput = this.name;
        this.input.addressInput = this.address;
        this.input.descriptionInput = this.description;
        this.input.websiteInput = this.website;
        this.input.phoneInput = this.phone;
        this.input.emailInput = this.email;
        this.input.businessHoursInput.day1 = {...this.businessHours[0]};
        this.input.businessHoursInput.day2 = {...this.businessHours[1]};
        this.input.businessHoursInput.day3 = {...this.businessHours[2]};
        this.input.businessHoursInput.day4 = {...this.businessHours[3]};
        this.input.businessHoursInput.day5 = {...this.businessHours[4]};
        this.input.businessHoursInput.day6 = {...this.businessHours[5]};
        this.input.businessHoursInput.day7 = {...this.businessHours[6]};
        this.input.tagsInput = this.tags;
      }
    }
  }
</script>

<style scoped>

</style>
