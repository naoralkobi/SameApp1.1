
$(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    //connection.start();


    connection.start().then(function () {
        const $currentUserId = $('#userNameShow');
        
        // add current User Id to Dict.
        connection.invoke("Connect", $currentUserId.text());

    })
    
    $('#send_button').click( () => {
        
        const $message = $('#message');
        const $currentContact = $('#userNameShow');
        const $receiverContactId = $('#combina');

        connection.invoke("Changed", $message.val(), $currentContact.text(), $receiverContactId.text());
    });

    $('#input-addContact').click( () => {
        const $newDisplayName = $('#Display-name');
        const $currentUser = $('#userNameShow');
        const $newUserName = $('#Username');
        const $newServer = $('#Server')
        connection.invoke("AddContact", $newDisplayName.val(), $currentUser.text(), $newUserName.val(), $newServer.val());
    });
    
    connection.on("Update", function (value, currentUser){
        //alert("update sender data");
        if (value.length > 0) {
            let tr = document.createElement("tr");

            let tdContent = document.createElement("td");
            let tdTime = document.createElement("td");
            let timeSmall = document.createElement("small");

            tdContent.textContent= value;
            timeSmall.textContent = insertDate() + " " + insertTimeMessage();

            const $currentContact = $('#userNameShow');
            if ($currentContact.text() === currentUser){
                tdContent.setAttribute('id', 'container');
                tdTime.setAttribute('id', 'timeSender');
            }
            else
            {
                tdContent.setAttribute('id', 'receive_container');
                tdTime.setAttribute('id', 'timeReceiver');
            }



            tdTime.setAttribute('style', 'color: grey');
            tdTime.appendChild(timeSmall);

            tr.appendChild(tdTime);
            tr.appendChild(tdContent);

            document.getElementById("message_tbody").appendChild(tr);

            let CurrentContactName = document.getElementById('chat_name').innerText;


            if (value.length > 10)
            {
                document.getElementById(CurrentContactName+"+message").innerText = value.toString().substring(0,9) + "...";
            }
            else
            {
                document.getElementById(CurrentContactName+"+message").innerText = value;
            }
            document.getElementById(CurrentContactName+"+time").innerText = timeSmall.textContent;

            document.getElementById('message').value=' ';
            
        }
        
    })
    
    connection.on("ChangeReceived", function (value, sender, receiver) {
        
        const $currentChat = $('#chat_name'); 
        const $currentContact = $('#combina');
        
        if ($currentContact.text() === sender)
        {
            if (value.length > 0) {
                let tr = document.createElement("tr");

                let tdContent = document.createElement("td");
                let tdTime = document.createElement("td");
                let timeSmall = document.createElement("small");

                tdContent.textContent= value;
                timeSmall.textContent = insertDate() + " " + insertTimeMessage();
                
                const $currentContact = $('#userNameShow');
                if ($currentContact.text() === sender){
                    tdContent.setAttribute('id', 'container');
                    tdTime.setAttribute('id', 'timeSender');
                }
                else
                {
                    tdContent.setAttribute('id', 'receive_container');
                    tdTime.setAttribute('id', 'timeReceiver');
                }


                tdTime.setAttribute('style', 'color: grey');
                tdTime.appendChild(timeSmall);

                tr.appendChild(tdTime);
                tr.appendChild(tdContent);

                document.getElementById("message_tbody").appendChild(tr);

                let CurrentContactName = document.getElementById('chat_name').innerText;


                if (value.length > 10)
                {
                    document.getElementById(CurrentContactName+"+message").innerText = value.toString().substring(0,9) + "...";
                }
                else
                {
                    document.getElementById(CurrentContactName+"+message").innerText = value;
                }
                document.getElementById(CurrentContactName+"+time").innerText = timeSmall.textContent;
                document.getElementById('message').value=' ';

                
            }
            
        }

        if (value.length > 0) {
            let result = transferManager('localhost:7001',value,sender, receiver);
        }
        

    })

    connection.on("ChangeContact", function (displayName, currentUser, newUserName, newServer) {

        let contactId = document.getElementById('Username').value;
        let server = document.getElementById('Server').value;

        //displayName.length > 0 && contactId.length > 0 && server.length > 0
        if (displayName.length > 0)
        {

            let tr = document.createElement("tr");

            let tdDisplayName = document.createElement("td");
            let tdLast = document.createElement("td");
            let tdLastTime = document.createElement("td");


            tdDisplayName.textContent = displayName;
            contactId = "'" + contactId + "'";
            tr.setAttribute("onclick", "contactClick("+contactId+")");

            tdLast.textContent= " ";
            tdLastTime.textContent= " ";



            tr.appendChild(tdDisplayName);
            tr.appendChild(tdLast);
            tr.appendChild(tdLastTime);

            document.getElementById("contact_tbody").appendChild(tr);

            document.getElementById('btnHideModal').click();
            

            let result = InviteManager(displayName, currentUser, newUserName, newServer);
        }

    })

    connection.on("UpdateContact", function (displayName, currentUser) {

        let contactId = document.getElementById('Username').value;
        let server = document.getElementById('Server').value;

        //displayName.length > 0 && contactId.length > 0 && server.length > 0
        if (displayName.length > 0)
        {

            let tr = document.createElement("tr");

            let tdDisplayName = document.createElement("td");
            let tdLast = document.createElement("td");
            let tdLastTime = document.createElement("td");


            tdDisplayName.textContent = currentUser;
            currentUser = $.trim(currentUser);
            currentUser = "'" + currentUser + "'";

            tr.setAttribute("onclick", "contactClick("+currentUser+")");

            tdLast.textContent= " ";
            tdLastTime.textContent= " ";



            tr.appendChild(tdDisplayName);
            tr.appendChild(tdLast);
            tr.appendChild(tdLastTime);

            document.getElementById("contact_tbody").appendChild(tr);

            document.getElementById('btnHideModal').click();

        }

    })
    

    
});

