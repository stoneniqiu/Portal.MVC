$(function() {
    $("#usermanage").addClass("active");
    $("#usermanage ul").slideDown();


    $("#dbt").click(function () {
        var cks = $("table input:checkbox:checked");
        if (cks.length == 0) {
            alert("请选择要删除用户");
        } else {
            if (confirm("确定删除这些用户?")) {
                cks.each(function () {
                    $(this).parent().parent().parent().parent().fadeOut();
                    var id = $(this).data("id");
                    $.post("/User/Delete", { id: id }, function () {

                    });
                });
            }
        }
    });

    $("#emptyBt").click(function () {
        $("form .editor-field input").val("");
    });
});
