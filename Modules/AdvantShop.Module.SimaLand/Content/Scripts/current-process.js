$(document).ready(function () {
    var processTimer = setInterval(getProcess, 500);

    function getProcess() {
        $.ajax({
            url: "../currentprocess",
            success: function (data) {
                if (data.process == false) {
                    clearInterval(processTimer);
                    $('#currentProcess-wrap').css('display', 'none');
                    $('#not-process').css('display', 'block');
                    return;
                }
                $('#currentProcess-wrap').css('display', 'block');
                $('#not-process').css('display', 'none');

                $('#currentProcess').html(data.currentProcess);
                $('#totalCount').html(data.totalCount);
                $('#totalHandled').html(data.totalHandled);
                $('#timeSpent').html(data.timeSpent);
            },
            error: function (data) {
                clearInterval(processTimer);
                $('#currentProcess-wrap').css('display', 'none');
                $('#not-process').css('display', 'block');
                return;
            }
        })
    }
})