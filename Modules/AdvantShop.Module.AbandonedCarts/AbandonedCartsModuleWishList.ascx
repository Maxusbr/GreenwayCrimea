﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.AbandonedCarts.AbandonedCartsModuleWishList" CodeBehind="AbandonedCartsModuleWishList.ascx.cs" %>
<%@ Register TagPrefix="adv" Namespace="AdvantShop.Core.Controls" Assembly="AdvantShop.Core" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="AdvantShop.Localization" %>

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        $("#SelectedIds").val("");
        initgrid();
    });
</script>
<style type="text/css">
    #tabs-contents div.selected {
        display: inline-block;
        width: auto;
    }
</style>
<div>
    <div style="font-size: 16px; margin: 10px 0;">
        <asp:Localize runat="server" Text="Список желаний зарегистрированных пользователей" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                             CellPadding="0" CellSpacing="0" Confirmation=""
                             CssClass="tableview" Style="cursor: pointer" GridLines="None" 
                             OnSorting="grid_Sorting" ShowFooter="false">
                <Columns>
                    <asp:TemplateField AccessibleHeaderText="CustomerId" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("CustomerId") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("CustomerId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="60px" HeaderStyle-Width="60px"
                                       HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <div style="font-size: 0px; height: 0px; width: 60px;">
                            </div>
                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# ((bool) Eval("IsSelected")) ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />" %>
                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("CustomerId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField AccessibleHeaderText="Email" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                Email
                                <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("Email") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField AccessibleHeaderText="Sum" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                        <HeaderTemplate>
                            <asp:Localize ID="Localize1" runat="server" Text="Сумма" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# RenderPrice((Guid)Eval("CustomerId")) %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="LastUpdate" HeaderStyle-Width="150px">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbLastUpdate" runat="server" CommandName="Sort" CommandArgument="LastUpdate">
                                <asp:Localize runat="server" Text="Время создания" />
                                <asp:Image ID="arrowAdded" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Culture.ConvertDate((DateTime) Eval("LastUpdate")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="SendingCount" ItemStyle-Width="200px">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbSendingCount" runat="server" CommandName="Sort" CommandArgument="SendingCount">
                                <asp:Localize runat="server" Text="Уведомления" />
                                <asp:Image ID="arrowModified" CssClass="arrow" runat="server" ImageUrl="~/Admin/images/arrowdownh.gif" />
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# RenderSendingCount(SQLDataHelper.GetInt(Eval("SendingCount")), SQLDataHelper.GetNullableDateTime(Eval("SendingDate"))) %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# "<a href=\"../Modules/AbandonedCarts/CartInfo.aspx?Id=" + Eval("CustomerId") + "&MasterPageEmpty=" + Request["MasterPageEmpty"] + "&ShoppingCartType=2" + "\" class='showtooltip'><img src='images/editbtn.gif' style='border: none;'  /></a>" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="header" />
                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                <EmptyDataTemplate>
                    <div style="margin-bottom: 20px; margin-top: 20px; text-align: center;">
                        Записей не найдено
                    </div>
                </EmptyDataTemplate>
            </adv:AdvGridView>
            <table class="results2">
                <tr>
                    <td></td>
                    <td align="center">
                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                          UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none"><span id="selectedIdsCount" class="bold">0</span> </div>
    <input type="hidden" id="SelectedIds" name="SelectedIds" />
    <div>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSendLetter" />
            </Triggers>
            <contenttemplate>--%>
                <asp:Label runat="server" ID="lblError" Visible="False" ForeColor="red" />
                <asp:Label runat="server" ID="lblSuccess" Visible="False" ForeColor="green" />
                <div style="padding: 5px 0 15px 0">
                    <asp:Localize ID="Localize2" runat="server" Text="Выберите шаблон" />
                    <asp:DropDownList runat="server" ID="ddlTemplate" AutoPostBack="True" OnSelectedIndexChanged="ddlTemplate_OnSelectedIndexChanged" Width="200px" />
                </div>
                <asp:Panel runat="server" ID="pnlTemplate" Visible="False">
                    <asp:TextBox runat="server" ID="txtSubject" Width="500px" />
                    <br/>
                    <br/>
                     <asp:TextBox ID="ckeBody" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" />
                </asp:Panel>
                <asp:Button runat="server" ID="btnSendLetter" OnClick="btnSendLetter_OnClick" Text="Послать письмо" Enabled="False" Width="150px" />
            <%--</contenttemplate>
        </asp:UpdatePanel>--%>
    </div>
</div>
