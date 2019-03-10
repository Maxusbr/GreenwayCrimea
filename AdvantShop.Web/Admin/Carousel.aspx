<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Carouseles" CodeBehind="Carousel.aspx.cs" %>

<%@ Import Namespace="AdvantShop.FilePath" %>
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
            var prm = Sys.WebForms.PageRequestManager.getInstance();
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
                        var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                    case "setActive":
                        document.getElementById('<%=lbSetActive.ClientID%>').click();
                        break;
                    case "setNotActive":
                        document.getElementById('<%=lbSetNotActive.ClientID%>').click();
                        break;
                }
            });
        });

        function Validation() {
            var valid = true;
            if ($("#<%=CarouselLoad.ClientID %>").val() == "") {
                valid = false;
            }

            if ($("#<%=txtSortedCarousel.ClientID %>").val() == "" || !parseInt($("#<%=txtSortedCarousel.ClientID %>").val())) {
                $("#<%=txtSortedCarousel.ClientID %>").css("background-color", "#FF0033");
                valid = false;
            }
            else {
                $("#<%=txtSortedCarousel.ClientID %>").css("border", "inherit");
            }
            return valid;
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
            </table>
        </div>
    </div>
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="Menu.aspx">
                <%= Resource.Admin_MasterPageAdmin_MainMenu%></a></li>
            <li class="neighbor-menu-item"><a href="NewsAdmin.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsMenuRoot%></a></li>
            <li class="neighbor-menu-item"><a href="NewsCategory.aspx">
                <%= Resource.Admin_MasterPageAdmin_NewsCategory%></a></li>
            <li class="neighbor-menu-item selected"><a href="Carousel.aspx">
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
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Carousels_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Carousels_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <div style="float: right; padding-right: 20px">
                            <div style="height: 41px" id="advButtonFiller" runat="server">
                            </div>
                            <div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" class="tableCarousel" width="100%" cellspacing="0" style="padding-left: 10px; padding-right: 10px">
                <tr>
                    <td style="width: 200px;">
                        <asp:Label ID="lbCheckFile" runat="server" Text="<%$ Resources:Resource, Admin_Carousels_SelectFile %>">:
                        </asp:Label>
                    </td>
                    <td>
                        <div style="color: Red; float: left;">
                            <asp:FileUpload ID="CarouselLoad" runat="server" />
                            <div>
                                <asp:Label runat="server" ID="lblErrorImage" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div style="float: left;">
                            Рекомендуемый размер изображения:<br />
                            <b>1160px * 553px</b> - одна колонка<br />
                            <b>865px * 554px</b> - две колонки<br />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:Label ID="lbURL" runat="server" Text="<%$ Resources:Resource, Admin_Carousels_UrlForNavigation %>">
                        </asp:Label>
                    </td>
                    <td style="color: Red;">
                        <asp:TextBox ID="txtURL" runat="server" Width="200px">
                        </asp:TextBox>*
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:Label ID="lbSortedCarousel" runat="server" Text="<%$ Resources:Resource,Admin_Carousels_SortOrder  %>">
                        </asp:Label>
                    </td>
                    <td style="color: Red;">
                        <asp:TextBox ID="txtSortedCarousel" runat="server" Width="200px">
                        </asp:TextBox>*
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="bthAddCarousel" runat="server" Text="<%$ Resources:Resource, Admin_Carousels_Add %>"
                            OnClientClick="javascript:return Validation();" OnClick="bthAddCarousel_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <ul id="ulValidationFailed" runat="server" visible="false" class="ulValidFaild">
            </ul>
            <div style="width: 100%">
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                        </td>
                    </tr>
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
                                    <option value="setActive">
                                        <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_Carousel_SetActive %>"></asp:Localize>
                                    </option>
                                    <option value="setNotActive">
                                        <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_Carousel_SetNotActive %>"></asp:Localize>
                                    </option>
                                </select>
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                                <asp:LinkButton ID="lbSetActive" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetActive %>"
                                    OnClick="lbSetActive_Click" />
                                <asp:LinkButton ID="lbSetNotActive" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetDeactive %>"
                                    OnClick="lbSetNotActive_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">|</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span></span>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <%=Resources.Resource.Admin_Catalog_Total%>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resources.Resource.Admin_Catalog_RecordsFound%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;"></td>
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
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 320px;">
                                    <div style="width: 320px; font-size: 0px; height: 0px;">
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 130px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtUrlFilter" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 130px;">
                                    <div style="width: 130px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSortOrder" Width="99%" runat="server"
                                        TabIndex="12" />
                                </td>
                                <td style="width: 130px; text-align: center;">
                                    <asp:DropDownList ID="ddlEnabled" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="145">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 90px; text-align: center;">
                                    <div style="width: 90px; font-size: 0px; height: 0px;">
                                    </div>
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="5"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="grid" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="grid" EventName="Sorting" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Currencies_QDelete %>"
                                CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                                OnSorting="grid_Sorting" ShowFooter="false">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="Label02" runat="server" Text='0'></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 70px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="CarouselID" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="220px" HeaderStyle-Width="220px">
                                        <HeaderTemplate>
                                            <div style="width: 220px; font-size: 0px; height: 0px;">
                                            </div>
                                            <%=Resources.Resource.Admin_Carousel_Image %>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <img src='<%# FoldersHelper.GetPath(FolderType.Carousel,  Eval("PhotoName") as string , true) %>'
                                                style="border: 1px solid #000000; vertical-align: middle;" width="200" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <img src='<%# FoldersHelper.GetPath(FolderType.Carousel,  Eval("PhotoName") as string , true) %>'
                                                style="border: 1px solid #000000; vertical-align: middle;" width="200" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="URL" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="width: 130px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbSortURL" runat="server" CommandName="Sort" CommandArgument="URL">
                                                URL
                                                <asp:Image ID="arrowURL" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtURLBind" runat="server" Text='<%# Eval("URL") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lURL" runat="server" Text='<%# Bind("URL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="width: 130px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDescription" runat="server" CommandName="Sort" CommandArgument="Description">
                                                <%=Resources.Resource.Admin_Carousels_Alt%>
                                                <asp:Image ID="arrowDescription" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescriptionBind" runat="server" Text='<%# Eval("Description") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DisplayInOneColumn" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 130px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDisplayInOneColumn" runat="server" CommandName="Sort" CommandArgument="DisplayInOneColumn">
                                                <%=Resources.Resource.Admin_TemplateSettings_MainPageModeDefault%>
                                                <asp:Image ID="arrowDisplayInOneColumn" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbDisplayInOneColumn" runat="server" Checked='<%# Eval("DisplayInOneColumn")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DisplayInTwoColumns" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 130px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDisplayInTwoColumns" runat="server" CommandName="Sort" CommandArgument="DisplayInTwoColumns">
                                                <%=Resources.Resource.Admin_TemplateSettings_MainPageModeTwoColumns%>
                                                <asp:Image ID="arrowDisplayInTwoColumns" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbDisplayInTwoColumns" runat="server" Checked='<%# Eval("DisplayInTwoColumns")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DisplayInMobile" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 130px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDisplayInMobile" runat="server" CommandName="Sort" CommandArgument="DisplayInMobile">
                                                <%=Resources.Resource.Admin_TemplateSettings_Mobile%>
                                                <asp:Image ID="arrowDisplayInMobile" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbDisplayInMobile" runat="server" Checked='<%# Eval("DisplayInMobile")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SortOrder" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="130px">
                                        <HeaderTemplate>
                                            <div style="width: 130px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                <%=Resources.Resource.Admin_Carousels_SortOrder%>
                                                <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSortOrderBind" runat="server" Text='<%# Eval("SortOrder") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Enabled" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 130px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="Enabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                <%=Resources.Resource.Admin_Carousels_Enabled%>
                                                <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbEnabled" runat="server" Checked='<%# Eval("Enabled")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="90px">
                                        <EditItemTemplate>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                                style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                            <asp:LinkButton ID="buttonDelete" runat="server"
                                                CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteCarousel" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_Caroucel_Confirmation %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#ccffcc" />
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_Carousels_NoRecords%>
                                </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                            runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
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
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
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
</asp:Content>
