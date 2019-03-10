<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.PopupGridCustomers" Codebehind="PopupGridCustomers.ascx.cs" %>

<asp:LinkButton ID="lbPopup" runat="server" Style="display: none;" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lbPopup"
    PopupControlID="modalPopup" BackgroundCssClass="blackopacitybackground" BehaviorID="ModalBehaviourCustomers">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="modalPopup" CssClass="modal-admin" runat="server" Style="display: none;">
    <span style="font-size: 12pt; margin-bottom: 5px">
        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_OrderSearch_ChooseUserHeader%>" />
    </span>
    <div style="border: 1px #c9c9c7 solid; width: 98%;">
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
                        <asp:TextBox CssClass="filtertxtbox" ID="txtSearchLastname" Width="99%" runat="server"
                            TabIndex="12" />
                    </td>
                    <td style="width: 150px;">
                        <div style="height: 0px; width: 150px; font-size: 0px;">
                        </div>
                        <asp:TextBox CssClass="filtertxtbox" ID="txtSearchFirstName" Width="99%" runat="server"
                            TabIndex="12" />
                    </td>
                    <td>
                        <div style="height: 0px; width: 180px; font-size: 0px;">
                        </div>
                        <asp:TextBox CssClass="filtertxtbox" ID="txtSearchEmail" Width="99%" runat="server"
                            TabIndex="12" />
                    </td>
                    <td>
                        <div style="height: 0px; width: 180px; font-size: 0px;">
                        </div>
                        <asp:TextBox CssClass="filtertxtbox" ID="txtSearchPhone" Width="99%" runat="server"
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
        <asp:UpdatePanel ID="upCustomers" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="agvCustomers" EventName="Sorting" />
                <asp:AsyncPostBackTrigger ControlID="agvCustomers" EventName="DataBinding" />
                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkSaveCustomer" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <adv:AdvGridView ID="agvCustomers" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_CustomersSearch_Confirmation %>"
                    CssClass="tableview" DataFieldForEditURLParam="CustomerID" EditURL="" GridLines="None"
                    OnRowCommand="agv_RowCommand" OnSorting="agvCustomers_Sorting">
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="CustomerID" Visible="false" HeaderStyle-Width="10px">
                            <ItemTemplate>
                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 70px; font-size: 0px;">
                                </div>
                                <%if (MultiSelection)
                                  {%>
                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                <%}%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <% if (MultiSelection)
                                   { %>
                                <%#(bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />"%>
                                <% }
                                   else
                                   { %>
                                <a href="javascript:void(0);" onclick="<%#"ChooseCustomer('" + Eval("CustomerID") + "');" %>">OK</a>
                                <% } %>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("CustomerID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Lastname" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 150px; font-size: 0px;">
                                </div>
                                <asp:LinkButton ID="lbOrderLastname" runat="server" CommandName="Sort" CommandArgument="Lastname"
                                    CausesValidation="false">
                                    <%=Resources.Resource.Admin_CustomerSearch_Surname1%>
                                    <asp:Image ID="arrowLastname" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLastname" runat="server" Text='<%# Eval("Lastname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Firstname" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 150px; font-size: 0px;">
                                </div>
                                <asp:LinkButton ID="lbOrderFirstname" runat="server" CommandName="Sort" CommandArgument="Firstname"
                                    CausesValidation="false">
                                    <%=Resources.Resource.Admin_CustomerSearch_Name1%>
                                    <asp:Image ID="arrowFirstname" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFirstname" runat="server" Text='<%# Eval("Firstname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Email" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 160px; font-size: 0px;">
                                </div>
                                <asp:LinkButton ID="lbEmail" runat="server" CommandName="Sort" CommandArgument="Email"
                                    CausesValidation="false">
                                    <%=Resources.Resource.Admin_CustomerSearch_Email1%>
                                    <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="Phone" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                <div style="height: 0px; width: 160px; font-size: 0px;">
                                </div>
                                <asp:LinkButton ID="lbPhone" runat="server" CommandName="Sort" CommandArgument="Phone" CausesValidation="false">
                                    <%=Resources.Resource.Admin_CustomerSearch_Phone%>
                                    <asp:Image ID="arrowPhone" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
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
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
    </div>
    <div style="text-align: center; width: 150px;">
        <%if (MultiSelection)
          {%>
        <div style="margin-top: 5px; float: left;">
            <asp:Button ID="btnOk" OnClientClick="ChooseMultipleCustomer();" runat="server" Text="<%$ Resources:Resource,Admin_OrderSearch_Ok%>"
                Width="70" />
        </div>
        <% } %>
        <div style="margin-top: 5px; float: right;">
            <asp:Button ID="btnHideUsers" OnClientClick="HideModalPopupCustomers();" runat="server"
                Text="<%$ Resources:Resource,Admin_OrderSearch_Cancel%>" Width="70" />
        </div>
    </div>
    <asp:HiddenField ID="hfSelectedCustomer" runat="server" Value='' />
    <div style="display: none">
        <asp:LinkButton ID="lnkSaveCustomer" runat="server" OnClick="btnSaveCustomer_Click"></asp:LinkButton>
        <%--<asp:Button ID="btnSaveCustomer" runat="server" OnClick="btnSaveCustomer_Click" />        --%>
    </div>
</asp:Panel>
<script type="text/javascript">
    function ShowModalPopupCustomers() {
        document.body.style.overflowX = 'hidden';
        $find('ModalBehaviourCustomers').show();
        document.getElementById('ModalBehaviourCustomers_backgroundElement').onclick = HideModalPopupCustomers;
    }

    function HideModalPopupCustomers() {
        $find("ModalBehaviourCustomers").hide();
        $('select', 'object', 'embed').each(function () {
            $(this).show();
        });
    }

    function ChooseCustomer(customer) {
        $('#<%=hfSelectedCustomer.ClientID%>').attr("value", customer);
        HideModalPopupCustomers();
        document.getElementById('<%=lnkSaveCustomer.ClientID%>').click();
        //        $("#<%=lnkSaveCustomer.UniqueID %>").click();
        //window.__doPostBack('$("#<%=lnkSaveCustomer.UniqueID %>")', '');
        <%=Page.ClientScript.GetPostBackEventReference(lnkSaveCustomer, string.Empty) %>
    }

    // TODO: Дописать логику работы для выбора множественных элементов
    function ChooseMultipleCustomer() {
        HideModalPopupCustomers();
    }
</script>
