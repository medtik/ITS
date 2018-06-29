new Vue({
    el: 'body',
    data() {
        return {
            password: '',
            email: ''
        }
    },
    methods: {
        emailLogin() {
            firebase.auth().createUserWithEmailAndPassword(email, password).catch(function (error) {
                // Handle Errors here.
                var errorCode = error.code;
                var errorMessage = error.message;
                // ...
            });
        },
        googleLogin() {

        },
        facebookLogin() {

        }
    }
});