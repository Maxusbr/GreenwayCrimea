<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.MainPageProduct" Codebehind="MainPageProduct.ascx.cs" %>
<table border="0" cellspacing="0" cellpadding="0" class="catalog_part catelog_list">
    <tr style="height: 28px;">
        <td style="width: 30px;">
            <img src="images/note.gif" alt="" />
        </td>
        <td class="catalog_label">
            <asp:Localize ID="locName" runat="server" Text=""></asp:Localize>
        </td>
        <td style="width: 18px;" align="right">
            <a href="ProductsOnMain.aspx?type=<%= Flag.ToString()  %>">
                <img class="showtooltip" title="<%= Resources.Resource.Admin_MasterPageAdminCatalog_Edit %>"
                    style="border: none;" src="images/gbpencil.gif" alt="" /></a>
        </td>
    </tr>
</table>
<div class="catalog_part catelog_listContent">
    <asp:Repeater ID="rMainProduct" runat="server">
        <HeaderTemplate>
            <ul class="catelog_ullist">
        </HeaderTemplate>
        <ItemTemplate>
            <li><a class="Link" href='<%# "Product.aspx?ProductId=" + Eval("ProductID")%>'>
                <%# Eval("Name")%></a></li>
        </ItemTemplate>
        <FooterTemplate>
            <li><a class="Link" href='<%# "ProductsOnMain.aspx?type=" + Flag.ToString()%>'>
                <% = Resources.Resource.Admin_UserControls_MainPageProduct_Other%></a></li>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <span style="display: inline-block; width: 100%; text-align: center; font-size: 14px"
        id="ProductsNoRecordsBlock" visible="false" runat="server">
        <%=Resources.Resource.Admin_Catalog_NoRecordsShort%></span>
</div>
