<%@ Control Language="C#" AutoEventWireup="true"
    EnableViewState="true" Inherits="Admin.UserControls.PopupGridProduct" Codebehind="PopupGridProduct.ascx.cs" %>
<asp:LinkButton ID="lbPopup" runat="server" Style="display: none;" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lbPopup"
    PopupControlID="modalPopup" BackgroundCssClass="blackopacitybackground" BehaviorID="ModalBehaviourProduct"
    CancelControlID="btnHideUsers">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="modalPopup" runat="server" CssClass="modal-admin" Style="display: none;">
    <div class="title">
        <%= Resources.Resource.Admin_mMenu_Products %></div>
    <div style="border: 1px #c9c9c7 solid; width: 96%;">
        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
            <table class="filter" style="border-collapse: collapse;" border="0" cellpadding="0"
                cellspacing="0">
                <tr style="height: 5px;">
                    <td colspan="7">
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <div style="height: 0px; width: 140px; font-size: 0px;">
                        </div>
                        <asp:TextBox CssClass="filtertxtbox" ID="txtSearchArtNo" Width="99%" runat="server"
                            TabIndex="11" />
                    </td>
                    <td style="width: 150px;">
                        <div style="height: 0px; width: 140px; font-size: 0px;">
                        </div>
                        <asp:TextBox CssClass="filtertxtbox" ID="txtSearchName" Width="99%" runat="server"
                            TabIndex="12" />
                    </td>
                    <td style="width: 69px; text-align: center;">
                        <div style="height: 0px; width: 69px; font-size: 0px;">
                        </div>
                        <center>
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" CausesValidation="false"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" CausesValidation="false"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </center>
                    </td>
                </tr>
                <tr style="height: 5px;">
                    <td colspan="7">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnSearchID" />--%>
                <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="agvProducts" EventName="Sorting" />
                <asp:AsyncPostBackTrigger ControlID="agvProducts" EventName="DataBinding" />
                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <adv:AdvGridView ID="agvProducts" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_CustomersSearch_Confirmation %>"
                    CssClass="tableview" DataFieldForEditURLParam="ProductID" EditURL="" GridLines="None"
                    TooltipImgCellIndex="2" TooltipTextCellIndex="5" OnRowCommand="agv_RowCommand"
                    OnSorting="agvProducts_Sorting">
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="ProductID" Visible="false" HeaderStyle-Width="10px">
                            <ItemTemplate>
                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ProductID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10px">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="ArtNo" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 140px; font-size: 0px;">
                                </div>
                                <asp:LinkButton ID="lbArtNo" runat="server" CommandName="Sort" CommandArgument="ArtNo"
                                    CausesValidation="false">
                                    <%= Resources.Resource.Admin_mMenu_AtrNo %><asp:Image ID="arrowArtNo" CssClass="arrow"
                                        runat="server" ImageUrl="../images/arrowdownh.gif" />
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblArtNo" runat="server" Text='<%# Eval("ArtNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                            HeaderStyle-Width="160">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 150px; font-size: 0px;">
                                </div>
                                <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name"
                                    CausesValidation="false">
                                    <%= Resources.Resource.Admin_mMenu_Name %><asp:Image ID="arrowName" CssClass="arrow"
                                        runat="server" ImageUrl="../images/arrowdownh.gif" />
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="69" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 69px; font-size: 0px;">
                                </div>
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="SelectContact" CausesValidation="false" runat="server" CommandName="Select"
                                    OnClientClick="HideModalPopupProduct();" CommandArgument='<%# Eval("ProductID") %>'
                                    Text="<%$ Resources : Resource, Admin_OrderSearch_ChooseUser %>"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
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
                            &nbsp;
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="margin-top: 5px">
        <asp:Button ID="btnHideUsers" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_Cancel%>" />
    </div>
</asp:Panel>
<script type="text/javascript">
    function ShowModalPopupProduct() {
        document.body.style.overflowX = 'hidden';
        $find('ModalBehaviourProduct').show();
        document.getElementById('ModalBehaviourProduct_backgroundElement').onclick = HideModalPopupProduct;
    }
    function HideModalPopupProduct() {
        $find("ModalBehaviourProduct").hide();
        $('select', 'object', 'embed').each(function () {
            $(this).show();
        });
    }
</script>
