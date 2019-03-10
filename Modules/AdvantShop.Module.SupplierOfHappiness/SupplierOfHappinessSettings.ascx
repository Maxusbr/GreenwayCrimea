<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierOfHappinessSettings.ascx.cs"
    Inherits="Advantshop.Module.SupplierOfHappiness.SupplierOfHappinessSettings" %>

<div>
    <span class="spanSettCategory">
        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Import_Header%>" /></span>
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
                                <asp:Label ID="Label5" runat="server" Text="Уникальный идентификатор Поставщика счастья"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUid" runat="server"></asp:TextBox>
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
                                <asp:Label ID="Label13" runat="server" Text="Деактивировать товары, которых нет в наличии"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbDeactivateProductsNotInStock" runat="server" />
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
                                <asp:Label ID="Label6" runat="server" Text="Розничная цена"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRetailPriceType" runat="server">
                                    <asp:ListItem Value="RetailPrice">Рекомендованная розничная цена (с учетом скидки)</asp:ListItem>
                                    <asp:ListItem Value="BaseRetailPrice">Базовая розничная цена</asp:ListItem>
                                    <asp:ListItem Value="BaseRetailPriceWithDiscount">Базовая розничная цена и скидка</asp:ListItem>
                                    <asp:ListItem Value="WholePrice">Оптовая цена c учетом скидки</asp:ListItem>
                                    <asp:ListItem Value="BaseWholePrice">Базовая оптовая цена</asp:ListItem>
                                    <asp:ListItem Value="BaseWholePriceWithDiscount">Базовая оптовая цена и скидка</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label11" runat="server" Text="Обновлять скидку у товара"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUpdateDiscount" runat="server">
                                    <asp:ListItem Value="false">Нет</asp:ListItem>
                                    <asp:ListItem Value="true">Да</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label8" runat="server" Text="Наценка магазина"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtExtraCharge" Text="0" Width="100px"></asp:TextBox>
                                %
                            </td>
                        </tr>

                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label7" runat="server" Text="Закупочная цена"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWholePriceType" runat="server">
                                    <%--               <asp:ListItem Value="RetailPrice">рекомендованная розничная цена c учетом скидки</asp:ListItem>
                                    <asp:ListItem Value="BaseRetailPrice">базовая рекомендованная розничная цена</asp:ListItem>--%>
                                    <asp:ListItem Value="WholePrice">Оптовая цена c учетом скидки</asp:ListItem>
                                    <asp:ListItem Value="BaseWholePrice">Базовая оптовая цена</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <br />
                                <br />
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize3" runat="server" Text="Настройка обновления по расписанию Полного каталога"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label12" runat="server" Text="Включить обновление по расписанию"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAutoUpdateActiveFull" runat="server" />
                                <div data-plugin="help" class="help-block" style="padding-bottom: 6px;">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Полное обновление по расписанию
                                    </header>
                                    <div class="help-content">
                                        Обновление будет происходить в 7 утра сразу после обновления данных у поставщика
                                    </div>
                                </article>
                            </div>
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <br />
                                <br />
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize1" runat="server" Text="Настройка обновления по расписанию Остатков"></asp:Localize></h4>
                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label1" runat="server" Text="Включить обновление по расписанию"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbAutoUpdateActiveQuick" runat="server" />
                            </td>
                        </tr>
                        <tr class="rowsPost">
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="Label2" runat="server" Text="Период обновления (час)"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTimePeriodValueQuick" runat="server" Width="70px" Text="1"></asp:TextBox>
                                <asp:RangeValidator runat="server" Type="Integer"
                                    MinimumValue="1" MaximumValue="23" ControlToValidate="txtTimePeriodValueQuick"
                                    ErrorMessage="Значение должно быть целым числом в промежутке от 1 до 23" />
                            </td>
                        </tr>
                        <tr class="rowPost">
                            <td colspan="2" style="height: 34px;">
                                <br />
                                <br />
                                <h4 style="display: inline; font-size: 10pt;">
                                    <asp:Localize ID="Localize2" runat="server" Text="Сопоставление категорий для новых товаров"></asp:Localize></h4>
                                <asp:LinkButton ID="btnSetDefaultCategoriesList" runat="server" Text="Установить по умолчанию" OnClick="btnSetDefaultCategoriesList_Click"
                                    Style="float: right;" CssClass="valid-confirm" data-confirm="Установить категории по умолчанию? Будут созданы новые категории в соответствии с поставщиком."></asp:LinkButton>
                                <asp:LinkButton ID="btnUpdateCategoriesList" runat="server" Text="Обновить список категорий" OnClick="btnUpdateCategoriesList_Click" Style="float: right; margin-right: 10px;"></asp:LinkButton>

                                <hr color="#C2C2C4" size="1px" />
                            </td>
                        </tr>

                        <asp:ListView ID="lvCategories" runat="server" ItemPlaceholderID="itemPlaceholderID" OnItemDataBound="lvCategories_OnDataBinding">
                            <layouttemplate>
                                <tr id="itemPlaceholderID" runat="server" class="rowsPost">
                                </tr>
                            </layouttemplate>
                            <itemtemplate>
                                <tr class="rowsPost">
                                    <td style="width: 400px; text-align: left;">
                                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Category") + " - " + Eval("SubCategory") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hfSoHCategory" runat="server" Value='<%# Eval("Category") %>' />
                                        <asp:HiddenField ID="hfSohSubCategory" runat="server" Value='<%# Eval("SubCategory") %>' />
                                        <asp:DropDownList runat="server" ID="ddlAdvantshopCategory" DataTextField="Text" DataValueField="Value" Width="300px" />
                                    </td>
                                </tr>
                            </itemtemplate>
                        </asp:ListView>


                        <tr class="rowsPost">
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="<%$ Resources: ImportSettings_Save %>"
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
