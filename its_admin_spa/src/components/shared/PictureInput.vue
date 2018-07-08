<template>
  <v-layout>
    <div :style="{width: `${width}px`}">
      <vue-picture-input
        :prefill="photoPrefill"
        :width="width"
        :height="height"
        :size="size"
        accept="image/jpeg,image/png"
        :removable="true"
        buttonClass="v-btn success"
        removeButtonClass="v-btn danger"
        :custom-strings="{
                  change: 'Đổi hình',
                  remove: 'Xóa',
                  drag: text
                }"
        :zIndex="0"
        @change="onChange"
        @remove="onChange">
      </vue-picture-input>
      <slot
        :value="value"
        name="extraAction">
      </slot>
    </div>
  </v-layout>
</template>

<script>
  import VuePictureInput from 'vue-picture-input';

  export default {
    name: "PictureInput",
    components: {VuePictureInput},
    props: [
      'value',
      'width',
      'height',
      'size',
      'text',
    ],
    data() {
      return {
        photoPrefill: undefined
      }
    },
    created() {
      if (this.value) {
        this.updatePrefill(this.value);
      }
    },
    watch: {
      value(val, oldVal) {
        this.updatePrefill(val);
      }
    },
    methods: {
      extraActionClick(){
        this.$emit('extraAction');
      },
      updatePrefill(val) {
        if (val) {
          this.base64toFile(val)
            .then(value => {
              this.photoPrefill = value;
            })
        } else {
          this.photoPrefill = undefined;
        }

      },
      onChange(image) {
        this.$emit('input', image);
      },

      base64toFile(url, filename = 'randomname', mimeType) {
        mimeType = mimeType || (url.match(/^data:([^;]+);/) || '')[1];
        return (fetch(url)
            .then(function (res) {
              return res.arrayBuffer();
            })
            .then(function (buf) {
              return new File([buf], filename, {type: mimeType});
            })
        );
      }
    }
  }
</script>

<style scoped>

</style>
