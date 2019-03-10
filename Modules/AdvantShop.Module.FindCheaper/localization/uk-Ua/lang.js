var langCallback = new Array();

langCallback['send'] = 'Надіслати';
langCallback['name'] = 'Iмя';
langCallback['phone'] = 'Телефон';
langCallback['comment'] = 'Коментар';
langCallback['result'] = 'Спасибо! Ваша заявка прийнята.';
langCallback['ok'] = 'OK';


function localizeCallback(param) {
    var p = param.toString();
    return langCallback[p] || '<span style="color:red;">NOT RESOURCED</span>';
}

;