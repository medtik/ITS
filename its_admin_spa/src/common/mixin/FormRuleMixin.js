export default {
  data() {
    return {
      rules: {
        required: (value) => {
          if (!value) {
            return 'Không được trống'
          } else {
            return true;
          }
        }
      }
    }
  }
}
