var selectedValues = [];

document.addEventListener("DOMContentLoaded", function () {
    var selectElements = document.querySelectorAll('select[name="status"]');
    selectElements.forEach(function (select) {
        select.addEventListener("change", function () {
            var postId = select.closest("tr").querySelector('input[name="id"]').value;
            var status = select.value;
            console.log(postId);
            console.log(status);
            var existingIndex = selectedValues.findIndex(function (item) {
                return item.id === postId;
            });

            if (existingIndex !== -1) {
                selectedValues[existingIndex].status = status;
            } else {
                selectedValues.push({ id: postId, status: status });
            }
        });
    });
});

document.getElementById("buttonStatus").addEventListener("click", function (e) {
    var selectedValuesInput = document.getElementById("selectedPosts");
    selectedValuesInput.value = JSON.stringify(selectedValues);
    console.log(selectedValuesInput);
});