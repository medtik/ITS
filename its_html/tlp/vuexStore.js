const googleProvider = new firebase.auth.GoogleAuthProvider();

const store = new Vuex.Store({
    state: {
        user: undefined,
        firestore: firestore
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
                .get()

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
                .doc(payload.uid)
                .delete();
        },
        inviteUserToGroup(context, payload) {
            firestore
                .collection('user')
                .doc(payload.userUid)
                .get()
                .then(doc => {
                    const groupInvitations = doc.data().groupInvitations;
                    groupInvitations.push({
                        groupId: payload.groupId,
                        groupName: payload.groupName,
                    })
                })
        },
        findUser(context, payload) {
            const start = payload.searchStr;
            const end = startText + '\uf8ff';

            return firestore.collection('user', ref =>
                ref.orderBy('displayName')
                    .startAt(start)
                    .endAt(end)
            )
        }


    }
});

firebase.auth().onAuthStateChanged((user) => {
    if (user) {
        firestore
            .collection("user")
            .doc(user.uid)
            .get()
            .then(doc => {
                store.commit('setUser', {
                    user: {
                        uid: user.uid,
                        ...doc.data()
                    }
                });
            });

        try {
            window.postMessage(JSON.stringify({
                type: 'uid',
                uid: user.uid
            }));
        } catch (e) {
            //DO NOTHING
        }
    } else {
        store.commit('setUser', {
            user: undefined,
        });
    }
});