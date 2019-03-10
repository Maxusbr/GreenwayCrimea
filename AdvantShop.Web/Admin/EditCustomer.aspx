<%@ Page AutoEventWireup="true" Inherits="Admin.EditCustomer" Language="C#"
    MasterPageFile="~/Admin/MasterPageAdmin.master" CodeBehind="EditCustomer.aspx.cs" %>

<%@ Import Namespace="Resources" %>

<%@ Register Src="UserControls/Customer/CustomerAddressBook.ascx" TagName="CustomerAddressBookAdmin" TagPrefix="adv" %>
<%@ Register Src="UserControls/Customer/CustomerOrderHistory.ascx" TagName="CustomerOrderHistoryAdmin" TagPrefix="adv" %>
<%@ Register Src="UserControls/Customer/CustomerRoleActionsAdmin.ascx" TagName="CustomerRoleActionsAdmin" TagPrefix="adv" %>
<asp:Content ID="ContentViewCustomer" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="CustomerSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Buyers%></a></li>
            <li class="neighbor-menu-item"><a href="CustomersGroups.aspx">
                <%= Resource.Admin_MasterPageAdmin_CustomersGroups%></a></li>
            <li class="neighbor-menu-item"><a href="Subscription.aspx">
                <%= Resource.Admin_MasterPageAdmin_SubscribeList%></a></li>
            <li class="neighbor-menu-item"><a href="Managers.aspx">
                <%= Resource.Admin_MasterPageAdmin_Managers%></a></li>
            <li class="neighbor-menu-item"><a href="Departments.aspx">
                <%= Resource.Admin_MasterPageAdmin_Departments%></a></li>
            <li class="neighbor-menu-item"><a href="ManagersTasks.aspx">
                <%= Resource.Admin_MasterPageAdmin_ManagersTasks%></a></li>
        </menu>
        <div class="panel-add">
            <a href="CreateCustomer.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>
        </div>
    </div>
    <center>
        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False" />
    </center>
    <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
        <tbody>
            <tr>
                <td style="width: 10px;">
                    <div style="width: 10px">
                    </div>
                </td>
                <td style="vertical-align: top; width: 100%">
                    <div style="width: 800px; font-size: 0px;">
                    </div>
                    <table style="width: 100%; table-layout: fixed;" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td style="width: 72px;">
                                    <img src="images/customers_ico.gif" alt="" />
                                </td>
                                <td>
                                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Header %>" /><br />
                                    <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" />
                                </td>
                                <td>
                                    <div style="float: right; padding-right: 10px;">
                                        <a <%= "href=\"viewcustomer.aspx?customerid=" + customer.Id + "\"" %> class="btn btn-middle btn-action"><%= Resource.Admin_BackToBrowse %></a>
                                        <asp:Button CssClass="btn btn-middle btn-add" ID="btnChangeCommonInfo" runat="server" Text="<%$ Resources:Resource, Admin_Update %>" ValidationGroup="0" Visible="true" OnClick="btnChangeCommonInfo_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; width: 100%" colspan="3">
                                    <table id="tabs">
                                        <tr>
                                            <td style="width: 225px;">
                                                <ul id="tabs-headers">
                                                    <li id="common">
                                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CommonInfo %>" />
                                                        <img id="itab1floppy" class="floppy" src="images/floppy.gif" />
                                                    </li>
                                                    <li id="address">
                                                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_AddressBook%>" />
                                                    </li>
                                                    <li id="history">
                                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_History  %>" />
                                                        <asp:Image ID="imgExcl2" ImageUrl="images/excl.gif" runat="server" Visible="false" CssClass="exclamation" />
                                                        <img id="itab2floppy" class="floppy" src="images/floppy.gif" alt="" />
                                                    </li>
                                                    <% if (ShowRoleAccess)
                                                       { %>
                                                    <li id="roles">
                                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Roles %>" />
                                                    </li>
                                                    <% } %>
                                                </ul>
                                                <input type="hidden" runat="server" id="tabid" class="tabid" />
                                            </td>
                                            <td id="tabs-contents">
                                                <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red" Visible="False"
                                                    Font-Names="Verdana" Font-Size="14px" EnableViewState="false" />
                                                <div class="tab-content">
                                                    <asp:UpdatePanel ID="upCommonInfo" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnChangeCommonInfo" runat="server" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:MultiView ID="mvCommonInfo" runat="server" ActiveViewIndex="0">
                                                                <asp:View ID="vCommonInfo" runat="server">
                                                                    <table class="tabsformtable" cellpadding="2" cellspacing="2">
                                                                        <tr>
                                                                            <td class="formheader" colspan="2">
                                                                                <h4 style="display: inline; font-size: 10pt;">
                                                                                    <%=Resources.Resource.Admin_ViewCustomer_CommonInfo%>
                                                                                </h4>
                                                                                <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="Blue" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formheaderfooter">
                                                                            <td style="width: 150px; height: 20px;"></td>
                                                                            <td style="height: 20px;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Email %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtEmail" runat="server" Width="200px" />
                                                                                (<asp:LinkButton ID="llbChangePassword" CssClass="Link" OnClick="llbChangePassword_Click" runat="server"
                                                                                    Text="<%$ Resources:Resource, Admin_ViewCustomer_ChangePasswordWithDots %>"></asp:LinkButton>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_RegistrationDate %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblRegistrationDate" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_LastName %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtLastName" runat="server" Width="200" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Name %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFirstName" runat="server" Width="200" />
                                                                                <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                                                                                    ErrorMessage="*" />
                                                                            </td>
                                                                        </tr>
                                                                         <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Patronymic %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPatronymic" runat="server" Width="200" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Phone %>"  />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPhone" runat="server" Width="200"  CssClass="phoneSource"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_StandardPhone %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtStandardPhone" runat="server" Width="200" CssClass="phoneDest" />
                                                                                <asp:RangeValidator runat="server" ControlToValidate="txtStandardPhone" MinimumValue="0" MaximumValue="999999999999"
                                                                                    ErrorMessage="<%$ Resources:Resource, Admin_ViewCustomer_MustBeANumber %>" Display="Dynamic"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Subscribed4News %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkSubscribed4News" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerGroup %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlCustomerGroup" runat="server" Style="width: 200px;">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trCustomerRole" runat="server">
                                                                            <td>
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerRole %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label runat="server" ID="lRole"></asp:Label>
                                                                                <asp:DropDownList ID="ddlCustomerRole" runat="server" Style="width: 200px;">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_AdminComment %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAdminComment" runat="server" CssClass="niceTextArea textArea3Lines" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                    <div id="divBonusCard" runat="server" visible="False">
                                                                        <table class="tabsformtable" cellpadding="2" cellspacing="2">
                                                                            <tr>
                                                                                <td class="formheader" colspan="2" style="width: 150px">
                                                                                    <h4 style="display: inline; font-size: 10pt;">
                                                                                        <%= Resource.Admin_ViewCustomer_BonusCard%>
                                                                                    </h4>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="formheaderfooter">
                                                                                <td style="width: 150px; height: 20px;"></td>
                                                                                <td style="height: 20px;"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_BonusCardNumber %>" />:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtBonusCardNumber" runat="server" Width="200px" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trBonusAmount" runat="server" visible="False">
                                                                                <td>
                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_BonusCardAmount %>" />:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblBonusCardAmount" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </asp:View>
                                                                <asp:View ID="vChangePassword" runat="server">
                                                                    <br />
                                                                    <ul id="ulUserRegistarionValidation" runat="server" visible="false" class="ulValidFaild" style="text-align: left; color: Red;">
                                                                        <li>Error1</li>
                                                                        <li>Error2</li>
                                                                    </ul>
                                                                    <table class="tabsformtable" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td class="first">
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_NewPassword %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />&nbsp;
                                                                                <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword"
                                                                                    ErrorMessage="*" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="first">
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_NewPasswordConfirm %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password" />&nbsp;
                                                                                <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNewPasswordConfirm"
                                                                                    ErrorMessage="*" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <br />
                                                                    <center>
                                                                            <asp:Button ID="btnChangePassword" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ChangePassword %>"
                                                                                OnClick="btnChangePassword_Click" />
                                                                        </center>
                                                                </asp:View>
                                                            </asp:MultiView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="tab-content">
                                                    <adv:CustomerAddressBookAdmin ID="CustomerAddressBookAdmin" runat="server" />
                                                </div>
                                                <div class="tab-content">
                                                    <adv:CustomerOrderHistoryAdmin ID="CustomerOrderHistoryAdmin" runat="server" />
                                                </div>
                                                <% if (ShowRoleAccess)
                                                   { %>
                                                <div class="tab-content">
                                                    <adv:CustomerRoleActionsAdmin ID="CustomerRoleActionsAdmin" runat="server" />
                                                </div>
                                                <% } %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="width: 10px; background-color: White;">
                    <div style="width: 10px; height: 1px;">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            &nbsp; &nbsp;
            <asp:Image ID="imgUpdating" runat="server" AlternateText="<%$ Resources:Resource, Admin_Loading %>" ImageUrl="images/loading.gif" />
            <asp:Label ID="lblUpdating" runat="server" Text="<%$ Resources:Resource, Admin_Loading %>" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">

        function toggleRightPanel() {
            if ($.cookie("isVisibleRightPanel") == "true") {
                $("div:.rightPanel").hide("fast");
                $("div:.right_hide_rus").hide("fast");
                $("div:.right_show_rus").show("fast");
                $("div:.right_hide_en").hide("fast");
                $("div:.right_show_en").show("fast");
                $.cookie("isVisibleRightPanel", "false", { expires: 7 });
            } else {
                $("div:.rightPanel").show("fast");
                $("div:.right_show_rus").hide("fast");
                $("div:.right_hide_rus").show("fast");
                $("div:.right_show_en").hide("fast");
                $("div:.right_hide_en").show("fast");
                $.cookie("isVisibleRightPanel", "true", { expires: 7 });
            }
        }

        $(".phoneSource").blur(function() {
            if ($(".phoneDest").val() == "") {
                $(".phoneDest").val(convertPhone($(".phoneSource").val()));
            }
        });

    </script>
</asp:Content>
