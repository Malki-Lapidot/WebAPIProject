const url = '/JobFinderAPI';
let JobsArr = [];

const getItems = () => {
    fetch(url)
        .then(Response => Response.json())
        .then(Data => displayItems(Data))
        .catch(error => console.error('Unable to get items.', error))
}

const addItem = () => {

    const newJob = {
        "location": (document.getElementById('Location').value.trim() === "" || document.getElementById('Location').value.trim() === undefined) ? null : document.getElementById('Location').value.trim(),
        "jobFieldCategory": (document.getElementById('JobFieldCategory').value.trim() === "" || document.getElementById('JobFieldCategory').value.trim() === undefined) ? null : document.getElementById('JobFieldCategory').value.trim(),
        "sallery": (document.getElementById('Sallery').value.trim() === "" || document.getElementById('Sallery').value.trim() === undefined) ? null : document.getElementById('Sallery').value.trim(),
        "jobDescription": (document.getElementById('JobDescription').value.trim() === "" || document.getElementById('JobDescription').value.trim() === undefined) ? null : document.getElementById('JobDescription').value.trim(),
        "postedDate": (document.getElementById('PostedDate').value.trim() === "" || document.getElementById('PostedDate').value.trim() === undefined) ? null : document.getElementById('PostedDate').value.trim()
    };

    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newJob)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(() => {
            getItems();
            document.getElementById('Location').value = ""
            document.getElementById('JobFieldCategory').value = ""
            document.getElementById('Sallery').value = ""
            document.getElementById('JobDescription').value = ""
            document.getElementById('PostedDate').value = ""
        })
        .catch(error => console.error('Unable to add item.', error));
}

