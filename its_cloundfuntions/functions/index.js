const functions = require('firebase-functions');
const admin = require('firebase-admin');
// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//
// exports.helloWorld = functions.https.onRequest((request, response) => {
//  response.send("Hello from Firebase!");
// });

admin.initializeApp();

exports.addUserToFirestore = functions.auth.user().onCreate((user) => {
    return new Promise((resolve, reject) => {
        admin.firestore()
            .doc(`user/${user.uid}`)
            .set({
                email: user.email,
                displayName: user.displayName,
                photoURL: user.photoURL
            })
            .then(value => {
                console.info('addUserToFirestore success', user.uid);
                resolve(user.uid);
            })
    })
});