$(function () {
    init();

    function init() {
        $.ajax({
            url: "/Home/GetCategory",
            cache: true,
            success: function (data) {
                $("#menu-category").append(data);
            },
            error: function (data) {

            }

        });

        $.ajax({
            url: "/Member/Customer",
            cache: false,
            success: function (data) {
                $("#auth-link").append(data);
                if ($(".customer-id").val() != null) {
                    CustomerSwitchTab("AccountInfo");
                }

            },
            error: function (data) {

            }

        });
    }

    $(window).on('scroll', function () {

            if ($(this).scrollTop() > 100) {
                $('.scroll-top-wrapper').addClass('show');
            } else {
                $('.scroll-top-wrapper').removeClass('show');
            }
        });

    $('.scroll-top-wrapper').on('click', function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });
    

    $("#product-details").dialog({
        autoOpen: false,
        position: { my: "center bottom", at: "center top", of: window },
        width: 1000,
        resizable: false,
        modal: true,
        open: function () {
            var url = '../../Product/PopupProduct';
            console.log($(".product-hoverbox input[type='hidden']").val());
            $.ajax({
                url: url,
                data: "ProductID=" + $(".active input[type='hidden']").val(),
                cache: false,
                success: function (data) {
                    $("#product-details").html(data);
                },
                error: function (err) {
                    $("#product-details").html(err);
                },
                close: function (event, ui) {
                    $("#product-details").dialog("close");
                }

            });
        }
    });

    $('.product-item').on('click', '.product-hoverbox', function () {
        $('.product-hoverbox').removeClass('active');
        $(this).addClass('active');
        $("#product-details").dialog("open");
        $('.product-hoverbox').removeClass('active');
    });

    $('.product-image, .product-description').on('click', function () {
        $('.product-hoverbox').removeClass('active');
        $(this).siblings('.product-hoverbox').addClass('active');
        var ProductID = $(this).siblings('.active').find('.product-id').val();
        window.location.href = "../../Product/ProductDetails?ProductID=" + ProductID;

    });    


    $('ul.tab-list li').click(function () {
        var tab_id = $(this).attr('data-tab');
        $('ul.tab-list li').removeClass('current');
        $('.tab-content').removeClass('current');
        $(this).addClass('current');
        $("#" + tab_id).addClass('current');
    });

    $('.tab-link').click(function () {
        $('.tab-link').removeClass('current');
        $(this).addClass('current');
        CustomerSwitchTab($(".current span").attr("id"));
    });

    $('.button-back').click(function () {
        alert("halo");
        $('.tab-link').removeClass('current');
        $(this).addClass('current');
        CustomerSwitchTab($(".current span").attr("id"));
    });


    function CustomerSwitchTab(page) {
        $("#tab").html("<div id='wait' style='display:block;width:69px;height:89px;border:none;position:absolute;top:50%;left:55%;padding:2px;'><img src='Content/images/ajax_loader.gif' width='64' height='64' /><br>Loading..</div>");
        $.ajax({
            url: "/Customer/" + page + "/" + $(".customer-id").val(),
            cache: false,
            success: function (data) {
                $("#tab").empty();
                $("#tab").append(data);

            },
            error: function (err) {
                $("#tab").append(data);
            }
        });
    }

    

});

