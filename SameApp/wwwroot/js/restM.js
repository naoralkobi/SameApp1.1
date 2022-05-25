async function getAll() {
    const r = await fetch('/api/Messages');
    const d = await r.json();
    console.log(d);
}

async function get() {
    const r = await fetch('/api/Messages/20000');
    const d = await r.json();
    console.log(d);
}

async function post() {
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
    const r = await fetch('/api/Messages/42', {
        method: 'PUT',
        Headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({Id:'10000', Content:'Hello World!', Created:'00:00', Sent: true})
    });
    console.log(r);
}


async function del() {
    const r = await fetch('/api/Messages/226', {
        method: 'DELETE'
    });
    console.log(r);
}
