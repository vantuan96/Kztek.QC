$(function () {
   
    $("body").on("click", "#btnSaveComment", function () {
        Comment.saveData($(this).attr("idata"));
    })

});

var Comment = {
    init: function () {
        Comment.Event();
    },
    Event: function () {
        $('#frmComment').validate({

            rules: {
                txtNote: {
                    required: true
                },
                txtName: {
                    required: true
                },
               
                txtEmail: {
                    required: true,
                    email: true
                }
                
            },
            messages: {
                txtNote: {
                    required: "Vui lòng nhập bình luận của bạn"
                },
                txtName: {
                    required: "Vui lòng nhập họ tên của bạn",
                },
                
                txtEmail: {
                    required: "Vui lòng nhập email của bạn",
                    email: "Địa chỉ email không đúng định dạng"
                }
                
            }
        });

        $("#boxComment").find("#btnSave").click(function () {
            //debugger;
            if ($('#frmComment').valid()) {
                Comment.saveData();
            }
        });
    },
    saveData: function (id) {

        var name = $("#frmComment").find("input[name=txtName]").val();
        var email = $("#frmComment").find("input[name=txtEmail]").val();     
        var note = $("#frmComment").find("textarea[name=txtNote]").val();

        if (note === '' || note === null || note === 'undefined') {
            $('#cm_note').show();
            return false;
        } else {
            $('#cm_note').hide();
        }

        if (name === '' || name === null || name === 'undefined') {
            $('#cm_name').show();
            return false;
        } else {
            $('#cm_name').hide();
        }

        if (email === '' || email === null || email === 'undefined') {
            $('#cm_email').show();
            return false;
        } else {
            $('#cm_email').hide();
        }


        var obj = {
            FullName: name,
            Email: email,       
            Description: note,
            Id: id
        }

        $.ajax({
            url: '/News/SendComment',
            type: 'POST',
            data: {
                obj: obj,
            },
            success: function (response) {
                if (response.isSuccess) {
                    Comment.resetForm();

                    toastr.success(response.Message)
                } else {
                    toastr.error(response.Message)
                }
            }
        })
    },
    resetForm: function () {
        $("#frmComment").find("input[name=txtName]").val("");
        $("#frmComment").find("input[name=txtEmail]").val("");
       
        $("#frmComment").find("textarea[name=txtNote]").val("");
    }
}