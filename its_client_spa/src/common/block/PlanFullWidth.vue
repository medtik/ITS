<template>
  <v-layout column>
    <v-flex ma-2>
      <v-layout style="justify-content: space-between; align-items: baseline">
        <router-link :to="{name:'PlanDetail',params:{id: this.id}}"
                     tag="span" class="fakeLink title font-weight-medium">
          {{name}}
        </router-link>
        <div>
          <v-btn icon flat large
                 color="success"
                 @click="$emit('save')">
            <v-icon>
              fas fa-download
            </v-icon>
          </v-btn>
          <v-btn icon flat large
                 color="red"
                 @click="$emit('delete')">
            <v-icon>
              fas fa-trash
            </v-icon>
          </v-btn>
        </div>
      </v-layout>
    </v-flex>
    <v-flex v-if="mode == 'private'" ma-2>
      <v-label>
        {{startDate}} - {{endDate}}
      </v-label>
    </v-flex>
    <v-flex v-if="mode == 'public'" ma-2>
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
    <v-divider></v-divider>
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
  import {LocationCard} from "../card"

  export default {
    name: "PlanFullWidth",
    components: {
      LocationCard
    },
    props: [
      'id',
      'name',
      'startDate',
      'endDate',
      'voteCount',
      'reason',
      'duration'
    ],
    computed: {
      mode() {
        if (this.voteCount) return 'public';
        else return 'private'
      }
    }
  }
</script>
