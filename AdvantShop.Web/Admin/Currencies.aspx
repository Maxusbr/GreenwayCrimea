<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Currencies" CodeBehind="Currencies.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Helpers" %>
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
                        var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });

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
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item"><a href="Country.aspx">
                <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
            <li class="neighbor-menu-item selected"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
            <li class="neighbor-menu-item"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
            <li class="neighbor-menu-item"><a href="Localizations.aspx">
                <%= Resource.Admin_Localizations_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Currencies_Header %>"></asp:Label>
                        <br />
                        <div style="margin-top: 6px;">
                            <asp:CheckBox runat="server" ID="chkAutoUpdateEnabled" CssClass="checkly-align" OnCheckedChanged="chkAutoUpdateEnabled_CheckedChanged" AutoPostBack="True" />
                            <label class="form-lbl2" for="<%= chkAutoUpdateEnabled.ClientID %>"><%= Resources.Resource.Admin_Currencies_AutoUpdate%></label>
                            <div data-plugin="help" class="help-block">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Автообновление валют
                                    </header>
                                    <div class="help-content">
                                        Включение этой опции означает, что валюты будут синхронизироваться по расписанию, в ночное время.<br />
                                        <br />
                                        Если вы используете не рубль, как основную валюту магазина, не включайте данную опцию.
                                    </div>
                                </article>
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: bottom;">
                        <div style="float: right;">
                            <div class="btns-main">
                                <asp:Button CssClass="btn btn-middle btn-action" ID="btnUpdateCurrencies" runat="server" Style="margin-right: 5px;"
                                    Text="<%$ Resources:Resource, Admin_Currencies_GetCurriencies%>" ValidationGroup="0" OnClick="btnUpdateCurrencies_Click" />
                                <asp:Button CssClass="btn btn-middle btn-add" ID="btnAddCurrency" runat="server"
                                    Text="<%$ Resources:Resource, Admin_Currencies_Add %>" ValidationGroup="0" OnClick="btnAddCurrency_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="margin-top: 5px;">
            <table style="width: 100%;" class="massaction">
                <tr>
                    <td colspan="2">
                        <center>
                            <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                        </center>
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
                            </select>
                            <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px; height: 20px;" />
                            <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                OnClick="lbDeleteSelected_Click" />
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
                            <td style="width: 80px; text-align: center;">
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="65">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div style="width: 110px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="99%" runat="server" TabIndex="12" />
                            </td>
                            <td style="width: 140px;">
                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                </div>
                                <%--<asp:TextBox CssClass="filtertxtbox" ID="txtCode" Width="99%" runat="server"
                                    TabIndex="12" />--%>
                            </td>
                            <td style="width: 140px;">
                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtValue" Width="99%" runat="server" TabIndex="12" />
                            </td>
                            <td style="width: 140px;">
                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtISO3" Width="99%" runat="server" TabIndex="12" />
                            </td>
                            <td style="width: 140px;">
                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                </div>
                                <%--<asp:TextBox CssClass="filtertxtbox" ID="txtFormat" Width="99%" runat="server"
                                    TabIndex="12" />--%>
                            </td>
                            <td style="width: 140px; text-align: center;">
                                <asp:DropDownList ID="ddBefore" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="145">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <div style="width: 210px; font-size: 0px; height: 0px;">
                                </div>
                                <%--<asp:TextBox CssClass="filtertxtbox" ID="txtFormat" Width="99%" runat="server"
                                    TabIndex="12" />--%>
                            </td>
                            <td style="width: 100px;">
                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                </div>
                                <center>
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </center>
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
                            OnSorting="grid_Sorting" OnRowDataBound="grid_RowDataBound" ShowFooter="false">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="Label01" runat="server" Text='0'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
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
                                <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 100px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                            <%=Resources.Resource.Admin_Currencies_Name %>
                                            <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCurrencyName" runat="server" Text='<%# Eval("Name") %>' Width="99%" MaxLength="30"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewName" CssClass="add" runat="server" Text='' Width="99%" MaxLength="30"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Code" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="130px">
                                    <HeaderTemplate>
                                        <div style="width: 130px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbCode" runat="server" CommandName="Sort" CommandArgument="Code">
                                            <%= Resources.Resource.Admin_Currencies_Code %>
                                            <asp:Image ID="arrowCode" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("Code") %>' Width="99%" MaxLength="7"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewCode" CssClass="add" runat="server" Text='' Width="99%" MaxLength="7"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="CurrencyValue" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="130px">
                                    <HeaderTemplate>
                                        <div style="width: 130px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbCurrencyValue" runat="server" CommandName="Sort" CommandArgument="CurrencyValue">
                                            <%=Resources.Resource.Admin_Currencies_Value %>
                                            <asp:Image ID="arrowCurrencyValue" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCurrencyValue" runat="server" Text='<%# SQLDataHelper.GetFloat(Eval("CurrencyValue")).ToString("F6") %>'
                                            Width="99%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lCurrencyValue" runat="server" Text='<%# SQLDataHelper.GetFloat(Eval("CurrencyValue")).ToString("F6") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewCurrencyValue" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="CurrencyISO3" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="130px">
                                    <HeaderTemplate>
                                        <div style="width: 130px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbCurrencyISO3" runat="server" CommandName="Sort" CommandArgument="CurrencyISO3">
                                            <%=Resources.Resource.Admin_Currencies_Iso3 %>
                                            <asp:Image ID="arrowCurrencyISO3" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCurrencyISO3" runat="server" Text='<%# Eval("CurrencyISO3") %>' MaxLength="3"
                                            Width="99%" Visible='<%#!(Eval("CurrencyISO3").ToString().Equals((AdvantShop.Configuration.SettingsCatalog.DefaultCurrencyIso3))) %>'></asp:TextBox>
                                        <asp:Label ID="labelCurrentISO3" runat="server" Text='<%# Eval("CurrencyISO3") %>'
                                            Visible='<%# (Eval("CurrencyISO3").ToString().Equals((AdvantShop.Configuration.SettingsCatalog.DefaultCurrencyIso3))) %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lCurrencyISO3" runat="server" Text='<%# Bind("CurrencyISO3") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewCurrencyISO3" CssClass="add" runat="server" Text='' Width="99%" MaxLength="3"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="CurrencyNumIso3" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="130px">
                                    <HeaderTemplate>
                                        <div style="width: 130px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbCurrencyNumIso3" runat="server" CommandName="Sort" CommandArgument="CurrencyNumIso3">
                                            <%=Resource.Admin_Currencies_NumIso3 %>
                                            <asp:Image ID="arrowCurrencyNumIso3" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCurrencyNumIso3" runat="server" Text='<%# Eval("CurrencyNumIso3") %>' MaxLength="3"
                                            Width="99%" Visible='<%#!(Eval("CurrencyISO3").ToString().Equals((AdvantShop.Configuration.SettingsCatalog.DefaultCurrencyIso3))) %>'></asp:TextBox>
                                        <asp:Label ID="labelCurrencyNumIso3" runat="server" Text='<%# Eval("CurrencyNumIso3") %>'
                                            Visible='<%# (Eval("CurrencyISO3").ToString().Equals((AdvantShop.Configuration.SettingsCatalog.DefaultCurrencyIso3))) %>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewCurrencyNumIso3" CssClass="add" runat="server" Text='' Width="99%" MaxLength="3"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="IsCodeBefore" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="width: 130px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbIsCodeBefore" runat="server" CommandName="Sort" CommandArgument="IsCodeBefore">
                                            <%=Resources.Resource.Admin_Currencies_IsCodeBefore %>
                                            <asp:Image ID="arrowIsCodeBefore" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="cbIsCodeBefore" runat="server" Checked='<%# Eval("IsCodeBefore")%>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="checkNewIsCodeBefore" CssClass="add" runat="server" Checked="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="PriceRounding" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="width: 200px; height: 0px; font-size: 0px;">
                                        </div>
                                        <%=Resource.Admin_Currencies_RoundNumbers %>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPriceRounding" runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlNewPriceRounding" CssClass="add" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="90px">
                                    <EditItemTemplate>
                                        <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                            src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>; return false;"
                                            style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                        <asp:LinkButton ID="buttonDelete" runat="server"
                                            CssClass="deletebtn showtooltip valid-confirm" CommandName="DeleteCurrency" CommandArgument='<%# Eval("ID")%>'
                                            Visible='<%# !(Eval("CurrencyISO3").ToString().Equals((AdvantShop.Configuration.SettingsCatalog.DefaultCurrencyIso3))) %>'
                                            data-confirm="<%$ Resources:Resource, Admin_Currencies_Confirmation %>"
                                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                        <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                            src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]); return false;"
                                            style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibAddCurrency" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddCurrency"
                                            ToolTip="<%$ Resources:Resource, Admin_Currencies_Add  %>" />
                                        <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                            CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Currencies_CancelAdd  %>" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#ccffcc" />
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_Currencies_NoRecords%>
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
        </div>
        <div class="dvSubHelp">
            <asp:Image runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
            <a href="http://www.advantshop.net/help/pages/currency" target="_blank">Инструкция. Настройка параметров валюты в магазине</a>
        </div>
    </div>
    <input type="hidden" id="SelectedIds" name="SelectedIds" />
    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
    </script>
</asp:Content>
