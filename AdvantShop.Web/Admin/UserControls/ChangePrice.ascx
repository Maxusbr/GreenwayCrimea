<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ChangePrice" Codebehind="ChangePrice.ascx.cs" %>
<%@ Register src="~/Admin/UserControls/TreeViewCategoryMultiSelect.ascx" tagName="TreeViewCategoryMultiSelect" tagPrefix="adv" %>
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align: top; text-align: right; width: 50%;">
                    <div style="padding: 0 10px;">
                        <asp:DropDownList ID="ddlAction" runat="server" Style="display: inline-block;">
                        </asp:DropDownList>
                        <asp:Label runat="server" Text="<%$  Resources: Resource, Admin_ChangePrice_By %>"></asp:Label>
                        <asp:TextBox ID="txtValue" runat="server" Width="50px"></asp:TextBox>&nbsp;%
                    </div>
                    <div style="padding: 20px 0 0; text-align: center; display: inline-block; width: 270px;">
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="<%$  Resources: Resource, Admin_ChangePrice_GO %>" />
                        <br />
                        <br />
                        <asp:Label runat="server" Visible="false" ID="lblMessage" Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                    </div>
                </td>
                <td style="vertical-align: top;">
                    <div style="padding: 0 10px;">
                        <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ChangePrice_SelectCategories %>" Font-Bold="true"></asp:Label>
                        <adv:TreeViewCategoryMultiSelect ID="tvCategories" runat="server" OnTreeNodeSelected="treeNode_Selected" />
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
