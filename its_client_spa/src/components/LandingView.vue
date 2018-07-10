<template>
  <v-content>
    <section>
      <v-parallax src="static/pexels-photo-490411.jpeg"
                  height="600">
        <v-layout
          column
          align-center
          justify-center
          text-xs-center
          class="white--text"
        >
          <h1 class="display-2 mb-3">Hệ thống hướng dẫn du lịch thông minh</h1>
        </v-layout>
      </v-parallax>
    </section>

    <section>
      <v-layout
        column
        wrap
        class="my-5"
        align-center
      >
        <v-flex xs12 sm4 class="my-3">
          <div class="text-xs-center">
            <h2 class="headline">Hãy để chúng tôi hướng dẫn bạn</h2>
          </div>
        </v-flex>
        <v-flex xs12>
          <v-container grid-list-xl>
            <v-layout row wrap align-center justify-center>
              <v-flex xs12 md6>
                <v-card class="elevation-0 transparent">
                  <v-card-text class="text-xs-center">
                    <v-icon x-large class="blue--text text--lighten-2">search</v-icon>
                  </v-card-text>
                  <v-card-title primary-title class="layout justify-center">
                    <div class="headline text-xs-center">Tìm kiếm thông minh</div>
                  </v-card-title>
                  <v-card-text text-xs-justify>
                    Tìm kiếm các địa điểm và chuyến đi bằng cách trả lời các câu hỏi được thiết kế sẵn.
                  </v-card-text>
                  <v-card-actions>
                    <v-layout row justify-center>
                      <v-btn class="blue lighten-2 mt-2"
                             dark
                             large>
                        Bắt đầu
                      </v-btn>
                    </v-layout>
                  </v-card-actions>
                </v-card>
              </v-flex>
            </v-layout>
          </v-container>
        </v-flex>
      </v-layout>
    </section>

    <section>
      <v-parallax src="static/place_holder.jpg" height="300">
        <v-layout column align-center justify-center>
          <div class="headline white--text mb-3 text-xs-center">
            Tìm kiếm thông thường
          </div>
        </v-layout>
      </v-parallax>
    </section>

    <section>
      <v-container grid-list-xl>
        <v-layout column my-5>
          <v-text-field v-model="searchInput"
                        label="Tìm địa điểm, khu vực"
                        large
                        append-icon="search"/>
          <v-flex d-flex justify-start pa-0 mt-5>
            <div class="title">Các khu vực nổi bật</div>
          </v-flex>
          <v-flex style="height: 30vh">
            <!--Height holder-->
          </v-flex>
        </v-layout>
      </v-container>
    </section>
  </v-content>
</template>

<script>
  export default {
    name: "LandingView",
    data() {
      return {
        searchInput: undefined
      }
    }
  }
</script>

<style scoped>

</style>
