$(document).ready(function () {

    //search
    $(document).on("keyup", "#input-search", function () {
        let search = $("#input-search").val().trim();
        $("#blog-search .single-mini-post").slice(0).remove();
        if (search.length > 0) {
            $.ajax({
                url: "/blog/search?search=" + search,
                type: "get",
                success: function (res) {
                    $("#blog-search").append(res);
                }
            })
        }
    })
})