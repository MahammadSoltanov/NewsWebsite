const tagContainer = document.querySelector('#tag-container');
const tagInput = document.querySelector('#tag-input');
const tagForm = document.querySelector('.second-form');
const tagsHiddenInput = document.querySelector('#tags-hidden-input');
const uniqueTags = new Set();

function createTag(text) {
    const cleanedTag = text.trim().toLowerCase();
    if (cleanedTag) {
        if (uniqueTags.has(cleanedTag)) {
            alert(`Tag '${cleanedTag}' already exists.`);
            return;
        }

        uniqueTags.add(cleanedTag);
        const tag = document.createElement('div');
        tag.className = 'tag';
        tag.textContent = text;

        tagContainer.appendChild(tag);


        const deleteButton = document.createElement('span');
        deleteButton.className = 'delete-tag';
        deleteButton.innerHTML = '&times;';
        deleteButton.addEventListener('click', function () {
            tag.remove();
            uniqueTags.delete(cleanedTag);

        });

        tag.appendChild(deleteButton);

        console.log("Created a tag");
        console.log(tag);
    }
}

tagInput.addEventListener('keyup', function (event) {
    if (event.key === 'Enter' && tagInput.value.trim() !== '') {
        createTag(tagInput.value.trim());
        tagInput.value = '';
    }
});

document.querySelector('.second-form').addEventListener('submit', function (event) {
    console.log("Form submitted");
    const selectedTags = Array.from(document.querySelectorAll('.tag')).map(tagElement => tagElement.textContent);
    tagsHiddenInput.value = JSON.stringify(selectedTags);
    console.log(tagsHiddenInput);
    console.log(selectedTags);    
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});