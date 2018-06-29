new Vue({
    el: 'div.result_item_box',
    data() {
        return {
            name: '',
            password: '',
            confirmPassword: '',
            email: '',
            error: undefined,
            success: undefined
        }
    },
    methods: {
        emailSignup() {
            if (this.password != this.confirmPassword) {
                this.error = {
                    message: "Mật khẩu nhập lại không khớp"
                };
                return;
            }
            firebase.auth()
                .createUserWithEmailAndPassword(this.email, this.password)
                .then(data => {
                    var user = firebase.auth().currentUser;
                    user.updateProfile({

                        displayName: this.name
                    }).then(() => {
                        this.success = data;
                    })
                    this.error = undefined;
                })
                .catch((error) => {
                    console.log(error);
                    this.error = error
                });
        },
        exit(){
            window.location = '/login.html'
        }
    }
});