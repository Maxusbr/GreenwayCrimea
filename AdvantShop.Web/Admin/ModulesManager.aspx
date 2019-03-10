<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin.ModulesManager"
    MasterPageFile="MasterPageAdmin.master" CodeBehind="ModulesManager.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Trial" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="contentMain" runat="server" ContentPlaceHolderID="cphMain">
    <div id="inprogress" style="display: none">
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
    <ul class="two-column">
        <li class="two-column-item">
            <h2><%= Resource.Admin_ModulesManager_ModulesHeader %></h2>
            <ul class="list-products-count">
                <li class="list-products-count-item">
                    <a href="ModulesManager.aspx?show=all" class="list-products-count-lnk ">
                        <span class="list-products-count-cat list-products-count-all"><%= Resource.Admin_ModulesManager_AllModules %></span>
                        <span class="list-products-count-number"><%=ModulesCountAll %></span>
                    </a>
                </li>
                <li class="list-products-count-item">
                    <a href="ModulesManager.aspx?show=active" class="list-products-count-lnk ">
                        <span class="list-products-count-cat list-products-count-withoutcategory"><%= Resource.Admin_ModulesManager_ActiveModules %></span>
                        <span class="list-products-count-number"><%=ModulesCountActive %></span>
                    </a>
                </li>
                <li class="list-products-count-item">
                    <a href="ModulesManager.aspx?show=popular" class="list-products-count-lnk ">
                        <span class="list-products-count-cat list-products-count-best"><%= Resource.Admin_ModulesManager_PopularModules %></span>
                        <span class="list-products-count-number"><%=ModulesCountPopular %></span>
                    </a>
                </li>
                <li class="list-products-count-item">
                    <a href="ModulesManager.aspx?show=new" class="list-products-count-lnk ">
                        <span class="list-products-count-cat list-products-count-new"><%= Resource.Admin_ModulesManager_NewModules %></span>
                        <span class="list-products-count-number"><%=ModulesCountNew %></span>
                    </a>
                </li>
            </ul>
            <div style="border-bottom: 1px solid #cbcbcb; margin: 20px 0;"></div>
            <div class="module-search">
                <h2>Поиск</h2>
                <asp:TextBox runat="server" ID="txtSearchModule" placeholder="<%$ Resources:Resource, Admin_ModuleManager_ModuleNameSearch %>" CssClass="module-search-inp" />
                <div class="module-search-b">
                    <asp:Button runat="server" ID="btnSearchModule" Text="<%$ Resources:Resource, Admin_ModuleManager_ModuleSearch %>" OnClick="btnSearchModule_Click"
                        CssClass="module-search-btn btn btn-action btn-middle" />
                </div>
            </div>

        </li>
        <li class="two-column-item">

            <div class="content-own">
                <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ModuleManager_Header %>" />
                <div style="padding: 20px 0 10px 0">
                    <asp:Label ID="lTrialMode" runat="server" Text="<%$ Resources:Resource, Admin_Module_TrialMode %>" ForeColor="Red" />
                </div>
                <script>
                    function progressShow() {
                        document.getElementById('inprogress').style.display = 'block';
                    }
                    function progressHide() {
                        document.getElementById('inprogress').style.display = 'none';
                    }

                    var searchTimeout = null;
                    var isSearch = false;
                    $("body").on("keyup", ".module-search-inp", function () {

                        var text = $(this).val();

                        if (searchTimeout) clearTimeout(searchTimeout);

                        searchTimeout = setTimeout(function () {

                            if (text.length > 2) {
                                $(".module-search-btn").click();
                                isSearch = true;
                            } else if (isSearch) {
                                $(".module-search-btn").click();
                                isSearch = false;
                            }
                        }, 300);

                    });
                </script>
                <div style="min-height: 690px; width: 100%; overflow: auto;">

                    <asp:UpdatePanel runat="server" ID="upModules">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearchModule" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>

                            <asp:ListView runat="server" ID="lvModulesManager" ItemPlaceholderID="pl" OnItemCommand="lvModules_ItemCommand" Visible="True">
                                <LayoutTemplate>
                                    <div>
                                        <ul class="modules-list">
                                            <asp:PlaceHolder runat="server" ID="pl" />
                                        </ul>
                                    </div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li class="modules-item-item">
                                        <div class="modules-item-pic-cell">
                                            <div class="modules-item-pic-wrap">
                                                <a class="modules-item-pic-lnk inset-shadow" href="javascript:void(0);" onclick="<%# !string.IsNullOrEmpty(Convert.ToString(Eval("DetailsLink"))) ? "parent.open('"+ Eval("DetailsLink") +"', 'AdvantshopDetails')"  : "return false;" %>">
                                                    <img alt="<%# Eval("Name") %>" src="<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Icon"))) ? Eval("Icon").ToString() : "images/new_admin/modules/nophoto.jpg" %>"
                                                        class="modules-item-pic">
                                                </a>
                                            </div>
                                            <div runat="server" visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("DetailsLink"))) %>' class="modules-item-more">
                                                <a href="javascript:void(0);" onclick="<%#!string.IsNullOrEmpty(Convert.ToString(Eval("DetailsLink"))) ? "parent.open('"+ Eval("DetailsLink") +"', 'AdvantshopDetails')" : "return false;" %>" class="modules-item-more-lnk">
                                                    <%= Resource.Admin_Module_More %>
                                                </a>
                                            </div>
                                        </div>
                                        <div class="modules-item-info">
                                            <div class="modules-item-title">
                                                <a class="modules-item-title-lnk" target="_blank" href="<%# !string.IsNullOrEmpty(Convert.ToString(Eval("DetailsLink"))) ? Eval("DetailsLink") : "javascript:void(0);" %>">
                                                    <%# Eval("Name") %>
                                                </a>
                                                <div class="modules-item-checkactive" runat="server" visible='<%# Convert.ToBoolean(Eval("IsInstall")) %>'>
                                                    <asp:Label ID="lblActiveModuleInfo" runat="server" Text="<%$ Resources:Resource, Admin_Module_ModuleActive %>"></asp:Label>&nbsp;
                                                <input type="checkbox" id="ckbActiveModule" runat="server" checked='<%# Convert.ToBoolean( Eval("Enabled")) %>'
                                                    class="ckbActiveModule" data-modulestringid='<%# Eval("StringId") %>' />
                                                </div>
                                                <asp:HiddenField ID="hfLastVersion" runat="server" Value='<%# Eval("Version") %>' />
                                                <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id") %>' />
                                            </div>
                                            <div class="modules-item-descr">
                                                <%# Eval("BriefDescription") %>
                                            </div>
                                            <div class="justify">
                                                <div class="justify-item">
                                                    <div runat="server" visible='<%# !Convert.ToBoolean(Eval("IsInstall")) %>'
                                                        class="modules-item-module-price">
                                                        <%#  Convert.ToDecimal(Eval("Price")) != 0 ? String.Format("{0:##,##0.##}", Eval("Price")) + " " + Eval("Currency") : Resource.Admin_Modules_FreeCost%>
                                                    </div>
                                                    <a class="btn btn-middle btn-action" runat="server" href='<%# "Module.aspx?module=" + Eval("StringId") %>' visible='<%#Convert.ToBoolean(Eval("HasSettings")) && Convert.ToBoolean(Eval("IsInstall")) %>'>
                                                        <%= Resource.Admin_ModulesManager_Settings %></a>
                                                </div>
                                                <%if(!TrialService.IsTrialEnabled) { %>
                                                <div class="justify-item">
                                                    <input type="button" onclick='<%# "progressShow();" + string.Format("installModule(\"{0}\",\"{1}\",\"{2}\")", Eval("StringId"), Eval("Id"),Eval("Version") ) %>'
                                                        runat="server" id="btnInstall" class="btn btn-middle btn-submit" value='<%$ Resources:Resource, Admin_ModulesManager_Install %>'
                                                        visible='<%# !Convert.ToBoolean(Eval("IsInstall"))%>' />

                                                    <a runat="server" visible='<%# Convert.ToBoolean(Eval("IsInstall")) && !(Convert.ToBoolean(Eval("Active")) && !Convert.ToString(Eval("Version")).Equals(Convert.ToString(Eval("CurrentVersion")))) %>'
                                                        class="btn btn-middle btn-disabled" href="javascript:void(0);">
                                                        <%= Resource.Admin_ModulesManager_Installed %>
                                                    </a>

                                                    <asp:Button OnClientClick="progressShow();" ID="btnInstallLastVersion" runat="server"
                                                        CssClass="btn btn-middle btn-update" CommandArgument='<%# Eval("StringId") %>'
                                                        CommandName="InstallLastVersion" Text='<%$ Resources : Resource, Admin_Modules_Update%>'
                                                        Visible='<%# Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("Active")) && !Convert.ToString(Eval("Version")).Equals(Convert.ToString(Eval("CurrentVersion"))) %>' />

<%--                                                    <a class="btn btn-middle btn-action" runat="server" href='<%# Eval("DetailsLink") %>'
                                                        visible='<%# !Convert.ToBoolean(Eval("IsInstall")) && !Convert.ToBoolean(Eval("Active")) %>'
                                                        target="_blank">
                                                        <%= Resource.Admin_ModulesManager_Buy %>
                                                    </a>--%>
                                                </div>
                                                <%} %>
                                            </div>
                                            <% if (!TrialService.IsTrialEnabled)
                                               { %>
                                            <asp:LinkButton CssClass="module-delete" runat="server" ID="btnDelete" CausesValidation="false"
                                                CommandArgument='<%# Eval("StringId") %>' CommandName="Uninstall" Visible='<%# Convert.ToBoolean(Eval("IsInstall")) %>'>
                                            <img src="images/deletebtn.png" onclick="if(confirm('<%= Resource.Admin_ThemesSettings_Confirmation %>')){ progressShow(); }else{ return false;}" alt='<%= Resource.Admin_ModulesManager_Delete %>' />
                                            </asp:LinkButton>
                                            <% } %>
                                        </div>
                                    </li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <%= Resource.Admin_ModulesManager_NotFounded %>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <div class="modules-paging">
                                <adv:AdvPaging runat="server" ID="paging" DisplayArrows="false" DisplayPrevNext="false" DisplayShowAll="false" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </li>
    </ul>
    <script>
        $(function () {

            $(document).on('keydown.pagenumber', function (e) {
                //37 - left arrow
                //39 - right arrow
                if (e.ctrlKey === true && e.keyCode === 37) {
                    if ($("#paging-prev").length)
                        document.location = $("#paging-prev").attr("href");
                } else if (e.ctrlKey === true && e.keyCode === 39) {
                    if ($("#paging-next").length)
                        document.location = $("#paging-next").attr("href");
                }
            });
        });


        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { progressHide(); });
    </script>
</asp:Content>
