// DirectionsRenderer.js
import {MapElementFactory} from 'vue2-google-maps'

export default MapElementFactory({
  name: 'directionsRenderer',
  ctr: () => google.maps.DirectionsRenderer,
  events: ['directions_changed'],
  mappedProps: {
    routeIndex: { type: Number },
    options: { type: Object },
    panel: { },
    directions: { type: Object },
  },
  props: {},
  beforeCreate (options) {},
  afterCreate (directionsRendererInstance) {},
})
