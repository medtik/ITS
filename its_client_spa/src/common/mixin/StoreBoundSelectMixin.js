import {mapState} from "vuex";

export default function mixin({alias, itemsPath, loadingPath, getItemPath}) {
  return {
    props: [
      'value'
    ],
    computed: {
      ...mapState(alias, {
        items: state => state[itemsPath],
        loading: state => state[loadingPath],
      })
    },
    mounted() {
      if (this.loading) {
        this.$store.dispatch(`${alias}/${getItemPath}`)
      }
    },
    methods: {
      onSelect(value) {
        this.$emit('input', value);
        this.$emit('change', value);
      }
    }
  }
}
