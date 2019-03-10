$(function () {

    if ($("#newtheme").length > 0) {

        var modalCreateTheme;
        $("body").on("click", ".createTheme", function (e) {
            modalCreateTheme = $.advModal({
                modalId: "create-theme-modal",
                htmlContent: $(".new-theme-modal-block." + $(this).attr("data-design")).html(),
                buttons: [{ textBtn: localize('Create'), isBtnClose: false, classBtn: 'btn-small btn-action createnewtheme' },
                          { textBtn: localize('Close'), isBtnClose: true, classBtn: 'btn-small btn-action' }]
            });
            modalCreateTheme.modalShow();
            e.stopPropagation();
        });

        $("body").on("click", ".createnewtheme", function () {
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                data: {
                    name: $("#create-theme-modal .themename").val().replace(" ", "_").replace("'", "").replace("\"", ""),
                    design: $("#create-theme-modal .themename").attr("data-design")
                },
                url: "httphandlers/design/createtheme.ashx",
                success: function (data) {
                    if (data.error) {
                        notify(data.error, notifyType.error);
                    } else {
                        window.location = data.msg;
                    }
                },
                error: function (data) {
                    alert("can't save");
                }
            });
        });

    }


    if ($("#editor").length > 0) {
        var editor = ace.edit("editor");
        editor.getSession().setMode("ace/mode/css");
        editor.setOptions({
            enableSnippets: true,
            enableLiveAutocompletion: true
        });
    }
    
    if ($("#themeeditor").length > 0) {
        
        $(".save-theme").on("click", function () {
            var css = editor.getValue();
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                data: {
                    css: css,
                    theme: $("#hfTheme").val(),
                    design: $("#hfDesign").val()
                },
                url: "httphandlers/design/savetheme.ashx",
                success: function (data) {
                    if (data.error) {
                        notify(data.error, notifyType.error);
                    } else {
                        notify(data.result, notifyType.notify);
                    }
                },
                error: function (data) {
                    alert("can't save");
                }
            });
        });

        loadFiles();

        $(".theme-pictures").on("click", function () {
            $(".tab-images").show();
            $(".tab-editor").hide();
        });

        $(".theme-editor").on("click", function () {
            $(".tab-editor").show();
            $(".tab-images").hide();
        });

        $('#file_upload').fileupload({
            url: 'httphandlers/design/treeview.ashx?theme=' + $("#hfTheme").val() + '&design=' + $("#hfDesign").val() + '&action=add&folder=images',
            dataType: 'json',
            success: function (e, data) {
                loadFiles();
                if (e.error) {
                    notify(e.msg, notifyType.error);
                } else {
                    notify(e.msg, notifyType.notify);
                }
            }
        });

        $(".files-info").on("click", ".remove-file", function (e) {

            if (confirm(localize("ConfirmDelete"))) {

                var fileName = $(this).parent().next().html();
                $.ajax({
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    async: false,
                    data: {
                        theme: $("#hfTheme").val(),
                        design: $("#hfDesign").val(),
                        action: "remove",
                        folder: "images",
                        file: fileName
                    },
                    url: "httphandlers/design/treeview.ashx",
                    success: function (data) {
                        if (data != null && data.error == false) {
                            loadFiles();
                            notify(e.msg, notifyType.notify);
                        }
                    }
                });
            }
            e.stopPropagation();
        });


        function loadFiles() {
            $(".files-info").html("");
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                data: {
                    theme: $("#hfTheme").val(),
                    design: $("#hfDesign").val(),
                    action: "getfolder",
                    folder: "images"
                },
                url: "httphandlers/design/treeview.ashx",
                success: function (data) {
                    if (data != null && data.error == null) {
                        var result = "";
                        for (var i = 0; i < data.length; i++) {
                            result +=
                                "<div class=\"files-item\">" +
                                    "<div class=\"files-item-preview\">" +
                                        "<a href=\"javascript:void(0)\" class=\"remove-file\"></a>" +
                                        "<img src=\"" + data[i].Preview + "\" alt=\"\" /></div>" +
                                    "<span class=\"file-name\">" + data[i].Name + "</span>" +
                                "</div>";
                        }
                        $(".files-info").html(result);
                    }
                }
            });
        }
    }
});