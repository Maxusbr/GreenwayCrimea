<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Admin.ProductPage" EnableViewStateMac="false" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Repository.Currencies" %>
<%@ Register Src="~/Admin/UserControls/Product/RightNavigation.ascx" TagName="RightNavigation" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Catalog/SiteNavigation.ascx" TagName="SiteNavigation" TagPrefix="uc1" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductProperties.ascx" TagName="ProductProperties" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductPhotos.ascx" TagName="ProductPhotos" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductPhotos360.ascx" TagName="ProductPhotos360" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductVideos.ascx" TagName="ProductVideos" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/RelatedProducts.ascx" TagName="RelatedProducts" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductGifts.ascx" TagName="ProductGifts" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductCustomOption.ascx" TagName="ProductCustomOption" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductOffers.ascx" TagName="ProductOffers" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductLandingPage.ascx" TagName="ProductLandingPage" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridBrand.ascx" TagName="PopupGridBrand" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Catalog/AddProduct.ascx" TagName="AddProduct" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Catalog/AddProductCopy.ascx" TagName="AddProductCopy" TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <title><%=GetPageTitle()%></title>
    <link rel="stylesheet" href="css/slimbox2.css" type="text/css" media="screen"
        id="slimboxStyle" runat="server" />
    <script type="text/javascript" src="js/slimbox2.js"></script>
    <script type="text/javascript" src="js/ajaxfileupload.js"></script>
    <link rel="stylesheet" href="js/plugins/chosen/css/chosen.css">
    <script type="text/javascript" src="js/plugins/chosen/chosen.jquery.js"></script>
    <script src="js/plugins/chosen/chosen.ajaxaddition.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">

        function focusoninput(sender) {
            $(sender).parent().find("td:last input").focus();
            $(sender).parent().find("td:last textarea").focus();
        }

        function removeunloadhandler(a) {
            window.onbeforeunload = null;
        }

        function endRequest() {
            window.onbeforeunload = beforeunload;
            window.onbeforeunload = beforeunload;
            $(".photoinput").val("");
        }

        var skip = false;
        var dirty = false;

        function beforeunload(e) {
            if (!skip) {
                if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                    var evt = window.event || e;
                    evt.returnValue = '<%=Resources.Resource.Admin_Product_LosingChanges%>';
                }
            } else {
                skip = false;
            }
        }

        function addbeforeunloadhandler() {
            window.onbeforeunload = beforeunload;
        }

        $(document).ready(function () {
            var szCookieString = document.cookie;
            if ($.cookie("isVisibleRightPanel") != "false") {
                showRightPanel();
            }

            window.onbeforeunload = beforeunload;

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);

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
    <style type="text/css">
        h2 {
            margin: 1px 0;
            font-size: 11pt;
            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
            font-weight: bold;
        }
    </style>
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
    <input type="hidden" name="tabid" id="tabid" class="tabid" runat="server" />
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="Catalog.aspx">
                <%= Resource.Admin_MasterPageAdmin_CategoryAndProducts %></a></li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="ProductsOnMain.aspx?type=New">
                <%= Resource.Admin_MasterPageAdminCatalog_FirstPageProducts%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=new">
                            <%= Resources.Resource.Admin_MasterPageAdminCatalog_New %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=best">
                            <%= Resources.Resource.Admin_MasterPageAdminCatalog_BestSellers %>
                        </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="ProductsOnMain.aspx?type=sale">
                            <%= Resources.Resource.Admin_MasterPageAdminCatalog_Discount %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item dropdown-menu-parent"><a href="Properties.aspx">
                <%= Resources.Resource.Admin_MasterPageAdmin_Directory%></a>
                <div class="dropdown-menu-wrap">
                    <ul class="dropdown-menu">
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Properties.aspx">Свойства товаров </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Colors.aspx">Справочник цветов </a></li>
                        <li class="dropdown-menu-item m-item"><a class="main-menu-subitem-lnk" href="Sizes.aspx">Справочник размеров </a></li>
                    </ul>
                </div>
            </li>
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx">
                <%= Resource.Admin_MasterPageAdmin_Export%></a></li>
            <li class="neighbor-menu-item"><a href="ImportCSV.aspx">
                <%= Resource.Admin_MasterPageAdmin_Import%></a></li>
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item"><a href="Reviews.aspx">
                <%= Resource.Admin_MasterPageAdmin_Reviews%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div id="notify" style="position: absolute; top: 5px; right: 5px; width: 350px;">
        </div>
        <adv:AddProduct ID="addProduct" runat="server" />
        <adv:AddProductCopy ID="addProductCopy" runat="server" />

        <table style="width: 100%;">
            <tr>
                <td style="width: 10px;"></td>
                <td style="vertical-align: top; width: 100%; padding: 0 5px 0 0;">
                    <div style="width: 800px; font-size: 0px;">
                    </div>
                    <table style="width: 100%;" cellpadding="0" cellspacing="0" id="tabs">
                        <tr>
                            <td style="width: 235px;">
                                <div style="min-height: 150px; text-align: center; margin-bottom: 10px; margin-right: 10px;">
                                    <asp:UpdatePanel ID="upPhoto" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="productPhotos" EventName="MainPhotoUpdate" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Literal ID="ltPhoto" runat="server" Text='<%# HtmlProductImage()%>' EnableViewState="false"></asp:Literal>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <ul id="tabs-headers">
                                    <li id="general">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Main %>"
                                            EnableViewState="false" />
                                        <asp:Image ID="imgWarningTab1" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                            CssClass="exclamation" EnableViewState="false" />
                                        <img id="itab1floppy" name="itab1floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="cat" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, Admin_Product_Categories%>"
                                            EnableViewState="false" /></span> </li>
                                    <li id="price">
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, Admin_Product_PriceAvailability  %>"
                                            EnableViewState="false" />
                                        <asp:Image ID="imgWarningTab3" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                            CssClass="exclamation" />
                                        <img id="itab2floppy" name="itab2floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="photos" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Photos %>"
                                            EnableViewState="false" /></span> </li>
                                    <li id="videos" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Videos %>"
                                            EnableViewState="false" /></span> </li>
                                    <li id="descr">
                                        <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:Resource, Admin_Product_Description %>"
                                            EnableViewState="false" />
                                        <input type="hidden" id="tab4floppy" name="tab4floppy" value="false" /><img id="itab4floppy"
                                            name="itab4floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="prop" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, Admin_Product_ProductProperties %>"
                                            EnableViewState="false" /></span> </li>
                                    <li id="customopt" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>">
                                        <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, Admin_Product_CustomOptions%>"
                                            EnableViewState="false" />
                                        <asp:Image ID="imgExcl6" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                            CssClass="exclamation" EnableViewState="false" />
                                        <img id="itab6floppy" name="itab6floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="seo">
                                        <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, Admin_Product_SEO  %>"
                                            EnableViewState="false" />
                                        <asp:Image ID="imgExcl7" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                            CssClass="exclamation" EnableViewState="false" />
                                        <img id="itab7floppy" name="itab7floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                    </li>
                                    <li id="related" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="lRelatedProduct" runat="server" Text="" EnableViewState="false" /></span>
                                    </li>
                                    <li id="alt" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="lAlternativeProduct" runat="server" Text="" EnableViewState="false" /></span>
                                    </li>
                                    <li id="gift" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="lGifts" runat="server" Text="<%$ Resources:Resource, Admin_Product_Gifts%>" EnableViewState="false" /></span>
                                    </li>
                                    <li id="landing" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                        <asp:Literal ID="Literal10" runat="server" Text="Посадочная страница" EnableViewState="false" /></span>
                                    </li>

                                    <asp:ListView ID="lvModuleTabs" runat="server">
                                        <ItemTemplate>
                                            <li id='<%# "moduletab_" + (Container.DataItemIndex + 1) %>'>
                                                <asp:Literal runat="server" Text='<%#Eval("Name") %>' EnableViewState="false" />
                                            </li>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ul>
                            </td>
                            <td style="height: 100px; min-height: 100px; max-height: 100px;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lProductName" CssClass="AdminHead" runat="server" EnableViewState="false"></asp:Label><br />
                                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Product_SubHeader %>"
                                                EnableViewState="false"></asp:Label>
                                            <br />
                                            <br />
                                        </td>
                                        <td style="text-align: right; min-width: 155px;">
                                            <div style="padding: 10px 0 15px 0; min-width: 150px; display: inline-block;">
                                                <a target="_blank" class="Link" runat="server" id="aToClient" enableviewstate="false" href="#">
                                                    <%=Resource. Admin_Product_Link_ShowInAdmin %>
                                                </a>
                                            </div>
                                            <div class="order-status-checkout">
                                                <div data-plugin="radiolist" class="radiolist radiolist-big radiolist-checkout">
                                                    <label>
                                                        <asp:RadioButton ID="rbActiveProduct" runat="server" GroupName="g-checkout" />
                                                        <%= Resource.Admin_Product_Active %>
                                                    </label>
                                                    <label>
                                                        <asp:RadioButton ID="rbNotActiveProduct" runat="server" GroupName="g-checkout" />
                                                        <%= Resource.Admin_Product_NotActive %>
                                                    </label>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin-bottom: 10px; width: 100%;">
                                    <tr>
                                        <td>
                                            <span style="font-weight: bold;">
                                                <asp:Localize ID="Localize_Admin_Catalog_CategoryLocation" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_CategoryLocation %>"
                                                    EnableViewState="false"></asp:Localize></span><br />
                                            <uc1:SiteNavigation ID="sn" runat="server" EnableViewState="false" />
                                            <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                                        </td>
                                        <td style="text-align: right;">
                                            <div style="display: inline-block">
                                                <div data-plugin="help" data-help-options="{position:'left bottom'}" class="help-block">
                                                    <a href="javascript:void(0)" class="btn btn-middle btn-action js-help-icon" runat="server" id="btnCopyProduct"
                                                        style="margin-right: 5px;" data-add-productcopy-call><%= Resource.Admin_Product_CopyProduct %></a>
                                                    <article class="bubble help js-help">
                                                        <header class="help-header">
                                                            Копия товара
                                                        </header>
                                                        <div class="help-content">
                                                            Сделать полную копию товара с новым артикулом. Функция помогает ускорить наполнение каталога, создавая копии товара.
                                                        </div>
                                                    </article>
                                                </div>
                                                <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" EnableViewState="false"
                                                    OnClick="btnSave_Click" OnClientClick="saveProperties();" onmousedown="window.onbeforeunload=null;" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div id="tabs-contents" class="tabs-contents_line">
                                    <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red"
                                        Visible="False" Font-Names="Verdana" Font-Size="14px" EnableViewState="false"></asp:Label>
                                    <!-- Main -->
                                    <div class="tab-content">
                                        <table class="table-p">
                                            <tr>
                                                <td class="formheader">
                                                    <h2>
                                                        <%=Resource.Admin_m_Product_Main%></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td></td>
                                            </tr>
                                        </table>
                                        <table class="table-p">
                                            <tr>
                                                <td style="width: 330px;">ID
                                                </td>
                                                <td style="vertical-align: middle; height: 29px;">
                                                    <asp:Label ID="lblProductId" runat="server" EnableViewState="false"></asp:Label>
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                ID товара
                                                            </header>
                                                            <div class="help-content">
                                                                Внутренний уникальный числовой идентификатор товара.
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trMs" runat="server" visible="False">
                                                <td style="width: 330px;">Внешний код в системе Мой Склад
                                                </td>
                                                <td style="vertical-align: middle; height: 29px;">
                                                    <asp:Label ID="lblExternMsCode" runat="server" EnableViewState="false" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Внешний код товара в системе Мой Склад
                                                            </header>
                                                            <div class="help-content">
                                                                Внешний код товара в системе Мой Склад. Если код не отображается, значит товар не синхронизирован с системой Мой склад.
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resources.Resource.Admin_Product_StockNumber%>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStockNumber" CssClass="niceTextBox shortTextBoxClass" runat="server" Width="450px" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Артикул товара
                                                            </header>
                                                            <div class="help-content">
                                                                Артикул - уникальный идентификатор товара.<br />
                                                                <br />
                                                                Используется для поиска и идентификации товара в магазине.<br />
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:Label runat="server" ID="lStockNumberError" ForeColor="Red" Visible="false" EnableViewState="false" />
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resources.Resource.Admin_Product_Name%>
                                                    <span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtName" CssClass="niceTextBox shortTextBoxClass" runat="server" Width="450px" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                <%=Resources.Resource.Admin_Product_Name%>
                                                            </header>
                                                            <div class="help-content">
                                                                Наименование товароной позиции.<br />
                                                                <br />
                                                                Обязательное поле.<br />
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                        EnableClientScript="false" Display="Dynamic" ValidationGroup="1" ErrorMessage='<%$ Resources:Resource,Admin_m_Product_RequiredField %>'> </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_m_Product_UrlSynonym%>
                                                    <span style="color: Red">*</span>
                                                </td>
                                                <td style="vertical-align: middle; height: 29px;">
                                                    <asp:TextBox ID="txtSynonym" CssClass="niceTextBox shortTextBoxClass" runat="server" Width="450px" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                <%=Resource.Admin_m_Product_UrlSynonym%>
                                                            </header>
                                                            <div class="help-content">
                                                                Часть URL адреса по которому будет доступен товар в магазине.<br />
                                                                <br />
                                                                Обязательное поле.<br />
                                                                <br />
                                                                Адрес строится по формуле:<br />
                                                                [домен]/products/[Синоним для URL запроса]<br />
                                                                <br />
                                                                Подробнее о построении URL в магазине - <a href="http://www.advantshop.net/help/pages/url-structure" target="_blank">Структура URL в интернет-магазине</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSynonym"
                                                        EnableClientScript="false" Display="Dynamic" ValidationGroup="1" ErrorMessage='<%$ Resources:Resource,Admin_m_Product_RequiredField %>'> </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="reValidatorUrl" runat="server" ControlToValidate="txtSynonym"
                                                        EnableClientScript="false" Display="Dynamic" ValidationGroup="1" ErrorMessage='<%$ Resources:Resource,Admin_m_Category_SynonymInfo %>'
                                                        ValidationExpression="^[a-zA-Z0-9_-]*$"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive" runat="server" id="trTags">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Tags%>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <asp:ListBox Width="456px" runat="server" ID="lbTag" Name="multiTag" CssClass="chosen" SelectionMode="Multiple" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                <%=Resource.Admin_Product_Tags%>
                                                            </header>
                                                            <div class="help-content">
                                                                С помошью тегов существует возможность создавать виртуальные категории (списки товаров) с набором товаров которым присвоены определенные метки (Теги).<br />
                                                                <br />
                                                                Инструкция: <a href="http://www.advantshop.net/help/pages/tags" target="_blank">Теги. Механизм тегов</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive" id="trReviews" runat="server">
                                                <td>
                                                    <%=Resource.Admin_Product_Reviews%>
                                                </td>
                                                <td style="vertical-align: middle;">В данный момент, отзывов о товаре:
                                                    <asp:Label runat="server" ID="lblReviewsCount" />&nbsp;
                                                <asp:HyperLink runat="server" ID="hlReviews"></asp:HyperLink>
                                                    <asp:Label runat="server" ID="lblRewReq" Text="(не обязательно)" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                <%=Resource.Admin_Product_Reviews%>
                                                            </header>
                                                            <div class="help-content">
                                                                Товары известных брендов традиционно пользуются доверием. Зарекомендовавший себя бренд означает престиж, качество и надежность.
                                                                <br />
                                                                <br />
                                                                Опция позволяет указать для товара его производителя.
                                                                <br />
                                                                <br />
                                                                Подробнее о производителях:<br />
                                                                <a href="http://www.advantshop.net/help/pages/product-review" target="_blank">Инструкция. Отзывы о товарах.</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2><%=Resource.Admin_Product_Additional%></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td></td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_m_Product_ProductBrand%>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel runat="server">
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="popUpBrand" />
                                                                        <asp:AsyncPostBackTrigger ControlID="ibRemoveBrand" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <asp:Label runat="server" ID="lBrand" EnableViewState="false"></asp:Label>
                                                                        <asp:ImageButton runat="server" ID="ibRemoveBrand" ImageUrl="~/Admin/images/remove.jpg"
                                                                            Style="margin-left: 2px; margin-right: 2px;" OnClick="ibRemoveBrand_Click" EnableViewState="false" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton runat="server" OnClientClick="ShowModalPopupBrand();return false"
                                                                    Text="<%$Resources:Resource, Admin_Product_Select %>" Style="margin-left: 5px; margin-right: 3px;"
                                                                    EnableViewState="false"></asp:LinkButton>
                                                                <div data-plugin="help" class="help-block">
                                                                    <div class="help-icon js-help-icon"></div>
                                                                    <article class="bubble help js-help">
                                                                        <header class="help-header">
                                                                            <%=Resource.Admin_m_Product_ProductBrand%>
                                                                        </header>
                                                                        <div class="help-content">
                                                                            Товары известных брендов традиционно пользуются доверием. Зарекомендовавший себя бренд означает престиж, качество и надежность.
                                                                            <br />
                                                                            <br />
                                                                            Опция позволяет указать для товара его производителя.
                                                                            <br />
                                                                            <br />
                                                                            Подробнее о производителях:<br />
                                                                            <a href="http://www.advantshop.net/help/pages/brand" target="_blank">Инструкция. Бренды (производители)</a>
                                                                        </div>
                                                                    </article>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resources.Resource.Admin_m_Product_ProductWeight%>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWeight" runat="server" Width="50px" Text="0" CssClass="niceTextBox shortTextBoxClass" />
                                                    <%=Resources.Resource.Admin_Product_Kg%>
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                <%=Resources.Resource.Admin_m_Product_ProductWeight%>
                                                            </header>
                                                            <div class="help-content">
                                                                Используется для расчёта стоимости в системах доставки, измеряется в килограммах.<br />
                                                                <br />
                                                                Чтобы задать вес в граммах укажите число, разделенное на 1000.<br />
                                                                <br />
                                                                Например: 
                                                            <ul>
                                                                <li>2 кг</li>
                                                                <li>1,2 кг</li>
                                                                <li>0.3 кг</li>
                                                            </ul>
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtWeight"
                                                        ValidationGroup="1" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="RangeValidator7" runat="server" ControlToValidate="txtWeight"
                                                        ValidationGroup="1" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'> </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resources.Resource.Admin_m_Product_Size%>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSizeLength" runat="server" Width="50px" Text="0" CssClass="niceTextBox shortTextBoxClass" />&nbsp;x
                                                <asp:TextBox ID="txtSizeWidth" runat="server" Width="50px" Text="0" CssClass="niceTextBox shortTextBoxClass" />&nbsp;x
                                                <asp:TextBox ID="txtSizeHeight" runat="server" Width="50px" Text="0" CssClass="niceTextBox shortTextBoxClass" />&nbsp;<span><%= Resource.Admin_Product_SizeDimension%></span>
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Габариты товара
                                                            </header>
                                                            <div class="help-content">
                                                                Используется для расчёта стоимости в системах доставки, измеряется в миллиметрах.<br />
                                                                <br />
                                                                Чтобы задать размеры в сантиметрах укажите число, умноженное на 10.<br />
                                                                <br />
                                                                Например, если товар 15см на 20см и 3мм толщиной, задайте:
                                                            <ul>
                                                                <li>Ш - Ширина - 150 мм</li>
                                                                <li>В - Высота - 3 мм</li>
                                                                <li>Д - Длинна - 200 мм</li>
                                                            </ul>
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtSizeLength"
                                                        ValidationGroup="3" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                    <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSizeWidth"
                                                        ValidationGroup="3" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                    <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtSizeHeight"
                                                        ValidationGroup="3" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                    <asp:Label ID="lSize" runat="server" Visible="false" Text="Error size" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_BarCode%>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBarCode" CssClass="niceTextBox shortTextBoxClass" runat="server" Width="250px" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Штрих код
                                                            </header>
                                                            <div class="help-content">
                                                                Штрих-код — изображение, которое наносится с целью автоматизации учета информации о товарах.<br />
                                                                <br />
                                                                Позволяет их идентифицировать, а также уменьшить время на обработку данных.<br />
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2><%=Resource.Admin_Product_Markers%></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td></td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td>
                                                    <%=Resources.Resource.Admin_Product_BestSeller%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkBestseller" runat="server" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Маркер "Хит продаж"
                                                            </header>
                                                            <div class="help-content">
                                                                Помечает товар маркером "Хит продаж"<br />
                                                                <br />
                                                                Подробнее - <a href="http://www.advantshop.net/help/pages/marker" target="_blank">Маркеры товара: хит, новинка, скидка, рекомендуем, распродажа</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td>
                                                    <%=Resources.Resource.Admin_Product_Recomended%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRecommended" runat="server" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Маркер "Рекомендованный"
                                                            </header>
                                                            <div class="help-content">
                                                                Помечает товар маркером "Рекомендованный"<br />
                                                                <br />
                                                                Подробнее - <a href="http://www.advantshop.net/help/pages/marker" target="_blank">Маркеры товара: хит, новинка, скидка, рекомендуем, распродажа</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td>
                                                    <%=Resources.Resource.Admin_Product_New%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkNew" runat="server" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Маркер "Новинка"
                                                            </header>
                                                            <div class="help-content">
                                                                Помечает товар маркером "Новинка"<br />
                                                                <br />
                                                                Подробнее - <a href="http://www.advantshop.net/help/pages/marker" target="_blank">Маркеры товара: хит, новинка, скидка, рекомендуем, распродажа</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td>
                                                    <%=Resources.Resource.Admin_Product_OnSale%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkOnSale" runat="server" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Маркер "Распродажа"
                                                            </header>
                                                            <div class="help-content">
                                                                Помечает товар маркером "Распродажа"<br />
                                                                <br />
                                                                Подробнее - <a href="http://www.advantshop.net/help/pages/marker" target="_blank">Маркеры товара: хит, новинка, скидка, рекомендуем, распродажа</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label runat="server" ID="lblMarkersDisabled" Text="<%$ Resources:Resource, Admin_Product_CanNotChange %>" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <!-- Categories -->
                                    <div class="tab-content">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lnAddLink" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDelLink" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnMainLink" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinksProductTree" EventName="TreeNodePopulate" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <table class="table-p">
                                                    <tr>
                                                        <td colspan="3" class="formheader">
                                                            <h2><%=Resources.Resource.Admin_Product_CategoriesContainsProducts%></h2>
                                                        </td>
                                                    </tr>
                                                    <tr class="formheaderfooter">
                                                        <td colspan="3"></td>
                                                    </tr>
                                                </table>
                                                <table class="table-p" style="table-layout: fixed;">
                                                    <tr>
                                                        <td style="vertical-align: top;">
                                                            <div style="min-width: 285px; height: 300px; overflow: scroll; background-color: White; margin-top: 10px; margin-bottom: 10px; border: 1px solid gray; border-radius: 3px;" id="divScroll">
                                                                <asp:TreeView ID="LinksProductTree" ForeColor="Black" SelectedNodeStyle-BackColor="Blue"
                                                                    EnableClientScript="true" PopulateNodesFromClient="true" runat="server" ShowLines="True"
                                                                    ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode"
                                                                    EnableViewState="true">
                                                                    <SelectedNodeStyle BackColor="Yellow" CssClass="selectedNodeClass" />
                                                                </asp:TreeView>
                                                            </div>
                                                        </td>
                                                        <td style="width: 80px; text-align: center; padding-bottom: 45px;">
                                                            <asp:Button ID="lnAddLink" runat="server" Text=">>>" OnClick="lnAddLink_Click" EnableViewState="false"
                                                                CssClass="btn btn-middle btn-add" Style="padding-left: 15px; padding-right: 15px;" />
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:Button ID="btnDelLink" runat="server" Text="<<<" OnClick="btnDelLink_Click" EnableViewState="false"
                                                                CssClass="btn btn-middle btn-action" Style="padding-left: 15px; padding-right: 15px;" />
                                                        </td>
                                                        <td style="vertical-align: top;">
                                                            <div style="min-width: 285px; height: 300px; overflow-x: auto; margin-top: 10px; border: 1px solid gray; border-radius: 3px;">
                                                                <asp:ListBox ID="ListlinkBox" runat="server" Height="283px" CssClass="TableContainer" Style="width: inherit; min-width: 100%;"></asp:ListBox>
                                                            </div>
                                                            <div style="margin-top: 10px; text-align: right;">
                                                                <asp:Button ID="btnMainLink" runat="server" CssClass="btn btn-middle btn-add"
                                                                    Text="<%$ Resources: Resource, Admin_Product_MainLink %>" OnClick="btnMainLink_Click" EnableViewState="false" />
                                                                <div data-plugin="help" class="help-block">
                                                                    <div class="help-icon js-help-icon"></div>
                                                                    <article class="bubble help js-help">
                                                                        <header class="help-header">
                                                                            Сделать категорию основной
                                                                        </header>
                                                                        <div class="help-content">
                                                                            Выделите категорию и нажмите "сделать основной", если нужно чтобы навигация ("Хлебные крошки"), строились по этой категории.<br />
                                                                            <br />
                                                                            При переходе в карточку товара, навигация будет построена от основной категории.
                                                                        </div>
                                                                    </article>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <script type="text/javascript">
                                                    var scrollTopcategories = 0, timerScroll;

                                                    var observeScroll = function () {
                                                        $("#divScroll").on('scroll', function () {

                                                            var blockWithScroll = $(this);

                                                            if (timerScroll != null) {
                                                                clearTimeout(timerScroll);
                                                            }

                                                            timerScroll = setTimeout(function () {
                                                                scrollTopcategories = blockWithScroll.scrollTop();
                                                            }, 300);

                                                        });
                                                    };

                                                    observeScroll();

                                                    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                                                        $("#divScroll").scrollTop(scrollTopcategories);

                                                        observeScroll();
                                                    });
                                                </script>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <!-- Prices -->
                                    <div class="tab-content">
                                        <table class="table-p">
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2><%=Resource.Admin_Product_PriceAvailability%></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <adv:ProductOffers ID="productOffers" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2><%=Resource.Admin_Product_Currency %></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 330px; height: 26px;">В какой валюте хранится цена товара
                                                </td>
                                                <td style="vertical-align: middle; height: 26px;">&nbsp;<asp:DropDownList runat="server" ID="ddlCurrecy" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Валюта.
                                                            </header>
                                                            <div class="help-content">
                                                                Для конкретного товара, вы можете выбрать, в какой валюте храниться его цена.<br />
                                                                <br />
                                                                Подробнее о валютах в магазине:<br />
                                                                <a href="http://www.advantshop.net/help/pages/currency" target="_blank">Инструкция. Настройка параметров валюты в магазине</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2>Налог</h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 330px; height: 26px;">Налог для данного товара
                                                </td>
                                                <td style="vertical-align: middle; height: 26px;">&nbsp;<asp:DropDownList runat="server" ID="ddlTax" DataTextField="Text" DataValueField="Value" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Налог
                                                            </header>
                                                            <div class="help-content">
                                                                Для конкретного товара, вы можете выбрать налог.
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2><%=Resource.Admin_Product_Other %></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_m_Product_DeliveryCost %>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtShippingPrice" runat="server" Width="88px" Text="0" CssClass="niceTextBox shortTextBoxClass2" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Стоимость доставки единицы товара
                                                            </header>
                                                            <div class="help-content">
                                                                Данный параметр используется для расчета стоимости доставки в методе "Доставка в зависимости от стоимости доставки товара"<br />
                                                                <br />
                                                                Так же, этот параметр выгружается в Яндекс.Маркет в поле стоимость доставки товара (тэг local_delivery_cost).<br />
                                                                <br />
                                                                Если параметр не нужен, укажите значение 0.
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="rvShippingPrice" runat="server" ControlToValidate="txtShippingPrice"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="1000000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="RangeValidator11" runat="server" ControlToValidate="txtShippingPrice"
                                                        ValidationGroup="1" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>' />
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_m_Product_Discount%>
                                                </td>
                                                <td>&nbsp;
                                                    <div>
                                                        <asp:TextBox ID="txtDiscount" runat="server" Text="0" Width="88px" CssClass="niceTextBox shortTextBoxClass2" />&nbsp;%
                                                        <div data-plugin="help" class="help-block">
                                                            <div class="help-icon js-help-icon"></div>
                                                            <article class="bubble help js-help">
                                                                <header class="help-header">
                                                                    Скидка
                                                                </header>
                                                                <div class="help-content">
                                                                    Скидка на цену товара, указывается только на этот товар, на всё вариации.<br />
                                                                    <br />
                                                                    Скидка указывается в процентах.
                                                                </div>
                                                            </article>
                                                        </div>
                                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtDiscount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber  %>'
                                                            MaximumValue="100" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RangeValidator8" runat="server" ControlToValidate="txtDiscount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber  %>'>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <div>
                                                        <asp:TextBox ID="txtDiscountAmount" runat="server" Text="0" Width="88px" CssClass="niceTextBox shortTextBoxClass2" />&nbsp;<%= CurrencyService.CurrentCurrency.Symbol %>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Unit%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtUnit" runat="server" Width="88px" CssClass="niceTextBox shortTextBoxClass2" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Единицы измерения
                                                            </header>
                                                            <div class="help-content">
                                                                Единицы измерения товара.<br />
                                                                <br />
                                                                Например:
                                                                <br />
                                                                штуки (шт.), литры (л.), упаковка (упак.) и т.д.
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_m_Product_OrderByRequest%>
                                                </td>
                                                <td>&nbsp;<asp:CheckBox ID="chkAllowPreOrder" runat="server" Checked="false" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Под заказ
                                                            </header>
                                                            <div class="help-content">
                                                                Опция определяет, если товара нет в наличии, показывать ли в этом случае кнопку "Под заказ".
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td>
                                                    <%=Resource.Admin_m_Product_AmountLimitation%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtMinAmount" runat="server" CssClass="niceTextBox shortTextBoxClass3" />
                                                    <asp:RangeValidator ID="RangeValidator13" runat="server" ControlToValidate="txtMinAmount"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="100000" MinimumValue="0" Type="Double" />
                                                    -
                                                <asp:TextBox ID="txtMaxAmount" runat="server" CssClass="niceTextBox shortTextBoxClass3" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Минимальное и максимальное количество 
                                                            </header>
                                                            <div class="help-content">
                                                                Определяет минимальное и максимальное возможное количество товара в заказе.
                                                                <br />
                                                                <br />
                                                                Например: 2-10. Означает, что купить можно минимум 2 штуки, но не более 10 за 1 заказ.<br />
                                                                <br />
                                                                Если ограничение не нужно, например, на верхнюю границу, оставьте поле пустым.
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="RangeValidator14" runat="server" ControlToValidate="txtMaxAmount"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="100000" MinimumValue="0" Type="Double" />
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_m_Product_AmountMultiplicity%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtMultiplicity" runat="server" Text="1" Width="88px" CssClass="niceTextBox shortTextBoxClass2" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Кратность количества
                                                            </header>
                                                            <div class="help-content">
                                                                Важная опция, если товар можно купить порциями, которые меньше единицы.<br />
                                                                <br />
                                                                Например: Метры, вы можете указать кратность, 0.2, если продаете, например ткань, кусочками по 20 см.
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="RangeValidator15" runat="server" ControlToValidate="txtMultiplicity"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="100000" MinimumValue="0" Type="Double" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMultiplicity"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formheader" colspan="2">
                                                    <h2><%=Resource.Admin_Product_Export%></h2>
                                                </td>
                                            </tr>
                                            <tr class="formheaderfooter">
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Yandex_SalesNote%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtSalesNote" runat="server" Text="" CssClass="niceTextBox textBoxClass" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Заметки для продажи
                                                            </header>
                                                            <div class="help-content">
                                                                Элемент "sales_notes", используется только для Яндекс.Маркета<br />
                                                                <br />
                                                                Элемент используется для отражения информации о минимальной сумме заказа, минимальной партии товара или необходимости предоплаты, а также для описания акций, скидок и распродаж.<br />
                                                                <br />
                                                                Допустимая длина текста в элементе — 50 символов.<br />
                                                                <br />
                                                                Необязательный элемент.<br />
                                                                <br />
                                                                Например: Необходима предоплата.<br />
                                                                <br />
                                                                <a href="http://www.advantshop.net/help/pages/yandex-market-sales-note" target="_blank">Инструкция. Заметки для продажи (Sales note)</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtSalesNote"
                                                        ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{0,50}$" runat="server"
                                                        ValidationGroup="2" ErrorMessage='<%$ Resources: Resource, Admin_Product_Yandex_MaxSalesNote %>'></asp:RegularExpressionValidator>
                                                    <br />
                                                </td>
                                            </tr>

                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">Размер комиссии на товарное предложение, участвующее в программе «Заказ на Маркете»
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtFee" runat="server" Text="1" Width="88px" CssClass="niceTextBox shortTextBoxClass2" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Размер комисии
                                                            </header>
                                                            <div class="help-content">
                                                                Вы можете повышать размер комиссии на товарное предложение, участвующее в программе
                                                                <br>
                                                                «Заказ на Маркете», чтобы оно занимало более высокие позиции на местах размещения
                                                                <br />
                                                                Таким образом, 1% комиссии соответствует значению 100. Примеры:
                                                                <br />
                                                                Значение 220 соответствует комиссии в 2,2% от стоимости товара.
                                                                <br />
                                                                Значение 1220 соответствует комиссии в 12,2%.
                                                                <br />
                                                                Значение 22 соответствует комиссии в 0,22%.
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtFee"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="100000" MinimumValue="0" Type="Double" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFee"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>' />
                                                </td>
                                            </tr>

                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">Cтавка для карточки модели
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtCbid" runat="server" Text="1" Width="88px" CssClass="niceTextBox shortTextBoxClass2" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                cтавка для карточки модели
                                                            </header>
                                                            <div class="help-content">
                                                                Вы можете передавать значения ставок в прайс-листе формата YML.
                                                                <br />
                                                                В качестве значений указываются условные центы. Значения должны быть целыми 
                                                                <br />
                                                                и положительными числами, например «80», что соответствует ставке 0,8 у. е.
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtCbid"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                        MaximumValue="100000" MinimumValue="0" Type="Double" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCbid"
                                                        ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>' />
                                                </td>
                                            </tr>


                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Gtin%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtGtin" runat="server" Text="" CssClass="niceTextBox textBoxClass" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Google GTIN
                                                            </header>
                                                            <div class="help-content">
                                                                Используется только для Google Merchant Center<br />
                                                                <br />
                                                                Подробнее:
                                                                <br />
                                                                <a href="https://support.google.com/merchants/answer/160161?hl=ru#valid" target="_blank">Google GTIN на support.google.com</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtGtin" ID="RegularExpressionValidator3"
                                                        ValidationGroup="2" ValidationExpression="^[\s\S]{0,50}$" runat="server" ErrorMessage='<%$ Resources: Resource, Admin_Product_Yandex_MaxSalesNote %>' />
                                                </td>
                                            </tr>

                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_GoogleBase_GoogleProductCategory%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtGoogleProductCategory" runat="server" Text="" CssClass="niceTextBox textBoxClass" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Классификация товаров Google
                                                            </header>
                                                            <div class="help-content">
                                                                Этот атрибут определяет категорию товара в соответствии с классификацией Google.<br />
                                                                <br />
                                                                Используется только для Google Merchant Center<br />
                                                                <br />
                                                                Подробнее:
                                                                <br />
                                                                <a href="https://support.google.com/merchants/answer/160081?hl=ru" target="_blank">Google Product Category на support.google.com</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtGoogleProductCategory"
                                                        ID="RegularExpressionValidator4" ValidationExpression="^[\s\S]{0,250}$" runat="server"
                                                        ValidationGroup="2" ErrorMessage='<%$ Resources: Resource, Admin_Product_Yandex_MaxSalesNote %>'></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Yandex_MarketCategory%>
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtYandexMarketCategory" runat="server" Text="" CssClass="niceTextBox textBoxClass" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Классификация товаров Яндекс
                                                            </header>
                                                            <div class="help-content">
                                                                Этот атрибут определяет категорию товара в соответствии с классификацией Яндекс.<br />
                                                                <br />
                                                                Используется только для Яндекс.Маркет<br />
                                                                <br />
                                                                Подробнее:
                                                                <br />
                                                                <a href="http://download.cdn.yandex.net/market/market_categories.xls" target="_blank">Список доступных категорий (*.xls)</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">Яндекс-маркет префикс
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtYandexTypePrefix" runat="server" Text="" CssClass="niceTextBox textBoxClass" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Тип или категория товара
                                                            </header>
                                                            <div class="help-content">
                                                                Элемент "typePrefix"<br />
                                                                <br />
                                                                Тип или категория товара, необходимо передавать правильно (см. инструкцию)<br />
                                                                <br />
                                                                Необязательный элемент.<br />
                                                                <br />
                                                                Подробнее:
                                                                <br />
                                                                <a href="https://yandex.ru/support/partnermarket/elements/vendor-name-model.xml" target="_blank">Название предложения: model, typePrefix</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">Яндекс-маркет модель
                                                </td>
                                                <td>&nbsp;<asp:TextBox ID="txtYandexModel" runat="server" Text="" CssClass="niceTextBox textBoxClass" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Модель
                                                            </header>
                                                            <div class="help-content">
                                                                Элемент "model"<br />
                                                                <br />
                                                                Используется для передачи модели товара или важных параметров.<br />
                                                                <br />
                                                                Подробнее:
                                                                <br />
                                                                <a href="https://yandex.ru/support/partnermarket/elements/vendor-name-model.xml" target="_blank">Название предложения: model, typePrefix</a>
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">Обозначения размерных сеток в атрибуте unit
                                                </td>
                                                <td>&nbsp;
                                                    <asp:DropDownList ID="ddlYandexSizeUnit" runat="server" CssClass="niceTextBox textBoxClass">
                                                        <asp:ListItem Value="">Не выбрано</asp:ListItem>

                                                        <asp:ListItem Value="AU">AU</asp:ListItem>
                                                        <asp:ListItem Value="DE">DE</asp:ListItem>
                                                        <asp:ListItem Value="EU">EU</asp:ListItem>
                                                        <asp:ListItem Value="FR">FR</asp:ListItem>
                                                        <asp:ListItem Value="Japan">Japan</asp:ListItem>
                                                        <asp:ListItem Value="INT">INT</asp:ListItem>
                                                        <asp:ListItem Value="IT">IT</asp:ListItem>
                                                        <asp:ListItem Value="RU">RU</asp:ListItem>
                                                        <asp:ListItem Value="UK">UK</asp:ListItem>
                                                        <asp:ListItem Value="US">US</asp:ListItem>


                                                        <asp:ListItem Value="INCH">INCH</asp:ListItem>
                                                        <asp:ListItem Value="Height">Height</asp:ListItem>
                                                        <asp:ListItem Value="Months">Months</asp:ListItem>
                                                        <asp:ListItem Value="Round">Round</asp:ListItem>
                                                        <asp:ListItem Value="Years">Years</asp:ListItem>

                                                    </asp:DropDownList>
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Обозначения размерных сеток
                                                            </header>
                                                            <div class="help-content">
                                                                - по странам:<br />
                                                                AU — Австралия;<br />
                                                                DE — Германия;<br />
                                                                EU — Европа;<br />
                                                                FR — Франция;<br />
                                                                Japan — Япония;<br />
                                                                INT — международные обозначения (для одежды: XS, S, M, L, XL, XXL, 2XL и т. д., для бюстгальтеров: AA, A, B, C и т. д.);<br />
                                                                IT — Италия;<br />
                                                                RU — Россия;<br />
                                                                UK — Англия;<br />
                                                                US — США.<br />
                                                                - по типу измерения:<br />
                                                                INCH или «дюймы» — дюймы;<br />
                                                                Height или «рост» — рост в сантиметрах;<br />
                                                                Months или «мес.» — возраст в месяцах;<br />
                                                                Round или «окружность» — окружность в сантиметрах;<br />
                                                                Years или «лет» — возраст в годах.<br />
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Yandex_Adult%>
                                                </td>
                                                <td>&nbsp;<asp:CheckBox runat="server" ID="chbAdult" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Товар для взрослых
                                                            </header>
                                                            <div class="help-content">
                                                                Элемент "adult"<br />
                                                                <br />
                                                                Элемент обязателен для обозначения товара, имеющего отношение к удовлетворению сексуальных потребностей, либо иным образом эксплуатирующего интерес к сексу.<br />
                                                                <br />
                                                                Необязательный элемент.<br />
                                                                <br />
                                                                Используется как для Яндекс.Маркет, так и Google Merchant Center.
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="rowsPost row-interactive">
                                                <td onclick="focusoninput(this)">
                                                    <%=Resource.Admin_Product_Yandex_ManufacturerWarranty%>
                                                </td>
                                                <td>&nbsp;<asp:CheckBox runat="server" ID="chbManufacturerWarranty" CssClass="checkly-align" />
                                                    <div data-plugin="help" class="help-block">
                                                        <div class="help-icon js-help-icon"></div>
                                                        <article class="bubble help js-help">
                                                            <header class="help-header">
                                                                Гарантия производителя
                                                            </header>
                                                            <div class="help-content">
                                                                Элемент "manufacturer_warranty"<br />
                                                                <br />
                                                                Элемент предназначен для отметки товаров, имеющих официальную гарантию производителя.<br />
                                                                <br />
                                                                Необязательный элемент.<br />
                                                            </div>
                                                        </article>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <!-- Photos -->
                                    <div class="tab-content">
                                        <adv:ProductPhotos runat="server" ID="productPhotos" OnMainPhotoUpdate="productPhotos_OnMainPhotoUpdate"
                                            onphotomessage="productPhotos_OnPhotoMessage" />
                                        <br />
                                        <br />
                                        <adv:ProductPhotos360 runat="server" ID="productPhotos360" OnMainPhotoUpdate="productPhotos_OnMainPhotoUpdate"
                                            onphotomessage="productPhotos_OnPhotoMessage" />
                                    </div>
                                    <!-- Videos -->
                                    <div class="tab-content">
                                        <adv:ProductVideos runat="server" ID="productVideos" />
                                    </div>
                                    <!-- Description -->
                                    <div class="tab-content">
                                        <table class="table-p">
                                            <tbody>
                                                <tr>
                                                    <td class="formheader">
                                                        <h2><%=Resource.Admin_Product_Description%></h2>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="fckDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formheader">
                                                        <h2><%=Resource.Admin_Product_ShortDescription%></h2>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="fckBriefDescription" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="400px" Width="100%" EnableViewState="False" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <!-- Properties -->
                                    <div class="tab-content">
                                        <adv:ProductProperties ID="productProperties" runat="server" />
                                    </div>
                                    <!-- Custom options -->
                                    <div class="tab-content">
                                        <adv:ProductCustomOption ID="productCustomOption" runat="server" />
                                    </div>
                                    <!-- Meta -->
                                    <div class="tab-content">
                                        <asp:Label ID="lResult" runat="server" CssClass="mProductLabelInfo" ForeColor="Blue"
                                            Text="<%$ Resources:Resource, Admin_m_Product_Saved %>" Visible="false" EnableViewState="false"></asp:Label>
                                        <table class="info-tb">
                                            <tbody>
                                                <tr class="rowPost">
                                                    <td class="formheader" colspan="2">
                                                        <h2><%=Resource.Admin_Product_SEO%></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr class="rowsPost row-interactive">
                                                    <td style="width: 30%; vertical-align: middle;">
                                                        <%=Resource.Admin_Catalog_UseDefaultMeta%>
                                                    </td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:CheckBox runat="server" ID="chbDefaultMeta" Checked="true" CssClass="checkly-align" />
                                                        <div data-plugin="help" class="help-block">
                                                            <div class="help-icon js-help-icon"></div>
                                                            <article class="bubble help js-help">
                                                                <header class="help-header">
                                                                    Использовать Meta по умолчанию
                                                                </header>
                                                                <div class="help-content">
                                                                    Если опция <b>включена</b>, SEO настройки будут взяты из общих настроек магазина.<br />
                                                                    <br />
                                                                    Если опция <b>выключена</b>, SEO настройки для товара будут взяты с формы ниже.
                                                                </div>
                                                            </article>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="rowsPost row-interactive">
                                                    <td style="width: 30%; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <%=Resource.Admin_m_Product_HeadTitle%>
                                                    </td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="niceTextBox textBoxClass" />
                                                    </td>
                                                </tr>
                                                <tr class="rowsPost row-interactive">
                                                    <td style="width: 30%; vertical-align: middle;" onclick="focusoninput(this)">H1
                                                    </td>
                                                    <td style="vertical-align: middle; height: 26px;">
                                                        <asp:TextBox ID="txtH1" runat="server" CssClass="niceTextBox textBoxClass" />
                                                    </td>
                                                </tr>
                                                <tr class="rowsPost row-interactive">
                                                    <td style="width: 30%;" onclick="focusoninput(this)">
                                                        <%=Resource.Admin_m_Product_MetaKeywords%>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px;">
                                                        <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" CssClass="niceTextBox textBoxClass" />
                                                    </td>
                                                </tr>
                                                <tr class="rowsPost row-interactive">
                                                    <td style="width: 30%; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <%=Resource.Admin_m_Product_MetaDescription%>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 26px;">
                                                        <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" CssClass="niceTextBox textBoxClass" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span class="subSaveNotify">
                                                            <%=Resource.Admin_m_Product_UseGlobalVariables%>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="dvSubHelp">
                                                            <asp:Image ID="Image6" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" AlternateText="" />
                                                            <a href="http://www.advantshop.net/help/pages/settings-of-seo-module" target="_blank">Инструкция. Настройка мета по умолчанию для магазина</a>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <!-- Alternative -->
                                    <div class="tab-content">
                                        <adv:RelatedProducts runat="server" ID="relatedProducts" RelatedType="0"></adv:RelatedProducts>
                                    </div>
                                    <div class="tab-content">
                                        <adv:RelatedProducts runat="server" ID="alternativeProducts" RelatedType="1"></adv:RelatedProducts>
                                    </div>
                                    <div class="tab-content">
                                        <adv:ProductGifts runat="server" ID="gifts"></adv:ProductGifts>
                                    </div>

                                    <div class="tab-content">
                                        <adv:ProductLandingPage runat="server" ID="landingPage"></adv:ProductLandingPage>
                                    </div>

                                    <!-- Additional controls -->
                                    <asp:Panel runat="server" ID="pnlModuleControls">
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <%=AdvantShop.Helpers.HtmlHelper.RenderSplitter()%>
                <td class="rightNavigation">
                    <div id="rightPanel" class="rightPanel">
                        <adv:RightNavigation runat="server" ID="rightNavigation"></adv:RightNavigation>
                    </div>
                </td>
                <td style="width: 10px;"></td>
            </tr>
        </table>
    </div>
    <adv:PopupGridBrand ID="popUpBrand" runat="server"></adv:PopupGridBrand>

    <script type="text/javascript">

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

        function fillUrl() {
            var text = $('#<%=txtName.ClientID %>').val();
            var url = $('#<%=txtSynonym.ClientID %>').val();
            if ((text != "") & (url == "")) {
                $('#<%=txtSynonym.ClientID %>').val(translite(text));
            }
        }

        $(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtH1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");
            }
        });

        $('#<%= chbDefaultMeta.ClientID %>').click(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').val("");
                $('#<%=txtH1.ClientID %>').val("");
                $('#<%=txtMetaDescription.ClientID %>').val("");
                $('#<%=txtMetaKeywords.ClientID %>').val("");

                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtH1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");

            } else {
                $('#<%=txtTitle.ClientID %>').removeAttr("disabled");
                $('#<%=txtH1.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaDescription.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaKeywords.ClientID %>').removeAttr("disabled");
            }
        });


        $('#<%=txtSynonym.ClientID %>').focus(fillUrl);

       
    </script>
</asp:Content>
