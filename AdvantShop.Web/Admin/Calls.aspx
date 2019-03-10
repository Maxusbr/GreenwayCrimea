<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Calls" EnableEventValidation="false" CodeBehind="Calls.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="AdvantShop.Core.Services.IPTelephony" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected selected dropdown-menu-parent">
                <a href="Managers.aspx"><%= Resource.Admin_MasterPageAdmin_Managers%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Managers.aspx">
                            <%= Resource.Admin_MasterPageAdmin_Managers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Departments.aspx">
                            <%= Resource.Admin_MasterPageAdmin_Departments %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item">
                <a href="ManagersTasks.aspx"><%= Resource.Admin_MasterPageAdmin_ManagersTasks%></a>
            </li>
            <li class="neighbor-menu-item">
                <a href="Leads.aspx"><%= Resource.Admin_MasterPageAdmin_Leads%></a>
            </li>
            <li class="neighbor-menu-item">
                <a href="Calls.aspx"><%= Resource.Admin_MasterPageAdmin_Calls%></a>
            </li>
            <li class="neighbor-menu-item">
                <a href="Calls.aspx?Type=Missed"><%= Resource.Admin_MasterPageAdmin_MissedCalls%></a>
            </li>
        </menu>
        <div class="panel-add">
            <a href="ManagerTask.aspx" class="panel-add-lnk"><%= Resource.Admin_ManagersTasks_AddTask %></a>, <a href="EditLead.aspx" class="panel-add-lnk"><%= Resource.Admin_Leads_AddTop %></a>
        </div>
    </div>
    <div class="content-own">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
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
                                    <td style="text-align: center; color: #0D76B8;">
                                        <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div style="text-align: center;">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                            <tr>
                                <td style="width: 72px;">
                                    <img src="images/orders_ico.gif" alt="" />
                                </td>
                                <td>
                                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="Вызовы"></asp:Label><br />
                                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="Журнал вызовов"></asp:Label>
                                </td>
                                <td class="btns-main"></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div style="height: 10px;">
                </div>


                <div id="gridTable" runat="server" style="text-align: center;">
                    <table style="width: 99%;" class="massaction">
                        <tr>
                            <td>
                                <span class="admin_catalog_commandBlock">
                                    <span style="display: inline-block; margin-right: 3px;">
                                        <asp:Localize ID="Localize_Admin_Catalog_Command" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                                    </span>
                                    <span style="display: inline-block">
                                        <select id="commandSelect" onchange="selectCange()">
                                            <option value="selectAll">
                                                <asp:Localize ID="Localize_Admin_Catalog_SelectAll" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                            </option>
                                            <option value="unselectAll">
                                                <asp:Localize ID="Localize_Admin_Catalog_UnselectAll" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                            </option>
                                            <option value="selectVisible">
                                                <asp:Localize ID="Localize_Admin_Catalog_SelectVisible" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                            </option>
                                            <option value="unselectVisible">
                                                <asp:Localize ID="Localize_Admin_Catalog_UnselectVisible" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                            </option>
                                            <option value="deleteSelected">
                                                <asp:Localize ID="Localize_Admin_Catalog_DeleteSelected" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                            </option>
                                        </select>
                                        <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                        <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                            OnClick="lbDeleteSelected_Click" />
                                    </span>
                                    <span class="selecteditems" style="vertical-align: baseline">
                                        <span style="padding: 0px 3px 0px 3px;">|</span>
                                        <span id="selectedIdsCount" class="bold"><%= SelectedCount() %></span>
                                        <span>&nbsp;<%=Resource.Admin_Catalog_ItemsSelected%></span>
                                    </span>
                                </span>
                            </td>
                            <td style="text-align: right;" class="selecteditems">
                                <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Localize ID="Localize_Admin_Catalog_Total" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Total %>"></asp:Localize>
                                        <span class="bold">
                                            <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" />
                                        </span>
                                        <asp:Localize ID="Localize_Admin_Catalog_RecordsFound" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_RecordsFound %>"></asp:Localize>
                                        <br />
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
                                    <td style="width: 70px;">
                                        <div style="height: 0px; font-size: 0px; width: 70px">
                                        </div>
                                        <asp:DropDownList ID="ddlCallType" TabIndex="10" CssClass="dropdownselect" runat="server" Width="99%"
                                            DataSourceID="edsECallType" DataTextField="LocalizedName" DataValueField="Name"
                                            OnDataBound="ddlFilter_DataBound" />
                                    </td>
                                    <td style="width: 110px;" runat="server" id="tdCallHangupStatus" visible="False">
                                        <div style="height: 0px; font-size: 0px; width: 110px">
                                        </div>
                                        <asp:DropDownList ID="ddlCallHangupStatus" TabIndex="10" CssClass="dropdownselect" runat="server" Width="99%"
                                            DataSourceID="edsECallHangupStatus" DataTextField="LocalizedName" DataValueField="Name"
                                            OnDataBound="ddlFilter_DataBound" />
                                    </td>
                                    <td style="width: 90px;" runat="server" id="tdCalledBack" visible="False">
                                        <div style="height: 0px; font-size: 0px; width: 90px">
                                        </div>
                                        <asp:DropDownList ID="ddlCalledBack" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="99%">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px;">
                                        <div style="height: 0px; width: 120px; font-size: 0px;">
                                        </div>
                                        <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%=Resource.Admin_Catalog_From%>:
                                                </td>
                                                <td style="width: 95px;">
                                                    <div class="dp">
                                                        <asp:TextBox CssClass="filtertxtbox" ID="txtDateFrom" Width="70" runat="server" TabIndex="10" />
                                                        <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%=Resource.Admin_Catalog_To%>:
                                                </td>
                                                <td>
                                                    <div class="dp">
                                                        <asp:TextBox CssClass="filtertxtbox" ID="txtDateTo" Width="70" runat="server" TabIndex="10" />
                                                        <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <div style="font-size: 0; line-height: 0; width: 120px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtSrcNum" Width="99%" runat="server" TabIndex="10" />
                                    </td>
                                    <td>
                                        <div style="height: 0px; font-size: 0px; width: 120px">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtDstNum" Width="99%" runat="server" TabIndex="10" />
                                    </td>
                                    <td style="width: 110px;">
                                        <div style="height: 0px; font-size: 0px; width: 110px">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtExtension" Width="99%" runat="server" TabIndex="10" />
                                    </td>
                                    <td style="width: 130px;" runat="server" id="tdDuration">
                                        <div style="height: 0px; width: 130px; font-size: 0px;">
                                        </div>
                                        <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%=Resource.Admin_Catalog_From%>:
                                                </td>
                                                <td style="width: 95px;">
                                                    <asp:TextBox CssClass="filtertxtbox" ID="txtDurationFrom" Width="40" runat="server" TabIndex="10" />
                                                    с
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%=Resource.Admin_Catalog_To%>:
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="filtertxtbox" ID="txtDurationTo" Width="40" runat="server" TabIndex="10" />
                                                    с
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 310px;" runat="server" id="tdRecordLink"></td>
                                    <td style="width: 140px;" id="tdManager" runat="server" visible="False">
                                        <div style="height: 0px; font-size: 0px; width: 140px">
                                        </div>
                                        <asp:DropDownList ID="ddlManagers" TabIndex="10" CssClass="dropdownselect"
                                            runat="server" Width="99%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 50px; padding-right: 10px; text-align: center;">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="10" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                            TabIndex="10" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="7"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                                    CssClass="tableview" GridLines="None" EnableModelValidation="True" EnableEdit="False"
                                    OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting" OnRowDataBound="grid_OnDataBound">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CallId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("CallId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# ((bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />")%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="40px" />
                                            <ItemStyle CssClass="checkboxcolumn" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Type">
                                            <HeaderTemplate>
                                                Тип
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="icon-call calltype-<%#Eval("Type").ToString().ToLower() %>" title="<%#Eval("Type").ToString().TryParseEnum<ECallType>().Localize() %>"></span>
                                            </ItemTemplate>
                                            <HeaderStyle Width="60px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="HangupStatus" Visible="False">
                                            <HeaderTemplate>
                                                Причина
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("HangupStatus").ToString().TryParseEnum<ECallHangupStatus>().Localize() %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="CalledBack" Visible="False">
                                            <HeaderTemplate>
                                                Перезвонили
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="cbCalledBack" Checked='<%# Bind("CalledBack") %>' CssClass="add"
                                                    AutoPostBack="True" OnCheckedChanged="cbCalledBack_CheckedChanged" />
                                                <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="CallDate">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbCallDate" runat="server" CommandName="Sort" CommandArgument="CallDate">
                                                    Дата
                                                        <asp:Image ID="arrowCallDate" CssClass="arrow" runat="server" ImageUrl="../admin/images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("CallDate") %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="110px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SrcNum">
                                            <HeaderTemplate>
                                                <div style="height: 0px; font-size: 0px; width: 110px">
                                                </div>
                                                Откуда
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("SrcNum") %>
                                                <%# Eval("Type").ToString().TryParseEnum<ECallType>() != ECallType.Out ? CurrentOperator.RenderCallButton(StringHelper.ConvertToStandardPhone(SQLDataHelper.GetString(Eval("SrcNum")))) : string.Empty %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="DstNum">
                                            <HeaderTemplate>
                                                <div style="height: 0px; font-size: 0px; width: 110px">
                                                </div>
                                                Куда
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("DstNum") %>
                                                <%# Eval("Type").ToString().TryParseEnum<ECallType>() == ECallType.Out ? IPTelephonyOperator.Current.RenderCallButton(StringHelper.ConvertToStandardPhone(SQLDataHelper.GetString(Eval("DstNum")))) : string.Empty %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Extension">
                                            <HeaderTemplate>
                                                Добавочный номер
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("Extension") %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Duration">
                                            <HeaderTemplate>
                                                Продолжительность
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# RenderDuration(SQLDataHelper.GetInt(Eval("Duration"))) %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="120px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="RecordLink">
                                            <HeaderTemplate>
                                                Запись разговора
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <a href="javascript:void(0);" data-recordlink-callid='<%# Eval("Id") %>' data-recordlink-type='<%# Eval("OperatorType") %>'
                                                     runat='server' visible='<%# Eval("CallAnswerDate") != DBNull.Value %>'>Прослушать</a>
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="ManagerName" Visible="False">
                                            <HeaderTemplate>
                                                <%=Resource.Admin_OrderSearch_Manager%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("ManagerName")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="130px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 readonlyrow" />
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                            <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_NoRecords %>"></asp:Localize>
                                        </div>
                                    </EmptyDataTemplate>
                                </adv:AdvGridView>
                                <div style="border-top: 1px #c9c9c7 solid;">
                                </div>
                                <table class="results2">
                                    <tr>
                                        <td style="width: 157px; padding-left: 6px;">
                                            <asp:Localize ID="Localize_Admin_Catalog_ResultPerPage" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ResultPerPage %>"></asp:Localize>:
                                                <asp:DropDownList ID="ddRowsPerPage" runat="server" OnSelectedIndexChanged="btnFilter_Click"
                                                    CssClass="droplist" AutoPostBack="true" Style="display: inline-block;">
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem Selected="True">50</asp:ListItem>
                                                    <asp:ListItem>100</asp:ListItem>
                                                </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                UseHref="false" OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
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
                    <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    <br />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
        <adv:EnumDataSource ID="edsECallType" runat="server" EnumTypeName="AdvantShop.Core.Services.IPTelephony.ECallType" />
        <adv:EnumDataSource ID="edsECallHangupStatus" runat="server" EnumTypeName="AdvantShop.Core.Services.IPTelephony.ECallHangupStatus">
            <ExceptValues>
                <asp:ListItem Value="0">None</asp:ListItem>
                <asp:ListItem Value="1">Answer</asp:ListItem>
            </ExceptValues>
        </adv:EnumDataSource>

        <script type="text/javascript">

            $(document).ready(function () {
                $("input.showtooltip").tooltip({
                    showURL: false
                });
            });

            function hide_wait_for_node(node) {
                if (node.wait_img) {
                    node.removeChild(node.wait_img);
                }
            }

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
            });

            function removeunloadhandler() {
                window.onbeforeunload = null;
            }


            function beforeunload(e) {
                if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                    var evt = window.event || e;
                    evt.returnValue = '<%=Resource.Admin_OrderSearch_LostChanges%>';
                }
            }

            function addbeforeunloadhandler() {
                window.onbeforeunload = beforeunload;
            }

            function selectCange() {
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
                            var r = confirm("<%= Resource.Admin_Catalog_Confirm%>");
                            if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                            break;
                    }
                });
            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { selectCange(); });
        </script>
    </div>
</asp:Content>
