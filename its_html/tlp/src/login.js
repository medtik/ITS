new Vue({
    store,
    el: "div.result_item_box",
    methods: {
        ...Vuex.mapActions([

            'validateAuthenticate'
        ]),
        signInGoogle(){
            this.$store.dispatch('signInGoogle')
                .catch(error => {
                    // alert(error);
                })
        }
    }
});

store.subscribe((mutation, state) =>{
   if(mutation.type === 'setUser'){
       if(state.user){
           const currentFile = store.getters.currentFile;
           if(currentFile.indexOf('redirect=') > 0){
               const redirectTo = currentFile.substring(
                   currentFile.indexOf('redirect=') + 'redirect='.length
               );

               if(redirectTo){
                   console.debug(redirectTo);
                   window.location = redirectTo;
               }
           }
       }
   }
});