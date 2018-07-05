export default {
  namespaced: true,
  actions: {
    signInEmail(context,payload) {
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          if(payload.email === 'admin@tlp.com'){
            resolve();
          }else{
            reject();
          }
        }, 1500)
      })
    }
  }
};
