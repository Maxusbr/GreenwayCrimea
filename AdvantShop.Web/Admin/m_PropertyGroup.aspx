<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="m_MasterPage.master" Inherits="Admin.m_PropertyGroup" ValidateRequest="false" Codebehind="m_PropertyGroup.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="contentCenter" runat="server" ContentPlaceHolderID="cphCenter">
    <div>
        <center>
            <asp:Label ID="lblCustomer" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_PropertyGroup_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_PropertyGroup_SubHeader %>"></asp:Label>
        </center>
        <asp:Panel ID="pnlAdd" runat="server" Height="84px" Width="100%">
            <center>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;</center>
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_PropertyGroup_Name%>"></asp:Label>:
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        Название в клиентской части:
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtNameDisplayed" runat="server" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_PropertyGroup_SortOrder %>"></asp:Label>:
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtSortOrder" runat="server" Width="300px" Text="0"></asp:TextBox>
                        <asp:RangeValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSortOrder"  MaximumValue="10000000" MinimumValue="-10000000"
                            ErrorMessage="*" Type="Integer"></asp:RangeValidator>
                    </td>
                </tr>
                <tr id="categoriesTr" runat="server" style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <%= Resource.Admin_m_PropertyGroup_Categories %>:
                    </td>
                    <td style="height: 27px;">
                        <asp:Literal runat="server" ID="liCategories" />
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Ok %>"
                    Width="110px" OnClick="btnOK_Click" />&nbsp;</center>
            <br />
        </asp:Panel>
    </div>
</asp:Content>