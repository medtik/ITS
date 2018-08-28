<template>
  <router-link v-bind="routerLink" tag="span" class="fakeLink">
    <v-card style="width: 300px; height: 100%">
      <v-layout column align-center>
        <v-card-media style="height: 200px">
          <img :src='coverPhoto'/>
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
            <v-flex class="text-xs-center">
              <v-label>
                {{address}}
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
    props:[
      'id',
      'photo',
      'name',
      'address',
      'primaryPhoto',
      'rating'
    ],
    computed: {
      coverPhoto(){
        if(this.primaryPhoto){
          return this.primaryPhoto;
        }else{
          return this.photo;
        }
      },
      routerLink() {
        return {
          to: {
            name: 'LocationDetail',
            query: {
              id: this.id
            }
          }
        }
      }
    }
  }
</script>
