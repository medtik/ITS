<template>
  <v-content>
    <ParallaxHeader
      src="static/pexels-photo-490411.jpeg"
      text="Hệ thống hướng dẫn du lịch thông minh"
    />

    <section>
      <!--SMART SEARCH-->
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
              <v-flex xs12 sm8 md6>
                <v-card class="elevation-0 transparent">
                  <v-card-text class="text-xs-center">
                    <v-icon x-large class="blue--text text--lighten-2">search</v-icon>
                  </v-card-text>
                  <v-card-title primary-title class="layout justify-center">
                    <div class="headline text-xs-center">Tìm kiếm thông minh</div>
                  </v-card-title>
                  <v-card-text text-xs-justify>
                    Tìm kiếm các địa điểm và chuyến đi phù hợp nhất với bạn bằng cách trả lời các câu hỏi được thiết kế
                    sẵn.
                  </v-card-text>
                  <v-card-actions>
                    <v-layout row justify-center>
                      <v-btn class="blue lighten-2 mt-2"
                             :to="{name:'SmartSearch'}"
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
      <!--NORMAL SEARCH-->
      <v-parallax src="static/place_holder.jpg" height="150">
        <v-layout column align-center justify-center>
          <v-flex xs2 class="headline font-weight-black white--text text-xs-center">
            Tìm kiếm thông thường
          </v-flex>
          <v-flex xs2>
            <v-btn color="light-blue lighten-2" dark depressed
                   :to="{name:'Search'}">
              <v-icon>search</v-icon>
            </v-btn>
          </v-flex>
        </v-layout>
      </v-parallax>
    </section>

    <section v-if="false">
      <!--NEARBY-->
      <v-layout column my-5>
        <v-flex class="text-xs-center display-1 font-weight-black">
          Gần đây
        </v-flex>
        <v-flex>
          <v-layout row pa-1>
            <v-flex>

            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>
    </section>

    <section v-if="!featuredPlansLoading && !featuredAreasLoading">
      <!--FEATURED-->
      <v-container grid-list-xl>
        <v-layout column my-5>
          <v-flex class="text-xs-center display-1 font-weight-black">
            Tiêu điểm
          </v-flex>
          <v-flex justify-start mt-5 v-if="featuredAreas">
            <div class="title">Các khu vực nổi bật</div>
            <div style="overflow-y: auto;">
              <v-layout row my-1>
                <v-flex v-for="area in featuredAreas"
                        :key="area.id"
                        shrink>
                  <AreaCard v-bind="area"/>
                </v-flex>
              </v-layout>
            </div>
          </v-flex>
          <v-flex justify-start mt-5 v-if="featuredPlans && featuredPlans.length > 0">
            <div class="title">Các chuyến đi nổi bật</div>
            <div style="overflow-x: auto;">
              <v-layout row my-1>
                <v-flex v-for="plan in featuredPlans"
                        :key="plan.id"
                        shrink>
                  <PlanCard v-bind="plan"/>
                </v-flex>
              </v-layout>
            </div>
          </v-flex>
        </v-layout>
      </v-container>
    </section>
    <section>
      <v-container class="text-xs-center" v-if="featuredPlansLoading || featuredAreasLoading">
        <v-progress-circular indeterminate size="40" color="primary"></v-progress-circular>
      </v-container>
    </section>
    <section>
      <v-flex style="height: 15vh">
        <!--Holder-->
      </v-flex>
    </section>
  </v-content>
</template>

<script>
  import AreaCard from "../common/card/AreaCard"
  import PlanCard from "../common/card/PlanCard"
  import {mapGetters, mapState} from "vuex";

  import ParallaxHeader from "../common/layout/ParallaxHeader"

  export default {
    name: "LandingView",
    components: {
      AreaCard,
      PlanCard,
      ParallaxHeader
    },
    computed: {
      ...mapGetters('plan', {
        featuredPlans: 'featuredPlans',
        featuredPlansLoading: 'featuredPlansLoading'
      }),
      ...mapGetters('area', {
        featuredAreas: 'featuredAreas',
        featuredAreasLoading: 'featuredAreasLoading'
      }),
    },
    mounted() {
      this.$store.dispatch('area/getFeatured');
      this.$store.dispatch('plan/getFeatured');
    }
  }
</script>

<style scoped>

</style>
