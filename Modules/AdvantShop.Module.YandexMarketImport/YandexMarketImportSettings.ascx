<%@ Control Language="C#" AutoEventWireup="true" Codebehind="YandexMarketImportSettings.ascx.cs"
    Inherits="AdvantShop.Module.YandexMarketImport.YandexMarketImportSettings" %>
<div>
    <span class="spanSettCategory">
        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: YandexMarketImport_Header%>" /></span>
    <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
    <hr color="#C2C2C4" size="1px" />
</div>

<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td>
            <div id="mainDiv" runat="server">
                <asp:Panel ID="pUploadExcel" runat="server">
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="lblProcess301Redirect" runat="server" Text="<%$ Resources: YandexMarketImportSettings_301Redirect%>"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbProcess301Redirect" runat="server" Checked="False" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label3" runat="server" Text="Деактивировать товары, которых нет в прайсе"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbDeactivateProducts" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label4" runat="server" Text="Обнулить количество у товаров, которых нет в прайсе"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAmountNulling" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label13" runat="server" Text="Удалять старые цены"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbDeleteOldPrices" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label14" runat="server" Text="Включить возможность покупки товаров под заказ"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAllowPreorder" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="lblCurrencyIso" runat="server" Text="<%$ Resources: YandexMarketImportSettings_DefaultCurrencyIso%>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDefaultCurrencyIso" runat="server" Text="1"></asp:TextBox>
                                <asp:RangeValidator ID="rv" runat="server" ControlToValidate="txtDefaultCurrencyIso"
                                    MinimumValue="0,0001" MaximumValue="10000000" Type="Double" ErrorMessage="*"
                                    EnableClientScript="True"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize1" runat="server" Text="Настройка соответствия Артикула Offer"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label5" runat="server" Text="Формат артикула"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblArtNoType" runat="server">
                                    <asp:ListItem Value="Attribute" Selected="True">В атрибуте id тега offer(по умолчанию)</asp:ListItem>
                                    <asp:ListItem Value="Tag">В тэге</asp:ListItem>
                                    <asp:ListItem Value="Param">В блоке параметров</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label1" runat="server" Text="Имя тега или параметра"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtArtnoFieldName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize4" runat="server" Text="Настройка соответствия Артикула Продукта"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label10" runat="server" Text="Формат артикула"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblArtNoProductType" runat="server">
                                    <asp:ListItem Value="Attribute" Selected="True">В атрибуте group_id тега offer(по умолчанию)</asp:ListItem>
                                    <asp:ListItem Value="Tag">В тэге</asp:ListItem>
                                    <asp:ListItem Value="Param">В блоке параметров</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label11" runat="server" Text="Имя тега или параметра"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtArtnoProductFieldName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize16" runat="server" Text="Настройка соответствия количества"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label6" runat="server" Text="Формат количества товара"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblAmountNulling" runat="server">
                                    <asp:ListItem Value="None" Selected="True">Нет в файле(по умолчанию)</asp:ListItem>
                                    <asp:ListItem Value="Tag">В тэге</asp:ListItem>
                                    <asp:ListItem Value="Param">В блоке параметров</asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label2" runat="server" Text="Имя тега или параметра"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmountFieldName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize5" runat="server" Text="Настройка соответствия названия товара"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label16" runat="server" Text="Имя тега с названием продукта"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlNameProduct" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize2" runat="server" Text="Настройка обновления продукта"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label7" runat="server" Text="Способ обновления"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblUpdateType" runat="server">
                                    <asp:ListItem Value="Full" Selected="true">Полное обновление продукта(по умолчанию)</asp:ListItem>
                                    <asp:ListItem Value="Amount">Обновлять только количество</asp:ListItem>
                                    <asp:ListItem Value="Price">Обновлять только стоимость</asp:ListItem>
                                    <asp:ListItem Value="AmountAndPrice">Обновлять количество и стоимость</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize3" runat="server" Text="Настройка обновления по расписанию"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                         <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label12" runat="server" Text="Включить обновление по расписанию"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAutoUpdateActive" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label15" runat="server" Text="Наценка магазина"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtExtraCharge" Text="0" Width="100px"></asp:TextBox>
                                %
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label9" runat="server" Text="Период обновления"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTimePeriodValue" runat="server" Width="100px" Text="1"></asp:TextBox> <asp:DropDownList ID="ddlTimePeriod" runat="server" style="width:100px; display:inline-block;"> 
                                    <asp:ListItem Value="Hours">Часы</asp:ListItem>
                                    <%--<asp:ListItem Value="Minutes">Минуты</asp:ListItem>--%>
                                </asp:DropDownList>
                                <asp:RangeValidator runat="server" Type="Integer"
                                    MinimumValue="1" MaximumValue="1440" ControlToValidate="txtTimePeriodValue"
                                    ErrorMessage="Значение должно быть целым числом в промежутке от 1 до 1440" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label8" runat="server" Text="Сссылка на файл Yml"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileUrlPath" runat="server"></asp:TextBox>
                            </td>
                        </tr>

                        <tr class="rowsPost" id="trStatisticLog" runat="server" visible="False">
                            <td style="width: 200px; text-align: left;"></td>
                            <td>
                                <a href="../modules/YandexMarketImport/temp/statisticLog.txt" target="_blank">Журнал импорта</a>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="<%$ Resources: YandexMarketImportSettings_Save %>"
                                    OnClick="btnSave_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>

            </div>
        </td>
    </tr>
</table>