const deleteItem = (id) => {
    fetch(`${url}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}


const displayEditForm = (id) => {
    const JobToEdit = JobsArr.find(item => item.JobID === id)

    document.getElementById('edit-id').value = JobToEdit.jobID
    document.getElementById('edit-Location').value = JobToEdit.location
    document.getElementById('edit-JobFieldCategory').value = JobToEdit.jobFieldCategory
    document.getElementById('edit-Sallery').value = JobToEdit.sallery
    document.getElementById('edit-JobDescription').value = JobToEdit.jobDescription
    document.getElementById('edit-PostedDate').value = JobToEdit.postedDate
    document.getElementById('editForm').style.display = 'block'
}

const editItem = () => {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        JobID: parseInt(itemId, 10),
        Location: document.getElementById('edit-Location').value.trim(),
        JobFieldCategory: document.getElementById('edit-JobFieldCategory').value.trim(),
        Sallery: document.getElementById('edit-Sallery').value.trim(),
        JobDescription: document.getElementById('edit-JobDescription').value.trim(),
        PostedDate: document.getElementById('edit-PostedDate').value.trim()
    };

    fetch(`${url}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

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

const displayItems = (JobsJson) => {
    console.log(JobsJson)
    const tbody = document.getElementById('Jobs');
    tbody.innerHTML = '';

    displayCounter(JobsJson.length)

    const button = document.createElement('button');

    JobsJson.forEach(element => {

        //edit button
        let editButton = button.cloneNode(false)
        editButton.innerText = 'Edit'
        editButton.setAttribute('onclick', `displayEditForm(${element.JobID})`)

        //delete button
        let deleteButton = button.cloneNode(false)
        deleteButton.innerText = 'Delete'
        deleteButton.setAttribute('onclick', `deleteItem(${element.jobID})`)

        let tr = tbody.insertRow()

        let td1 = tr.insertCell(0)
        let Location = document.createTextNode(element.location)
        td1.appendChild(Location)

        let td2 = tr.insertCell(1)
        let JobFieldCategory = document.createTextNode(element.jobFieldCategory)
        td2.appendChild(JobFieldCategory)

        let td3 = tr.insertCell(2)
        let Sallery = document.createTextNode(element.sallery)
        td3.appendChild(Sallery)

        let td4 = tr.insertCell(3)
        let JobDescription = document.createTextNode(element.jobDescription)
        td4.appendChild(JobDescription)

        let td5 = tr.insertCell(4)
        let PostedDate = document.createTextNode(element.postedDate)
        td5.appendChild(PostedDate)

        let td6 = tr.insertCell(5);
        td6.appendChild(editButton);

        let td7 = tr.insertCell(6);
        td7.appendChild(deleteButton);

    });

    JobsArr = JobsJson
}



// function getItems() {
//     fetch(url)
//         .then(response => response.json())
//         .then(data => _displayItems(data))
//         .catch(error => console.error('Unable to get items.', error));
// }

// function addItem() {
//     const addNameTextbox = document.getElementById('add-name');

//     const Job = {
//         isGlutenFree: false,
//         name: addNameTextbox.value.trim(),

//         JobID: 111,

//             Location:

//         JobFieldCategory:

//             Sallery:

//         JobDescription:

//             PostedDate:
//     };

//     fetch(uri, {
//         method: 'POST',
//         headers: {
//             'Accept': 'application/json',
//             'Content-Type': 'application/json'
//         },
//         body: JSON.stringify(item)
//     })
//         .then(response => response.json())
//         .then(() => {
//             getItems();
//             addNameTextbox.value = '';
//         })
//         .catch(error => console.error('Unable to add item.', error));
// }

// function deleteItem(id) {
//     fetch(`${uri}/${id}`, {
//         method: 'DELETE'
//     })
//         .then(() => getItems())
//         .catch(error => console.error('Unable to delete item.', error));
// }

// function displayEditForm(id) {
//     const item = pizzas.find(item => item.id === id);

//     document.getElementById('edit-name').value = item.name;
//     document.getElementById('edit-id').value = item.id;
//     document.getElementById('edit-isGlutenFree').checked = item.isGlutenFree;
//     document.getElementById('editForm').style.display = 'block';
// }

// function updateItem() {
//     const itemId = document.getElementById('edit-id').value;
//     const item = {
//         id: parseInt(itemId, 10),
//         isGlutenFree: document.getElementById('edit-isGlutenFree').checked,
//         name: document.getElementById('edit-name').value.trim()
//     };

//     fetch(`${uri}/${itemId}`, {
//         method: 'PUT',
//         headers: {
//             'Accept': 'application/json',
//             'Content-Type': 'application/json'
//         },
//         body: JSON.stringify(item)
//     })
//         .then(() => getItems())
//         .catch(error => console.error('Unable to update item.', error));

//     closeInput();

//     return false;
// }

// function closeInput() {
//     document.getElementById('editForm').style.display = 'none';
// }

// function _displayCount(itemCount) {
//     const name = (itemCount === 1) ? 'pizza' : 'pizza kinds';

//     document.getElementById('counter').innerText = `${itemCount} ${name}`;
// }

// function _displayItems(data) {
//     const tBody = document.getElementById('pizzas');
//     tBody.innerHTML = '';

//     _displayCount(data.length);

//     const button = document.createElement('button');

//     data.forEach(item => {
//         let isGlutenFreeCheckbox = document.createElement('input');
//         isGlutenFreeCheckbox.type = 'checkbox';
//         isGlutenFreeCheckbox.disabled = true;
//         isGlutenFreeCheckbox.checked = item.isGlutenFree;

//         let editButton = button.cloneNode(false);
//         editButton.innerText = 'Edit';
//         editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

//         let deleteButton = button.cloneNode(false);
//         deleteButton.innerText = 'Delete';
//         deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

//         let tr = tBody.insertRow();

//         let td1 = tr.insertCell(0);
//         td1.appendChild(isGlutenFreeCheckbox);

//         let td2 = tr.insertCell(1);
//         let textNode = document.createTextNode(item.name);
//         td2.appendChild(textNode);

//         let td3 = tr.insertCell(2);
//         td3.appendChild(editButton);

//         let td4 = tr.insertCell(3);
//         td4.appendChild(deleteButton);
//     });

//     computers = data;
// }