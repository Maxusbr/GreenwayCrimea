<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.CustomerRoleActions" CodeBehind="CustomerRoleActionsAdmin.ascx.cs" %>

<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Saas" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>

<asp:UpdatePanel ID="upAccessSettings" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="rolestb" cellpadding="0" cellspacing="0" style="width: 98%">
            <tr>
                <td class="formheader" colspan="2">
                    <h4 style="display: inline; font-size: 10pt;">
                        <%=Resources.Resource.Admin_ViewCustomer_Roles %>
                    </h4>
                </td>
            </tr>
            <tr class="formheaderfooter">
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>
                    <asp:Repeater ID="rprAccessSettigs" runat="server">
                        <HeaderTemplate>
                            <table>
                                <tr>
                                    <td></td>
                                    <td><a id="selAllRoles" href="#"><%=Resources.Resource.Admin_ViewCustomer_SelectAllRoles%></a></td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%--<%# RenderCategory(Eval("Category").ToString()) %>--%>
                            <tr>
                                <td>
                                    <asp:Label ID="lblName" runat="server" Text='<%# ((AdvantShop.Customers.RoleAction)Eval("Key")).Localize() %>' />
                                    <asp:HiddenField ID="hfRoleActionKey" runat="server" Value='<%# Eval("Key") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkRoleAction" runat="server" Checked='<%# Eval("Enabled") %>' CssClass="cbroles" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    <div id="divNotAvailableFeature" class="AdminSaasNotify" runat="server" visible="false">
                        <h2>
                            <%=  Resource.Admin_DemoMode_NotAvailableFeature%>
                        </h2>
                    </div>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            $(document).ready(function () {

                var check_status = false;
                $("#selAllRoles").click(function () {
                    if (check_status == false) {
                        $(".cbroles input[type='checkbox']").attr('checked', true);
                        $("#selAllRoles").text("<%=Resources.Resource.Admin_ViewCustomer_UnSelectAllRoles%>");
                    } else {
                        $(".cbroles input[type='checkbox']").attr('checked', false);
                        $("#selAllRoles").text("<%=Resources.Resource.Admin_ViewCustomer_SelectAllRoles%>");
                    }
                    check_status = !check_status;
                    return false;
                });
            });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
