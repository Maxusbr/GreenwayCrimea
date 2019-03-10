<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.CustomerSearch" CodeBehind="CustomerSearch.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<%@ Import Namespace="Resources" %>

<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
        function keyhook(ev) {
            ev = ev || window.event;
            var code = ev.keyCode;
            if (code == 27) {
                $find('calendar').hide();
                $find('calendar2').hide();
            }
        }
        document.onkeydown = keyhook;
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
                        var r = confirm("<%= Resources.Resource.Admin_CustomersSearch_Confirm%>");
                        if (r) __doPostBack('<%=lbDeleteSelected1.UniqueID%>', '');
                        break;
                   <%-- case "changeGroup":
                        document.getElementById('<%=lbChangeCustomerGroup.ClientID%>').click();--%>
                }
            });
            initgrid();
        });

        <%--  function ChangeCustomerGroup() {
            if ($("#commandSelect option:selected").val() == "changeGroup") {
                $("#<%= ddlChangeCustomerGroup.ClientID %>").show();
            } else {
                $("#<%= ddlChangeCustomerGroup.ClientID %>").hide();
            }
        }--%>
    </script>
</asp:Content>
<asp:Content ID="ContentCustomerSearch" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected  dropdown-menu-parent"><a href="CustomerSearch.aspx">
                <%= Resource.Admin_MasterPageAdmin_Users%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="CustomerSearch.aspx?role=User">
                            <%= Resource.Admin_MasterPageAdmin_Buyers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="CustomerSearch.aspx?role=Moderator">
                            <%= Resource.Admin_MasterPageAdmin_Moderators %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="CustomerSearch.aspx?role=Administrator">
                            <%= Resource.Admin_MasterPageAdmin_Administrators %>
                        </a></li>
                    </ul>
                </div>
            </li>
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
            <a href='<%= "CreateCustomer.aspx" + (string.IsNullOrEmpty(Request["role"]) ? string.Empty : "?role=" + Request["role"] ) %>' class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>
        </div>
    </div>
    <div class="content-own">
        <div style="text-align: center;">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/customers_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_CustomerSearch_Header %>"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_CustomerSearch_SubHeader %>"></asp:Label>
                        </td>
                        <td>
                            <div class="btns-main">
                                <asp:Button CssClass="btn btn-middle btn-add" ID="btnCreateCustomer" runat="server" Text="<%$ Resources:Resource, Admin_CreateCustomer_Header %>" ValidationGroup="0" Visible="true" OnClick="btnCreateCustomer_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
            <br />
            <table style="width: 100%;" class="massaction">
                <tr>
                    <td>
                        <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                        </span><span style="display: inline-block">
                            <select id="commandSelect" onchange="ChangeCustomerGroup();">
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
                                <%--   <option value="changeGroup">
                                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_Customers_ChangeGroup %>"></asp:Localize>
                                </option>--%>
                            </select>
                            <%--<asp:DropDownList ID="ddlChangeCustomerGroup" CssClass="dropdownselect" runat="server"
                                DataSourceID="sdsGroup" DataTextField="Name" DataValueField="CustomerGroupId"
                                OnDataBound="ddlCustomerGroup_DataBound" Style="display: none;">
                            </asp:DropDownList>--%>
                            <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                            <asp:LinkButton ID="lbDeleteSelected1" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_CustomersSearch_DeleteSelected %>"
                                OnClick="lbDeleteSelected1_Click" />
                            <%--        <asp:LinkButton ID="lbChangeCustomerGroup" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                OnClick="lbChangeCustomerGroup_Click" />--%>
                        </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span>
                        </span>
                    </td>
                    <td align="right" class="selecteditems">
                        <asp:UpdatePanel ID="upCounts" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Localize ID="Localize_Admin_Catalog_Total" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Total %>"></asp:Localize>
                                <span class="bold">
                                    <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<asp:Localize
                                        ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_RecordsFound %>"></asp:Localize>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 8px;"></td>
                </tr>
            </table>
            <div>
                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                    <table class="filter" style="border-collapse: collapse;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 5px;">
                            <td colspan="7"></td>
                        </tr>
                        <tr>
                            <td style="width: 80px; text-align: center;">
                                <div style="width: 80px; font-size: 0px;">
                                </div>
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="65px">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <%--<td style="width: 250px;">--%>
                            <td>
                                <div style="height: 0px; width: 150px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchName" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 110px;">
                                <div style="height: 0px; width: 110px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchLocation" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 125px;">
                                <div style="height: 0px; width: 125px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchPhone" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 150px;">
                                <div style="height: 0px; width: 150px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchEmail" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 110px;">
                                <div style="height: 0px; width: 100px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchOrdersCount" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 110px;">
                                <div style="height: 0px; width: 110px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchLastOrder" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 100px;">
                                <div style="height: 0px; width: 100px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtSearchOrdersSum" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>

                            <td style="width: 120px;">
                                <div style="height: 0px; width: 120px; font-size: 0px;">
                                </div>
                                <table cellpadding="1px;" cellspacing="0px;" style="margin-left: 5px;">
                                    <tr>
                                        <td style="text-align: left;">
                                            <%=Resources.Resource.Admin_Catalog_From%>:
                                        </td>
                                        <td style="width: 110px;">
                                            <div class="dp">
                                                <asp:TextBox CssClass="filtertxtbox" ID="txtDateFrom" Width="70" runat="server" TabIndex="12" />
                                                <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <%=Resources.Resource.Admin_Catalog_To%>:
                                        </td>
                                        <td style="width: 88px;">
                                            <div class="dp">
                                                <asp:TextBox CssClass="filtertxtbox" ID="txtDateTo" Width="70" runat="server" TabIndex="12" />
                                                <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                            </div>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 100px;" runat="server" id="tdManager" visible="False">
                                <div style="height: 0px; width: 100px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtManager" runat="server" Width="99%"
                                    TabIndex="12" />
                            </td>
                            <td style="width: 60px; text-align: center;">
                                <div style="height: 0px; width: 60px; font-size: 0px;">
                                </div>
                                <center>
                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                    TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                    TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                            </center>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td colspan="7"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected1" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="advCustomers" EventName="Sorting" />
                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="advCustomers" EventName="RowCommand" />
                    </Triggers>
                    <ContentTemplate>
                        <adv:AdvGridView ID="advCustomers" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                            CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_CustomersSearch_Confirmation %>"
                            CssClass="tableview" Style="cursor: pointer" DataFieldForImageDescription=""
                            DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="agv_RowCommand"
                            OnSorting="advCustomers_Sorting" EnableEdit="False">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false" HeaderStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 70px; font-size: 0px;">
                                        </div>
                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#(bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />"%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 140px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                            <%=Resources.Resource.Admin_CustomerSearch_Name1%>
                                            <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        <%# Convert.ToInt32(Eval("Rating")) != 0 ? "<span class='users-rate'>" +Eval("Rating") + "</span>" : string.Empty %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Location" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 100px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbLocation" runat="server" CommandName="Sort" CommandArgument="Location">
                                            Местоположение
                                            <asp:Image ID="arrowLocation" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Phone" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="115">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 115px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbPhone" runat="server" CommandName="Sort" CommandArgument="Phone">
                                            <%=Resources.Resource.Admin_CustomerSearch_Phone%>
                                            <asp:Image ID="arrowPhone" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("Phone")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="Email" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 140px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                            <%=Resources.Resource.Admin_CustomerSearch_Email1%>
                                            <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="OrdersCount" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 90px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbOrdersCount" runat="server" CommandName="Sort" CommandArgument="OrdersCount">
                                            Кол-во заказов
                                            <asp:Image ID="arrowOrdersCount" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrdersCount" runat="server" Text='<%# Eval("OrdersCount") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="LastOrder" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 100px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbLastOrder" runat="server" CommandName="Sort" CommandArgument="LastOrder">
                                            Последний заказ
                                            <asp:Image ID="arrowLastOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href='<%# "ViewOrder.aspx?OrderId=" + Eval("LastOrder") %>' runat='server' visible='<%# Eval("LastOrder") != DBNull.Value %>'>
                                            <%# "#" + Eval("LastOrderNumber") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="OrdersSum" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 90px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbOrdersSum" runat="server" CommandName="Sort" CommandArgument="OrdersSum">
                                            Сумма заказов
                                            <asp:Image ID="arrowOrdersSum" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrdersSum" runat="server" Text='<%#PriceFormatService.FormatPrice(Convert.ToSingle(Eval("OrdersSum")))  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="RegistrationDateTime" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="110">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 110px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbLastActive" runat="server" CommandName="Sort" CommandArgument="RegistrationDateTime">
                                            <%--Последняя активность--%>
                                            Дата регистрации
                                            <asp:Image ID="arrowRegistrationDateTime" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegDate" runat="server" Text='<%#AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("RegistrationDateTime"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="ManagerName" Visible="False" InsertVisible="False" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 90px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbManager" runat="server" CommandName="Sort" CommandArgument="ManagerName">
                                            Менеджер
                                            <asp:Image ID="arrowManager" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href="<%# "ordersearch.aspx?manager=" + Eval("ManagerId") %>"><%#Eval("ManagerName") %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="50px">
                                    <HeaderTemplate>
                                        <div style="height: 0px; width: 50px; font-size: 0px;">
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href='<%#"ViewCustomer.aspx?CustomerID=" + Eval("ID")%>' class="showtooltip"
                                            title="<%= Resources.Resource.Admin_CustomersSearch_Edit %>">
                                            <img src="images/editbtn.gif" style="border: none;" /></a>
                                        <asp:LinkButton ID="buttonDelete" runat="server"
                                            CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteCustomer"
                                            data-confirm="<%$ Resources:Resource, Admin_CustomersSearch_Confirmation %>"
                                            ToolTip="<%$ Resources:Resource, Admin_CustomersSearch_Delete %>"
                                            CommandArgument='<%# Eval("ID") %>' Visible='<%# CanDelete((Guid)Eval("ID")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 readonlyrow" />
                            <EmptyDataTemplate>
                                <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_NoRecords %>"></asp:Localize>
                                </center>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                        <div style="border-top: 1px #c9c9c7 solid;">
                        </div>
                        <table class="results2">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <asp:Localize ID="Localize_Admin_Catalog_ResultPerPage" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ResultPerPage %>"></asp:Localize>:&nbsp;<asp:DropDownList
                                        ID="ddRowsPerPage" runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist"
                                        AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                        UseHref="false" OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
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
                        <center>
                        <div style="text-align: left;">
                            <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label><br />
                        </div>
                    </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <input type="hidden" id="SelectedIds" name="SelectedIds" />
        </div>
        <div id="inprogress" style="display: none;">
            <div id="curtain" class="opacitybackground">
                &nbsp;
            </div>
            <div class="loader">
                <table width="100%" style="font-weight: bold; text-align: center;">
                    <tbody>
                        <tr>
                            <td align="center">
                                <img src="images/ajax-loader.gif" alt="" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="color: #0D76B8;">
                                <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <br />
        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(function () { setupTooltips(); });
            prm.add_beginRequest(function () { Darken(); });
            prm.add_endRequest(function () { Clear(); });
        </script>
    </div>
</asp:Content>
