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
        },
        currentUser(state) {
            return state.user
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
        },
        fetchPlansOfUser(context) {
            return firestore
                .collection("plan")
                .where('user', '==', context.getters.currentUser.uid)
                .get();
        },
        createPlan(context, payload) {
            return firestore
                .collection("plan")
                .add({
                    name: payload.name,
                    startDate: payload.startDate,
                    endDate: payload.endDate,
                    user: context.getters.currentUser.uid
                })
        },
        deletePlan(context, payload) {
            return firestore
                .collection("plan")
                .doc(payload.id)
                .delete();
        },
        inviteUserToGroup(context, payload) {
            return firestore
                .collection('groupInvitation')
                .add({
                    groupUid: payload.groupId,
                    userUid: payload.inviteeId,
                })
        },
        searchUser(){
            // return
        }
    }
});

firebase.auth().onAuthStateChanged((user) => {
    store.commit('setUser', {
        user,
    });

    if (user) {
        try{
            window.postMessage(JSON.stringify({
                type: 'uid',
                uid: state.user.uid
            }));
        }catch (e) {
            //DO NOTHING
        }
    }


});