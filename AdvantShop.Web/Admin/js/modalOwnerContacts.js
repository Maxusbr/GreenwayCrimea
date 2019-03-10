; (function ($) {

    'use strict';

    $(document).ready(function () {
        var modal;
        $('.js-intlTel').intlTelInput({
            initialCountry: 'ru'
        })

        if ($('#AdminUserInformation').length && $('.needShow').val()=='True') {
           modal= $.advModal({
                htmlContent: $('#AdminUserInformation'),
                closeEsc: false,
                cross: false,
                clickOut: false
            });
          modal.modalShow();
        }

          $('.segment-form-btn').on('click', function (e) {

            e.preventDefault();


            if ($('#aspnetForm').valid() === false) {
                return;
            }

            var model = {};

            model.Name = $('[name="name"]').val();
            model.Mobile = $('[name="phone"]').val();
            model.CompanyName = $('[name="company"]').val();

            model.Map = [
                {
                    Name: "Количество сотрудников в магазине",
                    Value: $('[name="staff"]').val(),
                },
                {
                    Name: "Ваш опыт продаж в интернете?",
                    Value: $('[name="segment-exp"] :selected').text(),
                },
                {
                    Name: "Есть ли у вас точки продаж в офлайне?",
                    Value: $('[name="segment-offline"] :selected').text(),
                }
            ];

            $.ajax({
                type: "POST",
                url: "HttpHandlers/SetUserProperties.ashx",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    modal.modalClose();
                },
                error: function (response) {
                    alert(response.d);
                }
            });
            return false;
        });

    });


})(jQuery);