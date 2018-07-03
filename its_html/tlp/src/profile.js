new Vue({
    store,
    el: "div.result_item_box",
    computed:{
        ...Vuex.mapGetters([
            'currentUser'
        ])
    },
    methods: {
        ...Vuex.mapActions([
            'signout'
        ])
    }
});

store.subscribe((mutation, state) => {
    if(mutation.type === 'setUser'){
        if(!state.user){
            window.location = 'login.html?redirect=profile.html'
        }
    }
});
