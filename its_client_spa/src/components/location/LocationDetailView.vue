<template>
  <v-content>
    <ParallaxHeader src="https://picsum.photos/1000/1000" height="390"/>
    <v-layout column>
      <v-flex mx-2 my-2>
        <!--Header-->
        <div class="headline font-weight-thin">{{location.name}}</div>
        <StarRating v-model="location.reviews[1].rating"
                    read-only
                    :star-size="20"
                    :increment="0.1"
                    :show-rating="false"/>
        <span class="subheading">{{location.reviews.length}} đánh giá</span>
        <div>
          <v-chip v-for="tag in summaryTag" :key="tag.id">{{tag.name}}</v-chip>
        </div>
        <div>
          <span>Hôm nay: Mở từ {{todayHours.from}} đến {{todayHours.to}} | </span>
          <span class="subheading font-weight-bold"
                v-if="todayHours.status == 'close'"
                style="color: red">
            Đang đóng cửa
          </span>
          <span class="subheading font-weight-bold"
                v-if="todayHours.status == 'open'"
                style="color: green">
            Đang mở cửa
          </span>
        </div>
      </v-flex>
      <v-flex my-4 mx-2>
        <v-flex d-flex align-baseline>
          <div class="title">Hình ảnh</div>
          <v-btn color="success"
                 flat
                 :loading="loading.addImgBtn"
                 @click="onAddPhotoClick">
            <v-icon small>add_a_photo</v-icon>
            &nbsp &nbsp
            Xem thêm
          </v-btn>
        </v-flex>
        <v-flex my-2>
          <v-carousel>
            <v-carousel-item
              v-for="(photo,i) in location.photos"
              :key="i"
              :src="photo.url"/>
          </v-carousel>
        </v-flex>
      </v-flex>
      <v-flex my-2 mx-2>
        <div class="title">Thông tin liên lạc</div>
        <v-layout column mx-3 my-2>
          <v-text-field
            label="Địa chỉ"
            readonly
            v-model="location.address"
            prepend-icon="map"
          />
          <v-text-field
            label="Email"
            readonly
            v-model="location.email"
            prepend-icon="email"
          />
          <v-text-field
            label="Điện thoại"
            readonly
            v-model="location.phone"
            prepend-icon="call"
          />
          <v-text-field
            label="Web"
            readonly
            v-model="location.website"
            prepend-icon="screen_share"
          />
        </v-layout>
      </v-flex>
      <v-flex my-2 mx-2>
        <v-flex d-flex align-baseline>
          <v-flex class="title">Bình luận</v-flex>
          <v-btn color="success" :to="{name:'ReviewWriting', params:{id: location.id}}">
            <v-icon>rate_review</v-icon>
            &nbsp &nbsp Đánh giá
          </v-btn>
        </v-flex>
        <v-layout column my-2>
          <v-flex v-for="review in location.reviews"
                  :key="review.id"
                  elevation-2>
            <LocationReview v-bind="review"/>
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex my-2 mx-2>
        <v-flex class="title">Địa điểm lân cận</v-flex>
        <v-layout column my-2>
          <v-divider></v-divider>
          <v-flex>
            <v-btn block flat>
              <v-icon>restaurant</v-icon>
              &nbsp &nbsp Nhà hàng
            </v-btn>
          </v-flex>
          <v-divider></v-divider>
          <v-flex>
            <v-btn block flat>
              <v-icon>hotel</v-icon>
              &nbsp &nbsp Khách sạn
            </v-btn>
          </v-flex>
          <v-divider></v-divider>
          <v-flex>
            <v-btn block flat>
              <v-icon>local_activity</v-icon>
              &nbsp &nbsp Vui chơi
            </v-btn>
          </v-flex>
          <v-divider></v-divider>
        </v-layout>
      </v-flex>
      <v-flex my-2 mx-2>
        <v-flex class="title">Khác</v-flex>
        <v-layout column my-2>
          <v-btn block flat>
            <v-icon>edit_location</v-icon>
            &nbsp &nbsp Cập nhật thông tin
          </v-btn>
          <v-btn block flat>
            <v-icon>person_pin_circle</v-icon>
            &nbsp &nbsp Tôi sở hữu địa điểm này
          </v-btn>
        </v-layout>
      </v-flex>

      <v-flex style="height: 25vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
    <ChooseImageDialog v-bind="chooseImageDialog"
                       @close="chooseImageDialog.dialog = false"
                       @confirm="onAddImageConfirm"/>
  </v-content>
</template>

<script>
  import StarRating from "vue-star-rating";
  import LocationReview from "./LocationReview";
  import Locations from "./Locations";
  import ParallaxHeader from "../shared/ParallaxHeader";
  import ChooseImageDialog from "../shared/ChooseImageDialog";


  export default {
    name: "LocationDetailView",
    components: {
      StarRating,
      LocationReview,
      ParallaxHeader,
      ChooseImageDialog
    },
    data() {
      return {
        loading: {
          addImgBtn: false
        },
        locationId: undefined,
        location: Locations[0],
        chooseImageDialog: {},
        error: {}
      }
    },
    computed: {
      summaryTag() {
        return [
          {id: 1, name: "Ẩm thực pháp"},
          {id: 2, name: "Có thực đơn chay"},
          {id: 3, name: "Sang trọng"}
        ]
      },
      todayHours() {
        return {
          from: "8:30 sáng",
          to: "6:30 chiều",
          status: 'close'
        }
      }
    },
    created() {
      const {
        id
      } = this.$route.params;

      this.locationId = id;
    },
    methods: {
      onAddPhotoClick() {
        this.chooseImageDialog = {
          dialog: true,
          text: 'Thêm ảnh cho địa điểm'
        }
      },
      onAddImageConfirm(photo) {
        this.loading.addImgBtn = true;
        this.chooseImageDialog = {
          dialog: false
        };

        if (photo) {
          this.$store.dispatch('location/addImage', {photo, id: this.location.id})
            .then(location => {
              // this.location = location;
              this.loading.addImgBtn = false;
            })
            .catch(reason => {
              this.error = {
                dialog: true,
                ...reason
              }
            })

        } else {
          this.loading.addImgBtn = false;
        }
      }
    }
  }
</script>

<style scoped>

</style>
