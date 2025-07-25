Dropzone.autoDiscover = false;

var myDropzone = new Dropzone("#myDropzone", {
    url: "/Image/UploadImage",
    paramName: "ImageFile",
    autoProcessQueue: false,    
    maxFiles: 1,
    maxFilesize: 5,             
    acceptedFiles: "image/*",    
    addRemoveLinks: true,       
    dictRemoveFile: "Remove",   
});

myDropzone.on("thumbnail", function (file) {    
    var progressEl = file.previewElement.querySelector("[data-dz-uploadprogress]");

    //if (progressEl) {
    //    progressEl.backgrounColor = "green";
    //    progressEl.color = "green";
    //    for (let p = 0; p <= 100; p++) {
    //        setTimeout(() => {
    //            progressEl.style.width = p + "%";
    //        }, p * 20);
    //}

    this.emit("dz-processing", file);
    this.emit("dz-success", file);
    this.emit("dz-complete", file);

    const parentEl = progressEl.parentElement;
    if (parentEl) {
        parentEl.style.width = "0px";
    }
});

myDropzone.on('success', function (file, response) {    
    if (response.success) {
        document.querySelector('#title-image-input').value = response.url;        
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
