new Vue({
    el: 'body',
    data() {
        return {
            name: '',
            password: '',
            confirmPassword: '',
            email: ''
        }
    },
    methods: {
        emailSignup() {
            firebase.auth()
                .createUserWithEmailAndPassword(this.email, this.password)
                .then(data => console.log(data))
                .catch(function (error) {
                // Handle Errors here.
                var errorCode = error.code;
                var errorMessage = error.message;
                // ...
            });
        }
    }
});