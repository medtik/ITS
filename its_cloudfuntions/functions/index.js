const functions = require('firebase-functions');
const admin = require('firebase-admin');
const request = require('request');

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//
// exports.helloWorld = functions.https.onRequest((request, response) => {
//  response.send("Hello from Firebase!");
// });

admin.initializeApp();

exports.addUserToFirestore = functions.auth
    .user()
    .onCreate((user) => {
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

exports.addUserGroupInvitation = functions.firestore
    .document('user/{uid}')
    .onUpdate((change, context) => {
        return new Promise((resolve, reject) => {
            const user = change.after.data();
            const previousUser = change.before.data();
            if (user.groupInvitaions
                && user.groupInvitaions.length !== previousUser.groupInvitaions.length
                && user.token) {

                let notYetNotifieds = user.groupInvitaions.filter(invitation => !invitation.notified);
                let notifications = [];

                for (const notification of notYetNotifieds) {
                    notifications.push({
                        to: user.token,
                        title: "Mời nhóm",
                        body: `bạn được mời vào ${notification.groupName}`
                    })
                }
                request({
                    url: 'https://exp.host/--/api/v2/push/send',
                    method: 'POST',
                    json: true,
                    headers: {
                        'accept': 'application/json',
                        'accept-encoding': 'gzip, deflate'
                    },
                    body: notifications
                });

                for (const invitation of user.groupInvitaions) {
                    invitation.notified = true
                }
                change.after.ref.update({
                    groupInvitaions: user.groupInvitaions
                }).then(resolve);
            } else {
                reject();
            }
        })
    });
