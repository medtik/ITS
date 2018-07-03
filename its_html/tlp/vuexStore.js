const googleProvider = new firebase.auth.GoogleAuthProvider();

const store = new Vuex.Store({
    state: {
        user: undefined
    },
    mutations: {
        setUser(state, payload) {
            this.state.user = payload.user;
        }
    },
    actions: {
        signInGoogle() {
            return new Promise((resolve, reject) => {
                firebase.auth()
                    .signInWithRedirect(googleProvider)
                    .then(resolve)
                    .catch(reject);
            })
        },

        signout() {
            return new Promise((resolve, reject) => {
                firebase.auth()
                    .signOut()
                    .then(resolve)
                    .catch(reject);
            })
        }
    }
});

firebase.auth().onAuthStateChanged(function (user) {
    store.commit('setUser',{
        user
    });
});


