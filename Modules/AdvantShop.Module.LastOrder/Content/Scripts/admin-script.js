$(document).ready(function () {
    //$.ajax({
    //    url: "../FNPAdmin/Index",
    //    success: function (data) {
    //        $("#adminModuleWrap").html(data);


    //    }
    //})

    $.ajax({
        url: "../fnpadmin/load",
        success: function (data) {
            $('#rmfrom').val(data.rm.from);
            $('#rmto').val(data.rm.to);
            $('#rtt').val(data.rm.rType);
            $('#smpmp').val(data.smpm.Period);
            $('#smpmpt').val(data.smpm.PeriodType);
            $('#sp').val(data.sp);
            $('#ucnac').attr('checked', data.ucnac);
            $('#n').val(data.n);
            $('#c').val(data.c);
            $('#ucnac').change();
            $('#loc').val(data.loc);
            $('#userCity').val(data.userCity);
            //$('#alwaysShow').attr('checked', data.alwaysShow);
        }
    })

    $('#adminModuleWrap').on('change', '#ucnac', function () {
        var ucnac = $('#ucnac').is(':checked');
        if (ucnac) {
            $('#n').removeAttr('disabled');
            $('#c').removeAttr('disabled');
        } else {
            $('#n').attr('disabled', 'disabled');
            $('#c').attr('disabled', 'disabled');
        }
    })

    $('#adminModuleWrap').on('click', '#btnSave', function () {

        var valid = Validate();
        if (valid.valid) {

            var rm = { from: $('#rmfrom').val(), to: $('#rmto').val(), rType: $('#rtt').val() }
            var smpm = { Period: $('#smpmp').val(), PeriodType: $('#smpmpt').val() }
            var sp = $('#sp').val();
            var ucnac = $('#ucnac').is(':checked');
            var n = $('#n').val().split(',');
            var c = $('#c').val().split(',');
            var loc = $('#loc').val();
            var userCity = $('#userCity').val();
            //var alwaysShow = $('#alwaysShow').is(':checked');
            var obj = { "rm": rm, "smpm": smpm, "sp": sp, "ucnac": ucnac, "n": n, "c": c, "loc": loc, "userCity": userCity /*, "alwaysShow": alwaysShow*/ };
            var dd = JSON.stringify(obj);

            $.ajax({
                type: "POST",
                url: "../fnpadmin/save",
                data: dd,
                contentType: "application/json;charset=utf-8",
                processData: false,
                success: function (data) {
                    $('#statusMessage div').removeClass();
                    $('#statusMessage div').addClass('success');
                    $("#statusMessage").css("visibility", "visible");
                    setTimeout('$("#statusMessage").css("visibility","hidden")', 3000);
                    $('#statusMessage div').text("Сохранено")
                }
            })
        } else {
            $('#statusMessage div').removeClass();
            $('#statusMessage div').addClass('error');
            $("#statusMessage").css("visibility", "visible");
            setTimeout('$("#statusMessage").css("visibility","hidden")', 3000);
            $('#statusMessage div').html(valid.message)
        }
    })

    function Validate() {
        var errors = { valid: true, message: "" };
        var timeStr = ["минут", "часов", "дней"];
        var timeType = $('#rtt').val();
        var timeFrom = +$('#rmfrom').val();
        if (timeType == 0) {
            if (timeFrom > 59 || timeFrom < 1) {
                errors.valid = false;
                errors.message += "Генерация уведомления должна быть в диапазоне [1,59] " + timeStr[timeType] + " <br/>";
            }

            var timeTo = +$('#rmto').val();
            if (timeTo > 59 || timeTo < 1) {
                errors.valid = false;
                errors.message += "Генерация уведомления должна быть в диапазоне [1,59] " + timeStr[timeType] + " <br/>";
            }
        } else if (timeType == 1) {
            if (timeFrom > 23 || timeFrom < 1) {
                errors.valid = false;
                errors.message += "Генерация уведомления должна быть в диапазоне [1,23] " + timeStr[timeType] + " <br/>";
            }

            var timeTo = +$('#rmto').val();
            if (timeTo > 23 || timeTo < 1) {
                errors.valid = false;
                errors.message += "Генерация уведомления должна быть в диапазоне [1,23] " + timeStr[timeType] + " <br/>";
            }
        } else {
            if (timeFrom > 30 || timeFrom < 1) {
                errors.valid = false;
                errors.message += "Генерация уведомления должна быть в диапазоне [1,30] " + timeStr[timeType] + " <br/>";
            }

            var timeTo = +$('#rmto').val();
            if (timeTo > 30 || timeTo < 1) {
                errors.valid = false;
                errors.message += "Генерация уведомления должна быть в диапазоне [1,30] " + timeStr[timeType] + " <br/>";
            }
        }
        if (timeFrom >= timeTo) {
            errors.valid = false;
            errors.message += "Начальное время в диапазоне не может быть больше или равно конечному <br/>";
        }

        var smp = +$('#smpmp').val();
        var smpt = +$('#smpmpt').val();
        if (smpt == 0) {
            if (smp > 23 || smp < 1) {
                errors.valid = false;
                errors.message += "Максимальный период хранения в часах должен быть в диапазоне [1,23] <br/>";
            }
        }

        if (smpt == 1) {
            if (smp > 7 || smp < 1) {
                errors.valid = false;
                errors.message += "Максимальный период хранения в днях должен быть в диапазоне [1,7] <br/>";
            }
        }

        var ucnac = $('#ucnac').is(':checked');
        if (ucnac) {
            var n = $('#n').val();
            if (n == "") {
                errors.valid = false;
                errors.message += "Введите список имен <br/>";
            }
            var c = $('#c').val();
            if (c == "") {
                errors.valid = false;
                errors.message += "Введите список городов <br/>";
            }
        }

        var suc = $('#userCity').val();

        if (suc == "") {
            $('#userCity').val("0");
        }

        var sp = $('#sp').val();
        if (sp == "")
        {
            errors.valid = false;
            errors.message += "Заполните поле  \"Период показа уведомления\"<br/>";
        }

        return errors;
    }

    $('#adminModuleWrap').on('keypress', '.n-time', function (e) {
        e = e || event;

        if (e.ctrlKey || e.altKey || e.metaKey) return;

        var chr = getChar(e);

        if (chr == null) return;

        if (chr < '0' || chr > '9') {
            return false;
        }
    })

    function getChar(event) {
        if (event.which == null) {
            if (event.keyCode < 32) return null;
            return String.fromCharCode(event.keyCode)
        }

        if (event.which != 0 && event.charCode != 0) {
            if (event.which < 32) return null;
            return String.fromCharCode(event.which)
        }

        return null;
    }

})