export default {
  namespaced: true,
  actions: {
    addImage(context, payload) {
      const {
        locationId,
        photo
      } = payload;

      return Promise.resolve();
    }
  }
}
