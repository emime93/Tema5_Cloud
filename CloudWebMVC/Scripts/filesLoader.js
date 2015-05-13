function loadFiles() {
    $.ajax({
        url: "/File/GetFiles",
        method: "GET",
        success: function (data) {
        }
    });
}