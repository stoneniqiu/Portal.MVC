$(function(){
    var min_height = 550;
    $(window).scroll(function() {
        //获取窗口的滚动条的垂直位置      
        var s = $(window).scrollTop();
        //当窗口的滚动条的垂直位置大于页面的最小高度时 
        if (s > min_height) {
            $(".indexhead").addClass("headerbg");
        } else {
            $(".indexhead").removeClass("headerbg");
        };
    });
});

$(function(){
    if   (document.all)   {   //IE     
        // alert("你用的浏览器是   Internet   Explorer");     
    }else{
        var t = $(".undline").css("left"),
            y = $(".active").parent().index(),
            line = $(".undline");
        $(".nav").find(".runline").mouseenter(function(){
            // 判断鼠标是否停留在当前页面的导航上
            if($(this).index() == y){
                return;
            }else{
                $(".nav").find(".runline").find("a").removeClass("active");
            }
            // 获取当前角标，设置当前角标对应的值
            var m = $(this).index();
            switch (m){ 
                case 6 : n=0; 
                break; 
                case 5 : n=1;
                break; 
                case 4 : n=2;
                break; 
                case 3 : n=3;
                break; 
                case 2 : n=4; 
                break; 
            } 
            // 设置当前下划线的left值

            var num = 20 + n*100 + "px";
            if (n == 4) {
                num = 460 + "px";
            }

            line.stop().animate({ "left": num }, "fast");
          //  $(this).find("a").addClass("active");
            // 判断如果当前鼠标悬停“关于我们”按钮时的效果
            if(n == 4||n==3){
                line.css({"width":"100px"});
            }else{
                line.css({"width":"60px"});
            }
             //设置鼠标离开当前导航时候的效果
            $(this).mouseleave(function(){
                // $(".runline").find("a").css({"color":"#fead00"});
                if (y == 3 || y == 2) {
                    line.css({"width":"100px"});
                }else{
                    line.css({"width":"60px"});
                }
                $(".nav").find("a").removeClass("active");
                line.stop().animate({ "left": t }, "fast").parent().find("li").eq(y).find("a");//.addClass("active")
            });
        });
    } 
    
});