<template>
  <v-layout column>
    <v-flex ma-2>
      <v-layout style="justify-content: space-between; align-items: baseline">
        <router-link :to="{name:'PlanDetail',query:{id: this.id}}"
                     tag="span" class="fakeLink title font-weight-medium">
          {{name}}
        </router-link>
        <div>
          <v-btn v-if="isSaveable"
                 icon flat large
                 color="success"
                 @click="$emit('save')">
            <v-icon>
              fas fa-download
            </v-icon>
          </v-btn>
          <v-btn v-if="isOwner"
                 icon flat large
                 color="red"
                 @click="$emit('delete',id)">
            <v-icon>
              fas fa-trash
            </v-icon>
          </v-btn>
        </div>
      </v-layout>
    </v-flex>
    <v-flex v-if="mode == 'private'" ma-2>
      <v-label>
        {{formattedStartDate}} - {{formattedEndDate}}
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
      <div style="overflow-x: auto;" v-if="isHaveLocations">
        <v-layout row my-1 justify-start>
          <v-flex v-for="(location,index) in locations"
                  :key="`${id}_${location.id}_${index}`"
                  mx-2 shrink>
            <LocationCard v-bind="location"/>
          </v-flex>
        </v-layout>
      </div>
      <v-layout v-else>
        <v-flex class="subheading text-xs-center">
          Chưa có địa điểm nào
        </v-flex>
      </v-layout>
    </v-flex>
  </v-layout>
</template>

<script>
  import {LocationCard} from "../card"
  import moment from "moment";

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
      'duration',
      'locations',
      'isSaveable',
      'isGroupOwner',
      'isOwnPlan'
    ],
    computed: {
      mode() {
        if (this.voteCount) return 'public';
        else return 'private'
      },
      isOwner() {
        return this.isGroupOwner || this.isOwnPlan;
      },
      isHaveLocations() {
        return this.locations && this.locations.length > 0;
      },
      formattedStartDate() {
        return moment(this.startDate).format('DD/MM/YYYY');
      },
      formattedEndDate() {
        return moment(this.endDate).format('DD/MM/YYYY');
      }
    }
  }
</script>
