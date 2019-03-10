<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/MasterPageAdmin.master" Inherits="Admin.LogViewer" Codebehind="LogViewer.aspx.cs" %>

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

    </script>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="cphMain" runat="Server">
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
            <li class="neighbor-menu-item"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
            <li class="neighbor-menu-item"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div style="text-align: center">
            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker %>"></asp:Label>
            <br />
            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Internal %>"></asp:Label>
            <br />
            <br />
            /&nbsp;<%--<asp:HyperLink ID="hlErr404" runat="server" CssClass="Link"><%= Resource.Admin_BugTrackerError404_aspx%></asp:HyperLink>&nbsp;/--%>
            <asp:HyperLink ID="hlErr500" runat="server" CssClass="Link"><%= Resource.Admin_MasterPageAdmin_BugTracker_Internal%></asp:HyperLink>&nbsp;/
            <asp:HyperLink ID="hlErrHttp" runat="server" CssClass="Link"><%= Resource.Admin_MasterPageAdmin_BugTracker_Other%></asp:HyperLink>&nbsp;/
            <asp:HyperLink ID="hlInfo" runat="server" CssClass="Link">Info</asp:HyperLink>&nbsp;/
            <br />
            <br />
            <asp:Label runat="server" ID="lblErr" ForeColor="Red"></asp:Label>
        </div>


        <adv:AdvGridView ID="grid" AutoGenerateColumns="false" runat="server" CssClass="tableview break"
            ReadOnlyGrid="true" CellPadding="2" CellSpacing="0" Width="100%" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Num" AccessibleHeaderText="Num" ItemStyle-Width="20px"
                    HeaderStyle-Width="20px">
                    <HeaderTemplate>
                        <span>
                            <%= Resource.Admin_MasterPageAdmin_BgTHeaderNum%></span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        &nbsp;<%#Container.DataItemIndex  + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date" AccessibleHeaderText="Date" ItemStyle-Width="180px"
                    HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left">
                    <HeaderTemplate>
                        <span>
                            <%= Resource.Admin_MasterPageAdmin_BgTHeaderDate%></span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("TimeStamp")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Message" AccessibleHeaderText="Message" ItemStyle-Width="250px"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderTemplate>
                        <span>
                            <% = Resource.Admin_MasterPageAdmin_BgTHeaderMessage%></span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#(string)Eval("Message")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ErorMessage" AccessibleHeaderText="ErorMessage" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>StackTrace</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#(string)Eval("ErrorMessage")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <a href="<%# "LogViewerDetailed.aspx?atTime=" + HttpUtility .UrlEncode(Eval("TimeStamp").ToString()) + "&ErrType=" + CurrentView.ToString( )%>">
                            <% = Resource.Admin_MasterPageAdmin_BgTHeaderDetails%></a>
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
    </div>
</asp:Content>
