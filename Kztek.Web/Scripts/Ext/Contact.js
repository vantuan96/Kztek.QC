$(function () {
    $("body").on("click", "a[name=btnContact]", function () {
        $.ajax({
            type: "GET",
            data: { },
            url: '/Home/ModalContact',
            async: false,
            success: function (response) {
                $("#boxModal").html(response);
                $("#ModalContact").modal("show");
               
            }
        });
    })
    $("body").on("click", "#btnSave", function () {
        Contact.saveData();
    })
   
});

var Contact = {
    init: function () {
        Contact.Event();
    },
    Event: function () {    
        $('#frmContact').validate({

            rules: {
                txtName: {
                    required: true
                },
                txtPhone: {
                    required: true

                },
                txtEmail: {
                    required: true,
                    email: true
                },
                txtNote: {
                    required: true
                }
            },
            messages: {
                txtName: {
                    required: "Vui lòng nhập họ tên của bạn",
                },
                txtPhone: {
                    required: "Vui lòng nhập số điện thoại của bạn"
                    //  number: "Số điện thoại phải có định dạng kiểu số",
                },
                txtEmail: {
                    required: "Vui lòng nhập email của bạn",
                    email: "Địa chỉ email không đúng định dạng"
                },
                txtNote: {
                    required: "Vui lòng nhập ý kiến của bạn"
                }
            }
        });

        $("#boxContact").find("#btnSave").click(function () {
            //debugger;
            if ($('#frmContact').valid()) {
                Contact.saveData();
            }
        });
    },
    saveData: function () {

        var name = $("#boxContact").find("input[name=txtName]").val();
        var email = $("#boxContact").find("input[name=txtEmail]").val();
        var phone = $("#boxContact").find("input[name=txtPhone]").val();
        var note = $("#boxContact").find("textarea[name=txtNote]").val();

        intRegex = /[0-9 -()+]+$/;

        if (name === '' || name === null || name === 'undefined') {
            $('#name').show();
            return false;
        } else {
            $('#name').hide();
        }

        if (phone === '' || phone === null || phone === 'undefined') {
            $('#phone1').show();
            return false;
        } else {
            $('#phone1').hide();
        }

        if ((phone.length < 10) || (!intRegex.test(phone))) {
            $('#phone2').show();
            //alert('Số điện thoại không đúng định dạng');
            return false;
        }
        else {
            $('#phone2').hide();
        }

        if (email === '' || email === null || email === 'undefined') {
            $('#email').show();
            return false;
        } else {
            $('#email').hide();
        }


        var obj = {
            FullName: name,
            Email: email,
            Phone: phone,
            Description: note
        }

        $.ajax({
            url: '/Home/Send',
            type: 'POST',
            data: {
                obj: obj,
            },
            success: function (response) {
                if (response.isSuccess) {
                    Contact.resetForm();

                    toastr.success(response.Message)
                } else {
                    toastr.error(response.Message)
                }
            }
        })
    },
    resetForm: function () {
        $("#boxContact").find("input[name=txtName]").val("");
        $("#boxContact").find("input[name=txtEmail]").val("");
        $("#boxContact").find("input[name=txtPhone]").val("");
        $("#boxContact").find("textarea[name=txtNote]").val("");
    }
}