<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.EditNewsCategory" Codebehind="EditNewsCategory.aspx.cs" %>

<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/UserControls/EditMetaFields.ascx" TagName="EditMetaFields" TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
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
                        <td align="center">
                            <img src="images/ajax-loader.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: #0D76B8;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Menu.aspx">
                <%= Resource.Admin_MasterPageAdmin_MainMenu%></a></li>
            <li class="neighbor-menu-item"><a href="NewsAdmin.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsMenuRoot%></a></li>
            <li class="neighbor-menu-item selected"><a href="NewsCategory.aspx">
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
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_NewsCategory_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_NewsCategory_Subheader %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HyperLink ID="HyperLink1" NavigateUrl="~/admin/newscategory.aspx" Text='<%$ Resources: Resource, Admin_Back %>'
                            runat="server" CssClass="Link"></asp:HyperLink>
                    </td>
                    <td>
                        <div class="btns-main">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" OnClick="btnSave_Click" onmousedown="window.onbeforeunload=null;" Text="<%$ Resources:Resource, Admin_Save %>" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width: 100%">
            <div>
                <div style="height: 10px">
                </div>
                <table id="tabs">
                    <tr>
                        <td style="width: 200px;">
                            <div style="width: 100px; font-size: 0; line-height: 0px;">
                            </div>
                            <ul id="tabs-headers">
                                <li id="general">
                                    <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_TabGeneral%>" />
                                    <img class="floppy" src="images/floppy.gif" />
                                </li>
                                <li id="seo">
                                    <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_TabSeo  %>" />
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
                                            <span class="spanSettCategory">
                                                <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_HeadGeneral%>" /></span>
                                            <hr color="#C2C2C4" size="1px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>
                                                <%=Resource.Admin_NewsCategory_CategoryName%></span> <span style="color: red;">*</span>
                                        </td>
                                        <td style="width: 80%">
                                            <asp:TextBox ID="txtNewsCategoryName" runat="server" CssClass="niceTextBox textBoxClass" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>UrlPath</span><span style="color: red;">*</span>
                                        </td>
                                        <td style="width: 80%">
                                            <asp:TextBox ID="txtNewsCategiryUrlPath" runat="server" CssClass="niceTextBox textBoxClass" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>
                                                <%=Resource.Admin_NewsCategory_Sort%></span>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNewsCategorySortOrder" Text="0" CssClass="niceTextBox shortTextBoxClass2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="tab-content">
                                <adv:EditMetaFields ID="editMetaFields" runat="server" Hint='<%$ Resources:Resource, Admin_NewsCategory_UseGlobalVariables %>' />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function fillUrl() {
            var text = $('#<%=txtNewsCategoryName.ClientID %>').val();
            var url = $('#<%=txtNewsCategiryUrlPath.ClientID %>').val();
            if ((text != "") & (url == "")) {
                $('#<%=txtNewsCategiryUrlPath.ClientID %>').val(translite(text));
            }
        };

        $('#<%=txtNewsCategiryUrlPath.ClientID %>').focus(fillUrl);        

    </script>
</asp:Content>
