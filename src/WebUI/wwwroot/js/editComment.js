//Selectors & variables.
let modalView = document.querySelector(".edit-comment-wrapper");
let commentInput = document.querySelector(".edit-comment-field");

let editButton;
let removeButton;
let commentField;
let commentId;
let commentContentContainer;

//Get view to edit comment.
function editCommentView(event) {

    if (modalView != null) {
        return;
    }

    //Initialize selectors.
    editButton = event.target;
    removeButton = editButton.parentElement.querySelector(".comment-remove-btn");
    commentContentContainer = editButton.parentElement.parentElement.querySelector(".comment-content-container");
    commentField = editButton.parentElement.parentElement.querySelector(".comment-content-text");
    commentId = editButton.parentElement.parentElement.querySelector(".comment-id");

    modalView = document.createElement("div");
    modalView.classList.add("edit-comment-wrapper");

    //Generate view to edit comment text.
    const editCommentFieldContainer = document.createElement("div");
    editCommentFieldContainer.classList.add("edit-comment-field-container");
    editCommentFieldContainer.classList.add("input-group-prepend");

    // Add input for comment new text.
    commentInput = document.createElement("textarea");
    commentInput.classList.add("form-control");
    commentInput.classList.add("edit-comment-field");
    commentInput.classList.add("auto-size");
    commentInput.value = commentField.textContent;
    editCommentFieldContainer.appendChild(commentInput);

    const doneButtonFieldContainer = document.createElement("div");
    doneButtonFieldContainer.classList.add("edit-comment-button-container");

    //Add done button.
    const doneButton = document.createElement("button");
    doneButton.classList.add("btn-main-sm");
    doneButton.classList.add("edit-comment-button");
    doneButton.innerText = "Done";
    //doneButton.innerText = @@Localizer["Done"];
    doneButton.addEventListener("click", saveComment);
    doneButtonFieldContainer.appendChild(doneButton);

    modalView.appendChild(editCommentFieldContainer);
    modalView.appendChild(doneButtonFieldContainer);

    commentContentContainer.appendChild(modalView);

    //Hide comment controls.
    editButton.hidden = true;
    removeButton.hidden = true;
    commentField.hidden = true;
    commentInput.focus();
}

//Save edited comment.
function saveComment(event) {

    //Save new comment to database.
    $.ajax({
        url: "/Comments/Edit",
        type: "GET",
        dataType: "json",
        data: {
            "id": commentId.value,
            "text": commentInput.value
        },
        function(data) {
            console.log(data);
        }
    })

    //Update comment on the page
    commentField.innerText = commentInput.value;

    removeButton.hidden = false;
    editButton.hidden = false;
    commentField.hidden = false;

    //Remove view for comment editing
    commentContentContainer.removeChild(modalView);
    modalView = null;
}