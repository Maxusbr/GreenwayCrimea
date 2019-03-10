<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/m_MasterPage.master" AutoEventWireup="true" CodeBehind="m_Manager.aspx.cs" Inherits="Admin.m_Manager" %>

<%@ Register Src="~/Admin/UserControls/PopupGridCustomers.ascx" TagName="PopupGridCustomers"
    TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .editlink input[type=radio]+label {
            cursor: pointer;
            color: blue;
            text-decoration: underline;
        }
        .hidden {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphCenter" runat="server">
    <adv:PopupGridCustomers ID="PopupGridCustomers" runat="server" />
    <div style="text-align: center;">
        <asp:Label ID="lblManager" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Manager_Header %>"></asp:Label>
    </div>
    <div style="height: 84px; width: 100%;">
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;
        </div>
        <asp:UpdatePanel ID="updPanel" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkCustomer"/>
            </Triggers>
            <ContentTemplate>
                <table class="info-center-tb">
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_m_Manager_Customer %>"></asp:Label>:
                        </td>
                        <td>
                            <asp:HiddenField runat="server" ID="hfCustomerId" />
                            <asp:LinkButton runat="server" ID="lnkCustomer" OnClick="lnkCustomer_Click"></asp:LinkButton>
                            <asp:Panel runat="server" ID="pnlChooseCustomer">
                                <asp:Label ID="lblCustomer" runat="server" />
                                <asp:RadioButton runat="server" ID="rbExistCustomer" GroupName="Customer" class="editlink" 
                                    Text="<%$ Resources:Resource, Admin_m_Manager_SelectCustomer %>" Checked="True" />
                                <br />
                                <asp:RadioButton runat="server" ID="rbNewCustomer" GroupName="Customer" Text="Создать нового" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr class="toggle hidden">
                        <td class="lpart">
                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Email %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="300px" ValidationGroup="NewCustomer"></asp:TextBox>
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_Required %>" CssClass="info-hint-text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="*" ValidationGroup="NewCustomer"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="toggle hidden">
                        <td class="lpart">
                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_EnterPassword%>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="300px" ValidationGroup="NewCustomer"></asp:TextBox>
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_Required %>" CssClass="info-hint-text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="*" ValidationGroup="NewCustomer"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="toggle hidden">
                        <td class="lpart">
                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_ConfirmPassword%>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPasswordConfirm" runat="server" TextMode="Password" Width="300px" ValidationGroup="NewCustomer"></asp:TextBox>
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_Required %>" CssClass="info-hint-text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPasswordConfirm"
                                ErrorMessage="*" ValidationGroup="NewCustomer"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="toggle hidden">
                        <td class="lpart">
                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_LastName %>"></asp:Label>
                        </td>
                        <td class="td_property2" style="height: 25px">
                            <asp:TextBox ID="txtLastName" runat="server" Width="300px" ValidationGroup="NewCustomer"></asp:TextBox>
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_Required %>" CssClass="info-hint-text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLastName"
                                ErrorMessage="*" ValidationGroup="NewCustomer"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="toggle hidden">
                        <td class="lpart">
                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Name %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" Width="300px" ValidationGroup="NewCustomer"></asp:TextBox>
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_Required %>" CssClass="info-hint-text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                                ErrorMessage="*" ValidationGroup="NewCustomer"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="toggle hidden">
                        <td class="lpart">
                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_WWW %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" Width="300px" ValidationGroup="NewCustomer"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_Manager_Position %>"></asp:Label>: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtPosition" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                            <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_Required %>" CssClass="info-hint-text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPosition"
                                ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, Admin_m_Manager_Department %>" />&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_Manager_Picture %>"></asp:Label>&nbsp;
                        </td>
                        <td>
                            <asp:Panel ID="pnlImage" runat="server" Width="100%">
                                &nbsp;<asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_m_Manager_CurrentImage %>"></asp:Label>
                                <br />
                                <asp:Image ID="imgManagerPhoto" runat="server" />
                                <br />
                                <asp:Button ID="btnDeleteImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                    OnClick="btnDeleteImage_Click" />
                                <br />
                            </asp:Panel>
                            <asp:FileUpload ID="fuManagerPhoto" runat="server" Width="308px" Height="20px" />
                            <asp:Label ID="lblImage" runat="server" Text="Label" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblImageInfo" runat="server" CssClass="info-hint-text"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_Activity %>" />&nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="ckbActive" runat="server" Checked="True" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div style="text-align: center;">
            <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Ok %>"
                Width="110px" OnClick="btnOK_Click" UseSubmitBehavior="false" />&nbsp;
        </div>
        <br />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        function HideModalUserPopup() {
            $find("ModalUserBehaviour").hide();
            $('select', 'object', 'embed').each(function () {
                $(this).show(); /*.style.visibility = 'visible'*/
            });
        }

        $("body").on("change", "#<%= rbNewCustomer.ClientID%>", function() {
            showHideRows($(this).is(":checked"));
        });
        $("body").on("change", "#<%= rbExistCustomer.ClientID%>", function () {
            showHideRows($(this).is(":not(:checked)"));
        });

        function showHideRows(show) {
            if(show)
                $("tr.toggle").removeClass("hidden");
            else
                $("tr.toggle").addClass("hidden");
        }

        $(document).ready(function () {
            showHideRows($("#<%= rbNewCustomer.ClientID%>").is(":checked"));
            $(".editlink").live("click", function () {
                ShowModalPopupCustomers();
            });
            $(".editcreate").live("click", function () {
                document.body.style.overflowX = 'hidden';
                $find('ModalUserBehaviour').show();
                document.getElementById('ModalUserBehaviour_backgroundElement').onclick = HideModalUserPopup;
            });
        });


    </script>
</asp:Content>
