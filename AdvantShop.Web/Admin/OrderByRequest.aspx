<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.OrderByRequest" CodeBehind="OrderByRequest.aspx.cs" %>

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
                    case "sendMailSelected":
                        var r = confirm("<%= Resources.Resource.Admin_OrderByRequest_SendMailConfirm%>");
                        if (r) __doPostBack('<%=lbSendMailSelected.UniqueID%>', '');
                        break;
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-own">
       
       
        <div class="content-top">
            <menu class="neighbor-menu neighbor-catalog">
                <li class="neighbor-menu-item"><a href="OrderSearch.aspx">
                    <%= Resource.Admin_MasterPageAdmin_Orders%></a></li>
                <li class="neighbor-menu-item"><a href="OrderStatuses.aspx">
                    <%= Resource.Admin_MasterPageAdmin_OrderStatuses%></a></li>
                <li class="neighbor-menu-item"><a href="OrderSources.aspx">
                    <%= Resource.Admin_MasterPageAdmin_OrderSources %></a></li>
                <li class="neighbor-menu-item selected"><a href="OrderByRequest.aspx">
                    <%= Resource.Admin_MasterPageAdmin_OrderByRequest%></a></li>
                <li class="neighbor-menu-item"><a href="StatisticsOrdersExportCsv.aspx">
                    <%= Resource.Admin_Statistics_ExportOrdersHeader%></a></li>
            </menu>
            <div class="panel-add">
                <a href="EditOrder.aspx?OrderID=addnew" class="panel-add-lnk">
                    <%= Resource.Admin_MasterPageAdmin_Add %>
                    <%= Resource.Admin_MasterPageAdmin_Order %></a>
            </div>
        </div>
        <div>
             <div id="inprogress" style="display: none;">
            <div id="curtain" class="opacitybackground">
                &nbsp;
            </div>
            <div class="loader">
                <table width="100%" style="font-weight: bold; text-align: center;">
                    <tbody>
                        <tr>
                            <td align="center">
                                <img src="images/ajax-loader.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="color: #0D76B8;">
                                <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_Header %>" /><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_SubHead %>" />
                        </td>
                        <td>
                            <div style="float: right; padding-right: 20px">
                                <div style="height: 41px" id="advButtonFiller" runat="server">
                                </div>
                                <div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 100%">
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td colspan="2">
                            <center>
                            <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                        </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                                    <option value="sendMailSelected">
                                        <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_SendMailSelected %>"></asp:Localize>
                                    </option>
                                </select>
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                                <asp:LinkButton ID="lbSendMailSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_OrderByRequest_SendMailSelected %>"
                                    OnClick="lbSendMailSelected_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span></span>
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
                        <table class="filter" cellpadding="2" cellspacing="0">
                            <tr style="height: 5px;">
                                <td colspan="9"></td>
                            </tr>
                            <tr>
                                <td style="width: 50px; text-align: center;">
                                    <div style="height: 0px; font-size: 0px; width: 50px">
                                    </div>
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="99%">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 60px;">
                                    <div style="height: 0px; font-size: 0px; width: 60px">
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 200px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtProductName" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtUserName" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 170px;">
                                    <div style="width: 170px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtEmail" Width="99%" runat="server" TabIndex="12" />
                                </td>
                                <td style="width: 170px;">
                                    <div style="width: 170px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtPhone" Width="99%" runat="server" TabIndex="12" />
                                </td>
                                <td style="width: 90px; text-align: center;">
                                    <asp:DropDownList ID="ddlIsComplete" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="90px">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 130px;">
                                    <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                        <tr>
                                            <td style="text-align: left;">
                                                <%=Resource.Admin_Catalog_From%>:
                                            </td>
                                            <td style="width: 110px;">
                                                <div class="dp">
                                                    <asp:TextBox CssClass="filtertxtbox" ID="txtDateFrom" Width="80" runat="server" TabIndex="12" />
                                                    <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                <%=Resource.Admin_Catalog_To%>:
                                            </td>
                                            <td style="width: 88px;">
                                                <div class="dp">
                                                    <asp:TextBox CssClass="filtertxtbox" ID="txtDateTo" Width="80" runat="server" TabIndex="12" />
                                                    <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                </div>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 80px;">
                                    <div style="width: 80px; font-size: 0px; height: 0px;">
                                    </div>
                                    <center>
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </center>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="5"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="grid" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="grid" EventName="Sorting" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                                CssClass="tableview" GridLines="None" EnableModelValidation="True" EnableEdit="False"
                                OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting">
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="45px" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 45px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="height: 0px; font-size: 0px; width: 60px">
                                            </div>
                                            <asp:LinkButton ID="lbOrderByRequestId" runat="server" CommandName="Sort" CommandArgument="ID">
                                                <%=Resources.Resource.Admin_OrderSearch_OrderNum%><asp:Image ID="arrowOrderByRequestID"
                                                    CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <a href='<%# "EditOrderByRequest.aspx?ID=" + Eval("ID") %>'>
                                                <asp:Literal ID="lOrderByRequestId" runat="server" Text='<%# Bind("ID") %>'></asp:Literal>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ProductName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="width: 200px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbProductName" runat="server" CommandName="Sort" CommandArgument="ProductName">
                                                <%=Resources.Resource.Admin_OrderByRequest_ProductName%>
                                                <asp:Image ID="arrowProductName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# GetProductLink(AdvantShop.Helpers.SQLDataHelper.GetInt(Eval("ProductId")), Eval("ProductName").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="UserName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <%=Resources.Resource.Admin_OrderByRequest_UserName%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lUserName" runat="server" Text='<%# Bind("UserName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Email" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="170px">
                                        <HeaderTemplate>
                                            <div style="width: 170px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                                <%=Resources.Resource.Admin_OrderByRequest_Email%>
                                                <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lEmail" runat="server" Text='<%# Bind("Email") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Phone" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="170px">
                                        <HeaderTemplate>
                                            <div style="width: 170px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbPhone" runat="server" CommandName="Sort" CommandArgument="Phone">
                                                <%=Resources.Resource.Admin_OrderByRequest_Phone%>
                                                <asp:Image ID="arrowPhone" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lPhone" runat="server" Text='<%# Bind("Phone") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="IsComplete" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 90px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbIsComplete" runat="server" CommandName="Sort" CommandArgument="IsComplete">
                                                <%=Resources.Resource.Admin_OrderByRequest_IsComplete %>
                                                <asp:Image ID="arrowIsComplete" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsComplete" runat="server" Checked='<%# Eval("IsComplete")%>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="RequestDate" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="150px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbRequestDate" runat="server" CommandName="Sort" CommandArgument="RequestDate">
                                                <% = Resources.Resource.Admin_OrderByRequest_RequestDate%>
                                                <asp:Image ID="arrowRequestDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("RequestDate"))%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="40px">
                                        <HeaderTemplate>
                                            <div style="width: 40px; font-size: 0; line-height: 0"></div>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <a href='<%# "EditOrderByRequest.aspx?ID=" + Eval("ID") %>' class="showtooltip" title="<%=Resources.Resource.Admin_OrderSearch_Edit%>">
                                                <img src="images/editbtn.gif" style="border: none;" /></a>
                                            <asp:LinkButton ID="buttonDelete" runat="server"
                                                CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteOrderByRequest" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_OrderByRequest_Confirmation %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#ccffcc" />
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_OrderByRequest_NoRecords%>
                                </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
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
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
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
            </div>
        </div>
        <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
        <script type="text/javascript">
            function setupTooltips() {
                $(".showtooltip").tooltip({
                    showURL: false
                });
            }
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
        </script>
    </div>
</asp:Content>
