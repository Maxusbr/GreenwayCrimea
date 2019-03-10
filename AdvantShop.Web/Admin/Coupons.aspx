<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Coupons" Codebehind="Coupons.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">

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
        });

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
                        var r = confirm("<%= Resources.Resource.Admin_Coupons_Confirm%>");
                        if (r) document.getElementById('<%=lbDeleteSelected.ClientID%>').click();
                        break;
                    case "setActive":
                        document.getElementById('<%=lbSetActive.ClientID%>').click();
                        break;
                    case "setDeactive":
                        document.getElementById('<%=lbSetDeactive.ClientID%>').click();
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
            <table width="100%" style="font-weight: bold; text-align: center;">
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
            <li class="neighbor-menu-item selected"><a href="Coupons.aspx">
                <%= Resource.Admin_MasterPageAdmin_Coupons%></a></li>
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
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
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td style="vertical-align:top;">
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Coupons_Header %>"></asp:Label><br />
                            <div style="display:block; margin-top:0px">
                                <%= Resource.Admin_Discount_Coupons_CustomerGroupMessage%>
                            </div>
                        </td>
                        <td style="text-align:right; vertical-align:bottom;">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnAdd" runat="server" Text="<%$ Resources:Resource, Admin_HeadCmdInsert %>" OnClientClick="javascript:open_window('m_Coupon.aspx',750,600);return false;" />
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
                                        <option value="setActive">
                                            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"></asp:Localize>
                                        </option>
                                        <option value="setDeactive">
                                            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"></asp:Localize>
                                        </option>
                                    </select>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbDeleteSelected_Click" />
                                    <asp:LinkButton ID="lbSetActive" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"
                                        OnClick="lbSetActive_Click" />
                                    <asp:LinkButton ID="lbSetDeactive" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"
                                        OnClick="lbSetDeactive_Click" />
                                </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span>
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
                                        <%=Resources.Resource.Admin_Catalog_Total%>
                                        <span class="bold">
                                            <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resources.Resource.Admin_Catalog_RecordsFound%>
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
                                    <td colspan="8"></td>
                                </tr>
                                <tr>
                                    <td style="width: 60px; text-align: center;">
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="55">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: center;">
                                        <div style="width: 110px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtCode" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 210px; text-align: center;">
                                        <div style="width: 210px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:DropDownList runat="server" ID="ddlTypeFilter" Width="90%" DataSourceID="edsCouponType" DataTextField="LocalizedName"
                                            DataValueField="Value" OnDataBound="ddl_DataBound">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                        <div style="width: 100px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtValue" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 180px;">
                                        <div style="width: 180px; height: 0px; font-size: 0px;">
                                        </div>
                                    </td>
                                    <td style="width: 120px; text-align: center;">
                                        <div style="width: 120px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:DropDownList runat="server" ID="ddlEnabled" Width="90%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 180px;">
                                        <div style="width: 180px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtMinimalPrice" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 180px;"></td>
                                    <td style="width: 85px;">
                                        <div style="width: 85px; height: 0px; font-size: 0px;">
                                        </div>
                                        <div style="text-align: center;">
                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();" TabIndex="23"
                                                Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();" TabIndex="24"
                                                Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="8"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <adv:EnumDataSource runat="server" ID="edsCouponType" EnumTypeName="AdvantShop.Catalog.CouponType" />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="0"
                                    CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>" CssClass="tableview"
                                    Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting"
                                    ShowFooter="false" ShowFooterWhenEmpty="true" OnRowDataBound="grid_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 60px; height: 0px; font-size: 0px">
                                                </div>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Code" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 110px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderCode" runat="server" CommandName="Sort" CommandArgument="Code">
                                                    <%=Resources.Resource.Admin_Coupons_Code%>
                                                    <asp:Image ID="arrowCode" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCodeBind" runat="server" Text='<%# Eval("Code") %>' Width="99%" MaxLength="10"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Type" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="170">
                                            <HeaderTemplate>
                                                <div style="width: 170px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderType" runat="server" CommandName="Sort" CommandArgument="Type">
                                                    <%=Resources.Resource.Admin_Coupons_Type%>
                                                    <asp:Image ID="arrowType" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList runat="server" ID="ddlType" Width="90%" DataSourceID="edsCouponType" DataTextField="LocalizedName"
                                                    DataValueField="Value">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Value" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderValue" runat="server" CommandName="Sort" CommandArgument="Value">
                                                    <%=Resources.Resource.Admin_Coupons_Value%>
                                                    <asp:Image ID="arrowValue" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtValueBind" runat="server" Text='<%# SQLDataHelper.GetDecimal(Eval("Value")).ToString("F2") %>'
                                                    Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lValue" runat="server" Text='<%# SQLDataHelper.GetDecimal(Eval("Value")).ToString("F2")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ExpirationDate" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div style="width: 150px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderExpirationDate" runat="server" CommandName="Sort" CommandArgument="ExpirationDate">
                                                    <%=Resources.Resource.Admin_Coupon_ExpirationDate%>
                                                    <asp:Image ID="arrowExpirationDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lExpirationDate" runat="server" Text='<%# Eval("ExpirationDate") != DBNull.Value ? AdvantShop.Localization.Culture.ConvertShortDate((DateTime) Eval("ExpirationDate")) : Resources.Resource.Admin_m_Coupon_NoDate %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lExpirationDate2" runat="server" Text='<%# Eval("ExpirationDate") != DBNull.Value ? AdvantShop.Localization.Culture.ConvertShortDate((DateTime) Eval("ExpirationDate")) : Resources.Resource.Admin_m_Coupon_NoDate %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Uses" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="110px"
                                            ItemStyle-Width="110px">
                                            <HeaderTemplate>
                                                <%=Resources.Resource.Admin_Coupons_Uses%>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lUses" runat="server" Text='<%# Eval("ActualUses") + "/" + Eval("PossibleUses") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lUses2" runat="server" Text='<%# Eval("ActualUses") + "/" + Eval("PossibleUses") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Enabled" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 40px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                    <%=Resources.Resource.Admin_Coupons_Enabled%>
                                                    <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# SQLDataHelper.GetBoolean(Eval("Enabled"))  %>' />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEnabled2" runat="server" Checked='<%# Eval("Enabled") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="MinimalOrderPrice" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="180">
                                            <HeaderTemplate>
                                                <div style="width: 180px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderMinimalOrderPrice" runat="server" CommandName="Sort" CommandArgument="MinimalOrderPrice">
                                                    <%=Resources.Resource.Admin_Coupons_MinimalOrderPrice%>
                                                    <asp:Image ID="arrowMinimalOrderPrice" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMinimalOrderPrice" runat="server" Text='<%# SQLDataHelper.GetDecimal(Eval("MinimalOrderPrice")).ToString("F2") %>'
                                                    Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lMinimalOrderPrice" runat="server" Text='<%# SQLDataHelper.GetDecimal(Eval("MinimalOrderPrice")).ToString("F2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="AddingDate" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div style="width: 150px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderAddingDate" runat="server" CommandName="Sort" CommandArgument="AddingDate">
                                                    <%=Resources.Resource.Admin_Coupon_AddingDate%>
                                                    <asp:Image ID="arrowAddingDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lAddingDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertShortDate((DateTime) Eval("AddingDate")) %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lAddingDate2" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertShortDate((DateTime) Eval("AddingDate")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="85px" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 85px; height: 0px; font-size: 0px;">
                                                </div>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <%# "<a href=\"javascript:open_window('m_Coupon.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("ID").ToString())) + "',750,600);\" class='editbtn showtooltip' title=" + Resources.Resource.Admin_MasterPageAdminCatalog_Edit + "><img src='images/editbtn.gif' style='border: none;' /></a>"%>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image" src="images/updatebtn.png"
                                                    onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                                <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                    data-confirm="<%$ Resources: Resource, Admin_Coupons_Confirmation%>"
                                                    CommandName="DeleteCoupon" CommandArgument='<%# Eval("ID")%>' ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image" src="images/cancelbtn.png"
                                                    onclick="row_canceledit($(this).parent().parent()[0]); return false;" style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                            <%=Resources.Resource.Admin_Catalog_NoRecords%>
                                        </div>
                                    </EmptyDataTemplate>
                                </adv:AdvGridView>
                                <table class="results2">
                                    <tr>
                                        <td style="width: 157px; padding-left: 6px;">
                                            <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage" runat="server"
                                                OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>100</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7" UseHref="false"
                                                UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                        </td>
                                        <td style="width: 157px; text-align: right; padding-right: 12px">
                                            <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                                <span style="color: #494949">
                                                    <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server" Width="30" />
                                                </span>
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
            </div>
            <div class="dvSubHelp">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                <a href="http://www.advantshop.net/help/pages/kupons-i-podaroxhnie-certifikaty" target="_blank">Инструкция. Купоны и подарочные сертификаты</a>
            </div>
        </div>
    </div>
</asp:Content>
