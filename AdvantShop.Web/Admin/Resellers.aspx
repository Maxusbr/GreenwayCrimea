<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Resellers" CodeBehind="Resellers.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
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
                        var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });

    </script>
    <style type="text/css">
        .style1 {
            height: 24px;
        }

        .style2 {
            width: 8px;
            height: 24px;
        }
    </style>
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
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
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
            <li class="neighbor-menu-item"><a href="Certificates.aspx">
                <%= Resource.Admin_MasterPageAdmin_Certificate%></a></li>
            <li class="neighbor-menu-item"><a href="SendMessage.aspx">
                <%= Resource.Admin_MasterPageAdmin_SendMessage%></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=YandexMarket">
                <%= Resources.Resource.Admin_MasterPageAdmin_YandexMarket %></a></li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx?ModuleId=GoogleBase">
                <%= Resources.Resource.Admin_MasterPageAdmin_GoogleBase %></a></li>
            <li class="neighbor-menu-item selected"><a href="Resellers.aspx">
                <%= Resources.Resource.Admin_MasterPageAdmin_Resellers %></a></li>
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
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Resellers_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Resellers_SubHeader %>"></asp:Label>
                    </td>
                    <td style="vertical-align: bottom; padding-right: 10px">
                        <div style="float: right; padding-right: 10px">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddResellers" runat="server" Text="<%$ Resources:Resource, Admin_Resellers_Add %>" ValidationGroup="0" OnClick="btnAddResellers_Click" />
                            <asp:LinkButton ID="LinkButton1" CssClass="btn btn-middle btn-add" runat="server" Text="<%$ Resources:Resource, Admin_Resellers_RefreshExportFiles %>" OnClick="OnClick"></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width: 100%">
            <table style="width: 99%;" class="massaction">
                <tr>
                    <td class="style1">
                        <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                        </span><span style="display: inline-block;">
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
                        </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span>
                        </span>
                    </td>
                    <td class="style1" style="text-align: right;">
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
                    <td class="style2"></td>
                </tr>
            </table>
            <div>
                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                    <table class="filter" cellpadding="2" cellspacing="0">
                        <tr style="height: 5px;">
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td style="width: 70px; text-align: center;">
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="65">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div style="width: 200px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="99%" runat="server" TabIndex="12" />
                            </td>
                            <td></td>
                            <td style="width: 90px;">
                                <div style="width: 90px; font-size: 0px; height: 0px;">
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="3"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddResellers" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <adv:AdvGridView ID="advGridResellers" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                            CellPadding="2" CellSpacing="0" CssClass="tableview" Style="cursor: pointer"
                            DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                            EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting"
                            OnRowDeleting="grid_RowDeleting" OnRowDataBound="grid_RowDataBound" ShowHeaderWhenEmpty="true" ShowFooter="False" ReadOnlyGrid="True">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="Label02" runat="server" Text='0'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="width: 40px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Id") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 150px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                            <%=Resource.Admin_Resellers_Name%>
                                            <asp:Image ID="arrowResellersName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'
                                            Width="99%"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewName" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ResellerID" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="270px">
                                    <HeaderTemplate>
                                        <div style="width: 270px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbResellerID" runat="server" CommandName="Sort" CommandArgument="ResellerID">
                                            <%=Resource.Admin_Resellers_ID%>
                                            <asp:Image ID="arrowResellerID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lResellerID" runat="server" Text='' Width="99%"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Files" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="300px">
                                    <HeaderTemplate>
                                        <div style="width: 300px; font-size: 0px; height: 0px;">
                                        </div>
                                        <%= Resource.Admin_Resellers_Files%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFile" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="center"
                                    FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# "<a href=\"exportfeedprogress.aspx?type=reseller&feedid=" + Eval("Id") + "\" >Обновить файл</a>"%>
                                        <%# "<a href=\"exportfeed.aspx?type=reseller&feedid=" + Eval("Id") + "\" title=" + Resources.Resource.Admin_MasterPageAdminCatalog_Edit + "><img src='images/editbtn.gif' style='border: none;' /></a>"%>
                                        <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                            CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteReseller" CommandArgument='<%# Eval("ID")%>'
                                            data-confirm="<%$ Resources:Resource, Admin_Resellers_Confirmation%>"
                                            ToolTip='<%$ Resources:Resource, Admin_Delete %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibAddCity" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddReseller"
                                            ToolTip="<%$ Resources:Resource, Admin_Cities_Add %>" />
                                        <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                            CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Resellers_CancelAdd %>" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <FooterStyle BackColor="#ccffcc" />
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resource.Admin_Resellers_NoRecords%>
                                </div>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                        <table class="results2" runat="server" visible="false">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <%=Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                        runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem Selected="True">20</asp:ListItem>
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
    </div>
    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
    </script>
</asp:Content>
