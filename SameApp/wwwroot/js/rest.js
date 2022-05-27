async function getAll() {
    const r = await fetch('/api/Contacts/');
    const d = await r.json();
    console.log(d);
}

async function get() {
    const r = await fetch('/api/Contacts/Aviv');
    const d = await r.json();
    console.log(d);
}

async function post() {
    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({"Id":'Aviv', "Name":'NewDisplayName', "Server":'localhost:7001', "Last":'', "LastDate":''})
    };

    const response = await fetch("https://localhost:7001/api/Contacts", request);
    return response.ok;
}

async function put() {
    
    const request = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ "Id": "Aviv", "UserNameOwner": "Naor", "Name": "newDisplayName", "Server": "localhost:7001", "Last": '', "LastDate": '' })
    };
    const response = await fetch("https://localhost:7001/api/Contacts/Aviv", request);
    return response.ok;
}



async function del() {
    alert("in del");

    const r = await fetch('https://localhost:7001/api/Contacts/Aviv', {
        method: 'DELETE'
    });
    console.log(r);
}

