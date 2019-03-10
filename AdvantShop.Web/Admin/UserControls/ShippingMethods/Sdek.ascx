<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ShippingMethods.SdekControl" CodeBehind="Sdek.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Sdek_AuthLogin %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAuthLogin" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Логин
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_ShippingMethod_Sdek_AuthLogin_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgAuthLogin" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Sdek_AuthPassword %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAuthPassword" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Пароль
                    </header>
                    <div class="help-content">
                        <%= Resources.Resource.Admin_ShippingMethod_Sdek_AuthPassword_Description%>
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgAuthPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize12" runat="server" Text="Город склада"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCityFrom" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Город склада
                    </header>
                    <div class="help-content">
                        Параметр передается в сервис доставки и означает место (город) откуда поедет посылка.<br />
                        <br />
                        Параметр необходим для расчета стоимости доставки.<br />
                        <br />
                        Например: Москва.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgCityFrom" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize11" runat="server" Text="Наценка на доставку"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtAdditionalPrice" Width="100" ValidationGroup="5" style="float:left;"></asp:TextBox>
            <asp:DropDownList ID="ddlTypeAddPrice" runat="server" DataValueField="TariffId" DataTextField="Name" Width="148px" Height="29px">
            </asp:DropDownList>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Наценка на доставку
                    </header>
                    <div class="help-content">
                        Дополнительная наценка на доставку, например, за дополнительные услуги.<br />
                        <br />
                        Наценка может быть фиксированной, либо процентной.<br />
                        <br />
                        Стоимость указывается в базовой валюте.<br />
                        <br />
                        Например: 100 (руб.), либо 10 (%)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msgAdditionalPrice" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="Активные тарифы"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlTariff" runat="server" DataValueField="TariffId" DataTextField="Name" Width="255px">
            </asp:DropDownList>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Активные тарифы доставки
                    </header>
                    <div class="help-content">
                        Тут необходимо выбрать один вариант, который доступен вашему интернет-магазину.<br />
                        <br />
                        Обратите внимание, что следует выбрать только один из представленных вариантов.<br />
                        <br />
                        Для каждого тарифного плана, если у вас в личном кабинете СДЭК доступно более чем один тариф, необходимо создавать отдельный способ доставки типа СДЭК.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="msglvTariffs" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize19" runat="server" Text="Количество копий накладных"></asp:Localize>
        </td>
        <td class="columnVal" style="padding-top: 10px;">
            <asp:TextBox runat="server" ID="txtCountDeviveryDoc"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtCountDeviveryDoc"
                ValidationGroup="1" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                MaximumValue="10" MinimumValue="1" Type="Integer"> </asp:RangeValidator>
            <asp:RequiredFieldValidator ID="RangeValidator7" runat="server" ControlToValidate="txtCountDeviveryDoc"
                ValidationGroup="1" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'> </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize18" runat="server" Text="Добавлять комментарий при отправке заказа"></asp:Localize>
        </td>
        <td class="columnVal" style="padding-top: 10px;">
            <asp:TextBox runat="server" ID="txtDescrForSend" Width="250" TextMode="MultiLine" Height="50px"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Комментарий при отправке заказа
                    </header>
                    <div class="help-content">
                        Данный комментарий будет отправлять в службу доставки СДЕК при офрмлении заказ.
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label9" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize9" runat="server" Text="Форма вызова курьера"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="Дата и время вызова курьера"></asp:Localize><span
                class="required">*</span>
        </td>
        <td>
            <div style="height: 34px;">
                <div class="dp" style="width: 260px;">
                    <asp:Label ID="Label4" runat="server" Text="День" Width="90px"></asp:Label><asp:TextBox
                        ID="txtDate" Width="140px" runat="server" TabIndex="12" />
                    <%--   <img class="icon-calendar" src="./images/Calendar_scheduleHS.png" alt="" />--%>
                </div>
            </div>
            <div style="height: 34px;">
                <asp:Label ID="Label3" runat="server" Text="Время" Width="90px"></asp:Label>
                <asp:TextBox ID="txtTimeFrom" runat="server" Width="62px"></asp:TextBox>
                <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender1" TargetControlID="txtTimeFrom"
                    MaskType="Time" Mask="99:99" />
                -
                <asp:TextBox ID="txtTimeTo" runat="server" Width="62px"></asp:TextBox>
                <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender2" TargetControlID="txtTimeTo"
                    MaskType="Time" Mask="99:99" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="Город интернет магазина"></asp:Localize><span
                class="required">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtSenderCity" runat="server" Width="230px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize6" runat="server" Text="Адрес интернет магазина"></asp:Localize><span
                class="required">*</span>
        </td>
        <td>
            <div style="height: 34px;">
                <asp:Label ID="lblSenderStreet" runat="server" Width="90px" Text="Улица"></asp:Label><asp:TextBox
                    ID="txtSenderStreet" runat="server" Width="140px"></asp:TextBox>
            </div>
            <div style="height: 34px;">
                <asp:Label ID="Label1" runat="server" Width="90px" Text="Дом"></asp:Label><asp:TextBox
                    ID="txtSenderHouse" runat="server" Width="140px"></asp:TextBox>
            </div>
            <div style="height: 34px;">
                <asp:Label ID="Label2" runat="server" Width="90px" Text="Квартира/офис"></asp:Label><asp:TextBox
                    ID="txtSenderFlat" runat="server" Width="140px"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize10" runat="server" Text="Имя контактного лица"></asp:Localize><span
                class="required">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtSenderName" runat="server" Width="230px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize7" runat="server" Text="Телефон контактного лица"></asp:Localize><span
                class="required">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtSenderPhone" runat="server" Width="230px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize8" runat="server" Text="Общий вес, в граммах"></asp:Localize><span
                class="required">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtSenderWeght" runat="server" Width="230px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <div style="height: 40px; margin-top: 15px; margin-bottom: 5px;">
                <asp:Button ID="btnCallCourier" runat="server" OnClick="btnCallCourier_OnClick" Text="Вызвать курьера" CssClass="btn btn-middle btn-action" CausesValidation="False" />
            </div>
        </td>
        <td>
            <asp:Label runat="server" ID="lblMessageSdek" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize14" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Weight %>"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWeight" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtWeight" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Вес товара
                    </header>
                    <div class="help-content">
                        Вес товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в кг, возможно указать дробное значение.<br />
                        <br />
                        Например: 1 или 0.2 (кг.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label5" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Length %>"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtLength" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator2" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtLength" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Длина товара
                    </header>
                    <div class="help-content">
                        Длина товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в мм.<br />
                        <br />
                        Например: 120 (мм.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label6" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Width %>"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWidth" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator3" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtWidth" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Ширина товара
                    </header>
                    <div class="help-content">
                        Ширина товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в мм.<br />
                        <br />
                        Например: 120 (мм.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label7" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Height %>"></asp:Localize><span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtHeight" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator4" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtHeight" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Высота товара
                    </header>
                    <div class="help-content">
                        Высота товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в мм.<br />
                        <br />
                        Например: 120 (мм.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label8" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<div class="dvSubHelp2">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/install-sdek" target="_blank">Инструкция. Подключение модуля доставки СДЭК</a>
</div>
