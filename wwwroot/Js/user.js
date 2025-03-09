const url = '/Users';
let UsersArr = [];

let currentPermission = null;
let currentPassword = null;

let token = localStorage.getItem("Token");
const payload = token.split('.')[1];
const decodedPayload = JSON.parse(atob(payload));
currentPermission = decodedPayload.type;
currentPassword = decodedPayload.password;
if (currentPermission !== "SuperAdmin"||currentPermission==null)
    document.getElementById("addForm").style.display = "none";
else
    document.getElementById("addForm").style.display = "block";


const logOut = () => {
    localStorage.removeItem("Token")
    location.href = 'index.html'
}

const getUsers = () => {
    var param = '';
    if (currentPermission === "SuperAdmin")
        param = '/GetAllUsers';
    fetch(`${url}${param}`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(Response => Response.json())
        .then((Data) => {
            if (currentPermission !== "SuperAdmin")
                displayUsers([Data])
            else
                displayUsers(Data)
        })
        .catch(error => console.error('Unable to get job.', error))
}

const displayUsers = (UsersJson) => {
    const tbody = document.getElementById('Users');
    tbody.innerHTML = '';

    displayCounter(UsersJson.length)

    const button = document.createElement('button');

    UsersJson.forEach(element => {

        //edit button
        let editButton = button.cloneNode(false)
        editButton.innerText = 'Edit'
        editButton.setAttribute('onclick', `displayEditForm('${element.password}')`)

        //delete button
        let deleteButton = button.cloneNode(false)
        deleteButton.innerText = 'Delete'
        deleteButton.setAttribute('onclick', `deleteUser('${element.password}')`)

        let tr = tbody.insertRow()

        let td1 = tr.insertCell(0)
        let Password = document.createTextNode(element.password)
        td1.appendChild(Password)

        let td2 = tr.insertCell(1)
        let Permission = document.createTextNode(element.permission)
        td2.appendChild(Permission)

        let td3 = tr.insertCell(2)
        let UserName = document.createTextNode(element.userName)
        td3.appendChild(UserName)

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);

    });
    UsersArr = UsersJson
}

const addUser = () => {
    const newUser = {
        "password": (document.getElementById('password').value.trim() === "" || document.getElementById('password').value.trim() === undefined) ? null : document.getElementById('password').value.trim(),
        "permission": (document.getElementById('permission').value.trim() === "" || document.getElementById('permission').value.trim() === undefined) ? null : document.getElementById('permission').value.trim(),
        "userName": (document.getElementById('userName').value.trim() === "" || document.getElementById('userName').value.trim() === undefined) ? null : document.getElementById('userName').value.trim()
    };

    fetch(url, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUser)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(() => {
            getUsers();
            document.getElementById('password').value = ""
            document.getElementById('permission').value = ""
            document.getElementById('userName').value = ""
        })
        .catch(error => {console.error('Unable to add user.', error)
            alert('Unable to add user.')
        });
}

const deleteUser = (password) => {
    fetch(`${url}/${password}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
            'Accept': 'application/json'
        },
    })
        .then(() => {
            if (password === currentPassword) {
                localStorage.removeItem("Token");
                location.href = 'index.html'
            }
        })
        .then(() => getUsers())
        .catch(error => {console.error('Unable to delete user.', error)
            alert('Unable to delete user.')
        });
}

const displayEditForm = (password) => {
    var UserToEdit;
    UsersArr.forEach((user) => {
        if (user.password === password)
            UserToEdit = user;
    });
    document.getElementById('current-password').value = UserToEdit.password
    document.getElementById('edit-password').value = UserToEdit.password
    document.getElementById('edit-permission').value = UserToEdit.permission
    document.getElementById('edit-userName').value = UserToEdit.userName
    document.getElementById('editForm').style.display = 'block'
}

const editUser = () => {
    const user = {
        Password: document.getElementById('edit-password').value.trim(),
        Permission: document.getElementById('edit-permission').value.trim(),
        UserName: document.getElementById('edit-userName').value.trim()
    };
    const userPassword = document.getElementById('current-password').value.trim();
    fetch(`${url}/${userPassword}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(() => {
            if (currentPermission !== "SuperAdmin" || (currentPermission === "SuperAdmin" && userPassword === currentPassword)) {
                localStorage.removeItem("Token");
                return fetch('/Login', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(user)
                })
                    .then(response => response.json())
                    .then((res) => {
                        localStorage.setItem("Token", res)
                    })
                    .catch(error => {console.error('Unable to connect.', error)
                        alert('Unable to connect.')
                    });
            }
        })
        .then(() => getUsers())
        .catch(error => {console.error('Unable to update user.', error)
            alert('Unable to update user.')
        });

    closeInput();
    return false;
}

const closeInput = () => {
    document.getElementById('editForm').style.display = 'none'
}

const displayCounter = (itemCount) => {
    let counter = document.getElementById('counter')
    counter.innerHTML = itemCount
}