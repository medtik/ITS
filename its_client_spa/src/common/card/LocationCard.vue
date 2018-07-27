<template>
  <router-link v-bind="routerLink" tag="span" class="fakeLink">
    <v-card>
      <v-layout column align-center>
        <v-card-media style="width: 250px; height: 250px">
          <img :src='photo'/>
        </v-card-media>
        <v-card-text>
          <v-layout column>
            <v-flex class="title" style="text-align: center">
              {{name}}
            </v-flex>

            <v-flex>
              <StarRating
                :star-size="25"
                v-model="rating"
                read-only
                :show-rating="false"
                style="display: flex;justify-content: center;"
              />
            </v-flex>
            <v-flex>
              <v-label>
                9159 Lake View St.
                Clifton, NJ 07011
              </v-label>
            </v-flex>
          </v-layout>
        </v-card-text>
      </v-layout>
    </v-card>
  </router-link>
</template>

<script>
  import StarRating from "vue-star-rating"

  export default {
    name: "LocationCard",
    components: {
      StarRating
    },
    computed: {
      routerLink() {
        return {
          to: {
            name: 'LocationDetail',
            params: {
              id: this.id
            }
          }
        }
      }
    },
    data() {
      return {
        id: 1,
        name: 'Địa điểm abc',
        rating: 4,
        photo: 'https://picsum.photos/400/400',
      }
    }
  }
</script>
