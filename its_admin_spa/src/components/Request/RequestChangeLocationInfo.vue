<template>
  <Request :user="user"
           :isOwner="isOwner"
           :title="title"
           :status="status">
    <template slot="detail">
      <v-text-field
        label="Tên"
        v-model="nameInput"
        readonly
      />
      <v-text-field
        label="Địa chỉ"
        v-model="addressInput"
        readonly
      />
      <v-textarea
        label="Mô tả"
        v-model="descriptionInput"
        readonly
      />
      <v-text-field
        label="Website"
        v-model="websiteInput"
        readonly
      />
      <v-text-field
        label="Điện thoại"
        v-model="phoneInput"
        readonly
      />
      <v-text-field
        label="Email"
        v-model="emailInput"
        readonly
      />
      <!--Business hours-->
      <LocationBusinessHoursInput v-model="businessHoursInput" readonly/>
      <!--Tags-->
      <TagManageSection v-model="tagsInput" readonly/>
    </template>
  </Request>
</template>

<script>
  import Request from './Request'
  import LocationBusinessHoursInput from '../shared/LocationBusinessHoursInput'
  import TagManageSection from '../shared/TagManageSection'

  export default {
    name: "RequestChangeLocationInfo",
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
        nameInput: undefined,
        addressInput: undefined,
        descriptionInput: undefined,
        websiteInput: undefined,
        phoneInput: undefined,
        emailInput: undefined,
        businessHoursInput: {},
        tagsInput: []
      }
    },
    created(){
      this.setInputs();
    },
    components: {
      Request,
      LocationBusinessHoursInput,
      TagManageSection
    },
    methods:{
      setInputs(){
        this.nameInput = this.name;
        this.addressInput = this.address;
        this.descriptionInput = this.description;
        this.websiteInput = this.website;
        this.phoneInput = this.phone;
        this.emailInput = this.email;
        this.businessHoursInput.day1 = {...this.businessHours[0]};
        this.businessHoursInput.day2 = {...this.businessHours[1]};
        this.businessHoursInput.day3 = {...this.businessHours[2]};
        this.businessHoursInput.day4 = {...this.businessHours[3]};
        this.businessHoursInput.day5 = {...this.businessHours[4]};
        this.businessHoursInput.day6 = {...this.businessHours[5]};
        this.businessHoursInput.day7 = {...this.businessHours[6]};
        this.tagsInput = this.tags;
      }
    }
  }
</script>

<style scoped>

</style>
