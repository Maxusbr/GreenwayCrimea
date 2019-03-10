$(function () {

    $.validator.addMethod('intlTel', function (value, element) {

        var jqEl = $(element);

        return jqEl.intlTelInput != null ? jqEl.intlTelInput("isValidNumber") : false;

    }, 'Неверный формат телефона')

    $.validator.addClassRules("valid-required", { required: true });
    $.validator.addClassRules("valid-money", { money: true, required: true });
    $.validator.addClassRules("valid-select", { required: true });
    $.validator.addClassRules("valid-number", { number: true, required: true });
    $.validator.addClassRules("valid-email", { email: true, required: true });
    $.validator.addClassRules("valid-newemail", { email: true, required: true, newemail: true });
    $.validator.addClassRules("valid-login", { login: true, required: true });
    $.validator.addClassRules("valid-compare", { required: true, equalTo: ".valid-comparesource" });
    $.validator.addClassRules("valid-comparesource", { required: true, minlength: 6 });
    $.validator.addClassRules("valid-captcha", { required: true, minlength: 4, maxlength: 4 });
    $.validator.addClassRules("valid-amount", { required: true, min: 1 });
    $.validator.addClassRules("valid-phone", { required: true, digits: true, minlength: 11, maxlength: 11 });
    $.validator.addClassRules("valid-intl-tel", { required: true, intlTel: true });
    $.validator.addClassRules("valid-phonemodule", { required: true, digits: true, minlength: 10, maxlength: 10 });
    initValidation($("#form"));
    initValidation($("form"));
});

function initValidation(form, group) {

    if (!$(form).length) return;

    var speedMessage = 600;

    form.data("validLoad", true);

    form.validate({
        showErrors: function (errorMap, errorList) {
            var obj = this;
            var isValidLoad = form.data("validLoad");

            $.each(errorList, function () {
                var element = $(this.element);
                

                var container;
                
                if (element.parent('.input-wrap').length > 0) {
                    container = element.parent('.input-wrap');
                } else if (element.parent('.textarea-wrap').length > 0) {
                    container = element.parent('.textarea-wrap');
                } else if (element.parent('.intl-tel-input').length > 0) {
                    container = element.parent('.intl-tel-input');
                }
                else {
                    container = element;
                }


                if (element.rules().required) {
                    var errorIcon = container.nextAll(".error-icon");

                    errorIcon.removeClass("valid-success").addClass("valid-error");
                }

                if (isValidLoad) {
                    return true;
                }

                var errorMessage = container.nextAll(".error-message");
                var message = $("<div />", { html: this.message }).addClass("error-content");

                errorMessage.empty().append(message);
                errorMessage.slideDown(speedMessage);
            });

            $.each(obj.successList, function () {
                var element = $(this);
                var container;

                if (element.parent('.input-wrap').length > 0) {
                    container = element.parent('.input-wrap');
                } else if (element.parent('.textarea-wrap').length > 0) {
                    container = element.parent('.textarea-wrap');
                } else if (element.parent('.intl-tel-input').length > 0) {
                    container = element.parent('.intl-tel-input');
                } 
                else {
                    container = element;
                }
                
                if (element.rules().required) {
                    var errorIcon = container.nextAll(".error-icon");
                    errorIcon.removeClass("valid-error").addClass("valid-success");
                }

                var errorMessage = container.nextAll(".error-message");
                var message = $("<div />", { html: this.message }).addClass("error-content");

                errorMessage.slideUp(speedMessage, function () {
                    errorMessage.empty().append(message);
                });
            });
        }
    });

    validateControlsPos(form);
    
    form.valid(group);
    form.data("validLoad", false);
}

function validateControlsPos(form) {

    var f = form != null ? $(form) : $("#form");

    if (!f.data('validator')) {
        return;
    }
    
    var validElements = f.data('validator').elements();

    validElements.each(function () {
        var inp = $(this);
        var wrap;

        if (inp.parent('.input-wrap').length > 0) {
            wrap = inp.parent('.input-wrap');
        } else if (inp.parent('.textarea-wrap').length > 0) {
            wrap = inp.parent('.textarea-wrap');
        } else {
            wrap = inp;
        }

        var rules = inp.rules();
        var errIcon = wrap.nextAll(".error-icon");
        var errMessage = wrap.nextAll(".error-message");
        var container;
        var position;

        if (inp.is("[type=checkbox]") === true || inp.is("[type=radio]") === true) {
            container = $("label[for=" + inp.attr("id") + "]");
        } else {
            container = wrap;
        }


        if (errIcon.length === 0) {
            errIcon = $('<span />', { 'class': 'error-icon"'});
            container.after(errIcon);
        }

        if (rules.required) {
            errIcon.addClass("error-icon valid-error");
        }

        if (errMessage.length === 0) {
            errMessage = $("<div />").addClass("error-message");
            container.after(errMessage);
        }

        position = container.position();

        if (errIcon.length) {
            errIcon.css({ top: position.top, left: container.outerWidth() + position.left });
        }

        errMessage.css({ width: container.outerWidth(), left: position.left });
        errMessage.hide();
    });
}