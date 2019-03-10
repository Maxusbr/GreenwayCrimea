<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeBehind="ShippingMethod.aspx.cs" Inherits="Admin.EditShippingMethod" %>

<%@ Reference Control="~/Admin/UserControls/ShippingMethods/MasterControl.ascx" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/ShippingMethods/MasterControl.ascx" TagName="ShippingMethod"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(".ShippingType").hover(function () {
                $(this).addClass("ptHovered");
            },
                function () {
                    $(this).removeClass("ptHovered");
                });

            $.advModal({
                title: "<%= Resources.Resource.Admin_ShippingMethod_Adding %>",
                control: $("#btnAddMethod"),
                htmlContent: $("#<%= modal.ClientID%>"),
                beforeOpen: clearModal,
                clickOut: false
            });
        });

        function Ok() { }

        function clearModal() {

            $("#<%= txtName.ClientID %>").val("");
            $("#<%= txtDescription.ClientID %>").val("");
            $("#<%= txtSortOrder.ClientID %>").val("");
            $("#<%= txtName.ClientID %>").focus();
        }
        function showElement(span) {
            var method_id = $("input:hidden", $(span)).val();
            if ($("hfShipping" + method_id).length == 0)
                location = "ShippingMethod.aspx?ShippingMethodID=" + method_id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="divModal" style="display: none;">
        <asp:Panel ID="modal" runat="server">
            <div style="background-color: white; padding-top: 10px; padding-bottom: 10px; text-align: center;">
                <table width="430px;">
                    <tr>
                        <td style="width: 130px;vertical-align: top;">
                            <asp:Label ID="Label1" runat="server" Text="<%$  Resources:Resource, Admin_ShippingMethods_Name %>"></asp:Label><span class="required">*</span>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:TextBox runat="server" ID="txtName" Width="300" ValidationGroup="AddShipping"></asp:TextBox>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtName" EnableClientScript="true" Style="display: inline;" ValidationGroup="AddShipping"
                                ErrorMessage='<%$ Resources: Resource, Admin_ShippingMethod_NameRequired %>'></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethods_Type %>"></asp:Label><span
                                class="required">*</span>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:DropDownList runat="server" ID="ddlType" DataTextField="Text" DataValueField="Value"
                                Width="300px" ValidationGroup="AddShipping" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Description %>"></asp:Label>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="298" Height="100" ValidationGroup="AddShipping" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_SortOrder %>"></asp:Label>
                        </td>
                        <td style="padding: 3px 0">
                            <asp:TextBox runat="server" ID="txtSortOrder" Width="300" Text="0" ValidationGroup="AddShipping" />
                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1" ValidationGroup="AddShipping"
                                runat="server" ControlToValidate="txtSortOrder" EnableClientScript="true" ValidationExpression="[0-9]*"
                                ErrorMessage="<%$ Resources: Resource, Admin_SortOrder_MustBeNumeric %>"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right;padding:3px 0;">
                            <asp:LinkButton runat="server" ID="btnOk" Text="<%$ Resources: Resource, Admin_ShippingMethod_Create %>" CausesValidation="true" 
                                ValidationGroup="AddShipping" CssClass="btn btn-middle btn-action" OnClick="btnOk_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item selected"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item"><a href="Country.aspx">
                <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
            <li class="neighbor-menu-item"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
            <li class="neighbor-menu-item"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
            <li class="neighbor-menu-item"><a href="Localizations.aspx">
                <%= Resource.Admin_Localizations_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <table style="width: 100%; table-layout: fixed;" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/customers_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Header %>"></asp:Label><br />
                        <asp:Label ID="lblShippingMethod" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblMessage" Visible="false" ForeColor="Blue"></asp:Label>
                    </td>
                    <td>
                        <div class="btns-main">
                            <a id="btnAddMethod" class="btn btn-middle btn-add" href="javascript:void(0);"><%= Resources.Resource.Admin_Add %></a>
                            <%--<asp:LinkButton CssClass="btn btn-middle btn-add" ID="btnAddMethod" runat="server" CausesValidation="false"
                                Text="<%$ Resources:Resource, Admin_Add %>"  />--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 100%" colspan="4">
                        <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
                            <tr>
                                <td style="vertical-align: top; width: 225px;">
                                    <ul class="tabs" id="tabs-headers">
                                        <asp:Repeater runat="server" ID="rptTabs">
                                            <ItemTemplate>
                                                <li id="Li1" runat="server" onclick="javascript:showElement(this)" class='<%# (int)Eval("ShippingMethodID") == ShippingMethodID ? "selected" : "" %>'
                                                    style="">
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ShippingMethodID") %>' />
                                                    <asp:Label ForeColor='<%# (bool)Eval("Enabled") ? Color.Black : Color.Gray %>' ID="Literal4"
                                                        runat="server" Text='<%# Eval("Name") %>' />
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </td>
                                <td class="tabContainer" id="tabs-contents">
                                    <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red"
                                        Visible="False" Font-Names="Verdana" Font-Size="14px" EnableViewState="false"></asp:Label>
                                    <asp:Panel runat="server" ID="pnMethods">
                                        <asp:Panel runat="server" ID="pnEmpty" Visible="False">
                                            <span>
                                                <% = Resource.Admin_ShippingMethod_EmptyShipping %></span>
                                        </asp:Panel>
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucFedEx" ShippingType="FedEx" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucUsps" ShippingType="Usps" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucUPS" ShippingType="UPS" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucFixedRate" ShippingType="FixedRate" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucFreeShipping" ShippingType="FreeShipping" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucShippingByWeight" ShippingType="ShippingByWeight" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucEdost" ShippingType="Edost" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucShippingByShippingCost" ShippingType="ShippingByShippingCost" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucShippingByOrderPrice" ShippingType="ShippingByOrderPrice" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucShippingByRangeWeightAndDistance" ShippingType="ShippingByRangeWeightAndDistance" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucNovaPoshta" ShippingType="NovaPoshta" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucSelfDelivery" ShippingType="SelfDelivery" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucSdek" ShippingType="Sdek" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucMultiship" ShippingType="Multiship" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucShippingByProductAmount" ShippingType="ShippingByProductAmount" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucEmsPost" ShippingType="EmsPost" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucCheckoutRu" ShippingType="CheckoutRu" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucYandexDelivery" ShippingType="YandexDelivery" OnSaved="ShippingMethod_Saved" />
                                        <adv:ShippingMethod Visible="false" runat="server" ID="ucPointDelivery" ShippingType="PointDelivery" OnSaved="ShippingMethod_Saved" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
