new Vue({
    store,
    el: "div.result_item_box",
    computed:{
        ...Vuex.mapGetters([
            'currentUser'
        ])
    }
});
