//消息提示 用于操作是否成功或失败 
$.extend({
    infoShow: function (txt, type) {
        $(".errorinfo").html(txt).show();
        if (type == "1") {
            $(".errorinfo").css("color", "blue");
        } else {
            $(".errorinfo").css("color", "#ff1493");
        }
        setTimeout(function () {
            $(".errorinfo").fadeOut();
        }, 2000);
    }
});

//弹出自己的内容
$.fn.alertSelf = function() {
    alert($(this).html());
};
// 子目录下的checkbox 全选或者全取消
$.fn.checkall = function ($childcheckbox) {
    this.click(function() {
        $childcheckbox.attr("checked", $(this).is(':checked'));
    });
};

//用于排序的时候 置反order的值
$.fn.triggerdataOrder = function() {
    var order = this.attr("data-order");
    if (order == undefined) order = 0;
    order = order == "0" ? 1 : 0;
    this.attr("data-order", order);
};

$(function() {
    $(".ncloes").on("click",function() {
        $(this).parent().fadeOut();
    });
});
