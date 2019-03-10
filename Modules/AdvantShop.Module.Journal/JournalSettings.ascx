<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Journal.JournalSettings" Codebehind="JournalSettings.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" style="float: right; margin-left: 10px;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 200px;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: JournalCoverType %>"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCoverType">
                <asp:ListItem Text="Желтый" Value="1" />
                <asp:ListItem Text="Красный" Value="2" />
                <asp:ListItem Text="Синий" Value="3" />
                <asp:ListItem Text="Зеленый" Value="4" />
                <asp:ListItem Text="Серый" Value="5" />
                <asp:ListItem Text="Темно-синий" Value="6" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize runat="server" Text="<%$ Resources: JournalCoverTop %>"></asp:Localize>
        </td>
        <td>
            <div style="padding: 5px 0 15px 0;">
                 <asp:TextBox ID="ckeCoverTop" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize runat="server" Text="<%$ Resources: JournalCoverMiddle %>"></asp:Localize>
        </td>
        <td>
            <div style="padding: 5px 0 15px 0;">
                <asp:TextBox ID="ckeCoverMiddle" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize runat="server" Text="<%$ Resources: JournalCoverBottom %>"></asp:Localize>
        </td>
        <td>
            <div style="padding: 5px 0 15px 0;">
                <asp:TextBox ID="ckeCoverBottom" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: JournalCatalogPageHead %>"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="ckeCatalogPageHead" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="200px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: JournalCatalogPageBottomLeft %>"></asp:Localize>
        </td>
        <td>
            <div style="padding: 10px 0;">
                <asp:TextBox runat="server" ID="txtCatalogPageBottomLeft" TextMode="MultiLine" Width="98%" Height="30px" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: JournalCatalogPageBottomRight %>"></asp:Localize>
        </td>
        <td>
            <div style="padding: 10px 0;">
                <asp:TextBox runat="server" ID="txtCatalogPageBottomRight" TextMode="MultiLine" Width="98%" Height="30px" />
            </div>
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2" style="text-align: left;">
            <span class="spanSettCategory">Каталог</span>
            <hr color="#C2C2C4" size="1px">
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 200px;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: JournalShowCover %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkShowJournalCover" CssClass="showcover" Checked="True" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: JournalProductType %>"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCoverProductType">
                <asp:ListItem Text="Плитка" Value="0" />
                <asp:ListItem Text="Список" Value="1" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left;vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text="Выгружать"></asp:Localize>
        </td>
        <td>
            <asp:UpdatePanel ID="upPanelCategories" runat="server">
            <ContentTemplate>
                Категории: 
                <asp:ListView runat="server" ID="lvCategories" OnItemCommand="lvCategories_OnItemCommand">
                    <ItemTemplate>
                        <div>
                            <%# Eval("Name").ToString() %> 
                            (<asp:LinkButton runat="server" ID="lbRemoveCategory" Text="Удалить" CommandName="RemoveCategory" CommandArgument='<%# Eval("CategoryId").ToString() %>'></asp:LinkButton>)
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        нет выбранных категорий
                    </EmptyDataTemplate>
                </asp:ListView>
                <div style="padding: 10px 0">
                    <asp:DropDownList runat="server" ID="ddlCategories" Width="250px" DataTextField="Text" DataValueField="Value" />
                    <asp:LinkButton runat="server" ID="lbAddCategory" OnClick="lbAddCategory_Click" Text="Добавить"/>
                </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 200px;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: JournalShowArtNo %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkShowArtNo" CssClass="showcover" Checked="True" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 200px;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: JournalShowOnlyAvailable %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkShowOnlyAvailable" CssClass="showcover" Checked="True" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 200px;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: JournalMoveNotAvaliableToEnd %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkMoveNotAvaliableToEnd" CssClass="showcover" Checked="True" />
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save %>" />
        </td>
    </tr>
        
    <tr>
        <td colspan="2">
            <div style="padding: 30px 0 0 0; font-weight: bold">
                Последние экспортированные файлы: 
            </div>
            <asp:ListView runat="server" ID="lvPdfs">
                <ItemTemplate>
                    <div style="padding: 5px 0">
                        <a target="_blank" href="<%# UrlService.GetUrl("modules/journal/pdfs/" + Eval("Text").ToString()) %>"><%# Eval("Text").ToString()%></a>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    нет pdf файлов
                </EmptyDataTemplate>
            </asp:ListView>
            
            <div style="padding: 5px 0 0 0">
                <div id="errorDiv" runat="server" style="color: red; font-size: 15px">
                    <asp:Label runat="server" ID="lblError" />
                </div>

                <asp:Button ID="btnMakeMagicPreview" runat="server" OnClick="btnMakeMagicPreview_Click" Text="Предпросмотр" CssClass="btn btn-middle btn-add" />
                <asp:Button ID="btnMakeMagic" runat="server" OnClick="btnMakeMagic_Click" Text="Экспортировать в pdf" CssClass="btn btn-middle btn-add" />
                
                <asp:HyperLink runat="server" ID="hlPreviewHtml" Target="_blank" Text="Предпросмотр в html" />
                
                <div style="padding: 10px 0;">
                    <asp:Label runat="server" ID="lblNotice" />
                </div>
                <div>
                    *Внимание: время экспорта и нагрузка на сайт зависят от количества выгружаемых товаров. Указывайте только те категории, которые вам нужны.
                </div>
            </div>
        </td>
    </tr>
</table>