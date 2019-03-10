<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.LastOrder.AdminView" Codebehind="AdminView.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Module.LastOrder.Service" %>

<%
    int v = ModuleSettings.Version;
    string moduleId = ModuleSettings.ModuleID;
    string lastClearNotifications = string.IsNullOrEmpty(ModuleSettings.LastClearNotifications) ? "" : "Последнее удаление устаревших уведомлений: " + ModuleSettings.LastClearNotifications + " (время сервера)";
%>

<link rel="stylesheet" href="../Modules/<%=moduleId %>/Content/Styles/admin-style.css?<%=v %>" />
<script type="text/javascript" src="../Modules/<%=moduleId %>/Content/Scripts/admin-script.js?<%=v %>"></script>
<div id="statusMessage" style="visibility: hidden">
    <div class="info"></div>
</div>
<div id="adminModuleWrap">
    <table class="lo-table">
        <tr>
            <td>
                <span class="spanSettCategory">Купили сейчас</span>
            </td>
        </tr>
        <tr>
            <td>
                <%=lastClearNotifications %>
            </td>
        </tr>
        <!--<tr>
            <td>
                <input type="checkbox" id="alwaysShow" />
                Показывать в товарах "Не в наличии" 
                <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                    <article class="bubble help js-help">
                        <header class="help-header">
                            Показывать в товарах "Не в наличии"
                        </header>
                        <div class="help-content">
                            Уведомление будет выводиться и в том случае, если товара нет в наличии
                        </div>
                    </article>
                </div>
            </td>
        </tr>-->
        <tr>
            <td>
                <div>
                    Генерировать уведомления в диапазоне от
                    <input type="text" id="rmfrom" class="n-time" style="width: 35px" value="" />
                    до
                    <input type="text" id="rmto" class="n-time" style="width: 35px" value="" />
                    <select id="rtt" style="width: 120px; height: 34px; margin-top: -4px;">
                        <option value="0">В минутах</option>
                        <option value="1">В часах</option>
                        <option value="2">В днях</option>
                    </select>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Диапазон минут
                            </header>
                            <div class="help-content">
                                В этом диапазоне будет генерироваться время для уведомления.<br />
                                <b>Например:</b> Если диапазон от 10 до 45 минут, то в случайно сгенерированных уведомлениях будет браться число в диапазоне 10 - 45, например 12, 25 или 30
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    Максимальное хранение данных
                    <input type="text" id="smpmp" class="n-time" style="width: 35px" value="" />
                    <select id="smpmpt" style="width: 120px; height: 34px; margin-top: -4px;">
                        <option value="0">В часах</option>
                        <option value="1">В днях</option>
                    </select>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Максимальное хранение данных
                            </header>
                            <div class="help-content">
                                Это значение указывает, какое время будет храниться уведомление в базе данных
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    Период показа уведомления
                    <input type="text" id="sp" class="n-time" style="width: 35px" />
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Период показа уведомления
                            </header>
                            <div class="help-content">
                                Сколько нужно посмотреть карточек товара, чтобы увидеть уведомление
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="ucnac" />
                Использовать имена и города из списков
                 <div data-plugin="help" class="help-block">
                    <div class="help-icon js-help-icon"></div>
                    <article class="bubble help js-help">
                        <header class="help-header">
                            Использовать имена и города из списков
                        </header>
                        <div class="help-content">
                            Если установлен флажок, то имена и города при генерировании случайных уведомлений будут браться из списков ниже, 
                    если не установлен, то имена и города берутся из базы данных магазина
                    <br>
                            <b>Внимание!</b> Реальные покупки сохраняют только время заказа
                        </div>
                    </article>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    Имена: &nbsp;
                    <input type="text" id="n" style="width: 325px" />
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Имена
                            </header>
                            <div class="help-content">
                                Укажите имена через запятую<br />
                                <b>Например:</b> Иван, Михаил, Анатолий
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    Города: &nbsp;
                    <input type="text" id="c" style="width: 325px" />
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Города
                            </header>
                            <div class="help-content">
                                Укажите города через запятую<br />
                                <b>Например:</b>  Москва, Санкт-Петербург, Волгоград
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    Расположение:         
                        <select id="loc" style="width: 170px">
                            <option value=".details-rating">Около рейтинга</option>
                            <option value=".details-payment">Под ценой</option>
                        </select>
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Расположение
                            </header>
                            <div class="help-content">
                                Место, где будет выводиться уведомление в карточке товара
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    Период показа города посетителя в блоке модуля
                    <input id="userCity" type="text" class="n-time" style="width: 35px;" placeholder="" />
                    <div data-plugin="help" class="help-block">
                        <div class="help-icon js-help-icon"></div>
                        <article class="bubble help js-help">
                            <header class="help-header">
                                Период показа города посетителя
                            </header>
                            <div class="help-content">
                                Указывается число, которое указывает на очередность показа города посетителя<br />
                                <b>Например:</b> Если установлено число 3, то в каждой 3й карточке c уведомлением будет показываться город посетителя<br />
                                <b>0</b> - город посетителя не будет подставляться
                            </div>
                        </article>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="btnSave" class="btn btn-add btn-middle" value="Сохранить" />
            </td>
        </tr>
    </table>
</div>
