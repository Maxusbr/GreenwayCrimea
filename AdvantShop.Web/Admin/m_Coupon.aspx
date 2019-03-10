<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin.m_Coupon"
    MasterPageFile="m_MasterPage.master" ValidateRequest="false" Codebehind="m_Coupon.aspx.cs" %>

<%@ Register Src="UserControls/PopupGridProductMultiSelect.ascx" TagName="PopupGridProductMultiSelect"
    TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="PopupTree" Src="~/Admin/UserControls/PopupTreeView.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="cphCenter">
    <asp:UpdatePanel runat="server" ID="updPanel">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="popTree" EventName="TreeNodeSelected" />
            <asp:AsyncPostBackTrigger ControlID="popTree" EventName="Hiding" />
            <asp:AsyncPostBackTrigger ControlID="PopupGridProductMultiSelect" />
            <asp:PostBackTrigger ControlID="btnOK" />
        </Triggers>
        <ContentTemplate>
            <adv:PopupTree runat="server" ID="popTree" OnTreeNodeSelected="popTree_Selected"
                Type="CategoryMultiSelect" HeaderText="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>" />
            <div>
                <div style="text-align: center;">
                    <asp:Label ID="lblHeader" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Header %>"></asp:Label>
                    <br />
                    <asp:Label ID="lblSubHeader" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_SubHeader %>"></asp:Label>
                </div>
                <asp:Panel ID="pnlAdd" runat="server" Height="84px" Width="100%">
                    <div style="text-align: center;">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;
                    </div>
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr style="background-color: #eff0f1;">
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Code %>" />&nbsp;
                            </td>
                            <td style="height: 27px; width: 45%; text-align: left;">
                                <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Type %>" />
                                &nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <adv:EnumDataSource ID="edsCouponType" runat="server" EnumTypeName="Advantshop.Catalog.CouponType">
                                </adv:EnumDataSource>
                                <asp:DropDownList ID="ddlCouponType" runat="server" DataSourceID="edsCouponType"
                                    DataTextField="LocalizedName" DataValueField="Value" Width="224px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="background-color: #eff0f1;">
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Value %>"></asp:Label>&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <asp:TextBox ID="txtValue" runat="server" Width="300px"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_m_Coupon_Required %>"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtValue"
                                    ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Value %>"></asp:Label>&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <asp:DropDownList runat="server" ID="ddlCurrency" DataTextField="Name" DataValueField="Iso3" />
                            </td>
                        </tr>
                        <tr id="trAddingDate" runat="server" visible="false">
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_AddingDate %>"></asp:Label>&nbsp;
                            </td>
                            <td style="width: 45%; height: 27px; text-align: left">
                                <asp:Label ID="lblAddingDate" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_AddingDate %>"></asp:Label>&nbsp;
                            </td>
                        </tr>
                        <tr style="background-color: #eff0f1;">
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_ExpirationDate %>"></asp:Label>&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:CheckBox type="checkbox" onclick="hideExpirationDate()" ID="chkExpirationDate"
                                                    runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_NoDate %>" />&nbsp;
                                            </td>
                                            <td style="width: 210px;" class="tdExpiration">
                                                <div class="dp">
                                                    <asp:TextBox ID="txtExpirationDate" runat="server" Width="150px" /> <asp:Image ID="popupDate" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_PossibleUses %>"></asp:Label>&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkPosibleUses" Text="<%$ Resources:Resource, Admin_m_Coupon_Unlimited %>"
                                                onclick="hidePosibleUses()" />&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPosibleUses" runat="server" Width="200px" CssClass="PosibleUses"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="background-color: #eff0f1;">
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Enabled%>" />&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <asp:CheckBox runat="server" ID="chkEnabled" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_MinimalOrderPrice %>"></asp:Label>&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <asp:TextBox ID="txtMinimalOrderPrice" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="background-color: #eff0f1;">
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Categories %>"></asp:Label>:&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <asp:Label runat="server" ID="lCategories"></asp:Label>
                                <asp:LinkButton ID="lbAddCategory" runat="server" OnClientClick="document.body.style.overflowX='hidden';_TreePostBack=true;removeunloadhandler();"
                                    OnClick="lbAddCategory_Click"><%=Resources.Resource.Admin_m_Coupon_Select%></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 45%; height: 27px; text-align: right">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Products %>"></asp:Label>:&nbsp;
                            </td>
                            <td style="vertical-align: middle; height: 27px;">
                                <asp:Label runat="server" ID="lProducts" CssClass="productsCount"></asp:Label>
                                <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="ShowModalPopupProduct();return false;"><%=Resources.Resource.Admin_m_Coupon_Select%></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <!-- editor -->
                    <br />
                    <div style="text-align: center;">
                        <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Resource, Admin_m_Coupon_Ok %>"
                            Width="110px" OnClick="btnOK_Click" />&nbsp;
                    </div>
                    <br />
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <adv:PopupGridProductMultiSelect runat="server" ID="PopupGridProductMultiSelect"
        OnGridSelected="grid_Selected" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphScript">
    <script type="text/javascript">
        function CreateHistory(hist) {
            $.historyLoad(hist);
        }

        var timeOut;
        function Darken() {
        }

        function Clear() {
            clearTimeout(timeOut);

            $("input.sel").each(function (i) {
                if (this.checked) $(this).parent().parent().addClass("selectedrow");
            });

            initgrid();
        }

        function removeunloadhandler(a) {
            window.onbeforeunload = null;
        }
        
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
        });

        $(document).ready(function () {
            hideExpirationDate();
            hidePosibleUses();
        });

        function hideExpirationDate() {
            if ($("#<% =chkExpirationDate.ClientID %>").is(":checked")) {
                $("#<% =txtExpirationDate.ClientID%>").val("");
                $(".tdExpiration").hide();
            } else {
                $(".tdExpiration").show();
            }
        }

        function hidePosibleUses() {
            if ($("#<% =chkPosibleUses.ClientID %>").is(":checked")) {
                $("#<% =txtPosibleUses.ClientID%>").val("0");
                $(".PosibleUses").hide();
            } else {
                $(".PosibleUses").show();
            }
        }

    </script>
</asp:Content>
