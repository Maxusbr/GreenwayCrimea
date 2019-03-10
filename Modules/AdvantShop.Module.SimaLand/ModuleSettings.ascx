<%@ Control Language="C#" CodeBehind="ModuleSettings.ascx.cs" Inherits="AdvantShop.Module.SimaLand.ModuleSettings" %>
<%@ Import Namespace="AdvantShop.Module.SimaLand.Service" %>

<script runat="server">
    int v = PSLModuleSettings.Version;
</script>

<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/styles.css?<%=v %>" />
<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/feedback.css?<%=v %>" />
<link rel="stylesheet" media="screen" href="../Modules/SimaLand/Content/vendors/colorpicker/css/colorpicker.css?<%=v %>" />

<%--<div class="dvSubHelp">
        <img id="ctl00_cphMain_CatalogSettings_Image1" src="images/messagebox_help.png" style="border-width: 0px;">
        <a href="http://promo-z.ru/instruktsiya-po-polzovaniyu-modulem-simalend//" target="_blank">Инструкция по настройке модуля Sima-land</a>
    </div>--%>
<div class="info-labels">
    <label id="errorMessageLabel"></label>
</div>
<div class="module-settings-controls">

    <%--<div class="add-link-on-artno">
        <label>
            <input id="checkArtno" type="checkbox" value="false" />Добавить ссылки на артикулы в заказах</label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Добавить ссылки на артикулы в заказах     
                </header>
                <div class="help-content">
                    На странице заказа, в поле "Содержание заказа", на артикулы товаров, будет установлена ссылка на товар sima-land.ru                          
                </div>
            </article>
        </div>
    </div>--%>
    <div class="add-price-in-range">
        <label>
            <input id="priceInRange" type="checkbox" value="false" />Загружать товары в диапазоне цен
        </label>
        <input id="fromPrice" class="range-price" type="text" placeholder="ОТ" value="50" />
        <input id="toPrice" class="range-price" type="text" placeholder="ДО" value="40000" />
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Цена    
                </header>
                <div class="help-content">
                    Цена товара на sima-land.ru                          
                </div>
            </article>
        </div>
    </div>
    <div class="download-markers">
        <label>
            <input id="downloadMarkers" type="checkbox" value="false" />Добавлять маркеры sima-land к товару</label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Маркеры sima-land
                </header>
                <div class="help-content">
                    Будут выгружаться и обновляться маркеры sima-land (хит, новинка, распродажа)                       
                </div>
            </article>
        </div>
    </div>
    <div class="added-categories">
        <label>
            <input id="addedcategories" type="checkbox" value="false" />Добавлять товар в дочерние категории</label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Дочерние категории
                </header>
                <div class="help-content">
                    Товар будет отображаться не только в главной категории                   
                </div>
            </article>
        </div>
    </div>
    <div class="auto-update">
        <label>
            <input id="autoupdate" type="checkbox" value="false" />Обновлять каталог автоматически
        </label>
        <%--<input id="autoupdatehour" class="time-update" type="text" value="00" /> часов
        <input id="autoupdateminute" class="time-update" type="text" value="00" /> минут--%>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Автоматическое обновление    
                </header>
                <div class="help-content">
                    Обновление товара 1 раз в сутки                      
                </div>
            </article>
        </div>
    </div>
    <div class="import-discount">
        <label>
            <input id="importDiscount" type="checkbox" value="false" />Загружать скидки
        </label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Загружать скидки  
                </header>
                <div class="help-content">
                    Будет устанавливаться скидка на товар как на sima-land                          
                </div>
            </article>
        </div>
    </div>
    <%--<div class="download-image">
        <label>
            <input id="downloadImage" type="checkbox" value="false" />Загружать изображения
        </label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Загружать изображения  
                </header>
                <div class="help-content">
                    Будет производиться загрузка изображений (значительно замедляет скорость загрузки товаров)                        
                </div>
            </article>
        </div>
    </div>--%>
    <div class="add-prefix">
        <label>
            <input id="addPrefix" type="checkbox" value="false" />Добавлять префикс к артикулу
        </label>
        <input id="txtPrefix" class="artno-prefix" type="text" placeholder="Префикс" value="" />
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Префикс  
                </header>
                <div class="help-content">
                    К артикулу sima-land будет добавляться ваш префикс. Используется для избежания конфликтов с имеющимися артикулами в магазине                       
                </div>
            </article>
        </div>
    </div>
    <div class="minMax-count-product-order">
        <label>
            <input id="minMax" type="checkbox" value="false" />Ограничения на минимальное и максимальное количество товара в заказе
        </label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Количество товара  
                </header>
                <div class="help-content">
                    Будет выгружаться ограничения на минимальное и максимальное количество товара в заказе        
                </div>
            </article>
        </div>
    </div>
    <div class="three-pay-two">
        <label>
            <input id="threePayTwo" type="checkbox" value="false" />Добавлять тег "3 по цене 2"
        </label>
        <input id="theePayTwoColor" style="width: 53px" type="text" placeholder="hex" />
        <input id="theePayTwoHref" style="width: 170px" type="text" placeholder="url" />

        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Добавлять тег "3 по цене 2" 
                </header>
                <div class="help-content">
                    Будет выгружаться тег "3 по цене 2" и добавляться в маркеры товара.
                    <br>
                    <b>hex</b> - цвет маркера и кнопки
                    <br>
                    <b>url</b> - url кнопки в карточке товара
                </div>
            </article>
        </div>
    </div>
    <div class="mt-gift">
        <label>
            <input id="mtGift" type="checkbox" value="false" />Добавлять тег "+ Подарок"
        </label>
        <input id="mtGiftColor" style="width: 53px" type="text" placeholder="hex" />
        <input id="mtGiftHref" style="width: 170px" type="text" placeholder="url" />

        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Добавлять тег "+ Подарок" 
                </header>
                <div class="help-content">
                    Будет выгружаться тег "+ Подарок" и добавляться в маркеры товара.
                    <br>
                    <b>hex</b> - цвет маркера и кнопки
                    <br>
                    <b>url</b> - url кнопки в карточке товара
                </div>
            </article>
        </div>
    </div>
    <div class="always-available">
        <label>
            <input id="alwaysAvailable" type="checkbox" value="false" />Всегда в наличии
        </label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Всегда в наличии
                </header>
                <div class="help-content">
                    После загрузки товаров в наличии, все товары из sima-land.ru будут в наличии
                </div>
            </article>
        </div>
    </div>
    <div class="download-image-all" style="padding: 5px 27px;">
        <label>
            Тип загрузки фотографий:
            <select id="dwnlimagetype">
                <option value="NoPhoto">Не загружать</option>
                <option value="MainPhoto">Одно изображение</option>
                <option value="AllPhoto">Все возможные</option>
            </select></label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Тип загрузки изображений    
                </header>
                <div class="help-content">
                    Параметр определяет сколько изображений бужет загружено с sima-land.ru
                    <br />
                    Внимание! При выгрузки большого количества товара учитывайте объем HDD на вашем сервере!                         
                </div>
            </article>
        </div>
    </div>
    <div class="download-image">
        <label>
            <input id="reloadImages" type="checkbox" value="false" />Перезагрузить изображения при следующей загрузке
        </label>
        <div data-plugin="help" class="help-block">
            <div class="help-icon js-help-icon"></div>
            <article class="bubble help js-help">
                <header class="help-header">
                    Перезагрузить изображения  
                </header>
                <div class="help-content">
                    У обновляемых товаров изображения будут удаляться и грузиться с sima-land                        
                </div>
            </article>
        </div>
    </div>
    <br/>
    <input id="saveSettings" class="btn btn-add btn-middle" type="button" value="Сохранить" />

    <div class="popup">
        <span class="popuptext" id="parsingStatus"></span>
        <input id="btnParse" type="button" class="btn btn-middle btn-add" value="Загрузить товары" />
    </div>
</div>
<div class="module-settings-wrapper">
    <div class="parse-categories">
    </div>
</div>
<%--<div id="btnFb" class="btn-feedback"><span class="q">?</span></div>--%>
<%--<div id="fbModal"></div>--%>

<script type="text/javascript" src="../Modules/SimaLand/Content/vendors/colorpicker/js/colorpicker.js?<%=v %>"></script>

<script type="text/javascript" src="../Modules/SimaLand/Content/Scripts/scripts.js?<%=v %>"></script>
<script type="text/javascript" src="../Modules/SimaLand/Content/Scripts/feedback.js?<%=v %>"></script>
