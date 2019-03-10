<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditNews" Codebehind="EditNews.aspx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/UserControls/EditMetaFields.ascx" TagName="EditMetaFields" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/News/RightNavigation.ascx" TagName="RightNavigation" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/News/NewsProducts.ascx" TagName="NewsProducts" TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:UpdateProgress runat="server" ID="uprogress">
        <ProgressTemplate>
            <div id="inprogress">
                <div id="curtain" class="opacitybackground">
                    &nbsp;
                </div>
                <div class="loader">
                    <table width="100%" style="font-weight: bold; text-align: center;">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <img src="images/ajax-loader.gif" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="color: #0D76B8;">
                                    <asp:Localize ID="Localize_Admin_Product_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Product_PleaseWait %>"
                                        EnableViewState="false"></asp:Localize>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Menu.aspx">
                <%= Resource.Admin_MasterPageAdmin_MainMenu%></a></li>
            <li class="neighbor-menu-item selected"><a href="NewsAdmin.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsMenuRoot%></a></li>
            <li class="neighbor-menu-item"><a href="NewsCategory.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsCategory%></a></li>
            <li class="neighbor-menu-item"><a href="Carousel.aspx">
                <%= Resource.Admin_MasterPageAdmin_Carousel%></a></li>
            <li class="neighbor-menu-item"><a href="StaticPages.aspx">
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

    <div class="content-own">
        <table style="width: 99%;">
            <tr>
                <td style="vertical-align: top; width: 100%; padding: 0 5px 0 0;">
                    <table style="width: 98%;">
                        <tr>
                            <td style="width: 72px;">
                                <img src="images/orders_ico.gif" alt="" />
                            </td>
                            <td>
                                <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Header %>" /><br />
                                <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_EditNews_AddingNews %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink NavigateUrl="~/admin/NewsAdmin.aspx" Text='<%$ Resources: Resource, Admin_Back %>' 
                                    runat="server" CssClass="Link" />
                            </td>
                            <td>
                                <div class="btns-main">
                                    <div style="padding: 10px 10px 15px 0; display: inline-block">
                                        <a target="_blank" class="Link" runat="server" id="aToClient" enableviewstate="false" href="#" Visible="False">
                                            <%=Resource. Admin_Product_Link_ShowInAdmin %>
                                        </a>
                                    </div>
                                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" OnClick="btnSave_Click" 
                                        onmousedown="window.onbeforeunload=null;" Text="<%$ Resources:Resource, Admin_Save %>" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <div style="height: 10px;"></div>
                    <table id="tabs">
                        <tr>
                            <td style="width: 200px;">
                                <div style="width: 100px; font-size: 0; line-height: 0px;">
                                </div>
                                <ul id="tabs-headers">
                                    <li id="general">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_TabGeneral%>" />
                                        <asp:Image ID="imgExclMain" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                            CssClass="exclamation" EnableViewState="false" />
                                        <img class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="text">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_EditNews_TabText %>" />
                                        <asp:Image ID="imgExclText" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                            CssClass="exclamation" EnableViewState="false" />
                                        <img class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="products"<%= NewsId == 0 ? " style=\"display: none;\"" : string.Empty %>>
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_EditNews_TabProducts %>" />
                                    </li>
                                    <li id="seo">
                                        <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_TabSeo%>" />
                                        <img class="floppy" src="images/floppy.gif" />
                                    </li>
                                </ul>
                            </td>
                            <td id="tabs-contents">
                                <div class="tab-content">
                                    <table class="info-tb">
                                        <tr class="rowPost">
                                            <td colspan="2" style="height: 34px;">
                                                <span class="spanSettCategory">
                                                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_HeadGeneral%>" /></span>
                                                <hr color="#C2C2C4" size="1px" />
                                            </td>
                                        </tr>
                                        <tr class="rowsPost row-interactive">
                                            <td>
                                                <span><%=Resource.Admin_m_News_Category%></span> <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlNewsCategory" runat="server" DataSourceID="sdsNewsCategories"
                                                    DataTextField="Name" DataValueField="NewsCategoryID" CssClass="niceTextBox">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="rowsPost row-interactive">
                                            <td onclick="focusoninput(this)">
                                                <span><%=Resource.Admin_m_News_Data%></span> <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <span class="dp">
                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="niceTextBox shortTextBoxClass2" />
                                                </span>
                                                <asp:TextBox ID="txtTime" runat="server" CssClass="niceTextBox shortTextBoxClass3" /><span id="validTime" style="color: red; display: none;">*</span>
                                                <ajaxToolkit:MaskedEditExtender ID="meeTime" runat="server" TargetControlID="txtTime" Mask="99:99" AutoComplete="False" MaskType="Time" />
                                                <asp:Image ID="popupDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                            </td>
                                        </tr>
                                        <tr class="rowsPost row-interactive">
                                            <td onclick="focusoninput(this)">
                                                <span><%=Resource.Admin_m_News_Title%></span> <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTitle" runat="server" CssClass="niceTextBox textBoxClass" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>" />
                                            </td>
                                        </tr>
                                        <tr class="rowsPost row-interactive">
                                            <td onclick="focusoninput(this)">
                                                <span><%= Resource.Admin_m_News_Url %></span><span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUrlPath" runat="server" CssClass="niceTextBox textBoxClass" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUrlPath" ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>" />
                                            </td>
                                        </tr>
                                        <tr class="rowsPost row-interactive">
                                            <td>
                                                <span><%=Resource.Admin_m_News_Picture%></span>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlImage" runat="server" Width="100%">
                                                    <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_m_News_CurrentImage %>" /><br />
                                                    <asp:Image ID="imgNewsPicture" runat="server" /><br />
                                                    <asp:Button ID="btnDeleteImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                                        OnClick="btnDeleteImage_Click" />
                                                    <br />
                                                </asp:Panel>
                                                <asp:FileUpload ID="fuNewsPicture" runat="server" Width="308px" Height="20px" /><br/>
                                                <asp:Label ID="lblImageInfo" runat="server" CssClass="info-hint-text" />
                                            </td>
                                        </tr>
                                        <tr class="rowsPost row-interactive">
                                            <td>
                                                <span><%=Resource.Admin_m_News_ShowOnMainPage%></span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkOnMainPage" runat="server" Checked="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-content">
                                    <table class="info-tb">
                                        <tr class="rowPost">
                                            <td style="height: 34px; width: 900px;">
                                                <span class="spanSettCategory">
                                                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_m_News_Annotation %>" /></span> <span style="color: red;">*</span>
                                            </td>
                                        </tr>
                                        <tr class="rowPost">
                                            <td>
                                                <asp:TextBox ID="txtAnnotation" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400" Width="900" />
                                            </td>
                                        </tr>
                                        <tr class="rowPost">
                                            <td style="height: 34px;">
                                                <span class="spanSettCategory">
                                                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_EditNews_Text %>" />:</span> <span style="color: red;">*</span>
                                            </td>
                                        </tr>
                                        <tr class="rowPost">
                                            <td>
                                                <asp:TextBox ID="txtNewsText" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="800" Width="900" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-content">
                                    <adv:NewsProducts ID="newsProducts" runat="server" />
                                </div>
                                <div class="tab-content">
                                    <adv:EditMetaFields ID="editMetaFields" runat="server" Hint="<%$ Resources: Resource, Admin_m_News_UseGlobalVariables %>" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <%=AdvantShop.Helpers.HtmlHelper.RenderSplitter()%>
                <td class="rightNavigation">
                    <div id="rightPanel" class="rightPanel">
                        <adv:RightNavigation ID="rightNavigation" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" runat="server" class="tabid" name="tabid" id="tabid" />
    <asp:SqlDataSource ID="sdsNewsCategories" runat="server" SelectCommand="SELECT [Name], [NewsCategoryID] FROM [Settings].[NewsCategory] ORDER BY SortOrder"
        OnInit="sds_Init"></asp:SqlDataSource>
    <script type="text/javascript">

        function focusoninput(sender) {
            $(sender).parent().find("td:last input").focus();
            $(sender).parent().find("td:last textarea").focus();
        }

        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });

            $(".imgtooltip[abbr]").tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    return $("<img/>").attr("src", $(this).attr("abbr"));
                }
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });

        var skip = false,
            dirty = false;
        function beforeunload(e) {
            if (!skip) {
                if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                    var evt = window.event || e;
                    evt.returnValue = '<%=Resource.Admin_Product_LosingChanges%>';
                }
            } else {
                skip = false;
            }
        }

        $(document).ready(function () {
            if ($.cookie("isVisibleRightPanel") != "false") {
                showRightPanel();
            }

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function() {
                window.onbeforeunload = beforeunload;
            });

            $("#<%= btnSave.ClientID %>").click(function () { skip = true; });

            window.onbeforeunload = beforeunload;
        });

        function showRightPanel() {
            document.getElementById("rightPanel").style.display = "block";
            document.getElementById("right_divHide").style.display = "block";
            document.getElementById("right_divShow").style.display = "none";
        }

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
    </script>
</asp:Content>
