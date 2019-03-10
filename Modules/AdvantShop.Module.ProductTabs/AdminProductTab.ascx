<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.ProductTabs.AdminProductTab" CodeBehind="AdminProductTab.ascx.cs" %>
<table class="table-p">
    <tbody>
        <tr>
            <td class="formheader">
                <h2>
                    <%= TabTitle.Title %>
                    <%= TabTitle.Active ? "" : " <span class=\"tab-no-active\">" + GetLocalResourceObject("AdminProductTab_TabNotActive") + "</span>" %>
                </h2>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="ckTabBody" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" />
            </td>
        </tr>
    </tbody>
</table>
