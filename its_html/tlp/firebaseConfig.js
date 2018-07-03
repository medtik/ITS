// Initialize Firebase
const config = {
    apiKey: "AIzaSyCouzeKTc_xf3r7QJZjCjyEr7rceMB7rgA",
    authDomain: "its-g8.firebaseapp.com",
    databaseURL: "https://its-g8.firebaseio.com",
    projectId: "its-g8",
    storageBucket: "its-g8.appspot.com",
    messagingSenderId: "917708153355"
};
firebase.initializeApp(config);
//Database
const firestore = firebase.firestore();
firestore.settings({timestampsInSnapshots: true});
