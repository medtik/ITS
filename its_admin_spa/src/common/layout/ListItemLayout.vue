<template>
  <v-container fluid pa-0 mx-0 :class="gridClass">
    <div style="grid-area: handle" v-if="$slots.handle">
      <slot name="handle"></slot>
    </div>
    <div style="grid-area: photo" v-if="$slots.photo">
      <slot name="photo"></slot>
    </div>
    <div style="grid-area: title" v-if="$slots.title">
      <slot name="title"></slot>
    </div>
    <div style="grid-area: detail" v-if="$slots.detail">
      <slot name="detail"></slot>
    </div>
    <div style="grid-area: action" v-if="$slots.action">
      <slot name="action"></slot>
    </div>
  </v-container>
</template>

<script>
  export default {
    name: "ListItemLayout",
    computed: {
      gridClass() {
        return {
          'grid-layout': true,
          'desktop': !this.isSmallScreen,
          'mobile': this.isSmallScreen && this.$slots.action,
          'mobile-noaction': this.isSmallScreen && !this.$slots.action,
          'handle': this.$slots.handle
        }
      },
      isSmallScreen() {
        return this.$vuetify.breakpoint.name === 'xs'
      },
    }
  }
</script>

<style scoped>
  .grid-layout {
    display: grid;
    grid-row-gap: 0.5rem;
  }

  .grid-layout.desktop {
    grid-template-columns: 15% auto 15%;
    grid-template-rows: auto auto auto;
    grid-template-areas: "photo title action" "photo detail action" "photo detail action";
  }

  .grid-layout.mobile {
    grid-template-columns: 40% auto 15%;
    grid-template-rows: auto auto auto;
    grid-template-areas: "photo title action" "photo detail action" "photo detail action";
  }

  .grid-layout.desktop-noaction {
    grid-template-columns: 15% auto 15%;
    grid-template-rows: auto auto auto;
    grid-template-areas: "photo title action" "photo detail action" "photo detail action";
  }

  .grid-layout.mobile-noaction {
    grid-template-columns: 35% auto;
    grid-template-rows: auto auto auto;
    grid-template-areas: "photo title" "photo detail" "photo detail";
  }

  .grid-layout.desktop.handle {
    grid-template-columns: 10% 30% auto;
    grid-template-rows: auto auto auto;
    grid-template-areas:
      "handle photo title"
      "handle photo detail"
      "handle photo detail";
  }

  .grid-layout.mobile.handle {
    grid-template-columns: 10% 35% auto;
    grid-template-rows: auto auto auto;
    grid-template-areas:
      "handle photo title"
    "handle photo detail"
    "handle photo detail";
  }
</style>
