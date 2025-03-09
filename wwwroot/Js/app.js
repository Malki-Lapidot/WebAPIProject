const url = '/Jobs';
let JobsArr = [];

let currentPermission = null;
let currentPassword = null;

let token = localStorage.getItem("Token");
if (token == null) {
    document.getElementById("logOut").style.display = "none";
    document.getElementById("userLink").style.display = "none";
}
else {
    const payload = token.split('.')[1];
    const decodedPayload = JSON.parse(atob(payload));
    currentPermission = decodedPayload.type;
    currentPassword = decodedPayload.password;
}
if (currentPermission === "User"||currentPermission==null)
    document.getElementById("addForm").style.display = "none";
else
    document.getElementById("addForm").style.display = "block";

const logOut = () => {
    localStorage.removeItem("Token")
    location.href = 'index.html'
}

const getItems = () => {
    fetch(url)
        .then(Response => Response.json())
        .then(Data => displayItems(Data))
        .catch(error => {console.error('Unable to get job.', error)
            alert('Unable to get job.')
        })
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
        editButton.setAttribute('onclick', `displayEditForm(${element.jobID})`)

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

        if (currentPermission === "SuperAdmin" || (currentPermission === "Admin" && currentPassword === element.createdBy)) {
            let td6 = tr.insertCell(5);
            td6.appendChild(editButton);

            let td7 = tr.insertCell(6);
            td7.appendChild(deleteButton);
        }
    });
    JobsArr = JobsJson
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
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
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
        .catch(error => {console.error('Unable to add job.', error)
            alert('Unable to add job.')
        });
}

const displayEditForm = (id) => {
    var JobToEdit;
    JobsArr.forEach((item) => {
        if (item.jobID == id)
            JobToEdit = item;
    });
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
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => {console.error('Unable to update job.', error)
            alert('Unable to update job.')
        });
    closeInput();
    return false;
}

const deleteItem = (id) => {
    fetch(`${url}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("Token")}`,
            'Accept': 'application/json'
        },
    })
        .then(() => getItems())
        .catch(error => {console.error('Unable to delete job.', error)
            alert('Unable to delete job.')
        });
}

const closeInput = () => {
    document.getElementById('editForm').style.display = 'none'
}

const displayCounter = (itemCount) => {
    let counter = document.getElementById('counter')
    counter.innerHTML = itemCount
}