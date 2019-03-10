<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedYandexSettingsUc" CodeBehind="ExportFeedYandexSettingsUc.ascx.cs" %>
<%@ Import Namespace="Resources" %>

<table style="width: 100%;" class="export-settings">
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal14" runat="Server" Text='Основная ставка' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtBid" runat="Server" CssClass="niceTextBox textBoxClass" />
                <asp:RangeValidator ID="rvBid" runat="server" ControlToValidate="txtBid"
                    ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                    MaximumValue="100000" MinimumValue="-100000" Type="Double"> </asp:RangeValidator>
                <div data-plugin="help" class="help-block">
                    <div class="help-icon js-help-icon"></div>
                    <article class="bubble help js-help">
                        <header class="help-header">
                            oсновная ставка
                        </header>
                        <div class="help-content">
                            Действует на всех местах размещения кроме карточки модели
                            <br>
                            В качестве значений указываются условные центы. Значения должны быть целыми
                            <br />
                            и положительными числами, например «80», что соответствует ставке 0,8 у. е.
                        </div>
                    </article>
                </div>
            </span>
        </td>
    </tr>


    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal3" runat="Server" Text='Выгружать Не доступные к покупке товары' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbExportNotAvailable" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost" runat="Server">
        <td style="width: 300px;">
            <asp:Literal ID="ltrlCompanyName" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_CompanyName %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtCompanyName" runat="Server" CssClass="niceTextBox textBoxClass" />
                <span class="warning">
                    <br />
                    <asp:Literal ID="ltrlShopNameWarning" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_ShopNameWarning %>' />
                    <asp:Literal ID="ltrlCompanyNameDote" runat="Server" />. </span></span>
        </td>
    </tr>
    <tr class="rowsPost" runat="Server">
        <td>
            <asp:Literal ID="ltrlShopName" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_ShopName %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtShopName" runat="Server" CssClass="niceTextBox textBoxClass" />
                <span class="warning">
                    <br />
                    <asp:Literal ID="Literal4" runat="Server" Text='Примечание:' />
                    <asp:Literal ID="companyName2" runat="Server" />. </span></span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedCurrencySelect %>' />
            <span class="warning" id="currencyWarning" visible="false" runat="Server">
                <br />
                <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedCurrencyWarning %>' />
                <asp:Literal ID="MainCurrencyLiteral" runat="Server" />.
                <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedCurrencySelectWarning %>' />.
            </span>
        </td>
        <td>
            <span class="parametrValueString">
                <asp:DropDownList Width="150px" ID="CurrencyListBox" runat="Server">
                </asp:DropDownList>
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='Выгружать в качестве идентификатора предложения' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:DropDownList Width="150px" ID="ddlOfferIdType" runat="Server">
                    <asp:ListItem Selected="True" Text="ID модификации" Value="id"></asp:ListItem>
                    <asp:ListItem Text="Артикул модификации " Value="artno"></asp:ListItem>
                </asp:DropDownList>
                <div data-plugin="help" class="help-block">
                    <div class="help-icon js-help-icon"></div>
                    <article class="bubble help js-help">
                        <header class="help-header">
                            Выгружать в качестве идентификатора предложения 
                        </header>
                        <div class="help-content">
                            "ID модификации" - значение по умолчанию, если вы не уверены, не стоит его изменять.
                            <br>
                            "Артикул модификации" может использоваться если он содержит только латинские буквы и цифры. Любые другие символы (кириллические буквы, тире, знаки препинания) будут вызывать ошибку при валидации файла.<br>
                            Длина должна быть не более 20 символов. 
                        </div>
                    </article>
                </div>
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedDescriptionSelect %>'></asp:Literal>
        </td>
        <td>
            <span class="parametrValueString">
                <asp:DropDownList Width="150px" ID="ddlProductDescriptionType" runat="Server">
                    <asp:ListItem Value="short" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedDescriptionShort %>' />
                    <asp:ListItem Value="full" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedDescriptionFull %>' />
                    <asp:ListItem Value="none" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedDescriptionNone %>' />
                </asp:DropDownList>
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedRecomendation %>' />
        </td>
        <td>
            <%= Resources.Resource.Admin_ExportFeed_YandexRecomendation %>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='Utm разметка ссылок' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtUrlTags" runat="Server" CssClass="niceTextBox textBoxClass" />
                <br />
                <span class="warning">
                    <asp:Label runat="server" ID="lblUrlTagsNote" Text="" Style="font-size: 10px;"></asp:Label></span>
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedSalesNotes %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtSalesNotes" runat="Server" CssClass="niceTextBox textBoxClass" />
                <br />
                <span class="warning">
                    <asp:Label runat="server" ID="Label1" Text="<%$ Resources:Resource,Admin_ExportFeed_FeedSalesNotesNote %>" Style="font-size: 10px;"></asp:Label></span>
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal7" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_RemoveHTML%>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbRemoveHTML" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal2" runat="Server" Text='Выгружать с учетом скидки' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="ckbExportProductDiscount" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal8" runat="Server" Text='Возможность самовывоза' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbPickup" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal5" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedDelivery %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbDelivery" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal15" runat="Server" Text='Выгружать рекомендованный товары' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbExportRelatedProducts" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal10" runat="Server" Text='Выгружать стоимоcть доставки' />
        </td>
        <td>
            <span class="parametrValueString delivery-list">
                <asp:RadioButtonList runat="server" ID="rbDeliveryCost" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost localdelivery-b">
        <td>
            <asp:Literal ID="Literal6" runat="Server" Text='Настройки из стоимости доставки товара' />
        </td>
        <td>
            <span class="parametrValueString">Срок доставки (Примеры: "0","1","1-3")
                <asp:TextBox ID="txtLocalDeliveryDays" runat="server" Text="" />
                Время
                <asp:TextBox ID="txtLocalDeliveryOrderBefore" runat="server" Text="" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='Глобальные настройки стоимости доставки' />
        </td>
        <td>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <contenttemplate>
                    <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                        CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>"
                        CssClass="tableview" Style="cursor: pointer" GridLines="None"
                        OnRowCommand="grid_RowCommand" ShowFooterWhenEmpty="true">
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="75" HeaderStyle-Width="75">
                                <HeaderTemplate>
                                    <div style="height: 0px; width: 75px; font-size: 0px;">
                                    </div>
                                    <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall headerCb" runat="server"
                                        onclick="javascript:SelectVisible(this.checked);" Style="margin-left: 0px;" />
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <input type='checkbox' class='sel' />
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Container.DataItemIndex %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Cost" HeaderStyle-HorizontalAlign="Left">
                                <HeaderTemplate>
                                    Стоимость
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCost" runat="server" Text='<%# Eval("Cost") %>' Width="99%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNewCost" runat="server" Text='0' Width="99%" CssClass="add" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Days" HeaderStyle-HorizontalAlign="Left">
                                <HeaderTemplate>
                                    Срок доставки (Примеры: "0","1","1-3")
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDays" runat="server" Text='<%# Eval("Days") %>' Width="99%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNewDays" runat="server" Text='1-3' Width="99%" CssClass="add" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="OrderBefore" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200">
                                <HeaderTemplate>
                                    Время, до которого нужно успеть заказать, чтобы сроки доставки не сдвинулись на один день вперед
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOrderBefore" runat="server" Text='<%# Eval("OrderBefore") %>' Width="99%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNewOrderBefore" runat="server" Text='24' Width="99%" CssClass="add" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100" HeaderStyle-Width="100" AccessibleHeaderText="Buttons"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-Width="100" FooterStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <div style="height: 0px; width: 60px; font-size: 0px;">
                                    </div>
                                </HeaderTemplate>
                                <EditItemTemplate>

                                    <asp:LinkButton ID="buttonDelete" runat="server"
                                        CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteItem" CommandArgument='<%# Container.DataItemIndex%>'
                                        ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>'
                                        data-confirm="<%$ Resources: Resource, Admin_ConfirmDeleting %>" />
                                    <%--<input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                        src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                        style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update %>' />--%>
                                    <asp:ImageButton runat="server" ID="lbUpdate" CssClass="updatebtn showtooltip" Style="display: none"
                                        ImageUrl="../../images/updatebtn.png" CommandName="UpdateItem" CommandArgument='<%# Container.DataItemIndex%>' />
                                    <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                        src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                        style="display: none" title='<%= Resource.Admin_MasterPageAdminCatalog_Cancel %>' />

                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="buttonAdd" ImageUrl="../../images/addbtn.gif" runat="server" ToolTip="<%$ Resources:Resource, Admin_OrderStatuses_Add  %>"
                                        CommandName="Add" />
                                    <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="../../images/cancelbtn.png"
                                        CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Currencies_CancelAdd  %>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="header" />
                        <FooterStyle CssClass="footer" />
                        <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                        <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                        <EmptyDataTemplate>
                            <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                <%=Resources.Resource.Admin_Catalog_NoRecords%>
                            </div>
                        </EmptyDataTemplate>
                    </adv:AdvGridView>

                    <div style="padding: 10px 0 0 0">
                        <asp:Button CssClass="btn btn-middle btn-add" runat="server" ID="lbAddDeliveryCostOption" Text="Добавить опцию" OnClick="lbAddDeliveryCostOption_Click" />
                    </div>
                </contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal1" runat="Server" Text='Добавлять к названию цвет и размер' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="ckbColorSizeToName" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal9" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedProperties %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbProperties" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal12" runat="Server" Text='Выгружать закупочную цену' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbPurchasePrice" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal13" runat="Server" Text='Наличие точки продажи' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbStoreSetting" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal16" runat="Server" Text='Выгружать штрихкод' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chkExportBarCode" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal17" runat="Server" Text='Выгружать все фотографии' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chkExportAllPhotos" CssClass="checkly-align" />
            </span>
        </td>
    </tr>

</table>
<script type="text/javascript">
    $(document).ready(function () {
        initgrid();
    });
</script>
