var langCallback = new Array();

langCallback['send'] = 'Send';
langCallback['name'] = 'Name';
langCallback['phone'] = 'Phone';
langCallback['comment'] = 'Comment';
langCallback['result'] = 'Thank you! Your application is accepted.';
langCallback['ok'] = 'OK';

function localizeCallback(param) {
    var p = param.toString();
    return langCallback[p] || '<span style="color:red;">NOT RESOURCED</span>';
}

;