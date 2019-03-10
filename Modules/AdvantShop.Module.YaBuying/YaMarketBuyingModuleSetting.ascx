<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.YaBuying.Admin_YaMarketBuyingModuleSetting" Codebehind="YaMarketBuyingModuleSetting.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>


<style type="text/css">
    .colleft { width: 150px; }
    .colleft2 { width: 325px; }
    .m-hint {padding: 5px 0 10px 0;}
    .yamarketbuying .tbs td {vertical-align: top;padding: 5px 5px 5px 0;}
</style>
<div class="yamarketbuying">
    <table class="tbs" border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="Покупка на Яндекс.Маркете" /></span>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="colleft">Номер компании:</td>
            <td>
                <asp:TextBox runat="server" ID="txtCampaignId" Text="" Width="300px" />
                <div class="m-hint">
                    В разделе <a href="https://partner.market.yandex.ru/" target="_blank">Маркет для магазинов</a> Яндекса вы увидите надпись вида "№ xx-yyyyyyyy".<br>
                    Номером компании будет являться число yyyyyyyy.
                </div>
            </td>
        </tr>
        <tr>
            <td class="colleft">Авторизационный токен (от Яндекс.Маркета к магазину):</td>
            <td>
                <asp:TextBox runat="server" ID="txtAuth" Text="" Width="300px" />
                <div class="m-hint">
                    Ваш магазин в Яндекс Маркете -> Настройки -> Настройки API покупки
                </div>
            </td>
        </tr>
        <tr>
            <td class="colleft">URL API:</td>
            <td>
                <asp:Label runat="server" ID="lblApiUrl" />
            </td>
        </tr>
        <tr>
            <td class="colleft">Тип авторизации:</td>
            <td>HEADER</td>
        </tr>
        <tr>
            <td class="colleft">Формат данных:</td>
            <td>JSON</td>
        </tr>
        <tr>
            <td class="colleft">Авторизационный токен (от магазина к Яндекс.Маркету):</td>
            <td>
                <asp:TextBox runat="server" ID="txtAuthTokenToYaMarket" Text="" Width="300px" />
                <div class="m-hint">
                    <a href="http://api.yandex.ru/oauth/doc/dg/tasks/get-oauth-token.xml" target="blank">Алгоритм получения токена</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="colleft">Идентификатор приложения:</td>
            <td>
                <asp:TextBox runat="server" ID="txtAuthClientId" Text="" Width="300px" />
                <div class="m-hint">
                    Идентификатор приложения, выданный при его регистрации.<br> 
                    Алгоритм получения идентификатора приложения описан в Руководстве разработчика: Регистрация приложения.
                </div>
            </td>
        </tr>
        <tr>
            <td class="colleft">Логин пользователя:</td>
            <td>
                <asp:TextBox runat="server" ID="txtLogin" Text="" Width="300px" />
                <div class="m-hint">
                    Логин, используемый при регистрации магазина в Яндекс.Маркете.
                </div>
            </td>
        </tr>
    </table>
    <div style="padding: 10px 0; font-weight: bold; text-align: left">Способы доставки</div>
    <asp:ListView ID="lvShippings" runat="server" OnItemDataBound="lvShippings_OnItemDataBound" ItemPlaceholderID="trPlaceholderID">
        <LayoutTemplate>
            <table class="table-ui" style="width: 900px">
                <thead>
                    <th>Метод доставки</th>
                    <th style="width: 120px">id метода доставки</th>
                    <th style="width: 200px">Тип</th>
                    <th style="width: 100px">Мин. срок, дни</th>
                    <th style="width: 100px">Макс. срок, дни</th>
                </thead>
                <tbody>
                    <tr id="trPlaceholderID" runat="server"></tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("Name")%>
                </td>
                <td>
                    <%# Eval("ShippingMethodId")%>
                    <asp:HiddenField runat="server" ID="hfShippingMethodId" Value='<%# Eval("ShippingMethodId")%>' />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlType">
                        <asp:ListItem Text="Нет" Value="" />
                        <asp:ListItem Text="Автоматически" Value="AUTO" />
                        <asp:ListItem Text="Курьерская доставка" Value="DELIVERY" />
                        <asp:ListItem Text="Почта" Value="POST" />
                        <asp:ListItem Text="Самовывоз" Value="PICKUP" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMinDate" Text='<%# Eval("MinDate")%>' Width="70px" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMaxDate" Text='<%# Eval("MaxDate")%>' Width="70px" />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyItemTemplate>
            <tr>
                <td colspan="5">
                    <asp:Localize ID="Localize6" runat="server" Text="Для коректной работы модуля необходимо включить методы доставки" />
                </td>
            </tr>
        </EmptyItemTemplate>
    </asp:ListView>
    <div style="padding: 10px 0; font-weight: bold; text-align: left; margin-top:10px">Доставка по умолчанию (если ни один способ доставки не подошел)</div>
    <table class="table-ui" style="width: 900px">
        <thead>
            <th style="width: 80px">Подключена</th>
            <th>Название доставки</th>
            <th style="width: 100px">Стоимость</th>
            <th style="width: 100px">Тип доставки</th>
            <th style="width: 100px">Мин. срок, дни</th>
            <th style="width: 100px">Макс. срок, дни</th>
        </thead>
        <tbody>
            <td style="text-align: center">
                <asp:CheckBox runat="server" ID="cbDefaultShippingEnabled" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDefaultShippingName" Width="300px" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDefaultShippingPrice" Width="100px" />
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlDefaultShippingType" Width="100px">
                    <asp:ListItem Text="Нет" Value="" />
                    <asp:ListItem Text="Курьерская доставка" Value="DELIVERY" />
                    <asp:ListItem Text="Почта" Value="POST" />
                    <asp:ListItem Text="Самовывоз" Value="PICKUP" />
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDefaultShippingMinDate" Width="100px" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDefaultShippingMaxDate" Width="100px" />
            </td>
        </tbody>
    </table>
    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
        <table class="tbs" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td colspan="2">
                    <div style="padding: 10px 0; font-weight: bold">Способы оплаты</div>
                </td>
            </tr>
            <tr>
                <td class="colleft2">
                    Оплата при оформлении (только для России)
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlYandex">
                        <asp:ListItem Text="Не использовать" Value="0" />
                        <asp:ListItem Text="Использовать" Value="1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display: none">
                <td class="colleft2">
                    Предоплата напрямую магазину (только для Украины)
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlShopprepaid">
                        <asp:ListItem Text="Не использовать" Value="0" />
                        <asp:ListItem Text="Использовать" Value="1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="colleft2">
                    Наличный расчет при получении заказа
                </td>
                <td>
                    <div style="display: inline-block">
                        <asp:DropDownList runat="server" ID="ddlCashOnDelivery">
                            <asp:ListItem Text="Не использовать" Value="0" />
                            <asp:ListItem Text="Использовать" Value="1" />
                        </asp:DropDownList>
                    </div>
                    <a href="javascript:toggle('#CashOnDelivery')">Настроить</a>
                    
                    <table id="CashOnDelivery" style="display: none">
                        <tr>
                            <td>Страны и города:</td>
                            <td><asp:Label runat="server" ID="lblCashOnDeliveryCountriesCities" /></td>
                        </tr>
                        <tr>
                            <td>Страна:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCashOnDeliveryCountry" Width="177" CssClass="country-autocomplete" />
                                <asp:LinkButton ID="btnCashOnDeliveryAddCountry" runat="server" Text="Добавить" OnClick="btnAddCashOnDeliveryCountry_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>Город:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCashOnDeliveryCity" Width="177" CssClass="city-autocomplete" />
                                <asp:LinkButton ID="btnCashOnDeliveryAddCity" runat="server" Text="Добавить" OnClick="btnCashOnDeliveryAddCity_Click" />
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td class="colleft2">
                    Оплата банковской картой при получении заказа
                </td>
                <td>
                    <div style="display: inline-block">
                        <asp:DropDownList runat="server" ID="ddlCardOnDelivery">
                            <asp:ListItem Text="Не использовать" Value="0" />
                            <asp:ListItem Text="Использовать" Value="1" />
                        </asp:DropDownList>
                    </div>
                    <a href="javascript:toggle('#CardOnDelivery')">Настроить</a>
                    
                    <table id="CardOnDelivery" style="display: none">
                        <tr>
                            <td>Страны и города:</td>
                            <td><asp:Label runat="server" ID="lblCardOnDeliveryCountriesCities" /></td>
                        </tr>
                        <tr>
                            <td>Страна:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCardOnDeliveryCountry" Width="177" CssClass="country-autocomplete" />
                                <asp:LinkButton ID="btnCardOnDeliveryAddCountry" runat="server" Text="Добавить" OnClick="btnAddCardOnDeliveryCountry_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>Город:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCardOnDeliveryCity" Width="177" CssClass="city-autocomplete" />
                                <asp:LinkButton ID="btnCardOnDeliveryAddCity" runat="server" Text="Добавить" OnClick="btnCardOnDeliveryAddCity_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
            
            <input type="hidden" id="countryId" runat="server" class="countryId" />
            <input type="hidden" id="cityId" runat="server" class="cityId" />
            <input type="hidden" id="methodId" runat="server" class="methodId" />
            
            <div style="display: none">
                <asp:LinkButton runat="server" ID="lbDeleteCountry" CssClass="lbDeleteCountry" OnClick="lbDeleteCountry_Click" />
                <asp:LinkButton runat="server" ID="lbDeleteCity" CssClass="lbDeleteCity" OnClick="lbDeleteCity_Click" />
            </div>  

        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div style="padding: 10px 0; font-weight: bold">График доставки</div>

    <table class="tbs" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td class="colleft2">
                Выходные дни недели доставки(отметьте выходные дни)
            </td>
            <td>
                <asp:CheckBoxList runat="server" ID ="chkScheduleDelivery" />
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                Время, до которого нужно успеть заказать, чтобы сроки доставки не сдвинулись на один день вперед (Формат ЧЧ:ММ)
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtTimeDileveryHour" Width="30" type="number" MaxLength="2" style="float:left" />:
                <asp:TextBox runat="server" ID="txtTimeDileveryMinutes" Width="30" type="number" MaxLength="2" />
            </td>
        </tr>
    </table>
        
    <div style="padding: 10px 0; font-weight: bold">Статусы заказа</div>

    <table class="table-ui" border="0" cellpadding="2" cellspacing="0" style="width:670px">
        <tr>
            <td class="colleft2">
                Заказ оформлен, но еще не оплачен (если выбрана оплата при оформлении) (UNPAID)
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlUnpaidStatus" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                Заказ можно выполнять (PROCESSING)
            </td>
            <td>
                <asp:UpdatePanel runat="server" ID="upProcessing">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddProcessingStatus" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:ListView ID="lvProcessingStatuses" runat="server" OnItemCommand="lvProcessingStatuses_ItemCommand">
                            <ItemTemplate>
                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                <span>
                                    <%# Eval("StatusName") %> 
                                    (<asp:LinkButton runat="server" ID="lbRemoveProcess" CommandName="DeleteProcessingStatus" CommandArgument='<%# Eval("StatusID")%>' Text="Удалить" CssClass="cat-lnk" />)
                                </span>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                Не выбрано
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <div>
                            <asp:DropDownList runat="server" ID="ddlProcessingStatus" Width="230px"/>
                            <asp:Button runat="server" ID="btnAddProcessingStatus" Text="Добавить" OnClick="btnAddProcessingStatus_Click" />
                        </div>        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>     
        <tr>
            <td class="colleft2">
                Заказ готов к передаче в службу доставки (DELIVERY)
            </td>
            <td>
                 <asp:UpdatePanel runat="server" ID="upDelivery">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddDeliveryStatus" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:ListView ID="lvDeliveryStatuses" runat="server" OnItemCommand="lvDeliveryStatuses_ItemCommand">
                            <ItemTemplate>
                                <%# Container.DataItemIndex != 0 ? ", " : "" %>
                                <span>
                                    <%# Eval("StatusName") %> 
                                    (<asp:LinkButton runat="server" ID="lbRemove" CommandName="DeleteDeliveryStatus" CommandArgument='<%# Eval("StatusID")%>' Text="Удалить" CssClass="cat-lnk" />)
                                </span>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                Не выбрано
                            </EmptyDataTemplate>
                        </asp:ListView>
                
                        <div>
                            <asp:DropDownList runat="server" ID="ddlDeliveryStatus" Width="230px"/> 
                            <asp:Button runat="server" ID="btnAddDeliveryStatus" Text="Добавить" OnClick="btnAddDeliveryStatus_Click" />
                        </div>        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>   
        <tr>
            <td class="colleft2">
                Заказ получен покупателем (DELIVERED)
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlDeliveredStatus" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                Заказ доставлен в пункт самовывоза (PICKUP) (не обяз.)
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPickupStatus" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                Заказ в резерве (ожидается подтверждение от пользователя) (RESERVED) (не обяз.)
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlReservedStatus" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                Заказ отменен (CANCELLED)
            </td>
            <td>
                <asp:Label runat="server" ID="lblCancelledStatus"  />
            </td>
        </tr>
    </table>
    
    <div style="padding: 25px 0 10px 0">
        <b>Причины отмены заказа (подстатус отмены):</b>
    </div>

    <table class="table-ui" border="0" cellpadding="2" cellspacing="0" style="padding: 25px 0 10px 0; width:670px">
        <tr>
            <td class="colleft2">
                магазин не обработал заказ вовремя
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_PROCESSING_EXPIRED"  Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупатель изменяет состав заказа
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_REPLACING_ORDER" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупатель не завершил оформление зарезервированного заказа вовремя
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_RESERVATION_EXPIRED" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупатель отменил заказ по собственным причинам
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_USER_CHANGED_MIND" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупатель не оплатил заказ (для типа оплаты PREPAID)
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_USER_NOT_PAID" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупателя не устраивают условия доставки
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_USER_REFUSED_DELIVERY" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупателю не подошел товар
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_USER_REFUSED_PRODUCT" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                покупателя не устраивает качество товара
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_USER_REFUSED_QUALITY" Width="230px"/>
            </td>
        </tr>
        <tr>
            <td class="colleft2">
                не удалось связаться с покупателем
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCanceled_USER_UNREACHABLE" Width="230px"/>
            </td>
        </tr>
    </table>


    <div style="padding: 25px 0 5px 0">
        <b>Внимание!</b> Заказ должен быть переведен в статус DELIVERY:<br />
        в течение 7 дней, если тип оплаты — YANDEX (оплата при оформлении)<br />
        в течение 21 дня с любым другим типом оплаты<br />
        Если заказ не переведен в статус DELIVERY вовремя, он автоматически отменяется, а магазину выставляется ошибка.<br />
                
        <p>
            <a href="https://tech.yandex.ru/market/partner/doc/dg/reference/purchase-methods-docpage/" target="_blank">
                Документация и описание работы магазина со статусами
            </a>
        </p>
        <p>
            <a href="https://yandex.ru/support/partnermarket/purchase/orders-processing.xml" target="_blank">
                Инструкция для работы с заказами в ЛК Яндекс.Маркета
            </a>
        </p>
    </div>
        
    <div>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Сохранить" />
    </div>
    
    <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
</div>

<script type="text/javascript">
    
    function DeleteCountry(methodId, countryId) {
        $(".methodId").val(methodId);
        $(".countryId").val(countryId);
        $(".lbDeleteCountry")[0].click();
    }

    function DeleteCity(methodId, cityId) {
        $(".methodId").val(methodId);
        $(".cityId").val(cityId);
        $(".lbDeleteCity")[0].click();
    }

    function toggle(id) {
        $(id).toggle();
    }

    $(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {


            $(".country-autocomplete").autocomplete('<%=UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetCountries.ashx") %>', {
                delay: 300,
                minChars: 1,
                matchSubset: 1,
                autoFill: false,
                matchContains: 1,
                cacheLength: 0,
                selectFirst: false,
                maxItemsToShow: 10,
            });

            $(".city-autocomplete").autocomplete('<%=UrlService.GetAbsoluteLink("/admin/HttpHandlers/Location/GetCities.ashx") %>', {
                delay: 300,
                minChars: 1,
                matchSubset: 1,
                autoFill: false,
                matchContains: 1,
                cacheLength: 0,
                selectFirst: false,
                maxItemsToShow: 10,
            });
            
        });
    });
</script>
