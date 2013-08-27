(function ($, document, undefined) {
    var pluses = /\+/g; function raw(s) { return s; }
    function decoded(s) { return decodeURIComponent(s.replace(pluses, ' ')); }
    var config = $.cookie = function (key, value, options) {
        if (value !== undefined) {
            options = $.extend({}, config.defaults, options);
            if (value === null) { options.expires = -1; }
            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setDate(t.getDate() + days);
            }
            value = config.json ? JSON.stringify(value) : String(value);
            return (document.cookie = [encodeURIComponent(key), '=', config.raw ? value : encodeURIComponent(value), options.expires ? '; expires=' + options.expires.toUTCString() : '', options.path ? '; path=' + options.path : '', options.domain ? '; domain=' + options.domain : '', options.secure ? '; secure' : ''].join(''));
        }
        var decode = config.raw ? raw : decoded;
        var cookies = document.cookie.split('; ');
        for (var i = 0, parts; (parts = cookies[i] && cookies[i].split('=')) ; i++) {
            if (decode(parts.shift()) === key) {
                var cookie = decode(parts.join('='));
                return config.json ? JSON.parse(cookie) : cookie;
            }
        }
        return null;
    };
    config.defaults = {};
    $.removeCookie = function (key, options) {
        if ($.cookie(key) !== null) { $.cookie(key, null, options); return true; }
        return false;
    };
})(jQuery, document);
jQuery(document).ready(function ($) {
    var productID=$('#product_id').val();
    var max_quantity = parseInt($('#max-quantity').html());
    $('.upNum').click(
		function (e) {
		    sumNum = 1;
		    sumNum += parseInt($('#quantity').val());
		    if (sumNum >= max_quantity) {
		        e.stopPropagation();
		        $('#quantity').val(max_quantity);
		    }
		    else {
		        $('#quantity').val(sumNum);
		    }
		    //$('#district_select').change();
		});
    $('.downNum').click(function (e) {
        sumNum = parseInt($('#quantity').val());
        sumNum -= 1;
        if (sumNum > max_quantity) {
            e.stopPropagation();
            $('#quantity').val(max_quantity);
        }
        else if (sumNum <= 1) {
            e.stopPropagation(); $('#quantity').val(1);
        }
        else { $('#quantity').val(sumNum); }
        //$('#district_select').change();
    });
    //o so luong o trang chi tiet
    $("#quantity").keyup(function(){
	    var quantity=parseInt($(this).val());
	    var str=new String(quantity);
	    if(str=='NaN'||quantity==0){$(this).val('1');}
	    if(quantity>parseInt($('#max-quantity').html())){
		    quantity=parseInt($('#max-quantity').html());
		    $("#quantity").val(quantity);
	    }
            //$('#district_select').change();
	    return;
    });
    //o so luong trong gio hang
    $(".count").keyup(function () {
        debugger;
        var quantity = parseInt($(this).val());
        var str = new String(quantity);
        if (str == 'NaN' || quantity == 0) { $(this).val('1'); }
        if (quantity > 100) {
            quantity = 100;
            $(this).val(quantity);
        }
        //$('#district_select').change();
        return;
    });
    //tien hanh gan cookie vao gio hang khi trang dc load
    if ($.cookie("onetop_str_item_id")) {
        debugger
        var coo = $.cookie("onetop_str_item_id");
        $.ajax({
            url: '/sanpham/command',
            type: 'post',
            data: { "co": coo },
            dataType: 'json',
            success: function (data) {
                $('.showCartTop').html(data.success);
                $('.numCart').text(data.html);
            }
        });
    }
    //Them sp vao gio hang - btn bieu tuong gio hang
    $(".add-to-cart").click(function(){
        debugger;
        var product_id=$('#item_id').val();
        var productX=$("#zoom1 img:last").offset().left;
        var productY=$("#zoom1 img:last").offset().top;
        var basketX=$(".your-cart").offset().left;
        var basketY=$(".your-cart").offset().top;
        var gotoX=basketX-productX;
        var gotoY=basketY-productY;
        var newImageWidth=$("#zoom1").width()/5;
        var newImageHeight = $("#zoom1").height() / 5;
        var count = parseInt($('#quantity').val());
        $("#zoom1 img:last").clone().prependTo("#zoom1").css({
            'position':'absolute'
        }).animate({opacity:0.4},100).animate(
        {
            opacity:0,
            marginLeft:gotoX,
            marginTop:gotoY,
            width:newImageWidth,
            height:newImageHeight
        },1200,function(){
            $(this).remove();
            debugger
            var num_cart=parseInt($('.numCart').html())+count;
            $('.numCart').html(num_cart);
            var date=new Date();
            date.setTime(date.getTime()+(2*3600*1000));
            var product_id=$('#item_id').val();
            if(product_id){
                var str_product=jQuery.cookie('onetop_str_item_id');
                if(str_product){
                    var obj=$.parseJSON(str_product);
                    var check=0;
                    var dem=0;
                    for(key in obj){
                        if(key==product_id){obj[key]+=count;check=1;}
                        dem++;
                        if (dem == 1) { var first_element = key; }
                        if (dem == 10) {
                            $('.numCart').html(num_cart-obj[first_element]);
                            delete obj[first_element];
                        }
                    };
                    if(check==0){obj[product_id]=count;}
                    str_product=JSON.stringify(obj);
                }
                else{
                    var arr={};
                    arr[product_id]=count;
                    str_product=JSON.stringify(arr);
                }
                $.cookie('onetop_str_item_id', str_product, { expires: date, domain: '', path: '/' });
                debugger
                var co = $.cookie('onetop_str_item_id');
                $.ajax({
                    url: '/sanpham/UpdateTopCart',
                    type:'post',
                    data: { "co": co, "type": 0 },
                    dataType:'json',
                    success:function(data){
                        $('.showCartTop').html(data.success);
                        $('.numCart').text(data.html);
                    }
                });
                return true;
            }
            else return;
        });
    });
    //Them sp vao gio hang - btn Mua ngay
    $('a.btnAddCart').click(function () {
        debugger;
        link = $(this).attr('rel') + '?cmd=buy';
        var product_id = $('#item_id').val();
        var str_product = $.cookie('onetop_str_item_id');
        var count = parseInt($('#quantity').val());
        if (str_product) {
            var obj = $.parseJSON(str_product);
            var dem = 0;
            var check = 0;
            for (key in obj) {
                if (key == product_id) { obj[key] += count; check = 1; }
                dem++;
            }
            if (check == 0) { obj[product_id] = count; }
            str_product = JSON.stringify(obj);
        } else {
            var arr = {};
            arr[product_id] = count;
            str_product = JSON.stringify(arr);
        }
        var date = new Date();
        date.setTime(date.getTime() + (2 * 3600 * 1000));
        $.cookie('onetop_str_item_id', str_product, { expires: date, domain: '', path: '/' });
        var co = $.cookie("onetop_str_item_id");
        if (count != '') {
            $.post('/sanpham/UpdateTopCart', { "co": co, "type": 1 },
                function (data) { window.location.href = link; }
            );
        }
    });
    //Xoa sp khoi gio hang
    $('.RemoveLink').click(function () {
    debugger
        // Get the id from the link 
        var product_id = $(this).attr('data-id');
        if (product_id) {
            clearUpdateMessage();
            var str_product = $.cookie('onetop_str_item_id');
            if (str_product) {
                var obj = $.parseJSON(str_product);
                var count = 0;
                for (key in obj) {
                    count++;
                    if (key == product_id) {
                        $('tr#row-' + product_id).remove();
                        delete (obj[product_id]);
                        count = count - 1;
                        $('.num-cart').html(parseInt($('.num-cart').html() - 1));
                        $('.quantity').change();
                    }
                }
                if (count) {
                    str_product = JSON.stringify(obj);
                    $.cookie('onetop_str_item_id', str_product, { expires: date, domain: '', path: '/' });
                }
                else {
                    str_product = '';
                    $.cookie('onetop_str_item_id', str_product, { expires: date, domain: '', path: '/' });
                    window.location.href = $('#root_url').val();
                }
                var date = new Date();
                date.setTime(date.getTime() + (2 * 3600 * 1000));
                var co = $.cookie('onetop_str_item_id');
                $.ajax({
                    url: '/sanpham/UpdateTopCart',
                    type: 'post',
                    data: { "co": co, "type": 0 },
                    dataType: 'json',
                    success: function (data) {
                        $('.showCartTop').html(data.success);
                        $('.numCart').text(data.html);
                    }
                });
                $.post('/sanpham/RemoveFromCart', { 'id': product_id },
                function (data) {
                    location.reload();
                });
            }
            else { return false; }
        }
        else
            return;
    });
    //Cap nhat so luong trong gio hang
    $('.RefreshQuantity').click(function () {
        debugger
        // Get the id from the link 
        var recordToUpdate = $(this).attr('data-id');
        //Set quantity number to 0 if input value is empty
        var countToUpdate = 0;
        if ($('#' + $(this).attr('txt-id')).val().trim() != '') {
            countToUpdate = $('#' + $(this).attr('txt-id')).val();
        }
        if (recordToUpdate != '') {
            clearUpdateMessage();
            $.post('/sanpham/UpdateCartCountItem', { 'id': recordToUpdate, 'cartCount': countToUpdate },
                function () { location.reload();});
        }
    });
    function clearUpdateMessage() {
        $('#update-message').text('');
    }
    function htmlDecode(value) {
        if (value) {
            return $('<div />').html(value).text();
        }
        else {
            return '';
        }
    }
    if (typeof String.prototype.trim !== 'function') {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        }
    }
    $('.grid-view-click').click(function () {
        $(this).addClass('act');
        $('.list-view-click').removeClass('act');
        $('.gridview').fadeIn(200);
        $('.listview').hide();
    });
    $('.list-view-click').click(function () {
        $(this).addClass('act');
        $('.grid-view-click').removeClass('act');
        $('.listview').fadeIn(200);
        $('.gridview').hide();
    });

    //addCart
    var cart_tm;
    $('.ViewCart').hover(function () {
        window.clearTimeout(cart_tm);
        $(this).find('a:first').addClass('activeCart');
        $('.showCartTop').slideDown(300);
    }, function () {
        cart_tm = setTimeout(function () {
            $('.ViewCart a:first').removeClass('activeCart');
            $('.showCartTop').slideUp(800);
        }, 400);
    });
    //viewHome
    $('.btnViewType').click(function () {
        debugger
        $('.activeViewType').removeClass('activeViewType').addClass('btnViewType');
        $(this).removeClass('btnViewType').addClass('activeViewType');
        var type = $(this).text();
        $.ajax({
            type: "POST",
            url: '/Home/Index',
            data: { req: type },
            dataType: "html",
            success: function (response) {
                debugger;
                $('body').html(response);
            }
        });
    });
    //tab chitiet
    $('.titleTabs ul li a.tab').click(function () {
        var activeTab = $(this).attr('href');
        $('.titleTabs ul li').removeClass('active');
        $(this).parent().addClass('active');
        auto_scroll(activeTab);
        return false;
    });
    function auto_scroll(anchor) {
        var target = jQuery(anchor);
        target = target.length && target || jQuery('[name=' + anchor.slice(1) + ']');
        if (target.length) {
            var targetOffset = target.offset().top - 50;
            jQuery('html,body').animate({ scrollTop: targetOffset }, 10);
            return false;
        }
    }
    $(window).scroll(function () {
        var toado = jQuery(window).scrollTop();
        //alert(toado);
        if (toado > 640) {
            jQuery('.titleTabs').css('position', 'fixed').css('top', '0')
            jQuery(".titleTabs").css('background', '#fff')
            .css('width', '73%').css('border-bottom', '1px solid #CCC').css('padding', '10px')
            jQuery('li.btn-cart').css('display', 'block')
            jQuery('li.active').css('height', '51px')
        }
        else {
            jQuery(".titleTabs").css('background', 'none').css('top', '300px').css('position', 'static').css('padding', '0px').css('padding-top', '7px')
            jQuery('li.btn-cart').css('display', 'none')
            jQuery('.titleTabs li').css('height', '41px')
            jQuery('.titleTabs li.btn-cart').css('height', '20px')

        }
    });
});

