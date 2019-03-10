<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Admin.UserControls.ExportFeedUc.ExportFeedGoogleSettingsUc" CodeBehind="ExportFeedGoogleSettingsUc.ascx.cs" %>
<table style="width: 100%;" class="export-settings">
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal1" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_AlowPreOrder %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbExportNotAvailable" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal ID="Literal2" runat="Server" Text='Добавлять к названию цвет и размер' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:CheckBox runat="server" ID="chbAddedColorAndSizeForName" CssClass="checkly-align" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 300px;">
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedTitle %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtDatafeedTitle" runat="Server" CssClass="niceTextBox textBoxClass" />
            </span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_FeedDescription %>' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtDatafeedDescription" runat="Server" CssClass="niceTextBox textBoxClass" />
            </span>
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
            <%= Resources.Resource.Admin_ExportFeed_GoogleRecomendation %>
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
            <asp:Literal ID="Literal6" runat="Server" Text='GoogleProductCategory' />
        </td>
        <td>
            <span class="parametrValueString">
                <asp:TextBox ID="txtGoogleProductCategory" runat="Server" CssClass="niceTextBox textBoxClass" />
            </span>
              <a href="https://support.google.com/merchants/answer/160081" target="_blank"><%= Resources.Resource.Admin_Product_Yandex_ListMarketCategory %></a>
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
</table>
