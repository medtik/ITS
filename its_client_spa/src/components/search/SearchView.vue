<template>
  <v-content>
    <v-layout column>
      <v-flex>
        <!--SEARCH-->
        <v-jumbotron gradient="-225deg, #FFFEFF 0%, #D7FFFE 100%" height="200px">
          <v-container fill-height>
            <v-layout>
              <v-flex text-xs-center>
                <v-text-field
                  label="Tìm kiếm"
                  v-model="searchInput"
                ></v-text-field>
                <v-layout row justify-center>
                  <v-flex xs10 sm6 md4>
                    <AreaInput
                      v-model="areaIdInput"
                    ></AreaInput>
                  </v-flex>
                </v-layout>
              </v-flex>
            </v-layout>
          </v-container>
        </v-jumbotron>
      </v-flex>
      <v-flex  v-if="result.show" class="grey lighten-4">
        <!--RESULT-->
        <v-layout column>
          <v-flex py-4 class="display-2 font-weight-medium text-xs-center white">
            Kết quả
          </v-flex>
          <v-flex v-for="location in result.locations" :key="location.id" mb-2 elevation-1 class="white">
            <LocationFullWidth v-bind="location"/>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-content>
</template>
<script>
  import AreaInput from "../../sharedComponents/input/AreaInput";
  import LocationFullWidth from "../../sharedComponents/block/LocationFullWidth";
  import _location from "../location/Locations";


  export default {
    name: "SearchView",
    components: {
      AreaInput,
      LocationFullWidth
    },
    data() {
      return {
        searchInput: '',
        areaIdInput: '',
        result: {
          show: true,
          loading: false,
          locations: _location
        }
      }
    },

  }
</script>

<style scoped>
</style>
