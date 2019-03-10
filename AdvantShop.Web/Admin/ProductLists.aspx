<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/CatalogLayout.master" AutoEventWireup="true" Inherits="Admin.ProductLists" ValidateRequest="false" Codebehind="ProductLists.aspx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script type="text/javascript">
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
                        if (r) window.__doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });
    </script>
    <style type="text/css">
        .style1 {height: 24px;}
        .style2 {width: 8px;height: 24px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
<div>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr>
                <td style="width: 72px;">
                    <img src="images/orders_ico.gif" alt="" />
                </td>
                <td>
                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text=""></asp:Label><br />
                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ProductsList %>"></asp:Label>
                    <asp:Label ID="lMessage" Style="float: left;" runat="server" ForeColor="Red" Visible="false"
                        EnableViewState="false" />
                </td>
                <td style="vertical-align: bottom; padding-right: 10px">
                    <div class="btns-main">
                        <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddList" runat="server" Text="<%$ Resources:Resource, Admin_ProductList_AddList %>" 
                            ValidationGroup="0" OnClick="btnAddList_Click" />
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
                <td align="right" class="style1" style="text-align: right;">
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
        <div style="border: 1px #c9c9c7 solid; width: 100%">
            <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                <table class="filter" cellpadding="2" cellspacing="0">
                    <tr style="height: 5px;">
                        <td colspan="5"></td>
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
                        <td style="width: 150px;">
                            <div style="width: 150px; font-size: 0px; height: 0px;">
                            </div>
                            <asp:TextBox CssClass="filtertxtbox" ID="txtSortOrder" Width="99%" runat="server"
                                TabIndex="12" />
                        </td>
                        <td style="width: 90px;">
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
                        <td style="height: 5px;" colspan="5"></td>
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
                    <asp:AsyncPostBackTrigger ControlID="btnAddList" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                        CellPadding="2" CellSpacing="0" CssClass="tableview" Style="cursor: pointer"
                        DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                        EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" OnSorting="grid_Sorting"
                        ShowFooter="false">
                        <Columns>
                            <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                <EditItemTemplate>
                                    <asp:Label ID="Label0" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label01" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
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
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                        <%= Resource.Admin_ProductLists_Name%>
                                        <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' Width="98%" />
                                </EditItemTemplate>
                                    <FooterTemplate>
                                    <asp:TextBox ID="txtNewName" runat="server" Text='' CssClass="add" Width="98%" />
                                    </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Left"
                                HeaderStyle-Width="150">
                                <HeaderTemplate>
                                    <div style="width: 150px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="SortOrder" CommandArgument="SortOrder">
                                        <%=Resources.Resource.Admin_Catalog_SortOrder%>
                                        <asp:Image ID="arrowSort" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Eval("SortOrder") %>' Width="99%"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNewSortOrder" runat="server" Text='0' Width="99%" CssClass="add" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="Enabled" HeaderStyle-HorizontalAlign="Left"
                                HeaderStyle-Width="150">
                                <HeaderTemplate>
                                    <div style="width: 150px; font-size: 0px; height: 0px;">
                                    </div>
                                        <%= Resource.Admin_ProductLists_Enabled %>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Eval("Enabled") %>' Width="99%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkNewEnabled" runat="server" Checked='True' Width="99%" CssClass="add" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="Enabled" HeaderStyle-HorizontalAlign="Left"
                                HeaderStyle-Width="150">
                                <HeaderTemplate>
                                    <div style="width: 150px; font-size: 0px; height: 0px;">
                                    </div>
                                    <%= Resource.Admin_ProductLists_ListProducts %>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <a href="ProductListMapping.aspx?listid=<%# Eval("Id")%>"><%= Resource.Admin_ProductLists_ListProductsLink %></a>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="center"
                                FooterStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <%--<asp:Image ID="buttonEdit" runat="server" ImageUrl="images/editbtn.gif" CssClass="editbtn showtooltip"
                                        title='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Edit %>' />--%>
                                    <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                        src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                        style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                    <asp:LinkButton ID="buttonDelete" runat="server"
                                        CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteProduct" CommandArgument='<%# Eval("Id")%>'
                                        data-confirm="<%$ Resources:Resource, Admin_ConfirmDeleting %>"
                                        ToolTip='<%$ Resources:Resource, Admin_Delete %>' />
                                    <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                        src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                        style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ibAddProperty" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddProductList"
                                    ToolTip="<%$ Resources:Resource, Admin_PropertiesValues_AddValue%>" />
                                    <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                        CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Property_CancelAdd  %>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#ccffcc" />
                        <HeaderStyle CssClass="header" />
                        <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                        <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                        <EmptyDataTemplate>
                            <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_Catalog_NoRecords%>
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
                                    <asp:ListItem Selected="true">20</asp:ListItem>
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
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
    </div>
</div>
</asp:Content>
