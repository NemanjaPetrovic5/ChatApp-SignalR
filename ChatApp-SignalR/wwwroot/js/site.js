// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var createRoomBtn = document.getElementById('create-room-btn')
var createRoomModal = document.getElementById('create-room-modal')

createRoomBtn.addEventListener('click', function () {
    createRoomModal.classList.add('active')
})

var adduserBtn = document.getElementById('add-user-btn')
var adduserModal = document.getElementById('add-user-modal')

adduserBtn.addEventListener('click', function () {
    adduserModal.classList.add('active')
})

function closeModal() {
    createRoomModal.classList.remove('active')
    adduserModal.classList.remove('active')

}

//---------------------------------//
var messageBody = document.querySelector('#messageBody');
messageBody.scrollTop = messageBody.scrollHeight - messageBody.clientHeight;