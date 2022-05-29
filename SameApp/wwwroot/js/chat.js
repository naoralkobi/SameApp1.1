function contactClick(name) {
    let contact = document.getElementById("chat_name").innerHTML;


    //if (name !== contact)
    //{
    document.getElementById('opening').setAttribute('hidden','hidden');
    document.getElementById('message-box').removeAttribute('hidden');
    document.getElementById('myTableData').removeAttribute('hidden');

    document.getElementById('contactProfile').removeAttribute('hidden');

    //document.getElementById("chat_name").textContent = displayName;

    document.getElementById("contactName").setAttribute('value', name);
    document.getElementById("send_contactName").removeAttribute('disabled');



    $('#send_contactName').trigger('click');
    // }
}

async function postContactToContactServer(displayName, currentUser, newUserName, newServer) {

    //var fromUser = document.getElementById("userNameShow").innerHTML;

    let toUser = newUserName;
    let server = newServer;
    let fromUser = currentUser;
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


async function postContactToMyServer(displayName, currentUser, newUserName, newServer) {

    let username = newUserName;
    let server = newServer;
    let fromUser = currentUser
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


async function InviteManager(displayName, currentUser, newUserName, newServer) {
    let result = postContactToMyServer(displayName, currentUser, newUserName, newServer);
    if (result) {
        await postContactToContactServer(displayName, currentUser, newUserName, newServer);
    }
    document.getElementById('Username').value = " ";
    document.getElementById('Display-name').value = " ";
    document.getElementById('Server').value = " ";
}


async function postTransfer(server, message, sender, receiver) {




    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ "from": sender, "to": receiver, "content": message })
    };

    const response = await fetch("https://" + server +"/api/transfer", request);
    return response.ok;

}

async function postTransferToMyServer(server, message, sender, receiver) {
    let time = insertDate() + " " + insertTimeMessage();

    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({"id":'20000', "content":message, "created":time, "sent":true, "UserId":sender, "ContactId":receiver})
    };
    //"api/Contacts/{id1}/[controller]"
    let string = "https://" + server + "/api/Contacts/" + receiver + "/Messages";
    const response = await fetch(string, request);
    console.log(request);
    return response.ok;
}


async function transferManager(server, message, sender, receiver) {
    if (message.length > 0){
        let result = postTransferToMyServer(server, message, sender, receiver);
        if (result) {
            await postTransfer(server, message, sender, receiver);
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