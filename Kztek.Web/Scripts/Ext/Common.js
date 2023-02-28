$(function () {
    $('.input-mask-date').mask('99/99/9999');
    $(".numbericMoney").mask("000,000,000,000,000", { reverse: true });
    $('.daterangpicker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: true,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY'
        },
        singleDatePicker: true,
        showDropdowns: true
    }).val(''); //set default null


    $('.daterangtimepicker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: true,
        timePicker: true,
        timePickerIncrement: 1,
        timePicker24Hour: true,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY HH:mm'
        },
        singleDatePicker: true,
        showDropdowns: true
    }).val('');


    $('.daterangnoautopicker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: false,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY'
        },
        singleDatePicker: true,
        showDropdowns: true
    }).val('');


    $(".datepicker").datepicker({
        showOtherMonths: true,
        selectOtherMonths: false,
        locale: {
            format: 'DD/MM/YYYY'
        }
        //isRTL:true,


        /*
        changeMonth: true,
        changeYear: true,
    	
        showButtonPanel: true,
        beforeShow: function() {
            //change button colors
            var datepicker = $(this).datepicker( "widget" );
            setTimeout(function(){
                var buttons = datepicker.find('.ui-datepicker-buttonpane')
                .find('button');
                buttons.eq(0).addClass('btn btn-xs');
                buttons.eq(1).addClass('btn btn-xs btn-success');
                buttons.wrapInner('<span class="bigger-110" />');
            }, 0);
        }
*/
    }).val('');

    if (!ace.vars['touch']) {
        $('.chosen-select').chosen({ allow_single_deselect: true });
        //resize the chosen on window resize

        $(window)
            .off('resize.chosen')
            .on('resize.chosen', function () {
                $('.chosen-select').each(function () {
                    var $this = $(this);
                    $this.next().css({ 'width': $this.parent().width() });
                })
            }).trigger('resize.chosen');
        //resize chosen on sidebar collapse/expand
        $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
            if (event_name != 'sidebar_collapsed') return;
            $('.chosen-select').each(function () {
                var $this = $(this);
                $this.next().css({ 'width': $this.parent().width() });
            });
        });
    }

    $('.multiselect').multiselect({
        enableFiltering: true,
        enableHTML: true,
        nonSelectedText: "-- Lựa chọn --",
        allSelectedText: "Tất cả đã chọn",
        nSelectedText: "Đã chọn",
        numberDisplayed: 1,
        enableCaseInsensitiveFiltering: true,
        buttonClass: 'btn btn-white btn-primary',
        templates: {
            button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> &nbsp;<b class="fa fa-caret-down"></b></button>',
            ul: '<ul class="multiselect-container dropdown-menu"></ul>',
            filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
            filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default btn-white btn-grey multiselect-clear-filter" type="button"><i class="fa fa-times-circle red2"></i></button></span>',
            li: '<li><a tabindex="0"><label></label></a></li>',
            divider: '<li class="multiselect-item divider"></li>',
            liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
        }
    });
});

function formatJSONDate(jsonTime) {
    var MyDate_String_Value = jsonTime
    var value = new Date
        (
        parseInt(MyDate_String_Value.replace(/(^.*\()|([+-].*$)/g, ''))
        );
    return value;
}

function formatDateFromJson(jsonTime) {
    var dateString = jsonTime.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = ('0' + (currentTime.getMonth() + 1)).slice(-2);
    var day = ('0' + currentTime.getDate()).slice(-2);
    var year = currentTime.getFullYear();
    //var hour = currentTime.getHours();
    //var minute = currentTime.getMinutes();
    var date = day + "/" + month + "/" + year;

    return date;
}

function formatDateFromJson1(jsonTime) {
    var dateString = jsonTime.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = ('0' + (currentTime.getMonth() + 1)).slice(-2);
    var day = ('0' + currentTime.getDate()).slice(-2);
    var year = currentTime.getFullYear();
    var hour = currentTime.getHours();
    var minute = currentTime.getMinutes();
    var date = day + "/" + month + "/" + year + " " + hour + ":" + minute;

    return date;
}

function SearchSubmit(formName) {
    $('button[name=btnFilter]').click(function () {
        $('#chkExport').val('0');
        $('#' + formName).submit();
    });
}

function ExcelSubmit(formName) {
    $('button[name=btnExport]').click(function () {
        $('#chkExport').val('1');
        $('#' + formName).submit();
    });
}

function DeleteSubmit(url) {
    $('.btnDelete').click(function () {
        var cmd = $(this);
        var _id = cmd.attr('idata');

        bootbox.confirm("Bạn chắc chắn muốn xóa bản ghi này?", function (result) {
            if (result) {
                $.ajax({
                    url: url,
                    data: { id: _id },
                    type: 'json',
                    //async:false,
                    success: function (data) {
                        if (data.isSuccess) {
                            cmd.parent().parent().parent().fadeOut();
                            toastr.success(data.Message, 'Thông báo');
                        } else {
                            toastr.error(data.Message, 'Thông báo');
                        }
                    }
                });
            }
        });
    });
}
