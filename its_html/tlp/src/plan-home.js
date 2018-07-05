new Vue({
    store,
    el: "div.result_item_box",
    data() {
        return {
            plans: [],
            planLoaded: false
        }
    },
    computed: {
        ...Vuex.mapGetters([
            'currentUser',
        ]),
        loading() {
            return !this.currentUser || !this.planLoaded
        }
    },
    created() {
        this.$store.subscribe(this.handleCommit);
    },
    methods: {
        ...Vuex.mapActions([
            'fetchPlansOfUser'
        ]),
        deletePlan(uid) {
            this.plans = this.plans.filter(plan => plan.uid !== uid);
            this.$store.dispatch('deletePlan', {uid});
        },
        handleCommit(mutation, state) {
            if (mutation.type === 'setUser') {
                if (mutation.payload.user) {
                    this.fetchPlansOfUser()
                        .then((querySnapshot) => {
                                this.planLoaded = true;
                                querySnapshot.forEach((doc) => {
                                    this.plans.push({
                                        uid: doc.id,
                                        ...doc.data()
                                    });
                                });
                            }
                        )
                        .catch(reason => {
                            console.error(reason);
                        })
                }

            }
        }
    }
});