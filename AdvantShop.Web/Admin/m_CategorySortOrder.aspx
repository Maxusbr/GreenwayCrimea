<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Admin.m_CategorySortOrder" MasterPageFile="m_MasterPage.master" Codebehind="m_CategorySortOrder.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphCenter">
    <ajaxToolkit:ModalPopupExtender ID="mpeTree" runat="server" PopupControlID="pTree"
        TargetControlID="hlChooseParent" BackgroundCssClass="blackopacitybackground"
        CancelControlID="btnCancelParent" BehaviorID="ModalBehaviour">
    </ajaxToolkit:ModalPopupExtender>
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
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
    <asp:Panel runat="server" ID="pTree" CssClass="tree-popup" Style="display: none; z-index: 10001">
        <div <%= (Request.Browser.Browser.Equals("IE"))? "class=\"mtree mcatsorttree_ie\"": "class=\"mtree\""%>>
            <center>
                <table style="margin-top: 13px;">
                    <tbody>
                        <tr>
                            <td>
                                <span style="font-size: 11pt;">
                                    <asp:Localize ID="Localize_Admin_m_CategorySortOrder_ParentCategory" runat="server"
                                        Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_ParentCategory %>">
                                    </asp:Localize>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="tree" EventName="TreeNodePopulate" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div style="height: 360px; width: 450px; overflow: scroll; background-color: White;">
                                            <asp:TreeView ID="tree" runat="server" OnTreeNodePopulate="tree_TreeNodePopulate">
                                                <NodeStyle ForeColor="Black" />
                                                <SelectedNodeStyle BackColor="#027DC2" ForeColor="White" />
                                            </asp:TreeView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom" style="height: 36px;">
                                <asp:Button ID="btnOkParent" runat="server" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_OK %>"
                                    Width="67" OnClick="btnOkParent_Click" />
                                <asp:Button ID="btnCancelParent" runat="server" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Cancel %>"
                                    Width="67" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </center>
        </div>
    </asp:Panel>
    <br />
    <div style="text-align: center;" >
        <asp:Label ID="lblCustomer" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Header %>"></asp:Label><br/>
        <asp:Label ID="lblCustomerName" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_SubHeader %>"></asp:Label>
    </div>
    <br />
    <table style="width: 100%;">
        <tr>
            <td style="width: 10px;">
            </td>
            <td>
                <div style="width: 100%;">
                    <div id="divSave" runat="server" style="margin-bottom: 10px">
                        <div style="background-color: #EFF4ED; text-align: left; border: 1px solid #99A098;
                            width: 100%;">
                            <div style="width: 580px; height: 0px; font-size: 0px;">
                            </div>
                            <table cellpadding="0" cellspacing="0" style="height: 24px">
                                <tr>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <img src="images/ok.gif" style="border: none">
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <span style="color: #52704C; font-family: Arial,Helvetica,sans-serif; font-size: 11px;
                                            font-weight: bold">
                                            <% =Resources.Resource.Admin_m_Category_SaveSuccesed%></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div>
                        <div style="background-color: #eff0f1; text-align: left; border: solid 1px #c9c9c7;">
                            <div style="width: 580px; height: 0px; font-size: 0px;">
                            </div>
                            <table cellpadding="0" cellspacing="0" style="height: 24px">
                                <tr>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Parent %>"></asp:Label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upParent" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                            OnPreRender="upParent_PreRender">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="lParent" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Root %>" /><asp:HiddenField
                                                    runat="server" ID="hfParent" Value="0" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnOkParent" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:HyperLink CssClass="Link" runat="server" ID="hlChooseParent" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Change %>"
                                            NavigateUrl="#" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="margin-top: 5px; width: 100%; text-align: left;">
                        <span style="font-family: Verdana; font-size: 10pt;">
                            <%=Resources.Resource.Admin_m_CategorySortOrder_SubCategory%></span></div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnOkParent" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <div style="width: 100%">
                                <center>
                                    <div style="border: 1px solid #C9C9C7">
                                        <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                                            CssClass="tableview" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                            DataFieldForImagePath="" EditURL="" GridLines="None" ReadOnlyRowInInit="false"
                                            OnSorting="grid_Sorting">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="CategoryID" Visible="true" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="100" HeaderStyle-Width="100">
                                                    <HeaderTemplate>
                                                        <div style="width: 100px; height: 0px; font-size: 0px;">
                                                        </div>
                                                        <asp:LinkButton ID="lbID" runat="server" CommandName="Sort" CommandArgument="CategoryID">
                                                            ID
                                                            <asp:Image ID="arrowID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="Label0" runat="server" Text='<%# Bind("CategoryID") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label01" runat="server" Text='<%# Bind("CategoryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div style="width: 380px; height: 0px; font-size: 0px;">
                                                        </div>
                                                        <asp:LinkButton ID="lbOrderCategory" runat="server" CommandName="Sort" CommandArgument="Name">
                                                            <%=Resources.Resource.Admin_Catalog_Name%>
                                                            <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' Width="99%" Style="text-align: left;"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="SortOrder" ItemStyle-Width="100" HeaderStyle-Width="100"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div style="width: 100px; height: 0px; font-size: 0px;">
                                                        </div>
                                                        <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                            <%=Resources.Resource.Admin_Catalog_SortOrder%>
                                                            <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Bind("SortOrder") %>' Width="99%"
                                                            Style="text-align: center;"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
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
                                                    <div style="width: 163px; height: 0px; font-size: 0px;">
                                                    </div>
                                                    <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                                        OnSelectedIndexChanged="ddRowsPerPage_SelectedIndexChanged" runat="server" CssClass="droplist"
                                                        AutoPostBack="true">
                                                        <asp:ListItem>10</asp:ListItem>
                                                        <asp:ListItem>20</asp:ListItem>
                                                        <asp:ListItem>50</asp:ListItem>
                                                        <asp:ListItem>100</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center">
                                                    <div style="width: 240px; height: 0px; font-size: 0px;">
                                                    </div>
                                                    <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                        UseHref="false" OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
                                                </td>
                                                <td style="width: 157px; text-align: right; padding-right: 12px">
                                                    <div style="width: 169px; height: 0px; font-size: 0px;">
                                                    </div>
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
                                    </div>
                                </center>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <center>
                        <table style="width: 210px; text-align: center; margin-bottom: 20px; margin-top: 10px;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button CssClass="btn btn-middle btn-add" ID="SaveAll" runat="server" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Save %>" ValidationGroup="0" OnClick="SaveAll_Click" />
                                </td>
                                <td style="width: 10px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 6px; font-size: 6px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btnOK" CssClass="btn btn-middle btn-action" runat="server" Text="<%$ Resources:Resource, Admin_m_CategorySortOrder_Close %>"
                                        Width="110px" OnClick="btnOK_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    &nbsp;
                    <br />
                    <br />
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            &nbsp; &nbsp;
                            <asp:Image ID="imgUpdating" runat="server" AlternateText="<%$ Resources:Resource, Admin_Loading %>"
                                ImageUrl="images/loading.gif" />
                            <asp:Label ID="lblUpdating" runat="server" Text="<%$ Resources:Resource, Admin_Loading %>"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </td>
            <td style="width: 10px;">
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphScript">
    <script type="text/javascript">
        function CreateHistory(hist) {
            $.historyLoad(hist);
        }
        var timeOut;
        function Darken() {
            timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
        }
        function Clear() {
            clearTimeout(timeOut); document.getElementById("inprogress").style.display = "none"; $("input.sel").each(function (i) {
                if (this.checked) $(this).parent().parent().addClass("selectedrow");
            }); initgrid();
        }
        $(document).ready(function () {
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            //$("ineditcategory").tooltip();
        });
    </script>
</asp:Content>
