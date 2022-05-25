
$(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    connection.start();

    $('#send_button').click( () => {
        const $message = $('#message');
        
        const $currentContact = $('#userNameShow');
        const $receiverContactId = $('#combina');
        
        connection.invoke("Changed", $message.val(), $currentContact.text(), $receiverContactId.text());
    });

    connection.on("ChangeReceived", function (value, currentUser) {
        
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



            if ($currentContact.text() === currentUser){
                let result = transferManager('localhost:7001',value);
            }
            
        }

    })
});


// handle with new contact event.
$(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    connection.start();

    $('#input-addContact').click( () => {
        const $displayName = $('#Display-name');

        const $currentContact = $('#userNameShow');
        
        connection.invoke("AddContact", $displayName.val(), $currentContact.text());
    });

    connection.on("ChangeContact", function (displayName, currentUser) {

        let contactId = document.getElementById('Username').value;
        let server = document.getElementById('Server').value;
        
        //displayName.length > 0 && contactId.length > 0 && server.length > 0
        if (displayName.length > 0)
        {
            
            let tr = document.createElement("tr");

            let tdDisplayName = document.createElement("td");
            let tdLast = document.createElement("td");
            let tdLastTime = document.createElement("td");


            const $currentContact = $('#userNameShow');
            if ($currentContact.text() === currentUser){
                tdDisplayName.textContent = displayName;
                contactId = "'" + contactId + "'";
                tr.setAttribute("onclick", "contactClick("+contactId+")");
            }else
            {
                tdDisplayName.textContent = currentUser;
                currentUser = $.trim(currentUser);
                currentUser = "'" + currentUser + "'";
                
                tr.setAttribute("onclick", "contactClick("+currentUser+")");
            }
            
            tdLast.textContent= " ";
            tdLastTime.textContent= " ";
            
            

            tr.appendChild(tdDisplayName);
            tr.appendChild(tdLast);
            tr.appendChild(tdLastTime);

            document.getElementById("contact_tbody").appendChild(tr);

            document.getElementById('btnHideModal').click();
            
            let result = InviteManager();
        }
        
    })
});