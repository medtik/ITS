<template>
  <section>
    <v-parallax :src="src"
                :height="height ? height : 600">
      <v-layout
        v-if="!$slots.default"
        column
        align-center
        justify-center
        text-xs-center
        class="white--text"
      >
        <h1 class="display-2 mb-3">{{text}}</h1>
      </v-layout>
      <slot v-else></slot>
    </v-parallax>
  </section>
</template>

<script>
  export default {
    name: "ParallaxHeader",
    props: [
      'src',
      'text',
      'height'
    ]
  }
</script>

<style scoped>
  #content-holder {
    background-image: url("../../../static/pexels-photo-239520.jpeg");
    background-size: cover;
    background-position: center center;
  }

  #layout {
    height: 100vh;
  }

  #form {
    width: 100%;
    max-width: 500px;
    position: relative;
    top: -15%;
  }
</style>
