<template>
  <v-content>
    <v-parallax src="https://picsum.photos/1000/1000"
                height="390">
      <v-layout
        column
        align-end
        justify-end
        class="white--text"
      >
        <span class="subheading">
          <v-icon>picture</v-icon>Xem thêm ({{location.photos.length}})
        </span>
      </v-layout>
    </v-parallax>
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
        <div class="title">Đánh giá</div>
        <v-layout column my-2>
          <v-flex v-for="review in location.reviews"
                  :key="review.id"
                  elevation-2>
          <LocationReview v-bind="review"/>
          </v-flex>
        </v-layout>
      </v-flex>
      <v-flex my-2 mx-2>
        <v-layout column>
            <v-btn flat>Viết đánh giá</v-btn>
            <v-btn flat>Đăng hình ảnh</v-btn>
        </v-layout>
      </v-flex>
      <v-flex style="height: 25vh">
        <!--Holder-->
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import StarRating from "vue-star-rating";
  import LocationReview from "./LocationReview";
  import Locations from "./Locations";

  export default {
    name: "LocationDetailView",
    components: {
      StarRating,
      LocationReview
    },
    data() {
      return {
        locationId: undefined,
        location: Locations[0]
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
    }
  }
</script>

<style scoped>

</style>
