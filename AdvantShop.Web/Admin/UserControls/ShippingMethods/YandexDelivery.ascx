<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YandexDelivery.ascx.cs" Inherits="AdvantShop.Admin.UserControls.ShippingMethods.YandexDelivery" %>

<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <% if (!IsActive) {%>
       <tr>
        <td colspan="3">
            Инструкция:<br><br>
            1) Зарегистрируйтесь на сайте <a href="https://delivery.yandex.ru" target="blank">delivery.yandex.ru</a><br><br>
            2) Заполните данные в разделах "Магазин", "Склад", "Реквизиты" Яндекс.Доставки<br><br>
            3) Перейдите в интерфейсе сервиса Яндекс.Доставки на страницу настроек "Интеграция" - "Api". Нажмите API-ключи "получить". Скопируйте полученные ключи в соответсвующие поля магазине.<br><br><br>
            <div>
                <asp:TextBox runat="server" ID="txtApiKeys" Width="550" ValidationGroup="5" TextMode="MultiLine" Rows="10" Columns="10" />
            </div>
            <div style="margin: 20px 0 0 0">
                <asp:TextBox runat="server" ID="txtApiClientKeys" Width="550" ValidationGroup="5" TextMode="MultiLine" Rows="10" Columns="10" />
            </div>
        </td>
    </tr>
    <% } else {%>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize8" runat="server" Text="Статус:"></asp:Localize>
        </td>
        <td class="columnVal status-msmodule">
            <asp:Label runat="server" ID="lblStatus" /> <a href="javascript:void(0)">Деактивировать</a>
            <asp:HiddenField runat="server" ID="hfIsActive"/>
            <asp:HiddenField runat="server" ID="hfMSObject"/>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName" style="vertical-align: top">
            <asp:Localize ID="Localize7" runat="server" Text="Код корзинного виджета"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            Перейдите на страницу виджетов в сервисе Яндекс.Доставка "Интеграция" - "Виджеты" - "Корзинный виджет". Нажмите установить и скопируйте код в поле снизу. 
            <br/>
            <asp:TextBox runat="server" ID="txtWidgetCode" Width="550" Height="50px" ValidationGroup="5" Text="" TextMode="MultiLine"/>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="client_id"></asp:Localize> <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtClientId" Width="250" ValidationGroup="5" Text=""></asp:TextBox>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="sender_id"></asp:Localize> <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtSenderId" Width="250" ValidationGroup="5" Text=""></asp:TextBox>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize9" runat="server" Text="warehouse_id"></asp:Localize> <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWarehouseId" Width="250" ValidationGroup="5" Text=""></asp:TextBox>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize10" runat="server" Text="requisite_id"></asp:Localize> <span class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtRequisiteId" Width="250" ValidationGroup="5" Text=""></asp:TextBox>
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="Город, из которого осуществляется доставка"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCityFrom" Width="250" ValidationGroup="5" Text="Москва"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="Город, из которого осуществляется доставка" /><asp:Label
                    runat="server" ID="msgCityFrom" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            Включать в стоимость доставки вознаграждение за объявление ценности заказа
        </td>
        <td class="columnVal">
            <asp:CheckBox runat="server" ID="chkShowAssessedValue"/> 
            <div>Значение этой опции должно совпадать со значением в ЛК сервиса Яндекс.Доставка</div>
        </td>
        <td class="columnDescr">
        </td>
    </tr>

    <tr>
        <td colspan="3" style="padding: 20px 0 0 0">
            Если у товаров не указан размер или вес, то будут применены следущие параметры<br><br>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize6" runat="server" Text="Средний вес"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWeight" Width="50px" Text="1" /> кг.
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="Длина, высота, ширина (мм) "></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtLengthAvg" Width="80px" Text="100" /> 
            <asp:TextBox runat="server" ID="txtHeightAvg" Width="80px" Text="100" /> 
            <asp:TextBox runat="server" ID="txtWidthAvg" Width="80px" Text="100" /> мм.
        </td>
        <td class="columnDescr">
        </td>
    </tr>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.status-msmodule a').on("click", function () {
                $('.status-msmodule span').text('Изменен. Нажмите кнопку сохранить');
                $('.status-msmodule input').val('False');
            });
        });
    </script>
    <% } %>

</table>
<asp:Label runat="server" ID="lError" ForeColor="Red" Visible="False" style="display:block; margin-bottom: 10px;" />
<br/>
<br/>


