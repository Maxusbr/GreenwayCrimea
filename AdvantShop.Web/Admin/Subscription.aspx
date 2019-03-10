<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.SubscriptionPage" CodeBehind="Subscription.aspx.cs" %>

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
                        var r = confirm("<%= Resource.Admin_Catalog_Confirm%>");
                        if (r) {
                            __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                            ResetFilter();
                        }
                        break;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="CustomerSearch.aspx">
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
            <li class="neighbor-menu-item selected"><a href="Subscription.aspx">
                <%= Resource.Admin_MasterPageAdmin_SubscribeList%></a></li>
            <li class="neighbor-menu-item "><a href="Managers.aspx">
                <%= Resource.Admin_MasterPageAdmin_Managers%></a></li>
            <li class="neighbor-menu-item"><a href="Departments.aspx">
                <%= Resource.Admin_MasterPageAdmin_Departments%></a></li>
            <li class="neighbor-menu-item"><a href="ManagersTasks.aspx">
                <%= Resource.Admin_MasterPageAdmin_ManagersTasks%></a></li>
        </menu>
        <div class="panel-add">
            <a href="CreateCustomer.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_Add %> <%= Resource.Admin_MasterPageAdmin_Customer %></a>
        </div>
    </div>
    <div class="content-own">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Subscription_Unreg_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Subscription_Unreg_SubHeader %>"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <a href="ImportSubscribersCSV.aspx"><%= Resource.Admin_Subscription_ImportSubscribers %></a>
                        <a href="httphandlers/subscription/ExportSubscribedEmails.ashx" class="btn btn-middle btn-add"><%= Resource.Admin_Subscription_ExportSubscribers %></a>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:UpdatePanel ID="updPanel" runat="server">
            <ContentTemplate>
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
                <div style="width: 100%">
                    <table style="width: 99%;" class="massaction">
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
                                    </select>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbDeleteSelected_Click" />
                                </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span></span>
                            </td>
                            <td align="right" class="selecteditems">
                                <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
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
                            <table class="filter" cellpadding="2" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="7"></td>
                                </tr>
                                <tr>
                                    <td style="width: 80px; text-align: center;" rowspan="2">
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server" Width="65">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 205px; padding: 0 5px;" rowspan="2">
                                        <div style="width: 205px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtEmail" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 150px; text-align: center;" rowspan="2">
                                        <asp:DropDownList ID="ddSubscribe" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="100">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px; padding-right: 4px">
                                        <div class="dp">
                                            <span class="textfromto"><%=Resource.Admin_Catalog_From%>:</span>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtSubscribeDateFrom" runat="server" Font-Size="10px" Width="64" TabIndex="21" />
                                            <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                        </div>
                                    </td>
                                    <td style="width: 150px; padding-right: 4px">
                                        <div class="dp">
                                            <span class="textfromto"><%=Resource.Admin_Catalog_From%>:</span>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtUnsubscribeDateFrom" runat="server" Font-Size="10px" Width="64" TabIndex="21" />
                                            <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                        </div>
                                    </td>
                                    <td rowspan="2">
                                        <div></div>
                                    </td>
                                    <td style="width: 90px;" rowspan="2">
                                        <div style="width: 90px; font-size: 0px; height: 0px;">
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
                                    <td style="width: 150px; padding-right: 4px">
                                        <div class="dp">
                                            <span class="textfromto"><%=Resource.Admin_Catalog_To%>:</span>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtSubscribeDateTo" runat="server" Font-Size="10px" Width="64" TabIndex="22" />
                                            <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                        </div>
                                    </td>
                                    <td style="width: 150px; padding-right: 4px">
                                        <div class="dp">
                                            <span class="textfromto"><%=Resource.Admin_Catalog_To%>:</span>
                                            <asp:TextBox CssClass="filtertxtbox" ID="txtUnsubscribeDateTo" runat="server" Font-Size="10px" Width="64" TabIndex="22" />
                                            <img class="icon-calendar" src="images/Calendar_scheduleHS.png" alt="" />
                                        </div>
                                    </td>
                                </tr>
                                <tr style="height: 5px;">
                                    <td colspan="7"></td>
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
                            </Triggers>
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Subscription_Unreg_QDelete %>"
                                    CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                    DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                                    OnSorting="grid_Sorting" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 70px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Id") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Email" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="200px">
                                            <HeaderTemplate>
                                                <div style="width: 200px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                                    <%=Resource.Admin_Subscription_Unreg_Email%>
                                                    <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Subscribe" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="140px">
                                            <HeaderTemplate>
                                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbSubscribe" runat="server" CommandName="Sort" CommandArgument="Subscribe">
                                                    <%=Resource.Admin_Subscription_Unreg_Status%>
                                                    <asp:Image ID="arrowSubscribe" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="lSubscribe" runat="server" Checked='<%# Eval("Subscribe") %>'></asp:CheckBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="SubscribeDate" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div style="width: 150px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbSubscribeDate" runat="server" CommandName="Sort" CommandArgument="SubscribeDate">
                                                    <%=Resource.Admin_Subscription_Date%>
                                                    <asp:Image ID="arrowSubscribeDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lSubscribeDate" runat="server" Text='<%# Eval("SubscribeDate") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="UnsubscribeDate" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 150px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbUnsubscribeDate" runat="server" CommandName="Sort" CommandArgument="UnsubscribeDate">
                                                    <%=Resource.Admin_Subscription_UnsubscribeDate%>
                                                    <asp:Image ID="arrowUnsubscribeDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="ckbUnsubscribeDate" runat="server" Text='<%# Eval("UnsubscribeDate")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="UnsubscribeReason" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 170px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbUnsubscribeReason" runat="server" CommandName="Sort" CommandArgument="UnsubscribeReason">
                                                    <%=Resource.Admin_Subscription_DeactivateReason_Header%>
                                                    <asp:Image ID="arrowUnsubscribeReason" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lUnsubscribeReason" runat="server" Text='<%# Eval("UnsubscribeReason") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="90px">
                                            <EditItemTemplate>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title='<%= Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                    style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                                <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip valid-confirm"
                                                    CommandName="DeleteUser" CommandArgument='<%# Eval("ID")%>' data-confirm="<%$ Resources:Resource, Admin_Subscription_Unreg_QDelete %>"
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#ccffcc" />
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                    <EmptyDataTemplate>
                                        <center style="margin-top: 20px; margin-bottom: 20px;">
                                            <%=Resource.Admin_Catalog_NoRecords%>
                                        </center>
                                    </EmptyDataTemplate>
                                </adv:AdvGridView>
                                <table class="results2">
                                    <tr>
                                        <td style="width: 157px; padding-left: 6px;">
                                            <%=Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
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
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
    </div>
</asp:Content>
