<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.RetailRocket.Admin_RetailRocketModule" Codebehind="RetailRocketModule.ascx.cs" %>
<div style="text-align: center;">
    <table class="info-tb">
        <tr>
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: RR_Header %>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: left;vertical-align:top;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: RR_PartnerId%>"></asp:Localize> <span style="color:red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtPartnerId" runat="server" Width="300px" />
                <br/>
                В настройках трекинга Retail Rocket найдите тип трекинга "Основной трекинг-код системы" и нажмите "Код трекера". 
                Скопируйте xxxxxxxxxxxxxxxxxxxxxxxx из строки "... var rrPartnerId = "xxxxxxxxxxxxxxxxxxxxxxxx";  ..."
            </td>
        </tr>
         <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: RR_Limit%>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtLimit" Text="8" Width="50px" />
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: RR_UseApi%>" />
            </td>
            <td class="useapi">
                <asp:CheckBox runat="server" ID="chkUseApi" />
            </td>
        </tr>
        <tr class="widgetcode">
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="locRelatedProduct" runat="server" />
            </td>
            <td style="padding: 5px 0">
                <asp:TextBox runat="server" ID="txtRelatedRecoms" TextMode="MultiLine" Width="300px" Height="70px" />
            </td>
        </tr>
        <tr class="widgetcode">
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="locAlternativeProduct" runat="server" />
            </td>
            <td style="padding: 5px 0">
                <asp:TextBox runat="server" ID="txtAlterRecoms" TextMode="MultiLine" Width="300px" Height="70px" />
            </td>
        </tr>
        <tr class="widgetcode">
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="locShoppingCart" runat="server" Text="<%$ Resources: RR_ShoppingCartRecoms%>" />
            </td>
            <td style="padding: 5px 0">
                <asp:TextBox runat="server" ID="txtShoppingCartRecoms" TextMode="MultiLine" Width="300px" Height="70px" />
            </td>
        </tr>
        <tr style="vertical-align:top;">
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: RR_SendMail%>" />
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkSendMail" />
                <div><asp:Label runat="server" ID="lblSendEmailHint" Text="<%$ Resources: RR_SendMailHint %>" /> </div>
            </td>
        </tr>
        

        <tr>
            <td colspan="2" style="height: 20px;">
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize5" runat="server" Text="Товары на главной (блок сверху)" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMainPageProductsTop" runat="server">
                    <asp:ListItem Text="Не показывать" Value="0"  />
                    <asp:ListItem Text="Самые популярные товары" Value="1"  />
                    <asp:ListItem Text="Персональные рекомендации" Value="2"  />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize6" runat="server" Text="Название блока Товары на главной (блок сверху)" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMainPageTopTitle" Width="400px"  />
            </td>
        </tr>
        
        <tr>
            <td colspan="2" style="height: 20px;">
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize8" runat="server" Text="Товары на главной (блок снизу)" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMainPageProductsBottom" runat="server">
                    <asp:ListItem Text="Не показывать" Value="0"  />
                    <asp:ListItem Text="Самые популярные товары" Value="1"  />
                    <asp:ListItem Text="Персональные рекомендации" Value="2"  />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize10" runat="server" Text="Название блока Товары на главной (блок снизу)" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMainPageBottomTitle" Width="400px"  />
            </td>
        </tr>
        
        <tr>
            <td colspan="2" style="height: 20px;">
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize ID="Localize7" runat="server" Text="Категория (сверху)" />
            </td>
            <td>
                <asp:DropDownList ID="ddlCategoryTop" runat="server">
                    <asp:ListItem Text="Не показывать" Value="0"  />
                    <asp:ListItem Text="Категория" Value="1"  />
                    <asp:ListItem Text="Персональные рекомендации" Value="2"  />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize runat="server" Text="Название блока категории (сверху)" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCategoryTopTitle" Width="400px"  />
            </td>
        </tr>
        
        <tr>
            <td colspan="2" style="height: 20px;">
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize runat="server" Text="Категория (снизу)" />
            </td>
            <td>
                <asp:DropDownList ID="ddlCategoryBottom" runat="server">
                    <asp:ListItem Text="Не показывать" Value="0"  />
                    <asp:ListItem Text="Категория" Value="1"  />
                    <asp:ListItem Text="Персональные рекомендации" Value="2"  />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 215px; text-align: left;">
                <asp:Localize runat="server" Text="Название блока категории (снизу)" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCategoryBottomTitle" Width="400px"  />
            </td>
        </tr>
        

        <tr>
            <td colspan="2" style="padding: 10px 0">
                Чтобы активировать 'С этим товаром покупают' и 'Похожие товары' зайдите в <a href="CommonSettings.aspx#tabid=details" target="blank">Настройки -&gt; Общие -&gt; Карточка товара -&gt; Источник продуктов для перекрестного маркетинга</a> и выберите "из модуля"
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: RR_Save%>" />
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    function ToggleWidget() {
        if ($(".useapi input").is(":checked")) {
            $(".widgetcode").hide();
        } else {
            $(".widgetcode").show();
        }
    }

    $(".useapi input").on("click", function(e) {
        ToggleWidget();
    });

    ToggleWidget();
</script>