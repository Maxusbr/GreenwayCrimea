var langCallback = new Array();

langCallback['send'] = 'Отправить';
langCallback['name'] = 'Имя';
langCallback['phone'] = 'Телефон';
langCallback['comment'] = 'Комментарий';
langCallback['result'] = 'Спасибо! Ваша заявка принята.';
langCallback['ok'] = 'OK';


function localizeCallback(param) {
    var p = param.toString();
    return langCallback[p] || '<span style="color:red;">NOT RESOURCED</span>';
}

;