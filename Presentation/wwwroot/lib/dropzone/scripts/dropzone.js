Dropzone.autoDiscover = false; // Disable auto-discovery

// Initialize Dropzone
var myDropzone = new Dropzone("#myDropzone", {
    url: "/Image/UploadImage",
    paramName: "ImageFile",
    maxFiles: 1,
    maxFilesize: 5, // Maximum file size in MB
    acceptedFiles: "image/*",
    dictDefaultMessage: "Drop files here or click to upload.",
    autoProcessQueue: false,
});

myDropzone.on('success', function (file, response) {
    console.log(response)
    if (response.success) {
        document.querySelector('#title-image-input').value = response.url;
        console.log(document.querySelector('#title-image-input'));
    }
    else {
        alert(response.error);
    }
});

myDropzone.on("addedfile", function (file) {
    if (this.files.length > this.options.maxFiles) {
        this.removeFile(file);
        alert("Please note that only one title image can be added per post.");
    }
    else {
        alert("Image will only be uploaded to the server upon submitting the post form.");
    }
});

document.querySelector('.second-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    myDropzone.processQueue();

    await new Promise((resolve) => {
        myDropzone.on('queuecomplete', resolve);

        setTimeout(() => {
            resolve();
        }, 2000);
    });

    document.querySelector('.second-form').submit();
});