//Drop Category
var min_angle = 60;
var coordinates = [{ 'x': 0, 'y': 0 }];
var mousestep = 10;
$(function () {
    var great_dad = $('#flyout');
    var parent_cat = $('#wrap-parent');
    var sub_cat = $('#wrap-child');
    var nav_timeout;
    var flyoutcomplete = function () { }
    var flyout = function () {
        sub_cat.show();
        sub_cat.stop().animate({ width: 200 }, { duration: "fast", }).css({ 'overflow': 'visible' });
    }
    var hide_all = function (mode) { sub_cat.css({ width: 0 }); sub_cat.hide(); }
    $('.sub-level1').mousemove(function (e) {
        add_coordinates({ 'x': e.pageX, 'y': e.pageY });
    });
    $("#wrap-parent li.par_cat").hover(function () {
        window.clearTimeout(nav_timeout);
        var match = /^nav_cat_(.+)/.exec(this.id);
        var cat = (match ? match[1] : "");
        var angle = get_angel(coordinates[0], coordinates[coordinates.length - 1]);
        if (angle <= 90 && angle >= min_angle) {
            $('.sub-level2').hide();
            $('#sub_cat_' + cat + ', #sub_cat_' + cat + ' .sub-level2').show();
            $('#sub_cat_' + cat).addClass('sub_active');
        }
        else {
            nav_timeout = setTimeout(function () {
                $('.sub-level2').hide();
                $('#sub_cat_' + cat + ', #sub_cat_' + cat + ' .sub-level2').show();
                $('#sub_cat_' + cat).addClass('sub_active');
            }, 250)
        }
    }, function () { window.clearTimeout(nav_timeout); });
    $('.wrap-sub, .sub-level2').hover(function (e) { $(this).show(); }, function () { });
    $('.par_cat').mouseenter(flyout);
    $('#big-daddy').mouseleave(hide_all);
    $('.no_sub').mouseleave(hide_all);
    var tm; $('#big-daddy').hover(function () {
        if ($(this).hasClass('act')) { return false; }
        $('.viewCat_item').addClass('act');
        $('#flyout').addClass('act');
    }, function () {
        if ($(this).hasClass('act')) { return false; }
        $('.viewCat_item').removeClass('act');
        $('#flyout').removeClass('act');
    });
});
function add_coordinates(coordinates_value) {
    coordinates.push(coordinates_value);
    if (coordinates.length > mousestep)
        coordinates.shift();
}
function get_angel(coordinates1, coordinates2) {
    var dx = coordinates1.x - coordinates2.x;
    var dy = coordinates1.y - coordinates2.y;
    return Math.acos(dx / Math.sqrt(dx * dx + dy * dy)) * (180 / Math.PI);
}

