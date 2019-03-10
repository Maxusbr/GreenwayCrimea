<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Cities" Codebehind="Cities.aspx.cs" %>

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
            height: 30px;
        }

        .style2 {
            width: 8px;
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item "><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item selected"><a href="Country.aspx">
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
        <ul class="two-column">
            <li class="two-column-item">
                <div class="panel-toggle">
                    <h2 class="justify-item products-header">
                        <asp:Literal ID="lblHead" runat="server" />
                    </h2>
                    <div>
                        <asp:HyperLink runat="server" ID="hlBack" CssClass="blueLink" />
                    </div>
                    <div style="margin: 10px 0 0 0;">
                        <asp:HyperLink runat="server" ID="hlBack2" CssClass="blueLink" />
                    </div>
                </div>
            </li>
            <li class="two-column-item">

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

                <div style="margin: 0 0 15px 0">
                    <h2 class="header-edit">
                        <asp:Literal ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_Cities_SubHeader %>" />
                    </h2>
                    <div class="panel-add" style="margin: 8px 0 0 0;">
                        <asp:LinkButton runat="server" ID="btnAddCity" CssClass="panel-add-lnk" OnClick="btnAddCity_Click" Text="<%$ Resources:Resource, Admin_Cities_AddCity %>" />
                    </div>
                </div>

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
                                <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
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
                    <div style="border: 1px #c9c9c7 solid; width: 100%">
                        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                            <table class="filter" cellpadding="2" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="6"></td>
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
                                    <td style="width: 170px;">
                                        <div style="width: 170px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtPhoneNumberFilter" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 170px;">
                                        <div style="width: 170px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtMobilePhoneNumberFilter" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                     <td style="width: 170px;">
                                        <div style="width: 170px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtSort" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 100px;">
                                        <div style="width: 100px; font-size: 0px; height: 0px;">
                                        </div>
                                    </td>
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
                                    <td style="height: 5px;" colspan="6"></td>
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
                                <asp:AsyncPostBackTrigger ControlID="btnAddCity" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="2" CellSpacing="0" CssClass="tableview" Style="cursor: pointer"
                                    DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                                    EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting"
                                    OnRowDeleting="grid_RowDeleting" OnRowDataBound="grid_RowDataBound" ShowFooter="false"
                                    ShowFooterWhenEmpty="true" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
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
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CityName" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 150px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="CityName">
                                                    <%= Resources.Resource.Admin_Cities_Name %>
                                                    <asp:Image ID="arrowCityName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCityName" runat="server" Text='<%# Eval("CityName") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewName" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PhoneNumber" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="170px" HeaderStyle-Width="170px">
                                            <HeaderTemplate>
                                                <div style="width: 170px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbPhoneNumber" runat="server" CommandName="Sort" CommandArgument="PhoneNumber">
                                                    <%= Resources.Resource.Admin_Cities_PhoneNumber %>
                                                    <asp:Image ID="arrowPhoneNumber" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewPhoneNumber" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="MobilePhoneNumber" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="230px" HeaderStyle-Width="230px">
                                            <HeaderTemplate>
                                                <div style="width: 230px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbMobilePhoneNumber" runat="server" CommandName="Sort" CommandArgument="MobilePhoneNumber">
                                                    <%= Resources.Resource.Admin_Cities_PhoneNumber %> в мобильной версии
                                                    <asp:Image ID="arrowMobilePhoneNumber" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMobilePhoneNumber" runat="server" Text='<%# Eval("MobilePhoneNumber") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewMobilePhoneNumber" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CitySort" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="170px" HeaderStyle-Width="170px">
                                            <HeaderTemplate>
                                                <div style="width: 170px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbCitySort" runat="server" CommandName="Sort" CommandArgument="CitySort">
                                                    <%= Resources.Resource.Admin_Cities_SortOrder %>
                                                    <asp:Image ID="arrowCitySort" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCitySort" runat="server" Text='<%# Eval("CitySort") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNewSort" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="RegionId" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="lblRegionId" runat="server" Text='<%# Bind("RegionId") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblNewRegionId" runat="server" Text='<%# Bind("RegionId") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DisplayInPopup" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbDisplayInPopup" runat="server" CommandName="Sort" CommandArgument="City.DisplayInPopup">
                                                    <%= Resources.Resource.Admin_Cities_DisplayInPopup%>
                                                    <asp:Image ID="arrowDisplayInPopup" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox runat="server" Checked='<%# SQLDataHelper.GetBoolean(Eval("DisplayInPopup")) %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="chkNewDisplayInPopup" runat="server" Checked='<%# SQLDataHelper.GetBoolean(Eval("DisplayInPopup")) %>' CssClass="add" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="center"
                                            FooterStyle-HorizontalAlign="Center">
                                            <EditItemTemplate>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                    style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                                    CssClass="deletebtn showtooltip valid-confirm" CommandName="Delete" CommandArgument='<%# Eval("ID")%>'
                                                    data-confirm="<%$ Resources:Resource, Admin_City_Confirmation%>"
                                                    ToolTip='<%$ Resources:Resource, Admin_Delete %>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                    style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="ibAddCity" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddCity"
                                                    ToolTip="<%$ Resources:Resource, Admin_Cities_Add %>" />
                                                <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                    CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Cities_Cancel %>" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#ccffcc" />
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                            <%= Resources.Resource.Admin_Cities_NoCities %>
                                        </div>
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
                                                <asp:ListItem Selected="True">20</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>100</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
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
                    <input type="hidden" id="SelectedIds" name="SelectedIds" />
                </div>
                <script type="text/javascript">
                    function setupTooltips() {
                        $(".showtooltip").tooltip({
                            showURL: false
                        });
                    }
                    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
                </script>
            </li>
        </ul>
    </div>
</asp:Content>
