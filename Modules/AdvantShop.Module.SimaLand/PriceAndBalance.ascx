<%@ Control Language="C#" CodeBehind="PriceAndBalance.ascx.cs" Inherits="AdvantShop.Module.SimaLand.PriceAndBalance" %>
<%@ Import Namespace="AdvantShop.Module.SimaLand.Service" %>

<script runat="server">
    int v = PSLModuleSettings.Version;
</script>

<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/styles.css?<%=v %>" />
<link rel="stylesheet" media="screen" href="../Modules/SimaLand/Content/vendors/colorpicker/css/colorpicker.css?<%=v %>" />

<div class="price-and-balace-wrap">
    <div class="tabs">
        <ul>
            <li style="display:none">
                <input type="radio" name="tabs-0"  id="tabs-0-0" />
                <label for="tabs-0-0">Авторизация</label>
                <div id="#tab-authorize" class="tab-contents">
                    <div id="authorize-status">
                        <div class="info">Для авторизации используйте форму ниже</div>
                    </div>
                    <div class="inputs">
                        <div class="inputs">
                            <label for="sl-email">E-Mail:</label>
                        </div>
                        <div class="inputs">
                            <input id="sl-email" type="email" class="niceTextBox textBoxClass" placeholder="email" required />
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        E-Mail    
                                    </header>
                                    <div class="help-content">
                                        E-Mail пользователя sima-land.ru                          
                                    </div>
                                </article>
                            </div>
                        </div>
                    </div>
                    <div class="inputs">
                        <div class="inputs">
                            <label for="sl-password">Пароль:</label>
                        </div>
                        <div class="inputs">
                            <input id="sl-password" type="password" class="niceTextBox textBoxClass" placeholder="password" required />
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Пароль     
                                    </header>
                                    <div class="help-content">
                                        Пароль пользователя sima-land.ru                          
                                    </div>
                                </article>
                            </div>
                        </div>
                    </div>
                    <div class="inputs">
                        <div class="inputs">
                            <label for="sl-tel">Телефон:</label>
                        </div>
                        <div class="inputs">
                            <input id="sl-tel" type="tel" class="niceTextBox textBoxClass" placeholder="phone" />
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Телефон      
                                    </header>
                                    <div class="help-content">
                                        Телефон пользователя sima-land.ru                          
                                    </div>
                                </article>
                            </div>
                        </div>
                    </div>
                    <div class="inputs">
                        <input id="sl-regulation" type="checkbox" required />
                        <label for="sl-regulation">Принимаю правила положения о работе с API v5</label>
                    </div>
                    <div class="inputs">
                        <input type="button" id="sl-login" class="btn btn-add btn-middle" value="Авторизоваться" />
                    </div>
                </div>
            </li>
            <li style="display:none">
                <input type="radio" name="tabs-0" id="tabs-0-1" />
                <label for="tabs-0-1">Цена и остатки</label>
                <div id="#tab-price-and-balance" class="tab-contents">        
                <div id="statusMessage" style="visibility:hidden">
                    <div class="info"></div>
                </div>            
                <div id="toUpdate"></div>
                    <div class="inputs">
                        <input type="checkbox" id="auto-update-balance" />
                        <label for="auto-update-balance">Обновлять автоматически остатки и цену</label>
                    </div>
                    <div class="inputs">
                        <label for="timePriod">Период обновления</label>
                        <input type="text" id="timePriod" style="width: 50px"/>
                        <span> в часах</span>
                    </div>
                    <div class="inputs">
                        <input type="button" id="btnSave" class="btn btn-add btn-middle" value="Сохранить" />
                        <input type="button" id="manualStart" class="btn btn-add btn-middle" value="Запуск в ручную" />
                    </div>
                </div>
            </li>
            <li>
                <input type="radio" name="tabs-0" checked="checked" id="tabs-0-2" />
                <label for="tabs-0-2">Наценка</label>
                <div id="#tab-markup" class="tab-contents">
                    <input id="add-markup" type="button" class="btn btn-add btn-middle" value="Добавить"/>
                    <div id="range-list">

                    </div>
                    <div>
                        <span style="color: darkred">Внимание! Если цена с sima-land не входит в указанные диапазоны, то наценки не будет.</span>
                    </div>
                </div>
            </li>
        </ul>
    </div>
</div>

<script type="text/javascript" src="../Modules/SimaLand/Content/vendors/colorpicker/js/colorpicker.js?<%=v %>"></script>

<script type="text/javascript" src="../Modules/SimaLand/Content/Scripts/price-and-balance.js?<%=v %>"></script>
