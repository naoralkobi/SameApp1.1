async function getAll() {
    alert("get all messages");
    const r = await fetch('/api/Messages');
    const d = await r.json();
    console.log(d);
}

async function get() {
    alert("get current messages");
    const r = await fetch('/api/Messages/20000');
    const d = await r.json();
    console.log(d);
}

async function post() {
    alert("in post");
    const request = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({"id":'20000', "content":'Hello', "created":'00:01', "sent":true})
    };

    const response = await fetch("https://localhost:7001/api/Messages", request);
    console.log(request);
    return response.ok;
}



async function put() {
    alert("in put");
    const request = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ "id":'331', "content":"new content", "created":"00:00", "sent":true, "UserId":"Naor", "ContactId":"Aviv"})
    };
    const response = await fetch("https://localhost:7001/api/Contacts/Aviv/Messages/331", request);
    return response.ok;
}


async function del() {
    alert("in del");

    const r = await fetch('https://localhost:7001/api/Contacts/Aviv/Messages/341', {
        method: 'DELETE'
    });
    console.log(r);
}
