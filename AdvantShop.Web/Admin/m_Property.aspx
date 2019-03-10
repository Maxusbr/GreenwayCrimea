<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="m_MasterPage.master" Inherits="Admin.m_Property" ValidateRequest="false" Codebehind="m_Property.aspx.cs" %>

<asp:Content ID="contentCenter" runat="server" ContentPlaceHolderID="cphCenter">
    <div>
        <center>
            <asp:Label ID="lblCustomer" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Properties_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Property_SubHeader %>"></asp:Label>
        </center>
        <asp:Panel ID="pnlAdd" runat="server" Height="84px" Width="100%">
            <center>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;</center>
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_PropertyGroup_Name%>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        Название, которое будет показываться в клиентской части&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtNameDisplayed" runat="server" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_m_Property_Description%>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtDescription" runat="server" Width="300px" Columns="2" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_m_Property_Unit%>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtUnit" runat="server" Width="100px" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_m_Property_PropertyType%>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:DropDownList runat="server" ID="ddlTypes" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_Property_Group%>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:DropDownList runat="server" ID="ddlGroup" Width="200px" DataTextField="Name" DataValueField="PropertyGroupId" />
                    </td>
                </tr>

                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="lblStringID" runat="server" Text="<%$ Resources:Resource, Admin_Properties_UseFilter%>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:CheckBox runat="server" ID="chkUseInFilter" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_Properties_UseInDetails%>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:CheckBox runat="server" ID="chkUseInDetails" Checked="True" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_Properties_UseInBrief%>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:CheckBox runat="server" ID="chkUseInBrief" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_Properties_Expanded%>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:CheckBox runat="server" ID="chkExpanded" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_Properties_SortOrder%>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:TextBox runat="server" ID="txtSortOrder" Width="100px" Text="0" />
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