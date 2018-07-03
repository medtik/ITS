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

firebase.auth().onAuthStateChanged(function (user) {
    store.commit('setUser', {
        user,
    });

    if (user) {
        const currentFile = store.getters.currentFile;
        const redirectTo = currentFile.substr(
            currentFile.indexOf('redirect=') + 'redirect='.length,
        );
        window.location = redirectTo;
    }
});