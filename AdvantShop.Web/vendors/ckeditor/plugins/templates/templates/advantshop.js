CKEDITOR.addTemplates('advantshop',
{
    imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath("templates") + "templates/images/"),
    templates:
        [
            {
                title: "Картинка и заголовок с текстом",
                image: "template1.gif",
                description: "Одна картинка с заголовком и текст, который обтекает картинку.",
                html: '<div class="clear"><img src="./images/nophoto.jpg" alt="" style="float:left; margin-right: 10px;"><h3>Заголовок</h3><div>Здесь текст</div></div>'
            },
            {
                title: "Заголовок с текстом и картинка",
                image: "template1.gif",
                description: "Одна картинка с заголовком и текст, который обтекает картинку.",
                html: '<div class="clear"><img src="./images/nophoto.jpg" alt="" style="float:right; margin-left: 10px;"><h3>Заголовок</h3><div>Здесь текст</div></div>'
            },
            {
                title: "Шаблон с двумя колонками",
                image: "template2.gif",
                description: "Шаблон, в котором две колонки с заголовком и текстом",
                html: '<table class="table-widthout-borders" style="table-layout:fixed;"><thead><tr><th style="width:50%;">Заголовок 1</th><th>&nbsp;</th><th style="width:50%;">Заголовок 2</th></tr></thead><tbody><tr><td>Текст 1</td><td>&nbsp;</td><td>Текст 2</td></tr></tbody></table>' +
                      '<p>Здесь дополнительный текст</p>'
            },
            {
                title: "Текст и таблица",
                image: "template3.gif",
                description: "Заголовок с текстом и таблицой",
                html: '<div class="clear"><table border="1" cellpadding="0" cellspacing="0" style="width:150px;float: right">' +
	                    '<tbody><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>' +
		              '</tr></tbody></table>' +
                      '<h3>Заголовок</h3>' +
                      '<p>Текст</p></div>'
            }
        ]
});