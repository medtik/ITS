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
