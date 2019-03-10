<%@ Page AutoEventWireup="true" Inherits="Admin.CreateCustomer" Language="C#"
    MasterPageFile="~/Admin/MasterPageAdmin.master" CodeBehind="CreateCustomer.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>

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
    <table style="width: 100%;">
        <tr>
            <td style="text-align: center;" class="AdminHead">
                <%= Resource.Admin_CreateCustomer_Header %>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <div style="text-align: center;">
        <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="Blue"></asp:Label>
        <ul id="ulUserRegistarionValidation" runat="server" visible="false" class="ulValidFaild">
        </ul>
    </div>
    <br />
    <table border="0" width="100%" cellspacing="0" cellpadding="2">
        <tr>
            <td class="td_property_alt" style="width: 50%;">
                <b>
                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Email %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2_alt">
                <asp:TextBox ID="txtEmail" runat="server" Width="155"></asp:TextBox>
                <span id="validEmail" style="color: grey;">*</span>
            </td>
        </tr>
        <tr>
            <td class="td_property" style="width: 50%;">
                <b>
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_NewPassword %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2" style="height: 25px">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="155"></asp:TextBox>
                <span id="validPassword" style="color: grey;">*</span>
            </td>
        </tr>
        <tr>
            <td class="td_property_alt" style="width: 50%;">
                <b>
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_NewPasswordConfirm %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2_alt" style="height: 25px">
                <asp:TextBox ID="txtPasswordConfirm" runat="server" TextMode="Password" Width="155"></asp:TextBox>
                <span id="validPasswordConfirm" style="color: grey;">*</span>
            </td>
        </tr>
        <tr>
            <td class="td_property" style="width: 50%;">
                <b>
                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Name %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2" style="vertical-align: top;">
                <asp:TextBox ID="txtFirstName" runat="server" Width="155"></asp:TextBox>
                <span id="validFirstName" style="color: grey;">*</span>
            </td>
        </tr>
        <tr>
            <td class="td_property_alt" style="width: 50%;">
                <b>
                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_LastName %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2_alt" style="height: 25px">
                <asp:TextBox ID="txtLastName" runat="server" Width="155"></asp:TextBox>
                <span id="validLastName" style="color: grey;">*</span>
            </td>
        </tr>
        <% if (SettingsCheckout.IsShowPatronymic)
           { %>
        <tr>
            <td class="td_property_alt" style="width: 50%;">
                <b>
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Patronymic %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2_alt" style="height: 25px">
                <asp:TextBox ID="txtPatronymic" runat="server" Width="155"></asp:TextBox>
            </td>
        </tr>
        <% } %>
        <tr>
            <td class="td_property" style="width: 50%;">
                <b>
                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_WWW %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2">
                <asp:TextBox ID="txtPhone" runat="server" Width="155"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td_property_alt" style="width: 50%;">
                <b>
                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Subscribed4News %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2_alt">
                <asp:CheckBox ID="chkSubscribed4News" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td_property_alt" style="width: 50%;">
                <b>
                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerGroup %>" />
                </b>
            </td>
            <td class="td_property2_alt">
                <asp:DropDownList ID="ddlCustomerGroup" runat="server" Style="width: 200px;">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="userRoleTr" runat="server">
            <td class="td_property" style="width: 50%;">
                <b>
                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerRole %>"></asp:Label>
                </b>
                <br />
            </td>
            <td class="td_property2">
                <asp:DropDownList ID="ddlCustomerRole" runat="server" Style="width: 155px;">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <div style="text-align: center;">
        <asp:Button ID="btnChangeCommonInfo" runat="server" Text="<%$ Resources:Resource, Admin_Update %>" OnClientClick="return ValidateData();"
            OnClick="btnChangeCommonInfo_Click" />
    </div>
    <br />
    <br />
    <div style="text-align: center;">
        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label><br />
    </div>
    <br />
    <script type="text/javascript">
        function ValidateData() {
            var valid = true;

            if ($("#<%= txtPassword.ClientID %>").val() == "") {
                $("#validPassword").css("color", "#FF0000");
                valid = false;
            }
            else {
                $("#validPassword").css("color", "#C0C0C0");
            }

            if ($("#<%= txtPasswordConfirm.ClientID %>").val() == "") {
                $("#validPasswordConfirm").css("color", "#FF0000");
                valid = false;
            }
            else {
                $("#validPasswordConfirm").css("color", "#C0C0C0");
            }

            if ($("#<%= txtPasswordConfirm.ClientID %>").val() != $("#<%= txtPassword.ClientID %>").val()) {
                $("#validPasswordConfirm").css("color", "#FF0000");
                valid = false;
            }

            if ($("#<%= txtFirstName.ClientID %>").val() == "") {
                $("#validFirstName").css("color", "#FF0000");
                valid = false;
            }
            else {
                $("#validFirstName").css("color", "#C0C0C0");
            }

            if ($("#<%= txtLastName.ClientID %>").val() == "") {
                $("#validLastName").css("color", "#FF0000");
                valid = false;
            }
            else {
                $("#validLastName").css("color", "#C0C0C0");
            }

            if ($("#<%= txtEmail.ClientID %>").val() == "") {
                $("#validEmail").css("color", "#FF0000");
                valid = false;
            }
            else {
                $("#validEmail").css("color", "#C0C0C0");
            }
            if (!$("#<%= txtEmail.ClientID %>").val().match(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/)) {
                $("#validEmail").css("color", "#FF0000");
                valid = false;
            }
            else {
                $("#validEmail").css("color", "#C0C0C0");
            }

            return valid;
        }

    </script>
</asp:Content>
