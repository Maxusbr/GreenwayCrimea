<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditTag" CodeBehind="Tag.aspx.cs" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Catalog.aspx">
                <%= Resource.Admin_MasterPageAdmin_CategoryAndProducts %></a></li>
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
            <li class="neighbor-menu-item dropdown-menu-parent selected"><a href="Properties.aspx">
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
            <li class="neighbor-menu-item"><a href="ExportFeed.aspx">
                <%= Resource.Admin_MasterPageAdmin_Export%></a></li>
            <li class="neighbor-menu-item"><a href="ImportCSV.aspx">
                <%= Resource.Admin_MasterPageAdmin_Import%></a></li>
            <li class="neighbor-menu-item"><a href="Brands.aspx">
                <%= Resource.Admin_MasterPageAdmin_Brands%></a></li>
            <li class="neighbor-menu-item"><a href="Reviews.aspx">
                <%= Resource.Admin_MasterPageAdmin_Reviews%></a></li>
        </menu>
        <div class="panel-add">
            <%= Resource.Admin_MasterPageAdmin_Add %>
            <a href="javascript:void(0);" class="panel-add-lnk" data-add-product-call>
                <%= Resource.Admin_MasterPageAdmin_Product %></a>, 
            <a href="m_Category.aspx?ParentCategoryID=<%=Request["categoryid"] ?? "0" %>" class="panel-add-lnk">
                <%= Resource.Admin_MasterPageAdmin_Category %></a>
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
                                <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Tags_Header %>"></asp:Label><br />
                                <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Tags_EditSubHeader %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Admin/Tags.aspx" Text='<%$ Resources: Resource, Admin_Back %>'
                                    runat="server" CssClass="Link"></asp:HyperLink>
                            </td>
                            <td>
                                <div class="btns-main">
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
                                    <table class="info-tb" border="0" cellpadding="2" cellspacing="0" width="95%">
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
                                                    <%=Resource.Admin_Tag_Name%></span> <span style="color: red;">*</span>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtName" runat="server" Width="400px"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resource.Admin_Tag_UrlSynonym%></span><span style="color: red;">
                                                        *</span>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtSynonym" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resource.Admin_Tag_Enabled%></span>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resource.Admin_Tag_Brief%></span>
                                            </td>
                                            <td>
                                                 <asp:TextBox ID="fckBrief" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resource.Admin_Tag_Desc%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="fckDesc" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-content">
                                    <table class="info-tb" border="0" cellpadding="2" cellspacing="0">
                                        <tr class="rowPost">
                                            <td colspan="2" style="height: 34px;">
                                                <h4 style="display: inline; font-size: 10pt;">
                                                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_HeadSeo%>"></asp:Localize></h4>
                                                <hr color="#C2C2C4" size="1px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="lpart">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UseDefaultMeta%>"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chbDefaultMeta" runat="server" Checked="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_MetaTitle%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTitle" runat="server" Width="400px" CssClass="niceTextBox textBoxClass" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>H1</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtH1" runat="server" Width="400px" CssClass="niceTextBox textBoxClass" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_MetaKeyWords%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetaKeywords" runat="server" Width="400px" Height="85px" TextMode="MultiLine" CssClass="niceTextBox textBoxClass" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <%=Resources.Resource.Admin_MetaDescription%></span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetaDescription" runat="server" Width="400px" Height="85px" TextMode="MultiLine" CssClass="niceTextBox textBoxClass" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="top" width="400">
                                                <asp:Localize ID="Localize2" Text="<%$ Resources: Resource, Admin_Tag_UseGlobalVariables %>"
                                                    runat="server"></asp:Localize>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            var dirty = false;
            var skipChecking = false;

            function checkForDirty(e) {
                if (!skipChecking) {
                    var evt = e || window.event;
                    if (dirty) {
                        evt.returnValue = '<%=Resource.Admin_Product_LosingChanges%>';
                    }
                } else {
                    skipChecking = false;
                }
            }

            $(document).ready(function () {

                $("#<%= btnSave.ClientID %>").click(function () { skipChecking = true; });

                $("#<%=txtSynonym.ClientID %>").on("focus", function () {
                    var text = $('#<%=txtName.ClientID %>').val();
                    var url = $('#<%=txtSynonym.ClientID %>').val();
                    if ((text != "") & (url == "")) {
                        $('#<%=txtSynonym.ClientID %>').val(translite(text));
                    }
                });
            });
        });

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

    </script>
</asp:Content>
