<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditStaticPage" CodeBehind="StaticPage.aspx.cs" %>

<%@ Import Namespace="Resources" %>


<%@ Register Src="~/Admin/UserControls/PopupTreeView.ascx" TagName="PopupTree" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/StaticPageRightNavigation.ascx" TagName="StaticPageRightNavigation"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Menu.aspx">
                <%= Resource.Admin_MasterPageAdmin_MainMenu%></a></li>
            <li class="neighbor-menu-item"><a href="NewsAdmin.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsMenuRoot%></a></li>
            <li class="neighbor-menu-item"><a href="NewsCategory.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsCategory%></a></li>
            <li class="neighbor-menu-item"><a href="Carousel.aspx">
                <%= Resource.Admin_MasterPageAdmin_Carousel%></a></li>
            <li class="neighbor-menu-item selected"><a href="StaticPages.aspx">
                <%= Resource.Admin_MasterPageAdmin_AuxPagesMenuItem%></a></li>
            <li class="neighbor-menu-item"><a href="StaticBlocks.aspx">
                <%= Resource.Admin_MasterPageAdmin_PageParts%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="EditNews.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_News %></a>, 
            <a href="StaticPage.aspx" class="panel-add-lnk"><%= Resource.Admin_MasterPageAdmin_StaticPage %></a>
        </div>
    </div>
    <div style="margin-left: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="width: 100%">
                    <table width="98%">
                        <tr>
                            <td style="width: 72px;">
                                <img src="images/orders_ico.gif" alt="" />
                            </td>
                            <td>
                                <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_lblMain %>"></asp:Label><br />
                                <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_AuxCreate %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Admin/StaticPages.aspx" Text='<%$ Resources: Resource, Admin_Back %>'
                                    runat="server" CssClass="Link"></asp:HyperLink>
                            </td>
                            <td>
                                <div class="btns-main">
                                    <div style="padding: 10px 10px 15px 0; display: inline-block">
                                        <a target="_blank" class="Link" runat="server" id="aToClient" enableviewstate="false" href="#">
                                            <%=Resource. Admin_Product_Link_ShowInAdmin %>
                                        </a>
                                    </div>
                                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" OnClick="btnSave_Click" onmousedown="window.onbeforeunload=null;" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="spAuxFoundNotification" runat="server" style="color: blue;"></span>
                                <asp:Label ID="Message" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table id="tabs">
                        <tr>
                            <td style="width: 200px;">
                                <div style="width: 100px; font-size: 0; line-height: 0px;">
                                </div>
                                <ul id="tabs-headers">
                                    <li id="general">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_TabGeneral%>" />
                                        <img class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="seo">
                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_TabSeo  %>" />
                                        <img class="floppy" src="images/floppy.gif" />
                                    </li>
                                </ul>
                                <input type="hidden" runat="server" class="tabid" name="tabid" id="tabid" value="1" />
                            </td>
                            <td id="tabs-contents">
                                <div class="tab-content">
                                    <table class="info-tb">
                                        <tr class="rowPost">
                                            <td colspan="2" style="height: 34px;">
                                                <h4 style="display: inline; font-size: 10pt;">
                                                    <asp:Localize ID="lzGeneral" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_HeadGeneral%>"></asp:Localize></h4>
                                                <hr color="#C2C2C4" size="1px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_PageName%></span> <span style="color: red;">*</span>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtPageName" runat="server" Width="400px"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_UrlSynonym%></span><span style="color: red;">
                                                        *</span>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtSynonym" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_Enabled%></span>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_Parent%></span>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upTree" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="TreeNodeSelected" />
                                                        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="Hiding" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Label runat='server' ID="lblParentName" Text="<%$ Resources: Resource, Admin_StaticPage_Root %>"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hfParentId" />
                                                        <asp:LinkButton runat="server" ID="btnSelectParent" Text="<%$ Resources: Resource, Admin_StaticPage_SelectParent %>"
                                                            CssClass="Link" OnClick="btnSelectParent_Click" />
                                                        <adv:PopupTree runat="server" ID="popTree" Type="StaticPage" OnTreeNodeSelected="popTree_TreeNodeSelected" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_Text%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" ID="fckPageText" Height="500px" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_SortOrder%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtSortOrder" Text="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_IndexAtSiteMap%></span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIndexAtSitemap" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-content">
                                    <asp:HiddenField ID="hfMetaId" runat="server" />
                                    <table class="info-tb">
                                        <tr class="rowPost">
                                            <td colspan="2" style="height: 34px;">
                                                <h4 style="display: inline; font-size: 10pt;">
                                                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_HeadSeo%>"></asp:Localize></h4>
                                                <hr color="#C2C2C4" size="1px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span><%=Resources.Resource.Admin_StaticPage_Title%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPageTitle" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>H1</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtH1" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_MetaKeyWords%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetaKeywords" runat="server" Width="400px" Height="85px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_StaticPage_MetaDescription%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetaDescription" runat="server" Width="400px" Height="85px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="top" width="400">
                                                <asp:Localize ID="Localize2" Text="<%$ Resources: Resource, Admin_StaticPage_UseGlobalVariables %>"
                                                    runat="server"></asp:Localize>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <%=AdvantShop.Helpers.HtmlHelper.RenderSplitter()%>
                <td class="rightNavigation">
                    <div id="rightPanel" class="rightPanel">
                        <adv:StaticPageRightNavigation ID="StaticPageRightNavigation" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var szCookieString = document.cookie;
            if ($.cookie("isVisibleRightPanel") != "false") {
                showRightPanel();
            }

            function showRightPanel() {
                document.getElementById("rightPanel").style.display = "block";
                document.getElementById("right_divHide").style.display = "block";
                document.getElementById("right_divShow").style.display = "none";
            }
        });

        function toggleRightPanel() {
            if ($.cookie("isVisibleRightPanel") == "true") {
                $("div:.rightPanel").hide("fast");
                $("div:.right_hide_rus").hide("fast");
                $("div:.right_show_rus").show("fast");
                $("div:.right_hide_en").hide("fast");
                $("div:.right_show_en").show("fast");
                $.cookie("isVisibleRightPanel", "false", { expires: 7 });
            } else {
                $("div:.rightPanel").show("fast");
                $("div:.right_show_rus").hide("fast");
                $("div:.right_hide_rus").show("fast");
                $("div:.right_show_en").hide("fast");
                $("div:.right_hide_en").show("fast");
                $.cookie("isVisibleRightPanel", "true", { expires: 7 });
            }
        }


        var dirty = false;
        var skipChecking = false;

        function checkForDirty(e) {
            if (!skipChecking) {
                var evt = e || window.event;
                if (dirty) {
                    evt.returnValue = '<%=Resources.Resource.Admin_Product_LosingChanges%>';
            }
        } else {
            skipChecking = false;
        }
    }

    $(document).ready(function () {

        $("#<%= btnSave.ClientID %>").click(function () { skipChecking = true; });

            $("#<%=txtSynonym.ClientID %>").on("focus", function () {
                var text = $('#<%=txtPageName.ClientID %>').val();
                var url = $('#<%=txtSynonym.ClientID %>').val();
                if ((text != "") & (url == "")) {
                    $('#<%=txtSynonym.ClientID %>').val(translite(text));
                }
            });
        });

    </script>
</asp:Content>
