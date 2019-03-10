<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Blog.BlogSettings" Codebehind="BlogSettings.ascx.cs" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 200px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: PageTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtPageTitle" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: MetaTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaTitle" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: MetaKeywords %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaKeywords" Width="300px"></asp:TextBox>
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: MetaDescription %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaDescription" Width="300px"></asp:TextBox>
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: CategoriesTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtCategoriesTitle" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: MaxWidth %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtMaxWidth" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: MaxHeight %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtMaxHeight" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: ShowAddDate %>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowAddDate" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources: ShowRssBlog %>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowRssBlog" runat="server" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: StaticBlock %>"></asp:Localize>
            </td>
            <td>
                 <asp:TextBox ID="txtStaticBlock" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources: StaticBlockRight %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="ckeSbRight" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg"/>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left;">
                <%--<asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: MaxHeight %>"></asp:Localize>--%>
            </td>
            <td>
                <asp:HyperLink ID="lnkGoToModule" runat="server" Text="<%$ Resources: ModuleUrl %>" Target="_blank"></asp:HyperLink>
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Save %>" />
            </td>
        </tr>
    </table>
</div>