new Vue({
    store,
    el: "div.result_item_box",
    data() {
        return {
            plans: undefined
        }
    },
    computed: {
        ...Vuex.mapGetters([
            'currentUser',
        ]),
        loading() {
            return !this.currentUser || !this.plans
        }
    },
    created() {
        this.$store.subscribe(this.handleCommit);
    },
    methods: {
        ...Vuex.mapActions([
            'fetchPlansOfUser'
        ]),
        deletePlan(id) {
            this.$store.dispatch('deletePlan', {id});
        },
        handleCommit(mutation, state) {
            if (mutation.type === 'setUser') {
                this.fetchPlansOfUser()
                    .then((querySnapshot) => {
                            if (!this.plans) {
                                this.plans = [];
                            }
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