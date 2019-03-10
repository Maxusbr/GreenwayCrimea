<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Answers" Codebehind="Answers.aspx.cs" %>

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
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            $("ineditcategory").tooltip();
        })

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
                        var r = confirm("<%= Resources.Resource.Admin_Answers_Confirm%>");
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
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td align="center">
                            <img src="~/admin/images/ajax-loader.gif" />
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
    <div style="padding-left: 10px; padding-right: 10px">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Answers_SubHeader %>"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:hyperlink ID="Hyperlink1" NavigateUrl="~/Admin/Voting.aspx" Text='<%$ Resources: Resource, Admin_Back %>' runat="server" CssClass="Link"></asp:hyperlink>
        <div style="width: 100%">
            <div>
                <div class="btns-main">
                    <asp:Button CssClass="btn btn-middle btn-add" ID="btnAdd" runat="server" Text="<%$ Resources:Resource, Admin_HeadCmdInsertVoitingAnswer %>" OnClick="btnAdd_Click" />
                </div>
                <div style="height: 10px">
                </div>
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block">
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
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                    height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                    OnClick="lbDeleteSelected_Click" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span>
                            </span>
                            <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
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
                        <td style="width: 8px;">
                        </td>
                    </tr>
                </table>
                <div style="border: 1px #c9c9c7 solid; width: 100%">
                    <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                        <table class="filter" cellpadding="0" cellspacing="0">
                            <tr style="height: 5px;">
                                <td colspan="6">
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
                                <td>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="99%" runat="server" />
                                </td>
                                <td style="width: 150px;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtCountVoice" Width="99%" runat="server" />
                                </td>
                                <td style="width: 150px;">
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtSort" Width="99%" runat="server" />
                                </td>
                                <td style="text-align: center; width: 120px;">
                                    <asp:DropDownList ID="ddlIsVisible" TabIndex="18" CssClass="dropdownselect" runat="server">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="any" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px;">
                                </td>
                                <td style="width: 100px;">
                                    <!--TODO date filter-->
                                </td>
                                <td style="width: 60px;">
                                    <center>
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                            TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="6">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Answers_Confirmation %>"
                                CssClass="tableview" Style="cursor: pointer" GridLines="None" OnRowCommand="grid_RowCommand"
                                OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" ShowFooter="false"
                                >
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="60px" HeaderStyle-Width="60px"
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
                                        <FooterTemplate>
                                            <div style="width: 60px; font-size: 0px">
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                <%=Resources.Resource.Admin_Voting_Name%>
                                                <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVoiceName" runat="server" Text='<%# Eval("Name") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewName" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="CountVoice" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="150px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderCountVoice" runat="server" CommandName="Sort" CommandArgument="CountVoice">
                                            <div style="float: left; width: 95px; text-align: center;">
                                                <%=Resources.Resource.Admin_Voting_VoiceCount%>
                                                </div>
                                                <asp:Image ID="arrowCountVoice" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" Style="margin-top: 7px; margin-left: 0px;"  /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCountVoiceBind" runat="server" Text='<%# Eval("CountVoice") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lCountVoice" runat="server" Text='<%# Bind("CountVoice") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewCountVoice" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Sort" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="150px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderSort" runat="server" CommandName="Sort" CommandArgument="Sort">
                                                <%=Resources.Resource.Admin_Voting_Sorting%>
                                                <asp:Image ID="arrowSort" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSortBind" runat="server" Text='<%# Eval("Sort") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lSort" runat="server" Text='<%# Bind("Sort") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewSort" runat="server" CssClass="add" Text='' Width="99%"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="IsVisible" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderIsVisible" runat="server" CommandName="Sort" CommandArgument="IsVisible">
                                                <%=Resources.Resource.Admin_Voting_Visible%>
                                                <asp:Image ID="arrowIsVisible" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkIsVisible" runat="server" Checked='<%# Bind("IsVisible") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsVisible2" runat="server" Checked='<%# Bind("IsVisible") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkNewIsVisible" runat="server" CssClass="add" Checked="True" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DateAdded" HeaderText="<%$ Resources:Resource, Admin_AddDate %>">
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        <ControlStyle CssClass="Link" />
                                        <HeaderStyle ForeColor="White" CssClass="GridView_HeaderStyle_BoundField" Width="100px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderDateAdded" runat="server" CommandName="Sort" CommandArgument="DateAdded">
                                                <div style="float: left; width: 80px; text-align: center;">
                                                    <%=Resources.Resource.Admin_AddDate%>
                                                </div>
                                                <asp:Image ID="arrowDateAdded" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif"
                                                    Style="margin-top: 7px; margin-left: 0px;" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("DateAdded"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DateModify" HeaderText="<%$ Resources:Resource, Admin_ModifyDate %>">
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        <ControlStyle CssClass="Link" />
                                        <HeaderStyle ForeColor="White" CssClass="GridView_HeaderStyle_BoundField" Width="100px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderDateModify" runat="server" CommandName="Sort" CommandArgument="DateModify">
                                                <%=Resources.Resource.Admin_ModifyDate%>
                                                <asp:Image ID="arrowDateModify" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#AdvantShop.Localization .Culture.ConvertDate((DateTime)Eval("DateModify"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <asp:Image ID="buttonEdit" runat="server" ImageUrl="images/editbtn.gif" CssClass="editbtn showtooltip"
                                                title='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Edit %>' />
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Update %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibAddProperty" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddAnswer"
                                                ToolTip="<%$ Resources:Resource, Admin_Property_Add  %>" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="buttonDelete" runat="server"
                                                CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteAnswer" CommandArgument='<%# Eval("ID")%>'
                                                data-confirm="<%$ Resources:Resource, Admin_Answers_Confirmation %>"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Property_CancelAdd  %>" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#ccffcc" />
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <center style="margin-top: 20px; margin-bottom: 20px;">
                                        <%=Resources.Resource.Admin_Catalog_NoRecords%>
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
