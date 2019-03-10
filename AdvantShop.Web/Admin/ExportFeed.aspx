<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.ExportFeedPage" CodeBehind="ExportFeed.aspx.cs" %>

<%@ Import Namespace="AdvantShop.ExportImport" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="adv" TagName="ExportFeedListUc" Src="~/Admin/UserControls/ExportFeed/ExportFeedListUc.ascx" %>
<%@ Register TagPrefix="adv" TagName="ExportFeedCatalogTab" Src="~/Admin/UserControls/ExportFeed/ExportFeedCatalogTab.ascx" %>
<%@ Register TagPrefix="adv" TagName="ExportFeedSettingsTab" Src="~/Admin/UserControls/ExportFeed/ExportFeedSettingsTab.ascx" %>
<%@ Register TagPrefix="adv" TagName="ExportFeedCsvFieldsSettingsTab" Src="~/Admin/UserControls/ExportFeed/ExportFeedCsvFieldsSettingsTab.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        function confirmDelete() {
            return confirm(localize("Admin_ExportFeed_ConfirmDelete"));
        }
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
            <li class="neighbor-menu-item"><a href="Catalog.aspx">
                <%= Resource.Admin_MasterPageAdmin_CategoryAndProducts %></a> </li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="ProductsOnMain.aspx?type=New"
                class="neighbor-menu-lnk">
                <%= Resource.Admin_MasterPageAdminCatalog_FirstPageProducts%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=Best">
                            <%= Resource.Admin_MasterPageAdminCatalog_BestSellers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=New">
                            <%= Resource.Admin_MasterPageAdminCatalog_New %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=Sale">
                            <%= Resource.Admin_MasterPageAdminCatalog_Discount %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductLists.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ProductLists %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Properties.aspx">
                <%= Resource.Admin_MasterPageAdmin_Directory%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Properties.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ProductProperties%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Colors.aspx">
                            <%= Resource.Admin_MasterPageAdmin_ColorDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Sizes.aspx">
                            <%= Resource.Admin_MasterPageAdmin_SizeDictionary%>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Tags.aspx">
                            <%= Resource.Admin_Tags_Header %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item selected"><a href="ExportFeed.aspx">
                <%= Resource.Admin_MasterPageAdmin_Export%></a></li>
            <li class="neighbor-menu-item"><a href="ImportCSV.aspx">
                <%= Resource.Admin_MasterPageAdmin_Import%></a></li>
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item"><a href="Reviews.aspx">
                <%= Resource.Admin_MasterPageAdmin_Reviews%></a></li>
        </menu>
    </div>

    <ul class="two-column">
        <li class="two-column-item">
            <adv:ExportFeedListUc ID="exportFeedListUc" runat="server" />
        </li>
        <li class="two-column-item">
            <asp:Panel ID="pnlMain" runat="server">
                <div style="margin: 0 0 15px 0">
                    <h2 class="header-edit">
                        <asp:Literal ID="lblRightHead" runat="server" Text="<%$ Resources:Resource, Admin_Region_SubHeader %>" />
                        <br />
                        <span class="AdminSubHead">
                            <asp:Literal ID="ltrlExportTypeName" runat="server" />
                            <br />
                            <asp:Literal ID="ltrlAdditionalInfo" runat="server" />
                            <br />
                            <span id="blockFileLink" runat="server">
                                <span>Ссылка на файл: </span>
                                <asp:HyperLink ID="lnkFile" runat="server" Target="Blank"></asp:HyperLink>
                            </span>
                        </span>
                    </h2>
                    <a class="btn btn-middle btn-add" href="" style="float: right; margin-top: 30px;" runat="server" id="btnExport"><%= Resource.Admin_ExportFeed_ExportButton %></a>
                    <asp:LinkButton CssClass="btn btn-middle btn-action" Style="float: right; margin: 30px 10px 0px 0px;" runat="server" ID="btnDelete" OnClientClick="return confirmDelete();" OnClick="btnDelete_OnClick">Удалить</asp:LinkButton>
                </div>
                <div class="ui-tabs tabs-headers" data-plugin="tabs">

                    <ul class="ui-tabs-nav">
                        <li data-tabs-header="true" id="tab-catalog">
                            <span><% = Resources.Resource.Admin_ExportFeed_ChooseProduct%></span></li>
                        <li data-tabs-header="true" <%= CurrentExportFeed != null && (CurrentExportFeed.Type == EExportFeedType.Csv || CurrentExportFeed.Type == EExportFeedType.Reseller) ? string.Empty : "class=\"tab-hidden\"" %> id="tab-csvfields">
                            <span>Поля для выгрузки</span>
                        </li>

                        <li data-tabs-header="true" id="tab-settings">
                            <span><% = Resources.Resource.Admin_ExportFeed_ModuleSettings %></span></li>
                    </ul>
                    <div class="ui-tabs-panel tabs-contents">
                        <div data-tabs-content="true">
                            <adv:ExportFeedCatalogTab ID="exportFeedCatalogTab" runat="server" />
                        </div>
                        <div data-tabs-content="true">
                            <adv:ExportFeedCsvFieldsSettingsTab ID="exportFeedCsvFieldsSettingsTab" runat="server" />
                        </div>
                        <div data-tabs-content="true">
                            <adv:ExportFeedSettingsTab ID="exportFeedSettingsTab" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="dvSubHelp" id="blockHelp" runat="server">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                    <asp:HyperLink runat="server" ID="linkHelp" Target="blank"></asp:HyperLink>
                    <%--<a href="http://www.advantshop.net/help/pages/import-csv" target="_blank">Инструкция. Импорт и экспорт данных в формате CSV (Excel)</a>--%>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlEmpty" runat="server" Style="text-align: center" Visible="false">
                <div class="AdminSaasNotify">
                    <h2>Нет выгрузок
                    </h2>
                </div>
                <%--<asp:LinkButton CssClass="btn btn-middle btn-add" ID="btnAddExportFeed" runat="server"
                    Text="<%$ Resources:Resource, Admin_Add %>" />--%>
            </asp:Panel>
        </li>
    </ul>

    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
    </script>
</asp:Content>
