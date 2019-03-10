<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="LogError404.aspx.cs" Inherits="Admin.LogError404" %>

<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
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
            var prm = window.Sys.WebForms.PageRequestManager.getInstance();
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
                        if (r) window.__doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
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
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item"><a href="Country.aspx">
                <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
            <li class="neighbor-menu-item"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
            <li class="neighbor-menu-item selected"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
            <li class="neighbor-menu-item"><a href="Localizations.aspx">
                <%= Resource.Admin_Localizations_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_LogError404_Head %>"></asp:Label>
                        <br />
                        <div style="margin-top: 6px;">
                            <asp:CheckBox runat="server" ID="chbEnabled301Redirect" CssClass="checkly-align" OnCheckedChanged="chbEnabled301Redirect_CheckedChanged" AutoPostBack="True" />
                            <label class="form-lbl2" for="<%= chbEnabled301Redirect.ClientID %>"><%= Resource.Admin_CommonSettings_Use_301_Redirects%></label>
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Использовать 301 редиректы
                                    </header>
                                    <div class="help-content">
                                        Включите данную опцию, чтобы активировать работу функции "301й редирект".<br />
                                        <br />
                                        Ознакомьтесь с инструкцией по работе с редиректами.
                                        <br />
                                        <br />
                                        <a href="http://www.advantshop.net/help/pages/redirect-setting" target="_blank">Инструкция. Настройка 301-редиректа</a>
                                    </div>
                                </article>
                            </div>
                        </div>

                    </td>
                    <td style="vertical-align: bottom; padding-right: 10px">
                        <div class="btns-main">
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
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
                        </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span>
                        </span>
                    </td>
                    <td style="text-align: right;">
                        <asp:UpdatePanel ID="upCounts" runat="server">
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
                    <td></td>
                </tr>
            </table>
            <div>
                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                    <table class="filter" cellpadding="2" cellspacing="0">
                        <tr style="height: 5px;">
                            <td colspan="5"></td>
                        </tr>
                        <tr>
                            <td style="width: 70px; text-align: center;">
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="65">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div style="width: 100px; font-size: 0; height: 0;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtUrl" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td  style="width: 260px;">
                                <div style="width: 260px; font-size: 0; height: 0;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtUrlReferer" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td  style="width: 160px;">
                                <div style="width: 160px; font-size: 0; height: 0;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtIpAddress" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td  style="width: 260px;">
                                <div style="width: 260px; font-size: 0; height: 0;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtUserAgent" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>

                            <td style="width: 260px;">
                                <div style="width: 260px; font-size: 0; height: 0;">
                                </div>
                                <asp:DropDownList ID="ddlHasRedirect" TabIndex="18" CssClass="dropdownselect" runat="server">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" Value="any" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 70px;">
                                <div style="width: 70px; font-size: 0; height: 0;">
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
                    </Triggers>
                    <ContentTemplate>
                        <div style="display: none;" id="pnlErrors" runat="server">
                        </div>
                        <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                            CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                            CssClass="tableview" Style="cursor: pointer; word-break: break-all;" GridLines="None" OnRowCommand="grid_RowCommand"
                            OnSorting="grid_Sorting">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="Label01" runat="server" Text='0'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="width: 60px; height: 0; font-size: 0;">
                                        </div>
                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        <asp:Image ID="arrowID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle CssClass="checkboxcolumn" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Url">
                                    <HeaderTemplate>
                                        <div style="width: 200px; font-size: 0; height: 0;">
                                        </div>
                                        <asp:LinkButton ID="lbUrl" runat="server" CommandName="Sort" CommandArgument="Url">
                                            <%=Resource.Admin_LogError404_Url %>
                                            <asp:Image ID="arrowUrl" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblUrl" runat="server" Text='<%# Eval("Url") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="UrlReferer"  ItemStyle-Width="250px">
                                    <HeaderTemplate>
                                        <div style="width: 200px; font-size: 0; height: 0;">
                                        </div>
                                        <asp:LinkButton ID="lbUrlReferer" runat="server" CommandName="Sort" CommandArgument="UrlReferer">
                                            <%=Resource.Admin_LogError404_UrlReferer %>
                                            <asp:Image ID="arrowUrlReferer" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="txtUrlReferer" runat="server" Text='<%# Eval("UrlReferer") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="IpAddress" ItemStyle-Width="150px">
                                    <HeaderTemplate>
                                        <div style="width: 150px; font-size: 0; height: 0;">
                                        </div>
                                        <asp:LinkButton ID="lbIpAddress" runat="server" CommandName="Sort" CommandArgument="IpAddress">
                                            <%=Resource.Admin_LogError404_IpAddress %>
                                            <asp:Image ID="arrowIpAddress" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="txtIpAddress" runat="server" Text='<%# Eval("IpAddress") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="UserAgent" ItemStyle-Width="250px">
                                    <HeaderTemplate>
                                        <div style="width: 250px; font-size: 0; height: 0;">
                                        </div>
                                        <asp:LinkButton ID="lbUserAgent" runat="server" CommandName="Sort" CommandArgument="UserAgent">
                                            <%=Resource.Admin_LogError404_UserAgent %>
                                            <asp:Image ID="arrowUserAgent" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="txtUserAgent" runat="server" Text='<%# Eval("UserAgent") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RedirectTo" ItemStyle-Width="250px">
                                    <HeaderTemplate>
                                        <div style="width: 250px; font-size: 0; height: 0;">
                                        </div>
                                        <%=Resource.Admin_LogError404_Redirect %>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <%# SQLDataHelper.GetString(Eval("RedirectTo")).IsNotEmpty() 
                                            ? string.Format("<input type='checkbox' readonly='readonly' checked='checked' /><span class='pad-left break-word'>{0}</span>", Eval("RedirectTo"))
                                            : string.Format("<input type='checkbox' readonly='readonly' /><a href='javascript:void(0);' class='pad-left redirect-create'>{0}</a>", Resource.Admin_LogError404_CreateRedirect) %>
                                        <asp:TextBox runat="server" ID="txtRedirectTo" Width="225px" Style="display: none;" CssClass="redirect-new" Text='<%# Eval("RedirectTo") %>' />
                                    </EditItemTemplate>
                                    <HeaderStyle Width="250" />
                                </asp:TemplateField>
                                <asp:TemplateField  ItemStyle-Width="60px">
                                    <EditItemTemplate>
                                        <input id="btnAdd" name="btnUpdate" class="ineditbtn showtooltip" type="image"
                                            src="images/addbtn.gif" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                            style="display: none" title='<%= Resource.Admin_LogError404_AddRedirect%>' />
                                        <asp:ImageButton ID="buttonDelete" runat="server"
                                            CssClass="outeditbtn showtooltip valid-confirm" CommandName="DeleteError404" CommandArgument='<%# Eval("ID")%>'
                                            data-confirm="<%$ Resources:Resource, Admin_301Redirect_Confirmation %>"
                                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' ImageUrl="images/deletebtn.png" />
                                        <input id="btnCancel" name="btnCancel" class="ineditbtn showtooltip" type="image"
                                            src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                            style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                    </EditItemTemplate>
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resource.Admin_Catalog_NoRecords%>
                                </div>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                        <table class="results2">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <%=Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                        runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem Selected="True">20</asp:ListItem>
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
            <input type="hidden" id="SelectedIds" name="SelectedIds" />
        </div>
        <script type="text/javascript">
            $("body").on("click", ".redirect-create", function () {
                $(this).closest("tr").find(".ineditbtn").show();
                $(this).closest("tr").find(".outeditbtn").hide();
                $(this).hide();
                $(this).siblings(".redirect-new").show();
            });
            $("body").on("row_canceledit", function () {
                $(".ineditbtn").hide();
                $(".outeditbtn").show();
                $(".redirect-create").show();
                $(".redirect-new").hide();
            });

            function showNotify() {
                if ($("#<%=pnlErrors.ClientID%>").html().length) {
                    notify($("#<%=pnlErrors.ClientID%>").html(), notifyType.error, false, { timer: 10000 });
                }
            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                showNotify();
            });
        </script>
    </div>
</asp:Content>
