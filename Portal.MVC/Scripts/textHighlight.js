function textHighLight(o, flag, rndColor, url) {
    var bgCor = '';
    var fgCor = '';
    if (rndColor) {
        bgCor = fRndCor(10, 20);
        fgCor = fRndCor(230, 255);
    } else {
        bgCor = 'yellow';
        fgCor = 'black';
    }
    var re = new RegExp(flag, 'i');
    for (var i = 0; i < o.childNodes.length; i++) {
        var o_ = o.childNodes[i];
        var o_p = o_.parentNode;
        if (o_.nodeType == 1) {
            textHighLight(o_, flag, rndColor, url);
        } else if (o_.nodeType == 3) {
            if (!(o_p.nodeName == 'A')) {
                if (o_.data.search(re) == -1) continue;
                var temp = fEleA(o_.data, flag);
                o_p.replaceChild(temp, o_);
            }
        }
    }

    function fEleA(text, flag) {
        var style = ' style="background-color:' + bgCor + ';color:' + fgCor + ';" '
        var o = document.createElement('span');
        var str = '';
        var re = new RegExp('(' + flag + ')', 'gi');
        if (url) {
            str = text.replace(re, '<a href="' + url +
            '$1"' + style + '>$1</a>'); //这里是给关键字加链接，红色的$1是指上面链接地址后的具体参数。
        } else {
            str = text.replace(re, '<span ' + style + '>$1</span>'); //不加链接时显示
        }
        o.innerHTML = str;
        return o;
    }

    function fRndCor(under, over) {
        if (arguments.length == 1) {
            var over = under;
            under = 0;
        } else if (arguments.length == 0) {
            var under = 0;
            var over = 255;
        }
        var r = fRandomBy(under, over).toString(16);
        r = padNum(r, r, 2);
        var g = fRandomBy(under, over).toString(16);
        g = padNum(g, g, 2);
        var b = fRandomBy(under, over).toString(16);
        b = padNum(b, b, 2);
        //defaultStatus=r+' '+g+' '+b 
        return '#' + r + g + b;
        function fRandomBy(under, over) {
            switch (arguments.length) {
                case 1: return parseInt(Math.random() * under + 1);
                case 2: return parseInt(Math.random() * (over - under + 1) + under);
                default: return 0;
            }
        }
        function padNum(str, num, len) {
            var temp = ''
            for (var i = 0; i < len; temp += num, i++);
            return temp = (temp += str).substr(temp.length - len);
        }
    }
}
