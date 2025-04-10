$(document).ready(function () {
    $(".fileUpload").on("change", function(event){
        const files = event.target.files;
        const filesLength = files.length;
        if (filesLength > 0) {
            const imageSrc = URL.createObjectURL(files[0]);
            const imagePreviewElement = document.querySelector("#tb-image");
            imagePreviewElement.src = imageSrc;
            imagePreviewElement.style.display = "block";
        }
    })
});