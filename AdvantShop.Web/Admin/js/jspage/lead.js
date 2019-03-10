
$(function () {

    if ($(".create-new-task").length > 0) {

        var taskNewModal = $.advModal({
            modalClass: "taskmodal",
            htmlContent: $('.new-task-block').html(),
            buttons: [
                { textBtn: localize("Admin_EditLead_InWork"), classBtn: 'btn-submit', func: function () { createTask(); } },
                { textBtn: localize("Admin_EditLead_Cancel"), classBtn: 'btn-action', isBtnClose: true }
            ]
        });

        // show modal window
        $(".create-new-task").on("click", function (e) {

            var name = $(".customer-name-field").val(),
                email = $(".customer-email-field").val(),
                phone = $(".customer-phone-field").val(),
                customerId = $(".customer-id").val(),
                managerId = $(".customer-manager").val();
            
            $(".task-customer-name-field input").val(name);
            $(".task-customer-email-field input").val(email);
            $(".task-customer-phone-field input").val(phone);
            $(".task-customer-id").val(customerId);
            $(".task-customer-manager").val(managerId);

            if ($(".task-customer-taskname-field input").val() == "") {
                $(".task-customer-taskname-field input").val(localize("Admin_EditLead_NewTaskName") + $(".taskmodal .task-leadid").val());
            }

            if (customerId != "") {
                $(".task-existing-customer").show();
                $(".task-customer-fields").hide();
                $(".task-existing-customer .task-existing-customer-info").html("<div>" + name + "</div> <div>" + phone + "</div> " + email + "</div>");
            } else {
                $(".task-existing-customer").hide();
                $(".task-customer-fields").show();
                findRelatedCustomer();
            }

            initValidation($("form"));           

            taskNewModal.modalShow();
            e.stopPropagation();
        });

        // update related customers
        $(".task-customer-name-field input").on("keyup", function () {
            var text = $(this).val();
            if (text.length > 3) {
                findRelatedCustomer();
            }
        });

        $(".task-customer-email-field input").on("keyup", function () {
            var text = $(this).val();
            if (text.length > 3) {
                findRelatedCustomer();
            }
        });

        $(".task-customer-phone-field input").on("keyup", function () {
            var text = $(this).val();
            if (text.length > 3) {
                findRelatedCustomer();
            }
        });

        // choose related customer
        $("body").on("click", ".task-related-choose-customer a", function () {
            var id = $(this).attr("data-id");
            if (id != "") {
                $(".taskmodal .task-customer-id").val(id);

                $(".task-existing-customer").show();
                $(".task-customer-fields").hide();
                $(".task-customers-search").hide();

                var info = $(this).parents(".task-related-customer").children(".task-related-customer-name").html();
                $(".task-existing-customer .task-existing-customer-info").html(info);
            }
        });

        // choose other customer
        $("body").on("click", ".task-existing-customer-other a", function () {

            $(".taskmodal .task-customer-id").val("");

            $(".task-existing-customer").hide();
            $(".task-customer-fields").show();
            $(".task-customers-search").show();
        });


        function createTask() {
            $(".task-error").hide();
            if ($(".taskmodal .task-customer-name-field input").val().trim() == "" && 
                $(".taskmodal .task-customer-email-field input").val().trim() == "" &&
                $(".taskmodal .task-customer-phone-field input").val().trim() == "") {

                $(".taskmodal .task-error").html(localize("Admin_EditLead_WrongData"));
                $(".taskmodal .task-error").show();
                return;
            }
            
            if (!$("form").valid()) {
                $(".taskmodal .task-error").html(localize("Admin_EditLead_WrongData"));
                $(".taskmodal .task-error").show();
                return;
            }

            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                url: "httphandlers/lead/createtask.ashx",
                data: {
                    leadid: $(".taskmodal .task-leadid").val(),
                    taskname: $(".taskmodal .task-customer-taskname-field input").val(),
                    name: $(".taskmodal .task-customer-name-field input").val(),
                    email: $(".taskmodal .task-customer-email-field input").val(),
                    phone: $(".taskmodal .task-customer-phone-field input").val(),
                    customerId: $(".taskmodal .task-customer-id").val(),
                    manager: $(".taskmodal .task-customer-manager").val(),
                },
                success: function (data) {
                    if (data != null && data.result != "error") {
                        window.location = "ManagerTask.aspx?TaskId=" + data.taskid;
                    } else {
                        $(".taskmodal .task-error").html(data.error);
                        $(".taskmodal .task-error").show();
                    }
                },
                error: function () {
                    notify("error", notifyType.error);
                }
            });
        }

        function findRelatedCustomer() {
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                url: "httphandlers/lead/findrelatedcustomers.ashx",
                data: {
                    name: $(".taskmodal .task-customer-name-field input").val(),
                    email: $(".taskmodal .task-customer-email-field input").val(),
                    phone: $(".taskmodal .task-customer-phone-field input").val(),
                },
                success: function (data) {
                    if (data.result == "success") {
                        var customersCount = data.customers.length;

                        if (customersCount != 0) {
                            $(".task-customers-search").show();
                            var customersHtml = "";
                            for (var i = 0; i < customersCount; i++) {
                                customersHtml +=
                                    "<div class=\"task-related-customer\">" +
                                        "<div class=\"task-related-customer-name\"> " + data.customers[i].Name + " " + data.customers[i].Email + " " + data.customers[i].Phone + "</div>" +
                                        "<div class=\"task-related-choose-customer\"><a data-id=\"" + data.customers[i].Id + "\" href=\"javascript:void(0)\">" + localize("Admin_EditLead_Choose") + "</a></div>" +
                                    "</div>";
                            }
                            $(".task-customers-search-results").html(customersHtml);
                        } else {
                            $(".task-customers-search").hide();
                        }
                    }
                },
                error: function () {
                    notify("error", notifyType.error);
                }
            });
        }


        $("body").on("change", ".lead-status", function() {
            if ($(this).val() == "Processing" && $(".customer-manager").val() == "") {
                notify("Для данного статуса необходимо выбрать менеджера", notifyType.error);
            }
        });
    }
});
