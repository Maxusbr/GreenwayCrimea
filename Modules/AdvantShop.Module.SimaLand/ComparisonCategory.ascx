<%@ Control Language="C#" CodeBehind="ComparisonCategory.ascx.cs" Inherits="AdvantShop.Module.SimaLand.ComparisonCategory" %>
<%@ Import Namespace="AdvantShop.Module.SimaLand.Service" %>

<script runat="server">
    int v = PSLModuleSettings.Version;
</script>

<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/styles.css?<%=v %>" />

<script type="text/javascript" src="../Modules/SimaLand/Content/vendors/colorpicker/js/colorpicker.js?<%=v %>"></script>
<script type="text/javascript" src="../Modules/SimaLand/Content/Scripts/scripts.js?<%=v %>"></script>

<div class="controls-container">    
    <div id="parseStatus"></div>
    <input id="parseCategory" type="button" class="btn btn-middle btn-add" value="Получить актуальные категории"/>
    <input id="btnComparisonCategory" type="button" class="btn btn-middle btn-add" value="Сопоставить категории" style="display: none" />
    <div data-plugin="help" class="help-block">
        <div class="help-icon js-help-icon" style="display: none"></div>
        <article class="bubble help js-help">
            <header class="help-header">
                Сопоставить категории                           
            </header>
            <div class="help-content">
                Будет произведена привязка всех возможных категорий simaland к категориям вашего магазина                           
            </div>
        </article>
    </div>
</div>
<div class="main-container">
</div>
<br/>
<input id="btnAddcategoryToAdv" type="button" class="btn btn-middle btn-add" value="Добавить все категории в магазин" />
    <div data-plugin="help" class="help-block">
        <div class="help-icon js-help-icon"></div>
        <article class="bubble help js-help">
            <header class="help-header">
                Добавить все категории в магазин                          
            </header>
            <div class="help-content">
                Создает все представленые в модуле категории в вашем магазин, осуществляет привязку "категория sima-land" -> "категория вашего магазина".     
                <br />
                <b>Примечание:</b> Если нужно создать только одну категорию*, то воспользуйтесь срелкой напротив названия категории   
                <br/>
                <br/>
                * Создать можно только главную категорию, подкатегории добавлять отдельно нельзя
            </div>
        </article>
    </div>

<div id="modal_form">
    <div>
        <div class="select-category-center">
            <span class="slcatname"></span><span class="advcatname"></span>
        </div>
        <input type="hidden" id="hCatId" data-categoryid="" />
        <div class="select-category-center">
            <img src="../Modules/SimaLand/Content/Images/downto50.png" />
        </div>
        <div class="adv-categories">
            <div class="dropdown">
                <input type="text" class="dropbtn d" placeholder="Выберите категорию" />
                <div class="wrap-myDropdown">
                    <div id="myDropdown" class="dropdown-content d">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="modal_form_AddcategoryToAdv">
    <div class="modal-header">
        <span class="header-text">Подтвердите действия</span>
    </div>
    <div class="modal-question">
        <span class="question-text">Будут созданы все категории из модуля в вашем магазине</span>
    </div>
    <div class="modal-controls">
        <input id="btnModalOk" type="button" class="btn btn-middle btn-add" value="Продолжить"/>
        <input id="btnModalCancel" type="button" class="btn btn-middle btn-add" value="Отменить"/>
    </div>
</div>
<div id="modal_form_AddOneCategoryToAdv">
    <div class="modal-header">
        <span class="header-text">Подтвердите действия</span>
    </div>
    <div class="modal-question">
        <span class="question-text">Добавить категорию "<span class="slCategory-name"></span>" и все подкатегории в магазин?</span>
    </div>
    <div class="modal-controls">
        <input id="btnOModalOk" data-categoryid="" type="button" class="btn btn-middle btn-add" value="Да"/>
        <input id="btnOModalCancel" type="button" class="btn btn-middle btn-add" value="Нет"/>
    </div>
</div>
<div id="overlay"></div>