// Get the modal
var modal = document.getElementById('myModal-2');
// Get the button that opens the modal
var btn = document.getElementById("myLink-2");
// Get the <span> element that closes the modal
var span = document.getElementById("close-2");

new Vue({
    store,
    el: "div.result_item_box",
    data(){
        return {
            searchInput: ''
        }
    },

    methods: {
        searchUser(){

        },
        showModal(show){
            if(show){
                modal.style.display = "block";
            }else{
                modal.style.display = "none";
            }
        }
    }
});

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
};