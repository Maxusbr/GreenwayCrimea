<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.News" Codebehind="NewsAdmin.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function setupAdvantGrid() {
            $(".imgtooltip").tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    var imagePath = $(this).attr("abbr");
                    if (imagePath.length == 0) {
                        return "<div><span><%= Resource.Admin_Catalog_NoMiniPicture %></span></div>";
                    }
                    else {
                        return $("<img/>").attr("src", imagePath);
                    }
                }
            });
            $(".showtooltip").tooltip({
                showURL: false
            });
        });

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
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            $("ineditcategory").tooltip();
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
                        var r = confirm("<%= Resource.Admin_NewsAdmin_Confirm%>");
                        if (r) document.getElementById('<%=lbDeleteSelected.ClientID%>').click();
                        //if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                    case "setInMainPage":
                        var r = confirm("<%= Resource.Admin_NewsAdmin_Confirm%>");
                        if (r) {
                            document.getElementById('<%=lbSetInMainPage.ClientID%>').click();
                        }
                        break;
                    case "setOutMainPage":
                        var r = confirm("<%= Resource.Admin_NewsAdmin_Confirm%>");
                        if (r) {
                            document.getElementById('<%=lbSetOutMainPage.ClientID%>').click();
                        }
                        break;
                    case "changeCategoryNews":
                        var r = confirm("<%= Resource.Admin_NewsAdmin_Confirm%>");
                        if (r) {
                            document.getElementById('<%=lbChangeCategoryNews.ClientID%>').click();
                        }
                        break;

                }
            });
        });

        function ChangeCategory() {
            if ($("#commandSelect option:selected").val() == "changeCategoryNews") {
                $("#<%= ddlChangeCategoryNews.ClientID %>").show();
            } else {
                $("#<%= ddlChangeCategoryNews.ClientID %>").hide();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: #0D76B8;">
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
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_NewsAdmin_Header %>"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_NewsAdmin_SubHeader %>"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 100%">
                <div>
                    <div class="btns-main">
                        <a href="EditNews.aspx" class="btn btn-middle btn-add"><%= Resource.Admin_HeadCmdInsert %></a>
                    </div>
                    <div style="height: 10px">
                    </div>
                    <table style="width: 99%;" class="massaction">
                        <tr>
                            <td>
                                <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                                </span><span style="display: inline-block">
                                    <select id="commandSelect" onchange="ChangeCategory();">
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
                                        <option value="setInMainPage">
                                            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_NewsAdmin_ShowOnMain %>"></asp:Localize>
                                        </option>
                                        <option value="changeCategoryNews">
                                            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_NewsAdmin_ChangeCategory %>"></asp:Localize>
                                        </option>
                                    </select>
                                    <asp:DropDownList ID="ddlChangeCategoryNews" DataSourceID="sdsNewsCategoryID" runat="server"
                                        DataTextField="Name" DataValueField="NewsCategoryID" OnDataBound="ddlNewsCategoryID_DataBound"
                                        Style="display: none;">
                                    </asp:DropDownList>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                        height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbDeleteSelected_Click" />
                                    <asp:LinkButton ID="lbSetInMainPage" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbSetInMainPage_Click" />
                                    <asp:LinkButton ID="lbSetOutMainPage" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbSetOutMainPage_Click" />
                                    <asp:LinkButton ID="lbChangeCategoryNews" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbChangeCategoryNews_Click" />
                                </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                    |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resource.Admin_Catalog_ItemsSelected%></span></span>
                                </span>
                                <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td class="selecteditems" style="text-align: right;">
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
                            <td style="width: 8px;">
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                            <table class="filter" cellpadding="0" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="7">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 60px; text-align: center;">
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="55">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 35px; text-align: center; font-size: 13px">
                                        <div style="width: 35px; height: 0px; font-size: 0px;">
                                        </div>
                                    </td>
                                    <td>
                                        <div style="width: 200px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtTitle" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 150px; text-align: center;">
                                        <div style="width: 150px; height: 0px; font-size: 0px;">
                                        </div>
                                    </td>
                                    <td style="width: 250px;">
                                        <asp:DropDownList ID="ddlNewsCategoryID" DataSourceID="sdsNewsCategoryID" runat="server"
                                            DataTextField="Name" DataValueField="NewsCategoryID" Width="99%" CssClass="dropdownselect"
                                            OnDataBound="ddlNewsCategoryID_DataBound">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 200px; text-align: center;">
                                        <div style="width: 200px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:DropDownList ID="ddlShowOnMainPage" TabIndex="18" CssClass="dropdownselect"
                                            runat="server">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                                Value="any" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 85px; text-align: center;">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                            TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="7">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:SqlDataSource ID="sdsNewsCategoryID" runat="server" SelectCommand="SELECT NewsCategoryID, Name FROM [Settings].[NewsCategory]"
                                    OnInit="sds_Init"></asp:SqlDataSource>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>"
                                    CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                                    OnSorting="grid_Sorting" ShowFooter="false" ShowFooterWhenEmpty="true" OnRowDataBound="grid_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 60px; height: 0px; font-size: 0px">
                                                </div>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Photo" HeaderStyle-Width="30px">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 30px; font-size: 0px;">
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# GetImageItem(SQLDataHelper.GetInt(Eval("ID")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 200px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderTitle" runat="server" CommandName="Sort" CommandArgument="Title">
                                                    <%=Resource.Admin_NewsAdmin_Title%>
                                                    <asp:Image ID="arrowTitle" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTitleBind" runat="server" Text='<%# Eval("Title") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="AddingDate" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div style="width: 150px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderAddingDate" runat="server" CommandName="Sort" CommandArgument="AddingDate">
                                                    <%=Resource.Admin_NewsAdmin_AddDate%>
                                                    <asp:Image ID="arrowAddingDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lAddingDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertDate((DateTime) Eval("AddingDate")) %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lAddingDate2" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertDate((DateTime) Eval("AddingDate")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="NewsCategoryID" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lbOrderNewsCategoryID" runat="server" CommandName="Sort" CommandArgument="NewsCategoryID">
                                                    <%=Resource.Admin_NewsAdmin_Category%>
                                                    <asp:Image ID="arrowNewsCategoryID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList runat="server" ID="ddlNewsCategoryIDET" DataSourceID="sdsNewsCategoryID"
                                                    DataTextField="Name" Width="98%" DataValueField="NewsCategoryID">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ShowOnMainPage" ItemStyle-Width="200" HeaderStyle-Width="200"
                                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderShowOnMainPage" runat="server" CommandName="Sort" CommandArgument="ShowOnMainPage">
                                                    <%=Resource.Admin_NewsAdmin_OnMainPage%>
                                                    <asp:Image ID="arrowShowOnMainPage" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkShowOnMainPage" runat="server" Checked='<%# Bind("ShowOnMainPage") %>' />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkShowOnMainPage2" runat="server" Checked='<%# Bind("ShowOnMainPage") %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="85px" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Center">
                                            <EditItemTemplate>
                                                <a id="cmdlink" href='EditNews.aspx?NewsId=<%# Eval("ID") %>' class="editbtn showtooltip" title='<%=Resource.Admin_MasterPageAdminCatalog_Edit%>'>
                                                    <img src="images/editbtn.gif" style="border: none;" /></a>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                    style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                                <asp:ImageButton ID="buttonSendMail" runat="server" ImageUrl="images/send_mail.png" CssClass="valid-confirm" data-confirm="<%$ Resources:Resource, Admin_NewsAdmin_SendConfirmation %>"
                                                CommandName="SendNews" CommandArgument='<%# Eval("ID")%>' ToolTip='<%$ Resources:Resource, Admin_NewsAdmin_Send %>' Visible="<%# CanSendNews %>" />
                                                <asp:LinkButton ID="buttonDelete" runat="server"
                                                    CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteNews" CommandArgument='<%# Eval("ID")%>'
                                                    data-confirm="<%$ Resources:Resource, Admin_NewsAdmin_Confirmation %>"
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                    style="display: none" title="<%=Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
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
                                <div style="border-top: 1px #c9c9c7 solid;">
                                </div>
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
                                        <td style="text-align: center;">
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
            </div>
        </div>
    </div>
</asp:Content>
