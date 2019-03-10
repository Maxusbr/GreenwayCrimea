<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.SearchStatistic" Codebehind="SearchStatistic.aspx.cs" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-own">
        <table style="width: 100%; border-collapse: collapse; margin: 0; padding: 0;">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_SearchStatistic_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_SearchStatistic_SubHeader %>"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="margin:10px 0;">            
            <%= Resources.Resource.Admin_SearchStatistic_Term%>
            <asp:Button ID="btnHistory" runat="server" PostBackUrl="SearchStatistic.aspx?view=history"
                Text="<%$ Resources:Resource, Admin_SearchStatistic_Chronology %>" />
            <asp:Button ID="btnFrequency" runat="server" PostBackUrl="SearchStatistic.aspx?view=frequency"
                Text="<%$ Resources:Resource, Admin_SearchStatistic_Frequency %>" />
            <asp:Label ID="lblDisplayMode" runat="server" Style="float: right;" Text=""></asp:Label>
        </div>
        <asp:Panel ID="pnlHistory" runat="server" Visible="False">

            <adv:AdvGridView ID="gridHistory" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="2" CellSpacing="0" CssClass="tableview" Style="cursor: pointer"
                                    DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                                    EditURL="" GridLines="None"
                                    ShowFooter="false">
                <Columns>
                    <asp:TemplateField AccessibleHeaderText="SearchTerm" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 150px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_Term%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href='<%# UrlService.GetAbsoluteLink(Convert.ToString( Eval("Request"))+"&ignorelog=1") %>'
                                target="_blank">
                                <%# Eval("SearchTerm")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField AccessibleHeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 150px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_QueryDescription%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("Description") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField AccessibleHeaderText="ResultCount" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 200px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_Found%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("ResultCount")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField AccessibleHeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 200px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_Date%>                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("Date")%>
                        </ItemTemplate>
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

            <div class="divNavigation">
                <br />                
                <%=Resources.Resource.Admin_SearchStatistic_ShowLast%>
                <ul>
                    <li><a href="SearchStatistic.aspx?view=history&rows=10" class='<%= Request["rows"] == "10" || string.IsNullOrEmpty(Request["rows"]) ? "activeLink" : string.Empty %>'>
                        10</a> </li>
                    <li><a href="SearchStatistic.aspx?view=history&rows=50" class='<%= Request["rows"] == "50" ? "activeLink" : string.Empty %>'>
                        50</a></li>
                    <li><a href="SearchStatistic.aspx?view=history&rows=100" class='<%= Request["rows"] == "100" ? "activeLink" : string.Empty %>'>
                        100</a> </li>
                    <li><a href="SearchStatistic.aspx?view=history&rows=200" class='<%= Request["rows"] == "200" ? "activeLink" : string.Empty %>'>
                        200</a> </li>
                    <li><a href="SearchStatistic.aspx?view=history&rows=500" class='<%= Request["rows"] == "500" ? "activeLink" : string.Empty %>'>
                        500</a> </li>
                </ul>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlFrequency" runat="server" Visible="False">

                <adv:AdvGridView ID="gridFrequency" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="2" CellSpacing="0" CssClass="tableview" Style="cursor: pointer"
                                    DataFieldForEditURLParam="" DataFieldForImageDescription="" DataFieldForImagePath=""
                                    EditURL="" GridLines="None"
                                    ShowFooter="false">
                <Columns>
                    <asp:TemplateField AccessibleHeaderText="SearchTerm" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 150px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_Term%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href='<%# UrlService.GetAbsoluteLink(Convert.ToString( Eval("Request"))+"&ignorelog=1") %>'
                                target="_blank">
                                <%# Eval("SearchTerm")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField AccessibleHeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 150px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_QueryDescription%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("Description") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField AccessibleHeaderText="ResultCount" HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>
                            <div style="width: 200px; font-size: 0px; height: 0px;">
                            </div>
                            <%= Resources.Resource.Admin_SearchStatistic_QueryCount%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("numOfRequest")%>
                        </ItemTemplate>
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

            <div class="divNavigation">
                <br />
               <%=Resources.Resource.Admin_SearchStatistic_ShowLast%>
                <ul>
                    <li><a href="SearchStatistic.aspx?view=frequency&span=day" class='<%= Request["span"] == "day" || string.IsNullOrEmpty(Request["span"]) ? "activeLink" : string.Empty %>'>
                        <%=Resources.Resource.Admin_SearchStatistic_ShowLastPerDay%></a> </li>
                    <li><a href="SearchStatistic.aspx?view=frequency&span=week" class='<%= Request["span"] == "week" ? "activeLink" : string.Empty %>'>
                        <%=Resources.Resource.Admin_SearchStatistic_ShowLastForWeek%></a> </li>
                    <li><a href="SearchStatistic.aspx?view=frequency&span=mounth" class='<%= Request["span"] == "mounth" ? "activeLink" : string.Empty %>'>
                        <%=Resources.Resource.Admin_SearchStatistic_ShowLastForMonth%></a> </li>
                </ul>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
