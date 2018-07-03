new Vue({
    store,
    el: "div.result_item_box",

    methods: {
        ...Vuex.mapActions([
            'signout'
        ])
    }
});



firebase.auth().onAuthStateChanged(function (user) {
    store.commit('setUser', {
        user,
    });

    if(!user){
        window.location = 'login.html?redirect=profile.html'
    }
});
