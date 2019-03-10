<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Certificates" Codebehind="Certificates.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function setupAdvantGrid() {
            $(".imgtooltip").tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    var imagePath = $(this).attr("abbr");
                    if (imagePath.length == 0) {
                        return "<div><span><%= Resource.Admin_Catalog_NoMiniPicture %></span></div>";
                    }
                    else {
                        return $("<img/>").attr("src", imagePath);
                    }
                }
            });
            $(".showtooltip").tooltip({
                showURL: false
            });
        });

        function CreateHistory(hist) {
            $.historyLoad(hist);
        }

        var timeOut;
        function Darken() {
            timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
        }

        function Clear() {
            clearTimeout(timeOut);
            document.getElementById("inprogress").style.display = "none";

            $("input.sel").each(function (i) {
                if (this.checked) $(this).parent().parent().addClass("selectedrow");
            });

            initgrid();
        }

        $(document).ready(function () {
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            $("ineditcategory").tooltip();
        })

        $(document).ready(function () {
            $("#commandButton").click(function () {
                var command = $("#commandSelect").val();

                switch (command) {
                    case "selectAll":
                        SelectAll(true);
                        break;
                    case "unselectAll":
                        SelectAll(false);
                        break;
                    case "selectVisible":
                        SelectVisible(true);
                        break;
                    case "unselectVisible":
                        SelectVisible(false);
                        break;
                    case "deleteSelected":
                        var r = confirm("<%= Resource.Admin_Certificates_Confirm%>");
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;
        </div>
        <div class="loader">
            <table style="width: 100%; font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Discount_PriceRange.aspx">
                <%= Resource.Admin_MasterPageAdmin_DiscountMethods%></a></li>
            <li class="neighbor-menu-item"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item selected"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Voting.aspx">
                <%= Resource.Admin_MasterPageAdmin_Voting%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Voting.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_Voting %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="VotingHistory.aspx"><%= Resources.Resource.Admin_MasterPageAdmin_VotingHistory %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item"><a href="Statistics.aspx">
                <%= Resources.Resource.Admin_Statistics_Header %></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerate.aspx">
                <%= Resource.Admin_SiteMapGenerate_Header%></a></li>
            <li class="neighbor-menu-item"><a href="SiteMapGenerateXML.aspx">
                <%= Resource.Admin_SiteMapGenerateXML_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td style="vertical-align:top;">
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_CertificateAdmin_Header %>"></asp:Label><br />
                        <div style="display: block; margin-top:0px">
                            <%=Resources.Resource.Admin_Discount_Certificates_CustomerGroupMessage%>
                        </div>
                        <asp:Label ID="lblCertificateAttention" runat="server" Visible="False" ForeColor="red" />
                    </td>
                    <td style="text-align:right; vertical-align:bottom;">
                        <asp:Button CssClass="btn btn-middle btn-action" ID="btnSettings" runat="server" style="margin-right:5px;"
                            Text="<%$ Resources:Resource, Admin_Certificates_Settings %>" OnClick="btnSettings_Click" />
                        <asp:Button CssClass="btn btn-middle btn-add" ID="btnAdd" runat="server"
                            Text="<%$ Resources:Resource, Admin_HeadCmdInsert %>" OnClientClick="javascript:window.open('../giftcertificate');" />
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width:100%; margin-top:10px;">
            <div>
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block">
                                <select id="commandSelect">
                                    <option value="selectAll">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                    </option>
                                    <option value="unselectAll">
                                        <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                    </option>
                                    <option value="selectVisible">
                                        <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                    </option>
                                    <option value="unselectVisible">
                                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                    </option>
                                    <option value="deleteSelected">
                                        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                    </option>
                                </select>
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span>
                            </span>
                            <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <%=Resource.Admin_Catalog_Total%>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resource.Admin_Catalog_RecordsFound%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;"></td>
                    </tr>
                </table>
                <div>
                    <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                        <table class="filter" cellpadding="0" cellspacing="0">
                            <tr style="height: 5px;">
                                <td colspan="7"></td>
                            </tr>
                            <tr>
                                <td style="width: 60px; text-align: center;">
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="60">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 140px; text-align: center;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtCertificateCode" runat="server" TabIndex="12"
                                        Width="120px" />
                                </td>
                                <td style="width: 210px; text-align: center;">
                                    <div style="width: 200px; height: 0px; font-size: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtOrderNumber" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 210px; text-align: center;">
                                    <div style="width: 200px; height: 0px; font-size: 0px;">
                                    </div>
                                    <%--<asp:TextBox CssClass="filtertxtbox" ID="TextBox1" Width="220" runat="server"
                                        TabIndex="12" />--%>
                                </td>
                                <td style="width: 190px; text-align: left;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSumFilter" Width="160px" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 85px; text-align: left;">
                                    <div style="width: 85px; height: 0px; font-size: 0px;">
                                    </div>
                                    <asp:DropDownList ID="ddlPaidFilter" TabIndex="18" CssClass="dropdownselect" runat="server">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="any" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 110px; text-align: left;">
                                    <asp:DropDownList ID="ddlEnabledFilter" TabIndex="18" CssClass="dropdownselect" runat="server">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="any" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 140px; text-align: left;"></td>
                                <td style="width: 230px; text-align: center;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtCreationDate" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 95px;">
                                    <div style="text-align: center;">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                            TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="7"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>"
                                CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                                OnSorting="grid_Sorting" ShowFooter="false" ShowFooterWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 60px; height: 0px; font-size: 0px">
                                            </div>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="CertificateCode" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                        <HeaderTemplate>
                                            <div style="width: 99%; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbCertificateCode" runat="server" CommandName="Sort" CommandArgument="CertificateCode">
                                                <%=Resources.Resource.Admin_CertificateAdmin_Code%>
                                                <asp:Image ID="arrowCertificateCode" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblCertificateCode" runat="server" Text='<%# Eval("CertificateCode") %>'
                                                Width="99%"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="OrderId" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <HeaderTemplate>
                                            <div style="width: 200px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbOrderID" runat="server" CommandName="Sort" CommandArgument="OrderId">
                                                <%=Resources.Resource.Admin_CertificateAdmin_OrderNumber_Purchased%>
                                                <asp:Image ID="arrowOrderId" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:HyperLink ID="lnkOrder" runat="server" Target="_blank" NavigateUrl='<%# "EditOrder.aspx?OrderID="+ Eval("OrderID") %>'><%# Eval("OrderID")%></asp:HyperLink>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ApplyOrderNumber" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <HeaderTemplate>
                                            <div style="width: 200px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbOrderNumber" runat="server" CommandName="Sort" CommandArgument="ApplyOrderNumber">
                                                <%=Resources.Resource.Admin_CertificateAdmin_OrderNumber_Paid%>
                                                <asp:Image ID="arrowApplyOrderNumber" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:HyperLink ID="lnkApplyOrder" runat="server" Target="_blank" NavigateUrl='<%# !string.IsNullOrEmpty(Eval("ApplyOrderNumber").ToString()) ? "EditOrder.aspx?OrderID=" + OrderService.GetOrderIdByNumber(Convert.ToString( Eval("ApplyOrderNumber"))) : string.Empty %>'><%# Eval("ApplyOrderNumber")%></asp:HyperLink>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Sum" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbSum" runat="server" CommandName="Sort" CommandArgument="Sum">
                                                <%=Resources.Resource.Admin_CertificateAdmin_Sum%>
                                                <asp:Image ID="arrowSum" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lSum" runat="server" Text='<%# PriceFormatService.FormatPrice(SQLDataHelper.GetFloat(Eval("Sum")), SQLDataHelper.GetFloat(Eval("CurrencyValue")),SQLDataHelper.GetString(Eval("CurrencySymbol")), SQLDataHelper.GetString(Eval("CurrencyCode")), SQLDataHelper.GetBoolean(Eval("IsCodeBefore"))) %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Paid" ItemStyle-Width="85" HeaderStyle-Width="85"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="width: 85px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbPaid" runat="server" CommandName="Sort" CommandArgument="PaymentDate">
                                                <%= Resource.Admin_CertificateAdmin_Paid%>
                                                <asp:Image ID="arrowPaid" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkPaid" runat="server" Checked='<%# Eval("PaymentDate") != DBNull.Value %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Enable" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbEnable" runat="server" CommandName="Sort" CommandArgument="Enable">
                                                <%= Resource.Admin_CertificateAdmin_Available%>
                                                <asp:Image ID="arrowEnable" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEnable" runat="server" Checked='<%# Eval("Enable") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Used" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbUsed" runat="server" CommandName="Sort" CommandArgument="Used">
                                                <%= Resource.Admin_CertificateAdmin_Used%>
                                                <asp:Image ID="arrowUsed" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkUsed" runat="server" Checked='<%# Eval("Used") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="CreationDate" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbCreationDate" runat="server" CommandName="Sort" CommandArgument="CreationDate">
                                                <%= Resource.Admin_CertificateAdmin_CreationDate%>
                                                <asp:Image ID="arrowCreationDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblCreationDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertShortDate((DateTime)Eval("CreationDate")) %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="95px" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <%# "<a href=\"javascript:open_window('m_Certificate.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("ID").ToString())) + "',750,600);\" class='editbtn showtooltip' title=" + Resource.Admin_MasterPageAdminCatalog_Edit + "><img src='images/editbtn.gif' style='border: none;' /></a>"%>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title="<%= Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                            <a href="#" onclick="javascript:open_printable_version('../giftcertificate/print?code=<%# Eval("CertificateCode") %>');return false;">
                                                <img src="images/printer.png" alt="<%=Resource.Admin_ViewOrder_PrintOrder%>" style="border: none;" /></a>
                                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                CommandName="DeleteCertificate" CommandArgument='<%# Eval("ID")%>' data-confirm="<%$ Resources:Resource, Admin_Certificate_Confirmation %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <asp:ImageButton ID="buttonSendMail" runat="server" ImageUrl="images/send_mail.png" CssClass="valid-confirm" data-confirm="<%$ Resources:Resource, Admin_Certificate_SendEmailConfirmation %>"
                                                CommandName="SendCertificate" CommandArgument='<%# Eval("ID")%>' ToolTip='<%$ Resources:Resource, Admin_CertificateAdmin_SendEmail %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                        <%= Resource.Admin_Catalog_NoRecords%>
                                    </div>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%= Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                            runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                            UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                    </td>
                                    <td style="width: 157px; text-align: right; padding-right: 12px">
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                            <span style="color: #494949">
                                                <%=Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                    Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                OnClick="linkGO_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <input type="hidden" id="SelectedIds" name="SelectedIds" />
            </div>
            <div class="dvSubHelp">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/kupons-i-podaroxhnie-certifikaty" target="_blank">Инструкция. Купоны и подарочные сертификаты</a>
            </div>
        </div>
    </div>
</asp:Content>
