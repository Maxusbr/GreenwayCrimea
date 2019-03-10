<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerAddressBook.ascx.cs" Inherits="Admin.UserControls.CustomerAddressBook" %>
<asp:UpdatePanel ID="upAddressBook" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
            <asp:MultiView ID="mvAdressBook" runat="server" ActiveViewIndex="0">
                <asp:View ID="vAdressBook" runat="server">
                    <table cellpadding="0" cellspacing="0" style="width: 98%">
                        <tr>
                            <td class="formheader" colspan="2">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <%=Resources.Resource.Admin_ViewCustomer_AddressBook%>
                                </h4>
                            </td>
                        </tr>
                        <tr class="formheaderfooter">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="CustomerContacts" runat="server">
                                </asp:RadioButtonList>
                                <br />
                                <asp:Button ID="btnAddNewContact" runat="server" Text="<%$ Resources:Resource, Admin_Insert %>"
                                    OnClick="btnAddNewContact_Click" />
                                <asp:Button ID="btnDeleteContact" runat="server" Text="<%$ Resources:Resource, Admin_Delete %>"
                                    OnClick="btnDeleteContact_Click" />
                                <asp:Button ID="btnEditContact" runat="server" Text="<%$ Resources:Resource, Admin_Edit %>"
                                    OnClick="btnEditContact_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vAddEditContact" runat="server">
                    <table cellpadding="0" cellspacing="0" style="width: 98%">
                        <tr>
                            <td class="formheader" colspan="2">
                                <h4 style="display: inline; font-size: 10pt;">
                                    <%=Resources.Resource.Admin_ViewCustomer_NewAdress%>
                                </h4>
                            </td>
                        </tr>
                    </table> 
                    <asp:Label ID="lblAddressBookMessage" runat="server" Visible="False" ForeColor="Blue"></asp:Label>
                    <br />
                    <table class="tabsformtable" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Name %>"></asp:Label><span
                                    class="required">&nbsp;*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactName" runat="server" Width="300px"></asp:TextBox>
                                <asp:Label ID="msgContactName" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ContactCountry %>"></asp:Label><span
                                    class="required">&nbsp;*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cboCountry" runat="server" Width="200px" DataTextField="Name" DataValueField="CountryID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ContactZone %>"></asp:Label><span
                                    class="required">&nbsp;*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactZone" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ContactCity %>"></asp:Label><span
                                    class="required">&nbsp;*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactCity" runat="server" Width="200px"></asp:TextBox>
                                <asp:Label ID="msgContactCity" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ContactAddress %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactAddress" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ContactZip %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactZip" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btnAddChangeContact" runat="server" CausesValidation="true" Text="<%$ Resources:Resource, Admin_Insert %>"
                        OnClick="btnAddChangeContact_Click" />
                </asp:View>
            </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
