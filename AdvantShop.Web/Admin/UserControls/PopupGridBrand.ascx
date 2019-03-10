<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.PopupGridBrand" Codebehind="PopupGridBrand.ascx.cs" %>

<asp:LinkButton ID="lbPopup" runat="server" Style="display: none;" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lbPopup"
    PopupControlID="modalPopup" BackgroundCssClass="blackopacitybackground" BehaviorID="ModalBehaviourBrand"
    CancelControlID="btnHideUsers">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="modalPopup" runat="server" CssClass="modal-admin" Style="display: none;">
        <div class="title">
            <%= Resources.Resource.Admin_mMenu_Brand%></div>
        <div style="border: 1px #c9c9c7 solid; width: 96%;">
            <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                <table class="filter" style="border-collapse: collapse;" border="0" cellpadding="0"
                    cellspacing="0">
                    <tr style="height: 5px;">
                        <td colspan="7">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20px;">
                            <div style="height: 0px; width: 20px; font-size: 0px;">
                            </div>
                        </td>
                        <td style="width: 150px;">
                            <div style="height: 0px; width: 140px; font-size: 0px;">
                            </div>
                            <asp:TextBox CssClass="filtertxtbox" ID="txtSearchName" Width="99%" runat="server"
                                TabIndex="11" />
                        </td>
                        <td style="width: 150px;">
                            <div style="height: 0px; width: 140px; font-size: 0px;">
                            </div>
                            <asp:TextBox CssClass="filtertxtbox" ID="txtSearchProducts_Count" Width="99%" runat="server"
                                TabIndex="12" />
                        </td>
                        <td style="width: 100px; text-align: right;">
                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" CausesValidation="false"
                                TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnReset" runat="server" CssClass="btn" CausesValidation="false"
                                TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
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
                    <asp:AsyncPostBackTrigger ControlID="agvBrand" EventName="Sorting" />
                    <asp:AsyncPostBackTrigger ControlID="agvBrand" EventName="DataBinding" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <adv:AdvGridView ID="agvBrand" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                        CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_CustomersSearch_Confirmation %>"
                        CssClass="tableview" EditURL="" GridLines="None" OnRowCommand="agv_RowCommand"
                        OnSorting="agvBrand_Sorting">
                        <Columns>
                            <asp:TemplateField AccessibleHeaderText="CategoryID" Visible="false" HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:Label ID="Label01" runat="server" Text='<%# Bind("BrandID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10px">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="BrandName" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="190">
                                <HeaderTemplate>
                                    <div style="height: 0px; width: 190px; font-size: 0px;">
                                    </div>
                                    <asp:LinkButton ID="lbBrandName" runat="server" CommandName="Sort" CommandArgument="BrandName"
                                        CausesValidation="false">
                                        <%=Resources.Resource.Admin_mMenu_BrandName%><asp:Image ID="arrowBrandName" CssClass="arrow"
                                            runat="server" ImageUrl="../images/arrowdownh.gif" />
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("BrandName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="ProductsCount" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150">
                                <HeaderTemplate>
                                    <div style="height: 0px; width: 150px; font-size: 0px;">
                                    </div>
                                    <asp:LinkButton ID="ProductsCount" runat="server" CommandName="Sort" CommandArgument="ProductsCount"
                                        CausesValidation="false">
                                        <%=Resources.Resource.Admin_mMenu_ProductsCount %><asp:Image ID="arrowProductsCount"
                                            CssClass="arrow" runat="server" ImageUrl="../images/arrowdownh.gif" />
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProducts_Count" runat="server" Text='<%# Eval("ProductsCount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="69" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <div style="height: 0px; width: 69px; font-size: 0px;">
                                    </div>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="SelectContact" CausesValidation="false" runat="server" CommandName="Select"
                                        OnClientClick="HideModalPopupBrand();" CommandArgument='<%# Eval("BrandID") %>'
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
    function ShowModalPopupBrand() {
        document.body.style.overflowX = 'hidden';
        $find('ModalBehaviourBrand').show();
        document.getElementById('ModalBehaviourBrand_backgroundElement').onclick = HideModalPopupBrand;
    }
    function HideModalPopupBrand() {
        $find("ModalBehaviourBrand").hide();
        $('select', 'object', 'embed').each(function () {
            $(this).show();
        });
    }
</script>
