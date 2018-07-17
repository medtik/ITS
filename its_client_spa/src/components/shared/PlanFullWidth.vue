<template>
  <v-layout column>
    <v-flex mb-2>
      <router-link :to="{name:'PlanDetail',params:{id: this.id}}"
                   tag="span"
                   class="title font-weight-medium fakeLink">
        {{title}}
      </router-link>
    </v-flex>
    <v-flex v-if="mode == 'private'">
      <v-label>
        {{startDate}} - {{endDate}}
      </v-label>
    </v-flex>
    <v-flex v-if="mode == 'public'">
      <v-layout column>
        <v-flex class="body-1">
          <v-icon small>thumb_up</v-icon>&nbsp; <span>{{voteCount}} lượt bình chọn</span>
        </v-flex>
        <v-flex>
            <span class="body-1 ">
            {{reason}}
          </span>
        </v-flex>
      </v-layout>
    </v-flex>
    <v-flex mt-2>
      <v-layout style="overflow-y: auto;">
        <v-flex v-for="n in 9" :key="n" mx-2>
          <LocationCard/>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-layout>
</template>

<script>
  import LocationCard from "../shared/LocationCard"

  export default {
    name: "PlanFullWidth",
    components: {
      LocationCard
    },
    props: [
      'voteCount',
      'reason',
      'duration'
    ],
    data() {
      return {
        id: 1,
        title: 'Plan abc',
        startDate: '20/7/2018',
        endDate: '25/7/2018'
      }
    },
    computed: {
      mode() {
        if (this.voteCount) return 'public';
        else return 'private'
      }
    }
  }
</script>
