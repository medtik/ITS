new Vue({
    el: 'div.result_item_box',
    data() {
        return {
            password: '',
            email: '',
            error: undefined
        }
    },
    methods: {
        emailLogin() {
            console.log('yo');
            firebase.auth()
                .signInWithEmailAndPassword(this.email, this.password)
                .then(data => window.location = "/index.html")
                .catch((error) => {
                    console.warn(error);
                    this.error = error;
                });
        },
        googleLogin() {

        },
        facebookLogin() {

        }
    }
});