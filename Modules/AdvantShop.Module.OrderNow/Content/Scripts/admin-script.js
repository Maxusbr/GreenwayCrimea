$(document).ready(function () {
    $.ajax({
        url: "../ONAdmin/Index",
        success: function (data) {
            $("#adminModuleWrap").html(data);
        }
    })
})

function Validate(show = false) {
    var x = document.getElementById("snackbar");
    var elements = $(".js-validateTime");
    var correct = true;
    elements.each(function () {
        var updatetime = $(this).val();
        if (updatetime.length != 5) {
            $(this).css("border-color", "red");
            correct = false;
            return true;
        }
        if (updatetime.split(':')[0].length != 2 || updatetime.split(':')[1].length != 2) {
            $(this).css("border-color", "red");
            correct = false;
            return true;
        }
        if (parseInt(updatetime.split(':')[0]) > 23 || parseInt(updatetime.split(':')[1]) > 59) {
            $(this).css("border-color", "red");
            
            correct = false;
            return true;
        }
        $(this).css("border-color", "#c3c3c3");
    })
    if (correct == false && show == true)
    {
        x.innerHTML = "Проверьте корректность заполнения!";
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
    }    
    return correct;
}

function saveSettings() {
    if (Validate(true)) {
        var text = $(".js-ONtxtOutputText").val();
        var timeout = $(".js-ONTimeoutTime").val();
        var weekends = $(".js-ONIncludeWeekends").is(':checked');
        var checkAvailability = $(".js-ONCheckAvailability").is(':checked');
        var showInCart = $(".js-ONShowInCart").is(':checked');
        var showInOrderConfirm = $(".js-ONShowInOrderConfirm").is(':checked');
        var ShowAt = $(".js-ONshowAt option:selected").text();
        var IconHeight = $(".js-ONiconHeight").val();
        var ShowStart = $(".js-ONShowStart").val();
        var ShowFinish = $(".js-ONShowFinish").val();
        var Ndays = $(".js-ONndays").val();
        var ShowMobile = $(".js-ONshowMobile").is(":checked");
        var timeoutText = $(".js-ONtxtOutputTimeoutText").val();

        var data = {
            "text": text,
            "timeout": timeout,
            "weekends": weekends,
            "checkAvailability": checkAvailability,
            "showInCart": showInCart,
            "showInOrderConfirm": showInOrderConfirm,
            "ShowAt": ShowAt,
            "IconHeight": IconHeight,
            "ShowStart": ShowStart,
            "ShowFinish": ShowFinish,
            "Ndays": Ndays,
            "ShowMobile": ShowMobile,
            "TimeoutMessage" : timeoutText
        };
        var send = JSON.stringify(data);
        $.ajax({
            url: "../ONAdmin/SaveSettings",
            type: "POST",
            dataType: "json",
            contentType: "application/json;charset=utf-8;",
            data: send,
            success: function (data) {
                $("#adminModuleWrap").html(data);
            }
        })
        var x = document.getElementById("snackbar");
        x.innerHTML = "Настройки успешно сохранены";
        x.className = "show";
        setTimeout(function () {
        x.className = x.className.replace("show", "");
        $.ajax({
            url: "../ONAdmin/Index",
            success: function (data) {
                $("#adminModuleWrap").html(data);
            }
        })}, 3000);
        
    }
}

function btnaddpicture() {
    var element = document.getElementById('ONupload');
    $(element).click();
}

function fileclick() {
    var element = document.getElementById('ONupload');
    if (typeof element.files[0] !== "undefined") {
        upload(element.files[0], this);
    }
}

function fileuploadimage() {
    var element = document.getElementById('ONupload');
    if (typeof element.files[0] !== "undefined") {
        upload(element.files[0], this);
    }
}

function upload(file, elem) {
    var xhr = new XMLHttpRequest();

    xhr.upload.onprogress = function (event) {
        var percent = event.loaded * 100 / event.total;
    }

    xhr.onload = xhr.onerror = function (data) {
        if (this.status == 200) {
            if (xhr.response != null)
                if (!!xhr.response.error) {
                    alert(xhr.response.message)
                    return;
                }
            $('#adminModuleWrap').html(xhr.response);
        } else {
            console.log("error " + this.statusText);
        }
    };
    var data = new FormData();
    data.append("image", file);
    xhr.responseType = "html";
    xhr.open("POST", "../ONadmin/uploadimage", true);
    xhr.send(data);

}

function deletePicture() {
    $.ajax({
        type:"POST",
        url: "../onadmin/DeleteImage",
        success: function (data) {
            $('#adminModuleWrap').html(data);
        }
    })
}

function AddUpdateProductTimeoutMessage(productid) {
    var message = $(".js-ONtxtOutputTimeoutText").val();
    $.ajax({
        type: "POST",
        url: "../onadmin/AddUpdateProductTimeoutMessage",
        data: { productid, message },
        success: function () {
            var x = document.getElementById("snackbar");
            x.innerHTML = "Текст уведомления после завершения времени показа основного уведомления успешно сохранен!";
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
        }
    });
}

function AddUpdateProductMessage(productid) {
    var message = $(".js-ONtxtOutputText").val();
    $.ajax({
        type: "POST",
        url: "../onadmin/AddUpdateProductMessage",
        data: { productid, message },
        success: function () {
            var x = document.getElementById("snackbar");
            x.innerHTML = "Текст уведомления успешно сохранен!";
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
        }
    });
}