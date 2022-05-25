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
    alert("in put");
    const request = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({"Id":'Hana', "Name":'update name', "Server":'localhost:7001', "Last":'n', "LastDate":'n'})
    };

    const response = await fetch("https://localhost:7001/api/Contacts/Hana", request);
    return response.ok;
}


async function del() {
    const r = await fetch('https://localhost:7001/api/Contacts/Aviv', {
        method: 'DELETE'
    });
}