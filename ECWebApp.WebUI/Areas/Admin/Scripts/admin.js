$(function () {
    //*************************Admin Dashboard********************************
    var chckFlag = 0;
    $('.BodyDiv').mouseenter(function () {
        
        $(this).addClass("active");
        //$('.active .productDiv .hiddenDiv').css('visibility', 'visible');

        $('.active .productDiv .hiddenDiv').css({ opacity: 0, visibility: "visible" }).animate({ opacity: 1.0 }, 500);
    });

    $('.BodyDiv').mouseleave(function () {
        //$('.active .productDiv .hiddenDiv').css('visibility', 'hidden');
        $('.active .productDiv .hiddenDiv').css({ opacity: 1.0, visibility: "hidden" }).animate({ opacity: 0 }, 500);
        $('.BodyDiv').removeClass("active");
        
    });


    $('.productchck').click(function () {
        if (this.checked) {
            $('.Center_Hidden').css('visibility', 'visible');
            chckFlag += 1;
            $(this).addClass('active');
            $(this).removeClass('active');
        }
        else chckFlag -= 1;

        if (chckFlag == 0) {
            $('.Center_Hidden').css('visibility', 'hidden');
        }
    });
});

//**************upload photo preview**********************************

function readURL(input, divName) {
    if (input.files && input.files[0]) {

        var reader = new FileReader();
        $(divName).css("visibility", "visible");
        reader.onload = function (e) {
            $(divName)
                .attr('src', e.target.result)
                .width(200)
                .height(150);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

$(document).ready(function () {
    //************************Add Product *****************************************
    //******************color picker function******************
    $('#picker').colpick({
        layout: 'hex',
        submit: 0,
        colorScheme: 'dark',
        onChange: function (hsb, hex, rgb, el, bySetColor) {
            $(el).css('border-color', '#' + hex);
            // Fill the text box just if the color was set using the picker, and not the colpickSetColor function.
            if (!bySetColor) $(el).val(hex);
            $('#Inputpicker').val('#' + hex);
        }
    }).keyup(function () {
        $(this).colpickSetColor(this.value);
    });


    //*****************add new*************************************
    $('#btnAdd').on('click', function () {
        var colorCode = $('#Inputpicker').val();
        $('#selected_color').text(colorCode);
        $('#selected_colorPreview').css('background-color', colorCode);
    });

   
});
