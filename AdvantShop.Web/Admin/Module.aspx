<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="Admin.Module" CodeBehind="Module.aspx.cs" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        function showElement(span) {
            var method_id = $("input:hidden", $(span)).val();
            location = "module.aspx?module=<%= Server.UrlEncode(Request["module"]) %>&currentcontrolindex=" + method_id + (<%=  !string.IsNullOrEmpty(Request["MasterPageEmpty"]) && Request["MasterPageEmpty"].AsBool() == true ? "true" : "false" %> ? "&MasterPageEmpty=true" : "" );
        }
    </script>
    <link href="css/new_admin/modules-settings.css" rel="stylesheet" type="text/css" />

    <%if (InNewView)
        { %>   
        <link href="css/new_admin/modules-settings-in-new-view.css" rel="stylesheet"  />
    <% } %>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMain" runat="Server">
    <div class="content-own <%= InNewView ? "module-in-new-view" : "" %>">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="3" style="vertical-align: middle; padding-bottom: 15px;">
                    <img src="images/orders_ico.gif" alt="" style="float: left; margin-right: 10px;" runat="server" id="imgDecor" />
                    <div style="float: left;" class="module-in-new-view-header">
                        <span style="font-family: Verdana; font-size: 18pt;">Модуль "</span>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Module_Header %>" />
                        <span style="font-family: Verdana; font-size: 18pt;">"</span>
                        <asp:Label ID="lblIsActive" CssClass="lblIsActive" runat="server" Style="margin-left: 15px; font-size: 18px;"></asp:Label>
                        <%--<br /><asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Module_SubHeader %>"></asp:Label>--%>
                        <div style="margin-top: 10px;">
                            <label>
                                <input type="checkbox" id="ckbActiveModule" runat="server" class="ckbActiveModule"
                                    data-modulestringid="" />&nbsp;<asp:Label ID="lblActiveModule" runat="server" Text="<%$ Resources:Resource, Admin_Module_ModuleActive%>"></asp:Label></label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="lnkInstruction_block" runat="server" class="dvSubHelp4" style="margin-top: 0;" visible="False">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
                        <asp:HyperLink ID="lnkInstruction" runat="server" Target="Blank"></asp:HyperLink>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 100%" colspan="3" id="tdModuleSettings" runat="server" Visible="True">
                    <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
                        <tr>
                            <td style="vertical-align: top; width: 225px;" id="tdTabsHeader" runat="server">
                                <ul class="tabs" id="tabs-headers">
                                    <asp:Repeater runat="server" ID="rptTabs">
                                        <ItemTemplate>
                                            <li id="Li1" runat="server" onclick="javascript:showElement(this)" class='<%# Container.ItemIndex == CurrentControlIndex ? "selected" : "" %>'>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Container.ItemIndex %>' />
                                                <asp:Label ID="Literal4" runat="server" Text='<%# Eval("NameTab") %>' />
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </td>
                            <td class="tabContainer" id="tabs-contents">
                                <asp:Panel ID="pnlBody" runat="server" Style="width: 100%">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%if (!InNewView)
        { %> 
        <br />
        <br />
        <asp:Label runat="server" ID="lblInfo"></asp:Label>
        <% } %>
    </div>
</asp:content>
