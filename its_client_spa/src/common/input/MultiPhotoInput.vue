<template>
  <v-layout row>
    <v-flex style="flex-grow: 0">
      <PictureInput v-model="photoInput"
                    width="200"
                    height="200"
                    text="Hình ảnh">
        <v-flex slot="extraAction">
          <v-btn block color="success" @click="addImageClick">
            <v-icon>fa fa-plus</v-icon>
          </v-btn>
        </v-flex>
      </PictureInput>
    </v-flex>
    <v-layout v-if="photos"
              mb-4
              wrap
              row>
      <v-flex v-for="(photo,index) in photos"
              pa-2
              xs4
              style="flex-grow: 0.05"
              :key="index">
        <v-card>
          <v-card-media>
            <img :src="photo" height="200"/>
          </v-card-media>
          <v-card-actions>
            <v-btn color="red" flat block
                   @click="removeSecondaryPhoto(index)">
              <v-icon color="red"
                      class="white--text">
                delete
              </v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>
  </v-layout>
</template>

<script>
  import PictureInput from "./PictureInput";

  export default {
    name: "MultiPhotoInput",
    props: {
      value: {
        type: Array,
        required: false
      }
    },
    components: {
      PictureInput
    },
    data() {
      return {
        photoInput: undefined,
        photos: [],
      }
    },
    mounted() {
      if (this.value) {
        this.photos = this.value;
      }
    },
    methods: {
      onInput() {
        this.$emit('input', this.photos);
      },
      addImageClick() {
        if (!this.photos) {
          this.photos = [];
        }
        if (this.photoInput) {
          this.photos.push(this.photoInput);
          this.photoInput = undefined;
        }

      },
      removeSecondaryPhoto(index) {
        this.photos.splice(index, 1);
      }
    }
  }
</script>

<style scoped>

</style>
