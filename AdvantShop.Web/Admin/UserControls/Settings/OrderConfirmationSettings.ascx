<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.OrderConfirmationSettings" CodeBehind="OrderConfirmationSettings.ascx.cs" %>
<table class="info-tb">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_HeadOrderConfirmation%>
            </span>
            <br />
            <span class="subTitleNotify">Настройки, влияющие на логику оформления заказ в магазине
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 345px;">
            <label class="form-lbl" for="<%= cbAmountLimitation.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_UseAmountLimit%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbAmountLimitation" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Оперировать ли наличием товара при оформлении заказа
                    </header>
                    <div class="help-content">
                        Если опция <b>включена</b>, то нельзя оформить заказ на количество большее, чем есть (указано) у товара в графе количество.<br />
                        <br />
                        Если <b>выключена</b>, то количество заказываемого товара может быть любым.
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td style="width: 345px;">
            <label class="form-lbl" for="<%= cbAmountLimitation.ClientID %>">Действие при отсутствии товара</label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlOutOfStockAction">
            </asp:DropDownList>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Как работать с товарами, которых нет в наличии
                    </header>
                    <div class="help-content">
                        При выбранных вариантах <b>Создавать заявку подзаказ</b>, <b>Создавать заказ</b>, <b>Создавать лид</b> - переход на отдельную страницу, где будет применяться соответствующая опция.
                        <br />
                        При выбранном варианте <b>Разрешить добавлять в корзину</b> - пользователю будет разрешено добовить отсутствующий товар в корзину наравне с остальными товарами.
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= cbProceedToPayment.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ProceedToPayment%></label>
        </td>
        <td>
            <asp:CheckBox ID="cbProceedToPayment" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Переходить к оплате сразу
                    </header>
                    <div class="help-content">
                        Опция определяет вызывать ли сразу переход к системе оплаты.
                        <br />
                        <br />
                        Если опция <b>включена</b>, то на последнем шаге оформления заказа, клиента автоматически перенаправит на форму платежной системы.
                        <br />
                        <br />
                        Если опция <b>выключена</b>, то на последнем шаге оформления заказа, клиенту покажется сообщение о завершении оплаты и кнопка "перейти к оплате".
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMinimalPriceForDefaultGroup.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_MinimalPriceForDefaultGroup%></label>
        </td>
        <td>
            <asp:TextBox ID="txtMinimalPriceForDefaultGroup" runat="server" class="niceTextBox shortTextBoxClass2"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Минимальная сумма заказа
                    </header>
                    <div class="help-content">
                        Параметр определяет минимальную сумму заказа, ниже которой нельзя оформить заказ.<br />
                        <br />
                        Если сумма заказа в корзине будет ниже, чем указанное значение, клиенту покажется сообщение с уведомлением.
                    </div>
                </article>
            </div>
            <a href="customersgroups.aspx" style="margin-left: 30px;"><%= Resources.Resource.Admin_CommonSettings_SpecifyMinimumOrderPrice%></a>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbManagerConfirmed.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_ManagerConfirmed%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbManagerConfirmed" runat="server" class="checkly-align ckbManagerConfirmed"></asp:CheckBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Запрещать оплату заказа до подтверждения менеджером
                    </header>
                    <div class="help-content">
                        Запрещать оплату заказа до подтверждения менеджером.
                    </div>
                </article>
            </div>
            <div style="margin-left: 30px; display: inline-block; display: none;" class="divManagerConfirmedValue">
                Что делать со старыми заказами
                <label>
                    <asp:RadioButton ID="rbManagerConfirmedTrue" runat="server" GroupName="ManagerConfirmedValue" Checked="True" />
                    Разрешить оплату</label>
                <label>
                    <asp:RadioButton ID="rbManagerConfirmedFalse" runat="server" GroupName="ManagerConfirmedValue" />
                    Запретить</label>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 345px;">
            <label class="form-lbl" for="<%= cbDenyToByPreorderedProductsWithZerroAmount.ClientID %>">Запрещать оформление отсутствующих предзаказанных товаров</label>
        </td>
        <td>
            <asp:CheckBox ID="cbDenyToByPreorderedProductsWithZerroAmount" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Запрещать оформление отсутствующих предзаказанных товаров
                    </header>
                    <div class="help-content">
                        Если опция <b>включена</b>, то при добавлении товара по ссылке из письма от магазина о покупке под заказ, не будет возможности оформить заказ, если товар не в наличии<br />
                        <br />
                        Если <b>выключена</b>, наличие не будет учитываться для товаров добавленных из письма.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Поля в оформлении заказа
        </td>
        <td>
            <asp:HyperLink ID="HyperLink2" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SetOrderFields %>"
                NavigateUrl="~/Admin/CheckoutFields.aspx" Target="_blank" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_BuyInOneclick_Header%>
            </span>
            <br />
            <span class="subTitleNotify">Настройки формы упрощённой покупки "в один клик"
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbBuyInOneClick.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BuyInOneclick_Active%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" CssClass="checkly-align" ID="ckbBuyInOneClick" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Включить функцию<br />
                        "Купить в один клик"
                    </header>
                    <div class="help-content">
                        Опция определяет <b>включить</b> или <b>выключить</b> возможность покупки товаров с использованием формы "Купить в один клик".
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbBuyInOneClickDisableInCheckout.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BuyInOneclick_DisableInCheckout%></label>
        </td>
        <td>
            <asp:CheckBox runat="server" CssClass="checkly-align" ID="ckbBuyInOneClickDisableInCheckout" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Скрывать кнопку
                        на странице оформления заказа
                    </header>
                    <div class="help-content">
                        Опция определяет <b>включить</b> или <b>выключить</b> возможность покупки товаров с использованием формы "Купить в один клик" на странице оформления заказа.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOneClickLinkText.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BuyInOneclick_LinkText%></label>
        </td>
        <td>
            <asp:TextBox ID="txtOneClickLinkText" runat="server" CssClass="niceTextBox "></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Текст ссылки
                    </header>
                    <div class="help-content">
                        Текст ссылки "Купить в один клик"
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOneClickFirstText.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_BuyInOneclick_FirstText%></label>
        </td>
        <td>
            <asp:TextBox ID="txtOneClickFirstText" runat="server" TextMode="MultiLine" CssClass="niceTextArea textArea2Lines"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Текст в первой форме
                    </header>
                    <div class="help-content">
                        Текст, который покажется вверху формы 'Покупка в один клик'
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOneClickFirstText.ClientID %>">Текст кнопки "Заказать"</label>
        </td>
        <td>
            <asp:TextBox ID="txtOneClickButtonText" runat="server" CssClass="niceTextBox"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Текст кнопки "Заказать"
                    </header>
                    <div class="help-content">
                        Текст, который будет отображаться на кнопке офомляющей заказ.
                    </div>
                </article>
            </div>
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>Поля в оформлении заказа
        </td>
        <td>
            <asp:HyperLink ID="HyperLink1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BuyInOneclick_SetFields %>"
                NavigateUrl="~/Admin/CheckoutFields.aspx" Target="_blank" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Действие
        </td>
        <td>
            <label runat="server" id="lActionCreateOrder">
                <asp:RadioButton ID="rbBuyInOneClickActionCreateOrder" runat="server" GroupName="BuyInOneClickAction" Checked="true" />
                Создавать заказ</label>
            <label runat="server" id="lActionCreateLead">
                <asp:RadioButton ID="rbBuyInOneClickActionCreateLead" runat="server" GroupName="BuyInOneClickAction" />
                Создавать лид</label>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>Метод доставки по умолчанию
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlOneClickDefaultShipping" />
        </td>
    </tr>

    <tr class="rowsPost row-interactive">
        <td>Метод оплаты по умолчанию
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlOneClickDefaultPayment" />
        </td>
    </tr>

    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_Gifts%>
            </span>
            <br />
            <span class="subTitleNotify">Настройки, связанные с подарками к товарам
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkMultiplyGiftsCount.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_Gifts_MultiplyGiftsCount%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkMultiplyGiftsCount" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Умножать подарки на число основных товаров в корзине
                    </header>
                    <div class="help-content">
                        Если опция <b>включена</b>, при добавлении в корзину товара с подарками, количество подарков будет соответствовать количеству добавленных товаров,<br />
                        <b>выключена</b> - подарок будет в корзине в единственном экземпляре.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_GiftCertificates%>
            </span>
            <br />
            <span class="subTitleNotify">Настройки работы с подарочными сертификатами и купонами
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbEnableGiftCertificateService.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_EnableGiftCertificateService%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbEnableGiftCertificateService" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Разрешить использование подарочных сертификатов 
                    </header>
                    <div class="help-content">
                        <b>Включает</b> или <b>выключает</b> возможность покупки и использования подарочных сертификатов.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= ckbDisplayPromoTextbox.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_DisplayPromoTextbox%></label>
        </td>
        <td>
            <asp:CheckBox ID="ckbDisplayPromoTextbox" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Поле ввода
                    </header>
                    <div class="help-content">
                        <b>Включает</b> или <b>выключает</b> поле для ввода купона или сертификата на странице оформления заказа.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMinimalPriceCertificate.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_MinimalPriceCertificate%></label>
        </td>
        <td>
            <asp:TextBox ID="txtMinimalPriceCertificate" runat="server" class="niceTextBox shortTextBoxClass2"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Минимальная сумма<br />
                        сертификата
                    </header>
                    <div class="help-content">
                        Минимально разрешённая сумма сертификата.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtMaximalPricecertificate.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_MaximalPriceCertificate%></label>
        </td>
        <td>
            <asp:TextBox ID="txtMaximalPricecertificate" runat="server" class="niceTextBox shortTextBoxClass2"></asp:TextBox>
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Максимальная сумма<br />
                        сертификата
                    </header>
                    <div class="help-content">
                        Соответственно, максимально разрешённая сумма сертификата.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvSubHelp" style="margin-top: 15px;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/kupons-i-podaroxhnie-certifikaty" target="_blank">Инструкция. Купоны и подарочные сертификаты</a>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_PrintOrder_Header%>
            </span>
            <br />
            <span class="subTitleNotify">Параметры распечатки заказа администратором из панели работы с заказами
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowStatusInfo.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_PrintOrder_ShowStatusInfo%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowStatusInfo" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать информацию о статусе заказа
                    </header>
                    <div class="help-content">
                        Опция определяет выводить или нет строчку со статусом заказа на распечатке заказа.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkShowMap.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_PrintOrder_ShowMap%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowMap" CssClass="checkly-align" runat="server" />
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Отображать карту c адресом покупателя
                    </header>
                    <div class="help-content">
                        Опция определяет отображать или нет карту по адресу клиента на распечатке заказа.<br />
                        <br />
                        В некоторых случаях карта может быть удобна, например курьеру.
                    </div>
                </article>
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= rbYandexMap.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_PrintOrder_UseMapType%></label>
        </td>
        <td>
            <%--<span class="checkly-align">
                <asp:RadioButton ID="rbGoogleMap" runat="server" GroupName="MapType" Checked="true" />
            </span>
            <span>
                <label class="form-lbl" style="display: inline;" for="<%= rbGoogleMap.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_PrintOrder_MapTypeGoogle%></label>
            </span>--%>
            <span class="checkly-align" style="margin-left: 10px;">
                <asp:RadioButton ID="rbYandexMap" runat="server" GroupName="MapType" />
            </span>
            <span>
                <label class="form-lbl" style="display: inline;" for="<%= rbYandexMap.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_PrintOrder_MapTypeYandex%></label>
            </span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="padding: 15px 0 0 0;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_OrderId%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOrderId.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OrderId%></label>
        </td>
        <td>
            <asp:TextBox ID="txtOrderId" runat="server" CssClass="niceTextBox shortTextBoxClass2" />
            <asp:Button runat="server" ID="btnChangeOrderNumber" OnClick="btnChangeOrderNumber_Click"
                Text="<%$ Resources:Resource, Admin_CommonSettings_ChangeOrderIdButton %>" />
            <div style="padding: 5px 0 0 0;">
                <asp:Label runat="server" ID="lblOrderSaveResult" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td></td>
        <td>
            <div style="width: 580px; padding: 5px 0;">
                <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ChangeOrderIdHint %>" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtOrderNumberFormat.ClientID %>"><%= Resources.Resource.Admin_CommonSettings_OrderNumberFormat%></label>
        </td>
        <td>
            <asp:TextBox ID="txtOrderNumberFormat" runat="server" CssClass="niceTextBox shortTextBoxClass2" Width="300px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td></td>
        <td>
            <div style="margin-top: 7px;">
                #NUMBER# - номер заказа<br />
                #YEAR# - год<br />
                #MONTH# - месяц<br />
                #DAY# - день<br />
                #RRR# - случайно сгенерированное число
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvSubHelp" style="margin-top: 15px;">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/order-num-custom" target="_blank">Инструкция. Формат номера заказа</a>
            </div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="padding: 15px 0 0 0;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_ContactFields_UserAgreement%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= chkIsShowUserAgreementText.ClientID %>"><%= Resources.Resource.Admin_ContactFields_UserAgreement %></label>
        </td>
        <td>
            <asp:CheckBox ID="chkIsShowUserAgreementText" runat="server" CssClass="checkly-align" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <label class="form-lbl" for="<%= txtUserAgreementText.ClientID %>">Текст пользовательского соглашения</label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtUserAgreementText" TextMode="MultiLine" Width="600px" Height="40px" />
        </td>
    </tr>
</table>
