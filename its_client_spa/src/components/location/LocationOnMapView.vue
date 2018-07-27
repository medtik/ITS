<template>
  <v-content>
    <v-toolbar color="light-blue" dark fixed>
      <v-toolbar-title>
        Địa điểm ABC
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-btn flat @click="setMapToMyLocation">
          <v-icon>
            my_location
          </v-icon>
        </v-btn>
        <v-btn flat @click="getDirection">
          <v-icon>
            navigation
          </v-icon>
        </v-btn>
        <v-btn flat
               :style="{opacity: toggle.restaurant ? 1 : 0.5}"
               @click="toggle.restaurant = !toggle.restaurant">
          <v-icon>
            restaurant
          </v-icon>
        </v-btn>
        <v-btn flat :style="{opacity: toggle.hotel ? 1 : 0.5}"
               @click="toggle.hotel = !toggle.hotel">
          <v-icon>
            hotel
          </v-icon>
        </v-btn>
        <v-btn flat :style="{opacity: toggle.activity ? 1 : 0.5}"
               @click="toggle.activity = !toggle.activity">
          <v-icon>
            local_activity
          </v-icon>
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>
    <GmapMap
      ref="mapRef"
      :center="current"
      :zoom="18"
      :options="{mapTypeControl: false,styles: mapStyles}"
      map-type-id="terrain"
      style="width: 100%; height: 95vh"
    >
      <GMarker v-for="(marker, index) in toggledMarkers" :key="index"
               v-bind="marker"
               @click="toggleInfoWindow(marker)"
               iconWidth="40"
               iconHeight="40"
      ></GMarker>
      <GMarker :key="current.id"
               v-bind="current"
               @click="toggleInfoWindow(current)"
               iconWidth="40"
               iconHeight="40"
      ></GMarker>
      <GmapInfoWindow
        :options="infoWindow.options"
        :position="infoWindow.pos"
        :opened="infoWindow.open"
        @closeclick="infoWindow.open = false"
      >
        <LocationCard/>
      </GmapInfoWindow>
    </GmapMap>
  </v-content>

</template>

<script>
  import {gmapApi} from 'vue2-google-maps'
  import GMarker from "./GMarker";
  import DirectionsRenderer from './DirectionsRenderer.js'
  import LocationCard from "../../common/card/LocationCard";


  export default {
    name: "LocationOnMapView",
    components: {
      GMarker,
      DirectionsRenderer,
      LocationCard
    },
    data() {
      return {
        current: {id: -1, lat: 10.8290990, lng: 106.6720520, type: 'current', location: {}},
        toggle: {
          activity: true,
          restaurant: true,
          hotel: true,
        },
        infoWindow: {
          pos: undefined,
          open: false,
          markerId: undefined,
          location: undefined,
          options: {
            pixelOffset: {
              width: 0,
              height: -35
            }
          }
        },
        markers: [
          {
            id: 1,
            lat: 10.8292360,
            lng: 106.6714590,
            type: 'activity',
            location: {name: 'activity abc', rating: 3.2, id: 1}
          },
          {
            id: 2,
            lat: 10.8298360,
            lng: 106.6733590,
            type: 'restaurant',
            location: {name: 'restaurant abc', rating: 3.2, id: 1}
          },
          {id: 2, lat: 10.8290990, lng: 106.6737590, type: 'activity'},
          // {id: 3, lat: 10.8270360, lng: 106.6711930, type: 'hotel'},
          // {id: 4, lat: 10.8294360, lng: 106.6713130, type: 'hotel'},
          // {id: 5, lat: 10.8282360, lng: 106.6719130, type: 'hotel'},
        ],
        mapStyles: [
          {
            featureType: 'poi.business',
            stylers: [{visibility: 'off'}]
          },
          {
            featureType: 'poi.attraction',
            stylers: [{visibility: 'off'}]

          }
        ]
      }
    },
    computed: {
      google: gmapApi,
      mapRef() {
        //Call this with mapRef.then((map)=>{code here})
        return this.$refs.mapRef.$mapPromise;
      },
      toggledMarkers() {
        let toggledMarkers = [];
        if (this.toggle.restaurant) {
          toggledMarkers.push(
            ...this.markers.filter(value => value.type === 'restaurant')
          )
        }
        if (this.toggle.activity) {
          toggledMarkers.push(
            ...this.markers.filter(value => value.type === 'activity')
          )
        }
        if (this.toggle.hotel) {
          toggledMarkers.push(
            ...this.markers.filter(value => value.type === 'hotel')
          )
        }
        return toggledMarkers;
      }
    },
    methods: {
      moveMapTo(long, lat) {
        this.mapRef.then(map => {
          map.panTo({lat: lat, lng: long})
        })
      },
      setMapToMyLocation() {
        navigator.geolocation.getCurrentPosition((pos) => {
          this.moveMapTo(pos.coords.longitude, pos.coords.latitude)
        }, error => {
          let errMessage = 'Có lỗi xẩy ra';
          switch (error.code) {
            case error.PERMISSION_DENIED:
              errMessage = "User denied the request for Geolocation.";
              break;
            case error.POSITION_UNAVAILABLE:
              errMessage = "Location information is unavailable.";
              break;
            case error.TIMEOUT:
              errMessage = "The request to get user location timed out.";
              break;
          }
        })
      },
      // https://developers.google.com/maps/documentation/javascript/reference/3/directions
      getDirection(from, to) {
        const req = {
          origin: {lat: 10.8290990, lng: 106.6720520},
          destination: {lat: 10.8298360, lng: 106.6733590},
          travelMode: 'DRIVING'
        };
        const service = new this.google.maps.DirectionsService();
        service.route(req, (result, status) => {
          let renderer = new this.google.maps.DirectionsRenderer();
          this.mapRef.then(map => {
            renderer.setDirections(result);
            renderer.setMap(map);
          });
        })
      },
      toggleInfoWindow(marker) {
        console.debug(marker.id, marker.type);
        this.infoWindow.pos = {
          lat: marker.lat,
          lng: marker.lng
        };
        this.infoWindow.location = marker.location;

        if (this.infoWindow.markerId === marker.id) {
          this.infoWindow.open = !this.infoWindow.open;
        }
        else {
          this.infoWindow.open = true;
          this.infoWindow.markerId = marker.id;
        }
      }
    }
  }
</script>

<style scoped>

</style>
