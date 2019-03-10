$(function () {

    if ($("#style-editor").length > 0) {

        var editor = ace.edit("style-editor");
        editor.getSession().setMode("ace/mode/css");

        editor.setOptions({
            enableSnippets: true,
            enableLiveAutocompletion: true
        });

        $(".save-styles").on("click", function () {
            var css = editor.getValue();
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                data: {
                    css: css,
                },
                url: "httphandlers/design/styleeditor.ashx",
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
    }
    

});