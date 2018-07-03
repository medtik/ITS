const googleProvider = new firebase.auth.GoogleAuthProvider();

const store = new Vuex.Store({
    state: {
        user: undefined
    },
    mutations: {
        setUser(state, payload) {
            state.user = payload.user;
        }
    },
    getters: {
        currentFile() {
            const url = window.location + '';
            return url.substr(
                url.lastIndexOf('/') + 1,
            )
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
            console.log('signout');
            return new Promise((resolve, reject) => {
                firebase.auth()
                    .signOut()
                    .then(resolve)
                    .catch(reject);
            })
        }
    }
});
