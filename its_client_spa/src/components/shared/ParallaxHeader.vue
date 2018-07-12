<template>
  <section>
    <v-parallax :src="src"
                height="600">
      <v-layout
        column
        align-center
        justify-center
        text-xs-center
        class="white--text"
      >
        <h1 class="display-2 mb-3">{{text}}</h1>
      </v-layout>
    </v-parallax>
  </section>
</template>

<script>
  export default {
    name: "ParallaxHeader",
    props: [
      'src',
      'text'
    ]
  }
</script>

<style scoped>

</style>
