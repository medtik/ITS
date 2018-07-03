new Vue({
    store,
    el: "div.result_item_box",
    data() {
        return {
            plans: []
        }
    },
    computed: {
        ...Vuex.mapGetters([
            'currentUser',
        ]),
        loading() {
            console.debug('loading', !this.currentUser, !this.plans);
            return !this.currentUser || this.plans.length === 0
        }
    },
    created() {
        this.$store.subscribe(this.handleCommit);
    },
    methods: {
        ...Vuex.mapActions([
            'fetchPlansOfUser'
        ]),
        handleCommit(mutation, state) {
            if (mutation.type === 'setUser') {
                this.fetchPlansOfUser()
                    .then((querySnapshot) => {
                            querySnapshot.forEach((doc) => {
                                this.plans.push({
                                    id: doc.id,
                                    ...doc.data()
                                });
                            });
                        }
                    )
            }
        }
    }
});