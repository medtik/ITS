new Vue({
    store,
    el: "div.result_item_box",
    methods:{
        ...Vuex.mapActions([
            'signInGoogle'
        ])
    }
});