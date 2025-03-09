const url = '/Login';

const dom = {
    password: document.getElementById("password"),
    userName: document.getElementById("useName")
}

document.querySelector("form").addEventListener("submit", (e) => {
    e.preventDefault();
    localStorage.removeItem("Token")
    const item = { password: dom.password.value, permission: null, userName: dom.userName.value }
    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then((res) => {
            if (res.status == 401) {
                alert("Unauthorized")
            }
            else {
                localStorage.setItem("Token", res)
            }
        })
        .then(()=>{
            location.href='index.html';
        })
        .catch(error => {console.error('Unable to connect.', error)
            alert('Unable to connect.')
        });
})

