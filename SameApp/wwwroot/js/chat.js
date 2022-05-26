function contactClick(name) {
    let contact = document.getElementById("chat_name").innerHTML;
    if (name !== contact)
    {
        document.getElementById('opening').setAttribute('hidden','hidden');
        document.getElementById('message-box').removeAttribute('hidden');
        document.getElementById('myTableData').removeAttribute('hidden');

        document.getElementById('contactProfile').removeAttribute('hidden');

        //document.getElementById("chat_name").textContent = displayName;

        document.getElementById("contactName").setAttribute('value', name);
        document.getElementById("send_contactName").removeAttribute('disabled');



        $('#send_contactName').trigger('click');
    }
}

async function postContactToContactServer() {

    //var fromUser = document.getElementById("userNameShow").innerHTML;

    let toUser = document.getElementById("Username").value;
    let server = document.getElementById("Server").value;
    let fromUser = document.getElementById("userNameShow").textContent;
    fromUser = fromUser.trim();

    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        //body: JSON.stringify({ "from": '1', "to": toUser, "server": 'localhost:7001' })
        body: JSON.stringify({ "from": fromUser, "to": toUser, "server": server })
    };

    const response = await fetch("https://" + server +"/api/invitations", request);
    return response.ok;

}


async function postContactToMyServer() {

    let username = document.getElementById("Username").value;
    let displayName = document.getElementById("Display-name").value;
    let server = document.getElementById("Server").value;
    let fromUser = document.getElementById("userNameShow").textContent;
    fromUser = fromUser.trim();
    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ "Id": username, "UserNameOwner": fromUser, "Name": displayName, "Server": server, "Last": '', "LastDate": '' })
    };


    const response = await fetch("https://" + server + "/api/Contacts", request);
    return response.ok;

}


async function InviteManager() {
    let result = postContactToMyServer();
    if (result) {
        await postContactToContactServer();
    }
    document.getElementById('Username').value = " ";
    document.getElementById('Display-name').value = " ";
    document.getElementById('Server').value = " ";
}


async function postTransfer(server, message) {

    let contactId = document.getElementById("combina").textContent;

    let fromUser = document.getElementById("userNameShow").textContent;
    fromUser = fromUser.trim();


    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ "from": fromUser, "to": contactId, "content": message })
    };


    const response = await fetch("https://" + server +"/api/transfer", request);
    return response.ok;

}

async function postTransferToMyServer(server, message) {

    let fromUser = document.getElementById("userNameShow").textContent;
    fromUser = fromUser.trim();
    let contactId = document.getElementById("combina").textContent;
    let time = insertDate() + " " + insertTimeMessage();

    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({"id":'20000', "content":message, "created":time, "sent":true, "UserId":fromUser, "ContactId":contactId})
    };
    let string = "https://" + server + "/api/Messages";
    const response = await fetch(string, request);
    console.log(request);
    return response.ok;
}


async function transferManager(server, message) {
    //let message = document.getElementById('message').value;

    alert("in transfer messages");

    if (message.length > 0){
        let result = postTransferToMyServer(server, message);
        if (result) {
            await postTransfer(server, message);
        }
    }
}



function insertDate(){
    let today = new Date();
    let month = today.getMonth()+1;
    if (month > 0 && month < 10){
        month = "0" + month;
    }
    return month +"/" +today.getDate()+'/'+today.getFullYear();
}



function insertTimeMessage(){
    let today = new Date();
    let hours = today.getHours();
    let minutes = today.getMinutes();

    minutes = minutes < 10 ? '0'+minutes : minutes;

    return hours + ':' + minutes;
}