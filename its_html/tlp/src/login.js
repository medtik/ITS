new Vue({
    store,
    el: "div.result_item_box",
    methods: {
        ...Vuex.mapActions([
            'signInGoogle',
            'validateAuthenticate'
        ])
    }
});

store.subscribe((mutation, state) => {
    if(mutation.type === 'setUser'){
        if(state.user){
            const currentFile = store.getters.currentFile;
            const redirectTo = currentFile.substr(
                currentFile.indexOf('redirect=') + 'redirect='.length,
            );
            window.location = redirectTo;
        }
    }
});